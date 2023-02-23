using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
	public Unit target { get; protected set; }
	public UnitAnimation unitAnimation;
	public StateType currentState;

	public FiniteStateMachine currentfsm;

	public TargetFilterBehaviorSO targetFilterBehavior;
	public TargetPriorityBehaviorSO targetPriorityBehavior;
	public Vector3 position { get => transform.position; set => transform.position = value; }
	public Vector3 localPos { get => transform.localPosition; set => transform.localPosition = value; }

	public Int32 CharID => GetInstanceID();
	public virtual string CharName => gameObject.name;
	public virtual string NameAndId => $"{CharName}({CharID})";
	public virtual string defaultAttackSoundPath => "";
	public abstract ControlSide ControlSide { get; }
	public virtual float SearchRange { get; } = 1;
	public abstract IdleNumber AttackPower { get; }
	public abstract float AttackSpeedMul { get; }
	public virtual float AttackTime => 1;
	public virtual float CriticalDamageMultifly => 1;
	public virtual float CriticalX2DamageMultifly => 1;

	public virtual CriticalType RandomCriticalType => CriticalType.Normal;
	protected float searchInterval;

	protected abstract string ModelResourceName { get; }
	protected GameObject model;




	public abstract void SetAttack();
	public abstract void DefaultAttack(float time);
	public abstract void ResetDefaultAttack();

	public virtual float MoveSpeed => 0;
	public virtual Vector3 MoveDirection => Vector3.zero;

	public abstract SkillEffectData GetSkillEffectData();


	public virtual void PlayAnimation(StateType type)
	{
		unitAnimation.PlayAnimation(type);
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

	protected virtual bool TargetRemovable()
	{
		if (target != null)
		{
			if (target.currentState == StateType.DEATH)
			{
				// 타겟이 죽은경우 타겟을 지워준다.
				return true;
			}

			else if (Mathf.Abs(target.transform.position.x - transform.position.x) >= SearchRange && unitAnimation.IsAttacking() == true)
			{
				// 타겟이 멀어진경우, 타겟을 지워준다
				return true;
			}
		}

		return false;
	}
	protected virtual Unit FindNewTarget(List<Unit> _searchTargets)
	{
		Unit newTarget = null;
		var array = targetPriorityBehavior.FilterObject(transform.position, _searchTargets);

		if (array != null && array.Count > 0)
		{
			newTarget = array[0];
		}
		//float minDistance = float.MaxValue;

		//for (int i = 0; i < _searchTargets.Count; i++)
		//{
		//	float distance = _searchTargets[i].transform.position.x - transform.position.x;

		//	if (distance < minDistance)
		//	{
		//		newTarget = _searchTargets[i];
		//		minDistance = distance;
		//	}
		//}

		//if (minDistance <= SearchRange)
		//{
		//	return newTarget;
		//}



		return newTarget;
	}


	protected virtual bool TargetAddable(Unit _newTarget)
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

	public virtual void FindTarget(float _time, bool _ignoreSearchDelay)
	{
		searchInterval += _time;

		// 타겟을 비워주는 조건 체크
		if (TargetRemovable())
		{
			SetTarget(null);
		}

	}

	public void SetTarget(Unit _target)
	{
		target = _target;
	}

	public void LoadModel()
	{
		var obj = UnitModelPoolManager.it.Get(ModelResourceName);
		obj.transform.SetParent(transform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;

		Camera sceneCam;
		if (SceneCameraV2.it != null)
		{
			sceneCam = SceneCameraV2.it.sceneCamera;
		}
		else
		{
			sceneCam = SceneCamera.it.sceneCamera;
		}

		obj.transform.LookAt(obj.transform.position + sceneCam.transform.rotation * Vector3.forward, sceneCam.transform.rotation * Vector3.up);
		AnimationEventReceiver eventReceiver = obj.animator.gameObject.GetComponent<AnimationEventReceiver>();
		if (eventReceiver == null)
		{
			eventReceiver = obj.animator.gameObject.AddComponent<AnimationEventReceiver>();
		}
		eventReceiver.Init(this);

		model = obj.gameObject;
		gameObject.name = NameAndId;
	}

	public void DisposeModel()
	{
		unitAnimation.Release();
		model = null;
	}

	public virtual void Move(float delta)
	{
		transform.Translate(MoveDirection * MoveSpeed * delta);
	}

	public virtual void Hit(HitInfo _hitInfo)
	{

	}

	public virtual void Heal(HealInfo _healInfo)
	{

	}

	public virtual void Debuff(DebuffInfo _debuffInfo)
	{

	}
	public virtual void Buff()
	{

	}
}
