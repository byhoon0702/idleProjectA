using UnityEngine;



public class IdleState : UnitFSM
{
	private Unit owner;



	public void Init(Unit _owner)
	{
		owner = _owner;
	}

	public void OnEnter()
	{
		owner.PlayAnimation(StateType.IDLE);
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{

	}
}

public class MoveState : UnitFSM
{
	private Unit owner;
	private SkillModule skillModule => owner.skillModule;

	public void Init(Unit _owner)
	{
		owner = _owner;
	}
	public void OnEnter()
	{
		if (SpawnManagerV2.it != null && owner is EnemyUnit)
		{

		}
		else
		{
			owner.PlayAnimation(StateType.MOVE);
			owner.unitAnimation.PlayParticle();
		}
	}

	public void OnExit()
	{
		if (SpawnManagerV2.it != null && owner is EnemyUnit)
		{

		}
		else
		{
			owner.PlayAnimation(StateType.IDLE);
			owner.unitAnimation.StopParticle();
		}
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
			owner.FindTarget(time, false);

			owner.Move(time);
		}
		else
		{
			if (SpawnManagerV2.it != null)
			{
				Vector3 direction = (owner.target.transform.position - owner.transform.position).normalized;
				direction.z = 0;
				direction.y = 0;
				float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);

				if (distance <= owner.SearchRange)
				{
					owner.ChangeState(StateType.ATTACK);
				}
				else
				{

					owner.Move(time);
				}
			}
			else
			{
				Vector3 direction = (owner.target.transform.position - owner.transform.position).normalized;
				direction.z = 0;
				direction.y = 0;
				float distance = Mathf.Abs(owner.target.transform.position.x - owner.transform.position.x);

				if (distance <= owner.SearchRange)
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
}

public class AttackState : UnitFSM
{
	private Unit owner;
	private SkillModule skillModule => owner.skillModule;



	public void Init(Unit _owner)
	{
		owner = _owner;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnUpdate(float time)
	{
		owner.FindTarget(time, false);

		if (owner.IsTargetAlive() == false)
		{
			owner.FindTarget(time, true);

			if (owner.IsTargetAlive() == false)
			{
				owner.ChangeState(StateType.MOVE);
				return;
			}
		}

		bool isAttacking = owner.unitAnimation.IsAttacking();
		if (isAttacking)
		{
			return;
		}

		if (skillModule.mainSkill != null && skillModule.mainSkill.Usable())
		{
			// 스킬을 사용할 수 있으면 무조건 스킬우선사용
			owner.AttackStart(skillModule.mainSkill);
		}
		else if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		{
			// 기본공격
			owner.AttackStart(skillModule.defaultAttack);
		}
	}
}

public class DeadState : UnitFSM
{
	private Unit owner;
	private float elapsedTime;



	public void Init(Unit _owner)
	{
		owner = _owner;
	}

	public void OnEnter()
	{
		elapsedTime = 0f;

		owner.PlayAnimation(StateType.DEATH);
		owner.SetTarget(null); // 죽으면 타겟을 비워줌
		owner.conditionModule.Collect();

		CheckReward(); // 보상체크는 다른데서
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{
		elapsedTime += time;

		if (elapsedTime > 3f)
		{
			owner.DisposeModel();
			owner.gameObject.SetActive(false);
			owner.ReleaseSkillEffectObject();
		}
	}

	private void CheckReward()
	{
		if (owner.ControlSide != ControlSide.ENEMY)
		{
			return;
		}


		if (owner.isBoss == true)
		{
			StageManager.it.GetBossKillReward(owner.transform);
		}
		else
		{
			UserInfo.AddExp(Random.Range(30, 50));
			StageManager.it.GetReward(owner.transform);
		}
	}
}
