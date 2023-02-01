using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CompanionMoveState : CharacterFSM
{
	private Companion owner;

	public void Init(Companion owner)
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
		//Debug.Log("Move");
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
			//if (distance < owner.info.data.pursuitDistance)
			{
				if (distance <= 6)
				{
					owner.ChangeState(StateType.ATTACK);
				}
			}

		}
	}
}
public class CompanionAttackState : CharacterFSM
{
	private Companion owner;
	private SkillModule skillModule => owner.skillModule;

	public void Init(Companion owner)
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

		bool isAttacking = owner.characterAnimation.IsAttacking();
		if (isAttacking)
		{
			return;
		}

		//if (skillModule.skillAttack != null && skillModule.skillAttack.Usable())
		//{
		//	// 스킬을 사용할 수 있으면 무조건 스킬우선사용
		//	owner.AttackStart(skillModule.skillAttack);
		//}
		//else if (skillModule.defaultAttack != null && skillModule.defaultAttack.Usable())
		//{
		//	// 기본공격
		owner.AttackStart();
		//}
	}
}
public class CompanionIdleState : CharacterFSM
{
	private Companion owner;
	public void Init(Companion owner)
	{
		this.owner = owner;
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

	private void FindTarget()
	{

	}
}


public class Companion : UnitBase
{
	public SkillModule skillModule;

	public CompanionInfo info;
	public CompanionIdleState idleState;
	public CompanionAttackState attackState;

	public CompanionMoveState companionMoveState;
	public override string defaultAttackSoundPath => info.data.attackSound;
	public override ControlSide controlSide => ControlSide.PLAYER;
	public void InitState()
	{
		idleState = new CompanionIdleState();
		companionMoveState = new CompanionMoveState();
		attackState = new CompanionAttackState();


		idleState.Init(this);
		companionMoveState.Init(this);
		attackState.Init(this);


		currentfsm = idleState;
		currentState = StateType.IDLE;
	}
	public void ChangeState(StateType stateType)
	{
		currentState = stateType;
		currentfsm?.OnExit();
		switch (stateType)
		{
			case StateType.IDLE:
				currentfsm = idleState;
				break;
			case StateType.MOVE:
				currentfsm = companionMoveState;
				break;
			case StateType.ATTACK:
				currentfsm = attackState;
				break;
		}
		currentfsm?.OnEnter();
	}
	public void Init()
	{
		// 스킬초기화
		var skillData = ScriptableObject.CreateInstance<DefaultAttackData>();
		//skillData.cooltime = info.attackTime;
		//skillModule = new SkillModule(this, new DefaultAttack(skillData));
		//InitSkill(skillModule);



		InitState();

		characterAnimation = characterView.GetComponent<UnitAnimation>();
		if (characterAnimation == null)
		{
			characterAnimation = characterView.AddComponent<UnitAnimation>();
		}
	}
	protected virtual void Update()
	{
		if (VGameManager.it.currentState != GameState.BATTLE
			&& VGameManager.it.currentState != GameState.REWARD
			&& VGameManager.it.currentState != GameState.BOSSBATTLE)
		{
			return;
		}

		float delta = Time.deltaTime;

		// idle상태이면 move로 이동 시도
		if (currentState == StateType.IDLE)
		{
			ChangeState(StateType.MOVE);
		}

		currentfsm?.OnUpdate(delta);
	}

	public void AttackStart()
	{

		PlayAnimation(StateType.ATTACK);
	}
	public void Spawn(CompanionData _data)
	{
		info = new CompanionInfo(this, _data);

		if (characterView == null)
		{
			var model = UnitModelPoolManager.it.GetModel(("B/" + this.info.data.resource));
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one * 0.008f;
			var cam = SceneCamera.it.sceneCamera;
			model.transform.LookAt(model.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
			AnimationEventReceiver eventReceiver = model.GetComponent<UnitAnimation>().animator.gameObject.GetComponent<AnimationEventReceiver>();
			if (eventReceiver == null)
			{
				eventReceiver = model.GetComponent<UnitAnimation>().animator.gameObject.AddComponent<AnimationEventReceiver>();
			}
			eventReceiver.Init(this);

			characterView = model;

			gameObject.name = info.charNameAndCharId;
		}
		Init();
		targeting = Targeting.OPPONENT;
	}

	public override IdleNumber AttackPower()
	{
		return info.AttackPower();
	}

	public override void Move(float _delta)
	{

	}

	public override void OnDefaultAttack_ActionEvent()
	{
		OnDefaultAttack();
	}

	public override void OnDefaultAttack()
	{

		{
			if (target != null)
			{
				ProjectileManager.it.Create(this);
			}
		}
	}
	float searchInterval;
	public override void FindTarget(float time, List<Unit> _searchTargets, bool _ignoreSearchDelay)
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

			else if (Mathf.Abs(target.transform.position.x - transform.position.x) <= 6)
			{
				// 타겟이 멀어진경우, 타겟을 지워준다
				SetTarget(null);
			}
		}


		if (searchInterval > 0.1f)//info.jobData.searchTime)
		{
			searchInterval = 0;


			// 새로운 타겟을 찾음
			Unit newTarget = null;
			List<Info> infos = new List<Info>();
			if (_searchTargets != null && _searchTargets.Count > 0)
			{
				for (int i = 0; i < _searchTargets.Count; i++)
				{
					float distance = _searchTargets[i].transform.position.x - transform.position.x;
					Info info = new Info();
					info.index = i;
					info.distance = distance;
					infos.Add(info);
				}
				infos.Sort((a, b) => { return a.distance.CompareTo(b.distance); });

				if (infos[0].distance <= 6)
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
	public override void SetTarget(Unit _target)
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
		// 로그
	}
	public override string GetProjectileName()
	{
		return info.data.projectileResource;
	}
}
