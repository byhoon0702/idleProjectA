using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnitFsmModule))]
public abstract class UnitBase : MonoBehaviour
{
	[HideInInspector] public new Rigidbody2D rigidbody2D;
	protected BoxCollider2D boxCollider2D;
	public HittableUnit target { get; protected set; }
	[HideInInspector] public UnitAnimation unitAnimation;
	[HideInInspector] public UnitFacial unitFacial;

	//public NormalUnitCostume unitCostume { get; protected set; }
	public StateType currentState;


	protected UnitFsmModule fsmModule;
	public SkillModule skillModule;

	public BuffModule buffModule;

	public TargetPriorityBehaviorSO targetPriorityBehavior;
	public Vector3 position { get => transform.position; set => transform.position = value; }
	public virtual Vector3 CenterPosition => unitAnimation.CenterPivot.position;

	public virtual Vector3 HeadPosition => unitAnimation.HeadPivot != null ? unitAnimation.HeadPivot.position : CenterPosition;
	public Vector3 localPos { get => transform.localPosition; set => transform.localPosition = value; }

	public Int32 CharID => GetInstanceID();
	public virtual string CharName => gameObject.name;
	public virtual string NameAndId => $"{CharName}({CharID})";
	public virtual string defaultAttackSoundPath => "";
	public abstract UnitType UnitType { get; }
	public abstract ControlSide ControlSide { get; }
	public virtual float SearchRange { get; } = 30;
	public virtual float AttackRange { get; } = 1;
	public virtual float PursuitRange { get; } = 10;
	public abstract IdleNumber AttackPower { get; }
	public abstract IdleNumber SkillBuffValue { get; }
	public abstract HitInfo HitInfo { get; }
	public abstract float AttackSpeed { get; }
	public virtual float AttackTime => 1;
	public virtual float CriticalDamageMultifly => 1;
	public virtual float CriticalX2DamageMultifly => 1;

	public UnitModeBase unitMode;

	protected UnitModeBase currentMode;
	public Vector3 headingDirection { get; set; }
	public GameObject dashEffet;

	public virtual CriticalType RandomCriticalType => CriticalType.Normal;
	protected float searchInterval;

	protected GameObject model;

	protected bool isRewardable;
	public virtual void EndUpdateSkill() { }
	public virtual void Death() { }
	public virtual float MoveSpeed => 0;
	public virtual Vector3 MoveDirection => Vector3.zero;

	public int currentDir { get; protected set; } = 1;


	public virtual void Dash(float power)
	{

	}

	public virtual void SetUnitCosutmeComponent(NormalUnitCostume _unitCostume)
	{
		//	unitCostume = _unitCostume;
	}
	public virtual void PlayAnimation(string name, float normalizedTime = 1)
	{
		unitAnimation.PlayAnimation(name, normalizedTime);
	}

	public virtual void PlayAnimation(StateType type)
	{
		unitAnimation.PlayAnimation(type);
	}
	public virtual Vector3 HeadingToTarget()
	{

		if (target != null)
		{
			Vector3 normal = (target.transform.position - transform.position).normalized;
			ChangeDirection(normal);
			return normal;
		}
		return Vector3.right;
	}

	public void ChangeDirection(Vector3 normal)
	{
		headingDirection = normal;
		if (headingDirection.x < 0)
		{
			if (currentDir != -1)
			{
				currentDir = -1;
				Vector3 scale = unitAnimation.transform.localScale;
				scale.x = Mathf.Abs(scale.x) * currentDir;
				unitAnimation.transform.localScale = scale;
			}
		}
		else if (headingDirection.x > 0)
		{
			if (currentDir != 1)
			{
				currentDir = 1;
				Vector3 scale = unitAnimation.transform.localScale;
				scale.x = Mathf.Abs(scale.x);
				unitAnimation.transform.localScale = scale;
			}

		}
	}


	protected virtual bool TargetRemovable()
	{
		if (target != null)
		{
			if (target.currentState == StateType.DEATH)
			{
				// 타겟이 죽은경우 타겟을 지워준다.
				return true;
			}

			else if (Mathf.Abs((target.transform.position - transform.position).magnitude) >= SearchRange && unitAnimation.IsAttacking() == true)
			{
				// 타겟이 멀어진경우, 타겟을 지워준다
				return true;
			}
		}

		return false;
	}
	public bool IsTargetInPursuitRange()
	{
		if (target == null)
		{
			return false;
		}

		float distance = (target.position - position).magnitude;

		return distance <= PursuitRange;
	}

	public bool IsTargetAlive()
	{
		if (target == null)
		{
			return false;
		}

		return target.IsAlive();
	}

	public HittableUnit FindNewTarget(List<HittableUnit> _searchTargets, float range)
	{
		var array = targetPriorityBehavior.FilterObject(transform.position, _searchTargets);
		if (array != null && array.Count > 0)
		{

			for (int i = 0; i < array.Count; i++)
			{
				HittableUnit enemy = array[i];
				if (enemy == null || enemy.IsAlive() == false)
				{
					continue;
				}
				float distance = (enemy.position - transform.position).magnitude;
				if (distance <= range)
				{
					return enemy;
				}

			}
		}
		return null;
	}

	protected virtual HittableUnit FindNewTarget(List<HittableUnit> _searchTargets)
	{
		var array = targetPriorityBehavior.FilterObject(transform.position, _searchTargets);

		if (array != null && array.Count > 0)
		{

			for (int i = 0; i < array.Count; i++)
			{
				HittableUnit enemy = array[i];
				if (enemy == null || enemy.IsAlive() == false)
				{
					continue;
				}

				if (TargetInRange(enemy, out float distance, out float additionalRange))
				{
					return enemy;
				}
				if (distance <= PursuitRange + additionalRange)
				{
					return enemy;
				}
				if (distance <= SearchRange + additionalRange)
				{
					return enemy;
				}
			}
		}

		return null;
	}


	protected virtual bool TargetAddable(HittableUnit _newTarget)
	{
		if (target == null && _newTarget != null)
		{
			// 타겟이 없다가 추가됨
			return true;
		}
		else if (target != null && _newTarget != null && target.CharID != _newTarget.CharID)
		{
			// 타겟이 있고, 새 타겟이 다른경우 공격중인경우엔 타겟을 바꾸지 않는다
			if (currentState != StateType.ATTACK)
			{
				return true;
			}
		}
		else if (_newTarget == null)
		{
			// 아무것도 안함
		}
		else
		{
			return true;
		}

		return false;
	}

	public float GetTargetDistance()
	{
		if (target == null)
		{
			return 1f;
		}
		float distance = 0;
		float additionalRange = 0;
		var collider = target.unitAnimation.collider2d;
		if (collider != null)
		{
			additionalRange = collider.size.x / 2f;
			distance = collider.bounds.SqrDistance(transform.position);
		}
		else
		{
			distance = Vector3.Distance(target.position, position);
		}

		return distance;
	}

	public virtual bool TargetInRange(HittableUnit target, out float distance, out float additionalRange)
	{
		distance = 0;
		additionalRange = 0;
		if (target == null)
		{
			return false;
		}
		if (target.IsAlive() == false)
		{
			return false;
		}
		//float distance = Mathf.Abs((target.transform.position - transform.position).magnitude);

		additionalRange = 0;
		var collider = target.unitAnimation.collider2d;
		if (collider != null)
		{
			additionalRange = collider.size.x / 2f;

			distance = collider.bounds.SqrDistance(transform.position);
		}
		else
		{
			distance = Vector3.Distance(target.position, position);
		}
		return distance <= AttackRange + additionalRange;
	}
	public virtual void FindTarget(float _time, bool _ignoreSearchDelay)
	{
		searchInterval += _time;

		// 타겟을 비워주는 조건 체크
		if (TargetRemovable())
		{
			SetTarget(null);
		}
	}
	public virtual void FindTarget(float _time, bool _ignoreSearchDelay, float range)
	{
		searchInterval += _time;

		// 타겟을 비워주는 조건 체크
		if (TargetRemovable())
		{
			SetTarget(null);
		}
	}

	public virtual void RandomTarget(float _time, bool _ignoreSearchDelay)
	{
		searchInterval += _time;

		// 타겟을 비워주는 조건 체크
		if (TargetRemovable())
		{
			SetTarget(null);
		}

	}

	public void SetTarget(HittableUnit _target)
	{
		target = _target;
	}



	public void DisposeModel()
	{
		unitAnimation.Release();
		model = null;
	}

	public virtual void OnMove(float delta)
	{

		transform.Translate(MoveDirection * MoveSpeed * delta);
	}

	public virtual void OverrideAnimator(AnimatorOverrideController overrideController)
	{
		unitAnimation.OverrideAnimation(overrideController);
	}

	public void ResetAnimator()
	{
		unitAnimation.ResetOverrideController();
	}


	public virtual void PlayDamageWhite()
	{

	}


}

