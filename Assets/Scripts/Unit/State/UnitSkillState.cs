using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Unit Skill State", menuName = "Unit State/Skill", order = 1)]
/// <summary>
/// 스킬이 애니메이션 변화가 있는 경우에만 해당 스테이트로 들어오도록 작성
/// </summary>
public class UnitSkillState : UnitFSM
{
	public void Init(Unit _owner)
	{
		owner = _owner;
	}
	public override void OnEnter()
	{

	}
	public override void OnEnter<T>(T data = default)
	{

	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		if (owner.isSkillAttack)
		{
			return;
		}
		owner.unitSkillModule.GetUsableSkill().Action();
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
		//	owner.unitSkillModule.GetUsableSkill().Action();
		//}
		//else
		//{
		//	// 스킬을 사용할 수 있으면 무조건 스킬우선사용
		//	owner.ChangeState(StateType.ATTACK);
		//}
	}
}
