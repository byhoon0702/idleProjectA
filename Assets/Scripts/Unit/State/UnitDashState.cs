using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit Dash State", menuName = "Unit State/Dash", order = 1)]
public class UnitDashState : UnitFSM
{

	private float elapsedTime = 0;
	public override FSM OnEnter()
	{
		elapsedTime = 0;
		owner.unitAnimation.PlayParticle();
		return this;

	}

	public override void OnExit()
	{
		owner.SetTarget(null);



		owner.unitAnimation.StopParticle();
	}

	public override void OnUpdate(float time)
	{


	}

	public override void OnFixedUpdate(float fixedTime)
	{
		if (SpawnManager.it != null && owner is EnemyUnit)
		{
			//owner.unitAnimation.SetParameter("moveSpeed", Mathf.Min(owner.MoveSpeed, 2f));
			owner.Dash(fixedTime);
		}
		else
		{
			if (owner.unitAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("run") == false)
			{
				owner.PlayAnimation(StateType.MOVE);



				return;
			}
			owner.Dash(fixedTime);
		}
	}
}
