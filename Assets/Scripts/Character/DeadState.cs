using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CharacterFSM
{
	private Unit owner;
	private float elapsedTime;

	public void Init(Unit owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
		elapsedTime = 0f;

		owner.PlayAnimation(StateType.DEATH);
		owner.SetTarget(null); // 죽으면 타겟을 비워줌
		owner.conditionModule.Collect();

		if (owner.info.controlSide == ControlSide.ENEMY)
		{
			UserInfo.AddExp(Random.Range(30, 50));
		}
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 3f)
		{
			owner.DisposeModel();
			owner.gameObject.SetActive(false);
		}
	}
}
