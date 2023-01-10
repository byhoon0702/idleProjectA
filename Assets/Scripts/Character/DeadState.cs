using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CharacterFSM
{
	private Character owner;
	public void Init(Character owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
		owner.DisposeModel();
		owner.gameObject.SetActive(false);
		owner.SetTarget(null); // 죽으면 타겟을 비워줌
		owner.conditionModule.Collect();
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{

	}
}
