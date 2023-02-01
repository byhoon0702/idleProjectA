using UnityEngine;
public class AttackState : CharacterFSM
{
	private Unit owner;
	private SkillModule skillModule => owner.skillModule;

	public void Init(Unit owner)
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
			owner.targetingBehavior.OnTarget(owner, owner.targeting, true);

			if (owner.IsTargetAlive() == false)
			{
				owner.ChangeState(StateType.MOVE);
				return;
			}
		}

		bool isAttacking = owner.characterAnimation.IsAttacking();
		if (isAttacking)
		{
			return;
		}

		if (skillModule.skillAttack != null && skillModule.skillAttack.Usable())
		{
			// 스킬을 사용할 수 있으면 무조건 스킬우선사용
			owner.AttackStart(skillModule.skillAttack);
		}
		else if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		{
			// 기본공격
			owner.AttackStart(skillModule.defaultAttack);
		}
	}
}

