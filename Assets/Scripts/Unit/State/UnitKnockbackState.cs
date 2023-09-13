using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Knockback State", menuName = "Unit State/Knockback", order = 1)]
public class UnitKnockbackState : UnitFSM
{
	public float power;
	public override FSM OnEnter()
	{
		owner.PlayAnimation(StateType.IDLE);

		return this;
	}


	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		//		owner.rigidbody2D.velocity = Vector2.Min(owner.rigidbody2D.velocity, new Vector2(20, 20));
		if (owner.rigidbody2D.velocity == Vector2.zero)
		{
			owner.ChangeState(StateType.IDLE);

			return;
		}
	}

	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
