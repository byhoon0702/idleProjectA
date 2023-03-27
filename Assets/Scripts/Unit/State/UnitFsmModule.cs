using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFsmModule : MonoBehaviour
{
	public UnitAttackState attackState;
	public UnitIdleState idleState;
	public UnitMoveState moveState;
	public UnitDeadState deadState;
	public UnitSkillState skillState;
	public UnitNeutralizeState neutralizeState;

	private UnitFSM currentfsm;
	private Unit owner;
	private StateType currentState;
	public void Init(Unit unit)
	{
		owner = unit;

		if (attackState != null)
		{
			attackState = Instantiate(attackState);
			attackState.Init(owner);
		}

		if (idleState != null)
		{
			idleState = Instantiate(idleState);
			idleState.Init(owner);
		}
		if (moveState != null)
		{
			moveState = Instantiate(moveState);
			moveState.Init(owner);
		}
		if (deadState != null)
		{
			deadState = Instantiate(deadState);
			deadState.Init(owner);
		}

		if (skillState != null)
		{
			skillState = Instantiate(skillState);
			skillState.Init(owner);
		}

		if (neutralizeState != null)
		{
			neutralizeState = Instantiate(neutralizeState);
			neutralizeState.Init(owner);
		}

	}

	//public void ChangeState<T>(StateType type, T param = default, bool force = false)
	//{
	//	if (currentState == type && force == false)
	//	{
	//		return;
	//	}
	//	OnChangeStateWithParam(type, param);
	//}
	public void ChangeState(StateType type, bool force = false)
	{
		if (currentState == type && force == false)
		{
			return;
		}
		OnChangeState(type);
	}

	public void OnUpdate(float time)
	{
		currentfsm?.OnUpdate(time);
	}

	protected UnitFSM GetFSM(StateType stateType)
	{
		switch (stateType)
		{
			case StateType.IDLE:
				return idleState;

			case StateType.MOVE:
				return moveState;

			case StateType.ATTACK:
				return attackState;

			case StateType.DEATH:
				return deadState;

			case StateType.SKILL:
				return skillState;

			case StateType.NEUTRALIZE:
				return neutralizeState;

		}
		return currentfsm;
	}

	//protected virtual void OnChangeStateWithParam<T>(StateType stateType, T param = default)
	//{
	//	currentState = stateType;
	//	currentfsm?.OnExit();
	//	currentfsm = GetFSM(stateType);
	//	currentfsm?.OnEnter(param);
	//}
	protected virtual void OnChangeState(StateType stateType)
	{
		currentState = stateType;
		currentfsm?.OnExit();
		currentfsm = GetFSM(stateType);
		currentfsm?.OnEnter();
	}
}
