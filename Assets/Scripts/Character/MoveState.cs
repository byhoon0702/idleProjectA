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
		owner.characterAnimation.PlayAnimation("move");
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
			VLog.SkillLog($"[{owner.info.charNameAndCharId}] 스킬 사용. SkillName: {skillModule.skillAttack.name}", owner);
		}

		if (owner.IsTargetAlive() == false)
		{
			owner.targetingBehavior.OnTarget(owner, owner.targeting);

			owner.Move(time);
		}
		else
		{
			var direction = (owner.target.transform.position - owner.transform.position).normalized;
			var distance = Vector3.Distance(owner.target.transform.position, owner.transform.position);
			//if (distance < owner.info.data.pursuitDistance)
			{
				if (distance > owner.info.jobData.attackRange)
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
