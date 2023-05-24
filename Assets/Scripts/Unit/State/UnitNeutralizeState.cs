using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit Neutralize State", menuName = "Unit State/Neutralize", order = 1)]
public class UnitNeutralizeState : UnitFSM
{
	float elapsedTime;
	float airborneTime;

	public override FSM OnEnter()
	{
		return this;
	}

	public override void OnExit()
	{
		//owner.PlayAnimation(StateType.IDLE);
		//owner.unitAnimation.StopParticle();
	}

	public override void OnUpdate(float time)
	{
		//if (owner.knockbackPower <= 0 && owner.airbornePower <= 0)
		//{

		//	owner.unitAnimation.transform.localPosition = Vector3.zero;
		//	owner.ChangeState(StateType.IDLE);
		//	return;
		//}

		//if (owner.knockbackPower > 0)
		//{
		//	Vector3 dir = Vector3.right;
		//	if (owner is PlayerUnit)
		//	{
		//		dir = Vector3.left;
		//	}

		//	owner.transform.Translate(dir * owner.knockbackPower * time);
		//	elapsedTime += time;

		//	owner.knockbackPower -= elapsedTime * 5;
		//}


		//if (owner.airbornePower > 0)
		//{
		//	owner.unitAnimation.transform.Translate(Vector3.up * owner.airbornePower * time, Space.Self);

		//	airborneTime += time;

		//	owner.airbornePower -= 9.8f * airborneTime;
		//}
	}
	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
