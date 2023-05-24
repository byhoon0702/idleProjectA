using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnit : Unit
{
	public override float SearchRange
	{

		get
		{
			return 0;
		}
	}

	public override IdleNumber Hp
	{
		set
		{
			Hp = value;
		}
		get
		{
			return (IdleNumber)100;
		}
	}

	public override IdleNumber MaxHp
	{
		get
		{
			return (IdleNumber)100;
		}
	}

	public override ControlSide ControlSide
	{
		get
		{
			return ControlSide.NO_CONTROL;
		}
	}

	public override UnitType UnitType => UnitType.NormalEnemy;

	public override IdleNumber AttackPower
	{
		get
		{
			return (IdleNumber)0;
		}
	}

	public override HitInfo HitInfo
	{
		get
		{
			return new HitInfo(AttackPower);
		}
	}
	public override float AttackSpeed
	{
		get
		{
			return 0;
		}
	}
	//protected SkillEffectObject normalSkillObject = null;

	public void Spawn(string resource)
	{
		if (unitMode != null)
		{
			unitMode = Instantiate(unitMode);
		}
		unitMode?.OnInit(this);
		unitMode?.OnSpawn("", resource);

		unitMode?.OnModeEnter(StateType.IDLE);
	}

	public override void Hit(HitInfo _hitInfo)
	{
		if (_hitInfo == null)
		{
			return;
		}
		unitMode.OnHit(_hitInfo);
		GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower, CenterPosition, CenterPosition, _hitInfo.criticalType);
	}


	protected override void Update()
	{
		for (int i = 0; i < neutralizeMoves.Count; i++)
		{
			neutralizeMoves[i].OnUpdate(Time.deltaTime);
			if (neutralizeMoves[i].IsEnd())
			{
				neutralizeMoves.RemoveAt(i);
			}
		}
		for (int i = 0; i < additionalDamageModules.Count; i++)
		{
			additionalDamageModules[i].OnUpdate(Time.deltaTime);
			if (additionalDamageModules[i].isEnd())
			{
				additionalDamageModules[i].RemoveParticle();
				additionalDamageModules.RemoveAt(i);
			}
		}
	}

	protected override void LateUpdate()
	{

	}

	public override void ChangeState(StateType stateType, bool force = false)
	{


		//if (currentState == StateType.DEATH)
		//{
		//	return;
		//}

		//if (currentState == stateType && force == false)
		//{
		//	return;
		//}

		//if (conditionModule.HasCondition(UnitCondition.Knockback))
		//{
		//	// 넉백은 Move, Attack상태 이동 불가
		//	if (stateType == StateType.MOVE || stateType == StateType.ATTACK)
		//	{
		//		return;
		//	}
		//}

		//if (conditionModule.HasCondition(UnitCondition.Stun))
		//{
		//	// 스턴은 Move, Attack상태 이동 불가
		//	if (stateType == StateType.MOVE || stateType == StateType.ATTACK)
		//	{
		//		return;
		//	}
		//}


		//if (stateType == StateType.DEATH && isRewardable)
		//{
		//	isRewardable = false;
		//	StageManager.it.CheckKillRewards(UnitType, transform);
		//}

		//VLog.AILog($"{NameAndId} StateChange {currentState} -> {stateType}");
		//fsmModule?.ChangeState(stateType);
	}


	public override void KnockBack(float power, Vector3 dir, int hitCount, bool isLastHit = true)
	{
		//knockbackPower = Mathf.Max(0, power);

		//KnockbackMove move = new KnockbackMove();
		//move.Set(transform, NeutralizeType.KNOCKBACK, knockbackPower, dir, 1, isLastHit);

		//var existMove = neutralizeMoves.Find(x => x.type.Equals(NeutralizeType.KNOCKBACK));

		//if (isLastHit == false)
		//{
		//	move.InstantlyMove((knockbackPower / 10) / hitCount);
		//	OnCrowdControl();
		//	return;
		//}

		//neutralizeMoves.Remove(existMove);
		//neutralizeMoves.Add(move);
		//OnCrowdControl();

	}
	public override void AirBorne(float power, int hitCount, bool isLastHit = true)
	{
		//airbornePower = Mathf.Max(0, power);
		//AirborneMove move = new AirborneMove();
		//move.Set(transform, NeutralizeType.AIRBORNE, airbornePower, Vector3.up, 1, isLastHit);

		//var existMove = neutralizeMoves.Find(x => x.type.Equals(NeutralizeType.AIRBORNE));

		//if (isLastHit == false)
		//{
		//	move.InstantlyMove((airbornePower / 10) / hitCount);
		//	OnCrowdControl();
		//	return;
		//}

		//neutralizeMoves.Remove(existMove);
		//neutralizeMoves.Add(move);
		////if (existMove == null)
		////{
		////	neutralizeMoves.Add(move);
		////}
		////else
		////{
		////	existMove = move;
		////}
		//OnCrowdControl();
	}

	public override void AdditionalDamage(AdditionalDamageInfo info, HitInfo hitinfo)
	{
		AdditionalDamageModule module = new AdditionalDamageModule();
		module.Set(this, info, hitinfo);
		additionalDamageModules.Add(module);
	}
}
