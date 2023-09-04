using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Pet Skill State", menuName = "Pet State/Skill", order = 1)]
/// <summary>
/// 스킬이 애니메이션 변화가 있는 경우에만 해당 스테이트로 들어오도록 작성
/// </summary>
public class PetSkillState : PetFSM
{

	public override FSM OnEnter()
	{
		base.OnEnter();
		return this;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{

	}
	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
