using UnityEngine;
public class MoveState : CharacterFSM
{
	private Character owner;
	private SkillModule skillModule => owner.skillModule;


	public void Init(Character owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
		owner.characterAnimation.PlayAnimation("walk");
	}

	public void OnExit()
	{
		owner.characterAnimation.PlayAnimation("idle");
	}

	public void OnUpdate(float time)
	{
		if (skillModule.skillAttack != null && skillModule.skillAttack.Usable())
		{
			skillModule.skillAttack.Action();
			GameManager.it.battleRecord.RecordSkillCount(owner.charID);
			VLog.SkillLog($"[{owner.info.charNameAndCharId}] 스킬 사용. SkillName: {skillModule.skillAttack.GetType()}", owner);
		}

		if (owner.IsTargetAlive() == false)
		{
			if (owner is PlayerCharacter)
			{
				owner.FindTarget(time, CharacterManager.it.GetEnemyCharacters());
			}
			else
			{
				owner.FindTarget(time, CharacterManager.it.GetPlayerCharacters());
			}
			owner.Move(time);
		}
		else
		{
			var direction = (owner.target.transform.position - owner.transform.position).normalized;
			var distance = Vector3.Distance(owner.target.transform.position, owner.transform.position);
			//if (distance < owner.info.data.pursuitDistance)
			{
				if (distance > owner.info.data.attackRange)
				{
					owner.transform.Translate(direction * owner.info.MoveSpeed() * time);
				}
				else
				{
					owner.ChangeState(StateType.ATTACK);
				}
			}
			//else
			//{
			//	owner.Move(time);
			//}
		}
	}
}
