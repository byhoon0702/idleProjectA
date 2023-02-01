using UnityEngine;
public class MoveState : CharacterFSM
{
	private Unit owner;
	private SkillModule skillModule => owner.skillModule;

	public void Init(Unit owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
		owner.PlayAnimation(StateType.MOVE);
		owner.characterAnimation.PlayParticle();
	}

	public void OnExit()
	{
		owner.PlayAnimation(StateType.IDLE);
		owner.characterAnimation.StopParticle();
	}

	public void OnUpdate(float time)
	{
		//if (skillModule.skillAttack != null && skillModule.skillAttack.Usable())
		//{
		//	skillModule.skillAttack.Action();
		//	VLog.SkillLog($"[{owner.info.charNameAndCharId}] 스킬 사용. SkillName: {skillModule.skillAttack.name}", owner);
		//}

		if (owner.IsTargetAlive() == false)
		{
			owner.targetingBehavior.OnTarget(owner, owner.targeting);

			owner.Move(time);
		}
		else
		{
			Vector3 direction = (owner.target.transform.position - owner.transform.position).normalized;
			direction.z = 0;
			direction.y = 0;
			float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);

			if (distance <= owner.info.searchRange)
			{
				owner.ChangeState(StateType.ATTACK);
			}
			else
			{
				owner.Move(time);
			}
		}
	}
}

