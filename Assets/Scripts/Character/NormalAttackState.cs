using UnityEngine;
public class NormalAttackState : CharacterFSM
{
	private Character owner;
	private SkillModule skillModule => owner.skillModule;


	public void Init(Character owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnUpdate(float time)
	{
		if (owner.IsTargetAlive() == false)
		{
			owner.ChangeState(StateType.MOVE);
			return;
		}

		
		if (skillModule.skillAttack != null && skillModule.skillAttack.Usable())
		{
			// 스킬을 사용할 수 있으면 무조건 스킬우선사용
			skillModule.skillAttack.Action();
			GameManager.it.battleRecord.RecordSkillCount(owner.charID);
			VLog.SkillLog($"[{owner.info.data.name}({owner.charID})] 스킬 사용. SkillName: {skillModule.skillAttack.GetType()}", owner);
		}
		else if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		{
			// 기본공격
			skillModule.defaultAttack.Action();
			GameManager.it.battleRecord.RecordAttackCount(owner.charID);
		}
	}
}

