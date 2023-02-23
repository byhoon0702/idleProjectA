using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections.ObjectModel;
using System;

public class PlayerUnit : Unit
{
	public PlayerUnitInfo info;


	List<Pet> pets = new List<Pet>();
	private float hpRecoveryRemainTime;

	public override string CharName => info.data.name;
	protected override string ModelResourceName => info.data.resource;
	public override ControlSide ControlSide => ControlSide.PLAYER;
	public override float SearchRange => info.searchRange;
	public override IdleNumber AttackPower => info.AttackPower();
	public override float AttackSpeedMul => info.AttackSpeedMul();
	public override float AttackTime => info.attackTime;


	public override CriticalType RandomCriticalType => info.IsCritical();
	public override float CriticalDamageMultifly => info.CriticalDamageMultifly();
	public override float CriticalX2DamageMultifly => info.CriticalX2DamageMultifly();


	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }
	public override IdleNumber MaxHp => info.maxHP;
	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.right;

	private FxSpriteEffectAuto trainingLevelupEffect;





	protected override void LateUpdate()
	{
		base.LateUpdate();

		if (currentState != StateType.DEATH)
		{
			HPRecoveryUpdate(Time.deltaTime);
		}
	}



	private void HPRecoveryUpdate(float _dt)
	{
		hpRecoveryRemainTime += _dt;
		if (hpRecoveryRemainTime >= ConfigMeta.it.PLAYER_HP_RECOVERY_CYCLE)
		{
			hpRecoveryRemainTime -= ConfigMeta.it.PLAYER_HP_RECOVERY_CYCLE;
			if (Hp < MaxHp)
			{
				var recoveryValue = info.HPRecovery();
				if (recoveryValue > 0)
				{
					Heal(new HealInfo(AttackerType.Player, recoveryValue));
				}
			}
		}
	}

	public override void Spawn(UnitData _data, int _level = 1)
	{
		if (_data == null)
		{
			VLog.ScheduleLogError("No Unit Data");
		}

		info = new PlayerUnitInfo(this, _data);

		if (model == null)
		{
			LoadModel();
		}

		Init();
		GameUIManager.it.ShowUnitGauge(this);
		pets.AddRange(UnitManager.it.GetPets());

		if (UnitGlobal.it.hyperModule.IsHyperMode)
		{
			ActiveHyperEffect();
		}
	}

	public override void DefaultAttack(float time)
	{
		base.DefaultAttack(time);
		UnitGlobal.it.hyperModule.AccumGauge();
	}

	/// <summary>
	/// 하이퍼 모드 활성화 가능여부
	/// 해당 캐릭터가 사용할 수 있는 상태인지만 체크하면 됨
	/// </summary>
	public bool IsActiveHyperMode()
	{
		return true;
	}

	/// <summary>
	/// 하이퍼 모드 활성화
	/// </summary>
	public void ActiveHyperEffect()
	{
		unitAnimation.ActiveHyperEffect();
		VLog.SkillLog($"[{NameAndId}] 하이퍼 모드 발동");
	}

	/// <summary>
	/// 하이퍼 모드 비활성화
	/// </summary>
	public void InactiveHyperEffect()
	{
		unitAnimation.InactiveHyperEffect();
		VLog.SkillLog($"[{NameAndId}] 하이퍼 모드 발동종료");
	}

	/// <summary>
	/// 하이퍼 브레이크 발동이 가능한 상태인지
	/// </summary>
	public bool UseableHyperBreak()
	{
		return target != null;
	}

	/// <summary>
	/// 하이퍼 브레이크 발동
	/// </summary>
	public void StartHyperBreak()
	{
		VLog.SkillLog($"[{NameAndId}] 하이퍼 브레이크");

		FinishHyperBreak(); // <- 애니메이션 끝나는 시점에 호출하게 바꿔주세요
	}

	/// <summary>
	/// 하이퍼 브레이크 스킬종료
	/// </summary>
	public void FinishHyperBreak()
	{
		VLog.SkillLog($"[{NameAndId}] 하이퍼 브레이크 종료");
		UnitGlobal.it.hyperModule.InactiveHyperMode();
	}

	public override SkillEffectData GetSkillEffectData()
	{
		return info.normalSkillEffectData;
	}

	public override void Hit(HitInfo _hitInfo)
	{
		base.Hit(_hitInfo);
		unitAnimation.PlayAnimation(StateType.HIT);
	}

	public void PlayLevelupEffect()
	{
		if (trainingLevelupEffect == null)
		{
			trainingLevelupEffect = Instantiate(Resources.Load<FxSpriteEffectAuto>("B/FX_TrainingLevelup"), transform);
		}

		trainingLevelupEffect.Stop();
		trainingLevelupEffect.Play();
	}


	public override void FindTarget(float _time, bool _ignoreSearchDelay)
	{
		base.FindTarget(_time, _ignoreSearchDelay);

		if (searchInterval > ConfigMeta.it.TARGET_SEARCH_DELAY || _ignoreSearchDelay)
		{
			searchInterval = 0;

			// 새로운 타겟을 찾음
			Unit newTarget = FindNewTarget(UnitManager.it.GetEnemyUnit());
			if (TargetAddable(newTarget))
			{
				SetTarget(newTarget);
			}
		}
	}

	public void ResetUnit()
	{
		Hp = MaxHp;
	}

	public override void Debuff(List<StatInfo> debufflist)
	{
		foreach (var debuff in debufflist)
		{
			switch (debuff.type)
			{
				case Stats._NONE:
					break;
				case Stats.Attackpower:
					break;
				case Stats.Hp:
					break;
				case Stats.HpRecovery:
					break;
				case Stats.CriticalChance:
					break;
				case Stats.CriticalDamage:
					break;
				case Stats.SuperCriticalChance:
					break;
				case Stats.SuperCriticalDamage:
					break;
				case Stats.AttackSpeed:
					break;
				case Stats.AttackpowerIncrease:
					break;
				case Stats.HpIncrease:
					break;
				case Stats.SkillCooldown:
					break;
				case Stats.SkillDamage:
					break;
				case Stats.EnemyDamagePlus:
					break;
				case Stats.BossDamagePlus:
					break;
				case Stats.Evasion:
					break;
				case Stats.GainGold:
					break;
				case Stats.GainUserexp:
					break;
				case Stats.GainItem:
					break;
				case Stats.Movespeed:
					break;
			}
		}
	}
}
