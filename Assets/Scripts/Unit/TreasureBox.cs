using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : HittableUnit
{
	public UnitData info;
	public override UnitStats GetStats => new UnitStats();
	public override IdleNumber AttackPower => (IdleNumber)0;

	protected override string ModelResourceName => info.resource;

	public override ControlSide ControlSide => ControlSide.ENEMY;
	public override UnitType UnitType => UnitType.TreasureBox;

	public override float AttackSpeedMul => 0;

	public override IdleNumber Hp { get; set; }

	private IdleNumber maxHp;
	public override IdleNumber MaxHp => maxHp;

	public override Vector3 CenterPosition => position;

	protected bool isRewardable = true;


	public void Spawn(EnemySpawnInfo _spawnInfo, GameStageInfo _stageInfo)
	{
		info = _spawnInfo.unitData;

		if (model == null)
		{
			var model = Instantiate(Resources.Load<GameObject>($"B/{ModelResourceName}"));
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one;
			maxHp = _stageInfo.UnitHP(false);
			Hp = maxHp;

			base.model = model.gameObject;
			model.gameObject.tag = "Enemy";
			unitAnimation = model.GetComponent<UnitAnimation>();
			unitAnimation.Init();
			GameUIManager.it.ShowUnitGauge(this);
		}
	}

	public override bool IsAlive()
	{
		return Hp > 0;
	}

	private void LateUpdate()
	{
		if (isRewardable && IsAlive() == false)
		{
			isRewardable = false;
			StageManager.it.CheckKillRewards(UnitType, transform);
		}
	}

	public override void Hit(HitInfo _hitInfo)
	{
		if (Hp > 0)
		{
			if (_hitInfo.ShakeCamera)
			{
				SceneCamera.it.ShakeCamera();
			}
			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower.ToString(), _hitInfo.fontColor, CenterPosition, _hitInfo.criticalType, isPlayer: _hitInfo.IsPlayerCast == false);
		}
		unitAnimation?.PlayDamageWhite();
		Hp -= _hitInfo.TotalAttackPower;

		VGameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		if (ControlSide == ControlSide.ENEMY)
		{
			UIController.it.UiStageInfo.RefreshDPSCount();
		}
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}
	}

	public override void SetAttack(SkillEffectData data, SkillInfoObject infoObject = null, SkillData _skillData = null)
	{
		//throw new System.NotImplementedException();
	}

	public override void OnUpdateAttack(float time)
	{
		//throw new System.NotImplementedException();
	}

	public override void EndUpdateAttack()
	{
		//throw new System.NotImplementedException();
	}

	public override SkillEffectData GetSkillEffectData()
	{
		throw new System.NotImplementedException();
	}
}
