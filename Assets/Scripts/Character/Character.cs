using System;
using System.Collections.Generic;
using UnityEngine;



public enum AttackType
{
	MELEE,
	RANGED,
	MAGIC,
	SKILL,
}

public enum ClassType
{
	WARRIOR,
	ARCHER,
	WIZARD,
	REWARDGEM
}


public interface CharacterFSM : FiniteStateMachine
{
}


public enum StateType
{
	IDLE,
	MOVE,
	ATTACK,
	HIT,
	SKILL,
	DEATH,
}

public enum ControlSide
{
	PLAYER,
	ENEMY,
}

public abstract class Character : MonoBehaviour, DefaultAttack.IDefaultAttackEvent
{
	public Character target { get; private set; }
	public CharacterData rawData;
	//public CharacterData data;

	public CharacterInfo info;
	public CharacterAnimation characterAnimation;
	public SkillModule skillModule;
	public ConditionModule conditionModule;

	public CharacterClass characterClass;


	public FiniteStateMachine currentfsm;

	public IdleState idleState;
	public MoveState moveState;
	public AttackState attackState;
	public DeadState deadStaet;

	public StateType currentState;
	public float attackInterval
	{
		get;
		protected set;
	}
	public float searchInterval
	{
		get;
		protected set;
	}

	public Int32 charID => GetInstanceID();

	protected class Info
	{
		public int index;
		public float distance;
	}

	protected GameObject characterView;


	// Start is called before the first frame update
	public void Init()
	{
		// 스킬초기화
		skillModule = new SkillModule(this, new DefaultAttack(new SkillBaseData(info.data.attackTime)));
		InitSkill(skillModule);

		conditionModule = new ConditionModule(this);

		GameUIManager.it.ShowCharacterGauge(this);

		idleState = new IdleState();
		moveState = new MoveState();
		attackState = new AttackState();
		deadStaet = new DeadState();

		idleState.Init(this);
		moveState.Init(this);
		attackState.Init(this);
		deadStaet.Init(this);

		currentfsm = idleState;
		currentState = StateType.IDLE;

		characterAnimation = characterView.GetComponent<CharacterAnimation>();
		if (characterAnimation == null)
		{
			characterAnimation = characterView.AddComponent<CharacterAnimation>();
		}
	}

	protected void SetCharacterClass()
	{
		if (info.data == null)
		{
			VLog.ScheduleLogError("No Character Data");
			return;
		}


		switch (info.data.classType)
		{
			case ClassType.WARRIOR:
				characterClass = new Warrior(this);
				break;
			case ClassType.ARCHER:
				characterClass = new Archer(this);
				break;
			case ClassType.WIZARD:
				characterClass = new Wizard(this);
				break;
			case ClassType.REWARDGEM:
				characterClass = new RewardGem(this);
				break;
		}
	}

	public void ChangeState(StateType stateType)
	{
		if (currentState == StateType.DEATH)
		{
			return;
		}

		if (conditionModule.HasCondition(UnitCondition.Knockback))
		{
			// 넉백은 Move, Attack상태 이동 불가
			if (stateType == StateType.MOVE || stateType == StateType.ATTACK)
			{
				return;
			}
		}

		if (conditionModule.HasCondition(UnitCondition.Stun))
		{
			// 스턴은 Move, Attack상태 이동 불가
			if (stateType == StateType.MOVE || stateType == StateType.ATTACK)
			{
				return;
			}
		}

		currentState = stateType;
		currentfsm?.OnExit();
		switch (stateType)
		{
			case StateType.IDLE:
				currentfsm = idleState;
				break;
			case StateType.MOVE:
				currentfsm = moveState;
				break;
			case StateType.ATTACK:
				currentfsm = attackState;
				break;
			case StateType.DEATH:
				currentfsm = deadStaet;
				break;
		}
		currentfsm?.OnEnter();
	}

	public virtual void Spawn(CharacterData data)
	{

	}

	public void SetTarget(Character _target)
	{
		// 로그
		string targetString;
		string newTargetString;

		if (target != null)
		{
			targetString = $"{target.info.charNameAndCharId}";
		}
		else
		{
			targetString = $"Target:null";
		}

		if (_target != null)
		{
			newTargetString = $"{_target.info.charNameAndCharId}";
		}
		else
		{
			newTargetString = $"NewTarget:null";
		}
		VLog.AILog($"[{info.charNameAndCharId}] 타겟변경. {targetString} -> {newTargetString}", this);

		// 타겟변경
		target = _target;
	}

	public virtual bool IsAlive()
	{
		return info.data.hp > 0;
	}

	public bool IsTargetAlive()
	{
		if (target == null)
		{
			return false;
		}

		return target.IsAlive();
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.it.currentState != GameState.BATTLE && GameManager.it.currentState != GameState.REWARD && GameManager.it.currentState != GameState.BOSSBATTLE)
		{
			return;
		}

		float delta = Time.deltaTime;

		// idle상태이면 move로 이동 시도
		if (currentState == StateType.IDLE)
		{
			ChangeState(StateType.MOVE);
		}

		skillModule.Update(delta);
		conditionModule.Update(delta);

		currentfsm?.OnUpdate(delta);
	}

	private void LateUpdate()
	{
		if (info.data.hp <= 0)
		{
			ChangeState(StateType.DEATH);
		}
	}

	protected virtual void InitSkill(SkillModule _skillModule)
	{
		characterClass.OnInitSkill(_skillModule);
	}

	public virtual void Move(float delta)
	{

	}

	public virtual void Hit(Character _attacker, IdleNumber _attackPower, Color _color, float _criticalChanceMul)
	{

	}

	public virtual void Heal(Character _attacker, IdleNumber _attackPower, Color _color)
	{

	}

	public virtual void KnockbackStart()
	{
		ChangeState(StateType.IDLE);
	}

	public virtual void StunStart()
	{
		ChangeState(StateType.IDLE);
	}

	public virtual void OnDefaultAttack_ActionEvent()
	{
		characterClass?.OnAttack();
	}

	public virtual void FindTarget(float time, List<Character> _searchTargets)
	{
		searchInterval += time;

		//////////////////////////////////////////////
		// 타겟을 비워주는 조건 체크
		if (target != null)
		{
			if (target.currentState == StateType.DEATH)
			{
				// 타겟이 죽은경우 타겟을 지워준다.
				SetTarget(null);
			}
			else if (Vector3.Distance(transform.position, target.transform.position) <= info.data.searchRange)
			{
				// 타겟이 멀어진경우, 타겟을 지워준다
				SetTarget(null);
			}
		}


		if (searchInterval > info.data.searchTime)
		{
			searchInterval = 0;


			// 새로운 타겟을 찾음
			Character newTarget = null;
			List<Info> infos = new List<Info>();
			if (_searchTargets != null && _searchTargets.Count > 0)
			{
				for (int i = 0; i < _searchTargets.Count; i++)
				{
					float distance = Vector3.Distance(_searchTargets[i].transform.position, transform.position);
					Info info = new Info();
					info.index = i;
					info.distance = distance;
					infos.Add(info);
				}
				infos.Sort((a, b) => { return a.distance.CompareTo(b.distance); });

				if (infos[0].distance <= info.data.searchRange)
				{
					newTarget = _searchTargets[infos[0].index];
				}
			}

			/////////////////////////////////////////////////
			// 타겟 추가 조건 체크
			if (target == null && newTarget != null)
			{
				// 타겟이 없다가 추가됨
				SetTarget(newTarget);
			}
			else if (target != null && newTarget != null && target.charID != newTarget.charID)
			{
				// 타겟이 있고, 새 타겟이 다른경우 공격중인경우엔 타겟을 바꾸지 않는다
				if (currentState != StateType.ATTACK)
				{
					SetTarget(newTarget);
				}
			}
			else if (newTarget == null)
			{
				// 아무것도 안함
			}
			else
			{
				SetTarget(newTarget);
			}
		}
	}
}
