using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class UnitBase : MonoBehaviour, DefaultAttack.IDefaultAttackEvent
{
	public Unit target { get; protected set; }
	public UnitAnimation characterAnimation;
	public StateType currentState;
	public TargetingBehavior targetingBehavior;
	public Targeting targeting;
	public FiniteStateMachine currentfsm;
	protected Transform cachedTransform;
	public Transform cachedTr => cachedTransform;

	public Vector3 position
	{
		get
		{
			return cachedTransform.position;
		}
	}
	public Int32 charID => GetInstanceID();
	public virtual string defaultAttackSoundPath => "";
	public virtual ControlSide controlSide => ControlSide.ENEMY;


	protected class Info
	{
		public int index;
		public float distance;
	}
	protected GameObject characterView;

	protected virtual void Awake()
	{
		cachedTransform = transform;
	}
	public abstract void OnDefaultAttack_ActionEvent();
	public abstract void OnDefaultAttack();

	public virtual void PlayAnimation(StateType type)
	{
		characterAnimation.PlayAnimation(type);

	}

	public virtual bool IsAlive()
	{
		return false;
	}
	public bool IsTargetAlive()
	{
		if (target == null)
		{
			return false;
		}

		return target.IsAlive();

	}

	public virtual void FindTarget(float time, List<Unit> _searchTargets, bool _ignoreSearchDelay)
	{

	}
	public virtual void FindTarget(float time, Unit _searchTarget, bool _ignoreSearchDelay)
	{

	}
	public virtual void SetTarget(Unit _target)
	{
		target = _target;
		// 로그
	}

	public void DisposeModel()
	{
		UnitModelPoolManager.it.ReturnModel(characterView);
		characterView = null;
	}
	public virtual void Move(float delta)
	{

	}

	public virtual void Hit(HitInfo _hitInfo)
	{

	}

	public virtual void Heal(HealInfo _healInfo)
	{

	}
	public virtual IdleNumber AttackPower()
	{
		return null;
	}
	public virtual float CriticalDamageMultifly()
	{
		return 1;
	}
	public virtual float CriticalX2DamageMultifly()
	{
		return 1;
	}
	public virtual CriticalType IsCritical()
	{
		return CriticalType.Normal;
	}

	public virtual string GetProjectileName()
	{
		return "";
	}

}
