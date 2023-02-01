using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.UI.GridLayoutGroup;





public interface CharacterFSM : FiniteStateMachine
{
}


public abstract class Unit : UnitBase
{
	public CharacterInfo info;

	public SkillModule skillModule;
	public ConditionModule conditionModule;

	public IdleState idleState;
	public MoveState moveState;
	public AttackState attackState;
	public DeadState deadStaet;

	public float shakeAmount = 0.3f;
	public Vector2 colliderSize = new Vector2(1f, 1.5f);

	public SkillBase usingSkillBase;

	public override string defaultAttackSoundPath => info.data.attackSound;
	public override ControlSide controlSide => info.controlSide;

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
	private BoxCollider2D boxCollider;
	public void InitState()
	{
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
	}
	// Start is called before the first frame update
	public void Init()
	{
		// 스킬초기화
		var skillData = ScriptableObject.CreateInstance<DefaultAttackData>();
		skillData.cooltime = info.attackTime;
		skillModule = new SkillModule(this, new DefaultAttack(skillData));
		InitSkill(skillModule);

		conditionModule = new ConditionModule(this);

		InitState();
		cachedTransform = transform;
		characterAnimation = characterView.GetComponent<UnitAnimation>();
		if (characterAnimation == null)
		{
			characterAnimation = characterView.AddComponent<UnitAnimation>();
		}

		if (boxCollider == null)
		{
			boxCollider = transform.GetComponent<BoxCollider2D>();
			if (boxCollider == null)
			{
				boxCollider = gameObject.AddComponent<BoxCollider2D>();
			}
		}

		boxCollider.offset = new Vector2(0, colliderSize.y / 2);
		boxCollider.size = new Vector2(1f, 1.5f);
		boxCollider.isTrigger = true;
	}


	public void ShakeCharacter()
	{
		characterView.transform.localPosition = UnityEngine.Random.insideUnitCircle * 0.3f;
		characterView.transform.DOLocalMove(Vector3.zero, 0.1f);
	}



	public void AttackStart(SkillBase _skillBase)
	{
		usingSkillBase = _skillBase;
		usingSkillBase.SetCooltime();
		usingSkillBase.coolDowning = true;
		PlayAnimation(StateType.ATTACK);
	}

	protected void SetCharacterClass()
	{
		if (info.data == null)
		{
			VLog.ScheduleLogError("No Character Data");
			return;
		}
	}

	public void ChangeState(StateType stateType)
	{
		if (currentState == StateType.DEATH)
		{
			return;
		}

		if (conditionModule.HasCondition(CharacterCondition.Knockback))
		{
			// 넉백은 Move, Attack상태 이동 불가
			if (stateType == StateType.MOVE || stateType == StateType.ATTACK)
			{
				return;
			}
		}

		VLog.AILog($"{info.charNameAndCharId} StateChange {currentState} -> {stateType}");
		OnChangeState(stateType);


	}
	protected virtual void OnChangeState(StateType stateType)
	{
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



	public abstract void Spawn(UnitData data);


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
	}

	public override void Hit(HitInfo _hitInfo)
	{
		//base.Hit(_hitInfo);

		if (info.hp > 0)
		{
			if (_hitInfo.ShakeCamera)
			{
				SceneCamera.it.ShakeCamera();
			}
			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower.ToString(), _hitInfo.fontColor, characterAnimation.CenterPivot.position, _hitInfo.criticalType, isPlayer: _hitInfo.IsPlayerAttack == false);
			ShakeCharacter();
		}
		info.hp -= _hitInfo.TotalAttackPower;

		VGameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}

		if (_hitInfo.hitEffect.IsNullOrWhiteSpace() == false)
		{
			var model = Instantiate(Resources.Load<GameObject>($"B/{_hitInfo.hitEffect}"));
			model.transform.SetParent(SpawnManager.it.enemyRoot.parent, false);
			model.transform.position = transform.position;
		}
	}

	public override void Heal(HealInfo _healInfo)
	{
		//base.Heal(_healInfo);

		if (currentState != StateType.DEATH)
		{
			IdleNumber newHP = info.hp + _healInfo.healRecovery;
			IdleNumber rawHP = info.rawHp;
			if (rawHP < newHP)
			{
				newHP = rawHP;
			}

			IdleNumber addHP = newHP - info.hp;
			info.hp += addHP;
			VGameManager.it.battleRecord.RecordHeal(_healInfo, addHP);
			GameUIManager.it.ShowFloatingText(_healInfo.healRecovery.ToString(), _healInfo.color, characterAnimation.CenterPivot.position, CriticalType.Normal, isPlayer: _healInfo.IsPlayerHeal == false);
		}
	}


	public override bool IsAlive()
	{
		return info.hp > 0;
	}

	public void Kill()
	{
		info.hp = (IdleNumber)0;
	}

	// Update is called once per frame
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

		skillModule.Update(delta);
		conditionModule.Update(delta);

		currentfsm?.OnUpdate(delta);
	}

	protected virtual void LateUpdate()
	{
		if (info.hp <= 0)
		{
			ChangeState(StateType.DEATH);
		}
	}

	protected virtual void InitSkill(SkillModule _skillModule)
	{
		OnInitSkill(_skillModule);
	}
	public virtual void OnInitSkill(SkillModule _skillModule)
	{
		Int64 skillTid = info.data.skillTid;

		if (skillTid != 0)
		{
			if (SkillMeta.it.dic.ContainsKey(skillTid) == false)
			{
				VLog.SkillLogError($"스킬 타입이 테이블에 없음. tid: {skillTid}");
				return;
			}

			SkillBaseData skillBaseData = SkillMeta.it.dic[info.data.skillTid];

			string id = skillBaseData.skillPreset;
			id = id.Substring(0, id.Length - 4); // 끝에 'Data' 텍스트를 지우기 위함
			var type = System.Type.GetType(id);

			if (type == null)
			{
				VLog.SkillLogError($"스킬 타입 정의 안됨. Skill ID: {id}");
				return;
			}

			// 스킬 생성
			SkillBase skill;
			try
			{
				object classObject = System.Activator.CreateInstance(type, new object[] { skillBaseData });

				skill = classObject as SkillBase;
			}
			catch (System.Exception e)
			{
				VLog.SkillLogError($"스킬 생성실패. SKill ID : {id}\n{e}");
				return;
			}

			_skillModule.AddSkill(skill);
		}
	}
	public override void OnDefaultAttack()
	{
		if (attackInterval == 0)
		{
			if (target != null)
			{
				ProjectileManager.it.Create(this);
			}
		}
	}

	public override IdleNumber AttackPower()
	{
		return info.AttackPower();
	}
	public override float CriticalDamageMultifly()
	{
		return info.CriticalDamageMultifly();
	}

	public override float CriticalX2DamageMultifly()
	{
		return info.CriticalX2DamageMultifly();
	}

	public override CriticalType IsCritical()
	{
		return info.IsCritical();
	}

	public virtual void KnockbackStart()
	{
		ChangeState(StateType.IDLE);
	}

	public override void OnDefaultAttack_ActionEvent()
	{
		if (usingSkillBase is DefaultAttack)
		{
			OnDefaultAttack();
		}
		else
		{
			usingSkillBase.Action();
			VLog.SkillLog($"[{info.charNameAndCharId}] 스킬 사용. SkillName: {usingSkillBase.name}", this);
		}
	}

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

			else if (Mathf.Abs(target.transform.position.x - transform.position.x) <= info.searchRange)
			{
				// 타겟이 멀어진경우, 타겟을 지워준다
				SetTarget(null);
			}
		}

		if (searchInterval > 0.1f || _ignoreSearchDelay == true)//info.jobData.searchTime)
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

				if (infos[0].distance <= info.searchRange)
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


	public override void FindTarget(float time, Unit _searchTarget, bool _ignoreSearchDelay)
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

			else if (Mathf.Abs(target.transform.position.x - transform.position.x) <= info.searchRange)
			{
				// 타겟이 멀어진경우, 타겟을 지워준다
				SetTarget(null);
			}
		}

		if (searchInterval > 0.1f || _ignoreSearchDelay == true)//info.jobData.searchTime)
		{
			searchInterval = 0;


			// 새로운 타겟을 찾음
			Unit newTarget = null;
			if (_searchTarget != null)
			{
				float distance = _searchTarget.cachedTr.position.x - cachedTr.position.x;

				if (distance <= info.searchRange)
				{
					newTarget = _searchTarget;
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

	public override string GetProjectileName()
	{
		return info.data.projectileResource;
	}

}
