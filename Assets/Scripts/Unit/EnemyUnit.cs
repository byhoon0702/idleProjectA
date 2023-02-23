
using UnityEngine;
using System.Collections.Generic;

public class EnemyLevelData
{
	public int level;
	public string attack;
}

public class EnemyData : BaseData
{
	public string name;
	public List<EnemyLevelData> leveldata;
}

public class EnemyUnit : Unit
{
	public EnemyUnitInfo info;

	protected override string ModelResourceName => info.data.resource;
	public override ControlSide ControlSide => ControlSide.ENEMY;
	public override float SearchRange => info.searchRange;
	public override float AttackTime => info.attackTime;
	public override IdleNumber AttackPower => info.AttackPower();
	public override float AttackSpeedMul => info.AttackSpeedMul();


	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }
	public override IdleNumber MaxHp => info.maxHP;

	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.left;

	public override void Spawn(UnitData _data, int _level = 1)
	{
		//rawData = data;
		if (_data == null)
		{
			VLog.ScheduleLogError("No Unit Data");
		}

		info = new EnemyUnitInfo(this, _data, _level);

		if (model == null)
		{
			LoadModel();
			model.gameObject.tag = "Enemy";
		}

		Init();
		GameUIManager.it.ShowUnitGauge(this);
	}

	public override bool ConditionApplicable(ConditionBase _condition)
	{
		bool result = info.ConditionApplicable(_condition);
		return result;
	}

	public override SkillEffectData GetSkillEffectData()
	{
		return info.normalSkillEffectData;
	}

	public override void Hit(HitInfo _hitInfo)
	{
		//base.Hit(_hitInfo);

		if (Hp > 0)
		{
			if (_hitInfo.ShakeCamera)
			{
				if (SceneCameraV2.it != null)
				{
					SceneCameraV2.it.ShakeCamera();
				}
				else
				{
					SceneCamera.it.ShakeCamera();
				}
			}
			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower.ToString(), _hitInfo.fontColor, unitAnimation.CenterPivot.position, _hitInfo.criticalType, isPlayer: _hitInfo.IsPlayerAttack == false);
			ShakeUnit();
		}
		Hp -= _hitInfo.TotalAttackPower;

		VGameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}

		//if (_hitInfo.hitEffect.IsNullOrWhiteSpace() == false)
		//{
		//	HitEffect hitEffect = HitEffectPoolManager.it.Get(_hitInfo.hitEffect);
		//	hitEffect.transform.SetParent(SpawnManager.it.enemyRoot.parent, false);
		//	hitEffect.transform.position = transform.position;
		//	hitEffect.Play();
		//}
	}

	public override void FindTarget(float _time, bool _ignoreSearchDelay)
	{
		base.FindTarget(_time, _ignoreSearchDelay);
		if (searchInterval > ConfigMeta.it.TARGET_SEARCH_DELAY || _ignoreSearchDelay)
		{
			searchInterval = 0;

			// 새로운 타겟을 찾음
			Unit newTarget = UnitManager.it.Player;
			if (TargetAddable(newTarget))
			{
				SetTarget(newTarget);
			}
		}

	}
}
