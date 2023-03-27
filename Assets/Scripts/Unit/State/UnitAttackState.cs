using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Attack State", menuName = "Unit State/Attack", order = 1)]
public class UnitAttackState : UnitFSM
{
	private SkillModule skillModule => owner.skillModule;

	public void Init(Unit _owner)
	{
		owner = _owner;
	}
	public override void OnEnter()
	{

	}
	public override void OnEnter<T>(T data)
	{

	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		{
			owner.AttackStart(skillModule.defaultAttack);
		}
		//if (owner.IsTargetAlive() == false)
		//{
		//	owner.ChangeState(StateType.MOVE);
		//	return;
		//}

		//float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);

		//if (distance > owner.SearchRange)
		//{
		//	owner.ChangeState(StateType.MOVE);
		//	return;
		//}

		//if (owner.canSkillAutoAttack)
		//{
		//	owner.ChangeState(StateType.SKILL, true);
		//	return;
		//}

		//if (owner.canNormalAttack)
		//{
		//	if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		//	{
		//		// 기본공격
		//		owner.AttackStart(skillModule.defaultAttack);
		//	}
		//}
	}
}
