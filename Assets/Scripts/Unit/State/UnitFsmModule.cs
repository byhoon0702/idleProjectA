using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFsmModule : MonoBehaviour
{
	public UnitFSM[] fsmStates;

	public Dictionary<StateType, UnitFSM> fsmDictionary = new Dictionary<StateType, UnitFSM>();
	private UnitFSM currentfsm;
	private Unit owner;
	private StateType currentState;

	public void Init(Unit unit)
	{
		owner = unit;

		for (int i = 0; i < fsmStates.Length; i++)
		{
			if (fsmStates[i] == null)
			{
				continue;
			}

			fsmStates[i] = Instantiate(fsmStates[i]);
			fsmStates[i].Init(owner);

			fsmDictionary.Add(fsmStates[i].State, fsmStates[i]);
		}
	}

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

	public void OnFixedUpdate(float time)
	{
		currentfsm?.OnFixedUpdate(time);
	}

	protected UnitFSM GetFSM(StateType stateType)
	{
		if (fsmDictionary.ContainsKey(stateType))
		{
			return fsmDictionary[stateType];
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
