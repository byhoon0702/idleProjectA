using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public interface UnitFSM : FiniteStateMachine
{
}


public abstract class Unit : UnitBase
{
	public SkillModule skillModule;
	public ConditionModule conditionModule;

	public IdleState idleState;
	public MoveState moveState;
	public AttackState attackState;
	public DeadState deadStaet;

	public float shakeAmount = 0.3f;
	public Vector2 colliderSize = new Vector2(1f, 1.5f);

	public SkillBase usingSkillBase;

	public bool isBoss = false;


	protected bool isRewardable = true;

	private BoxCollider2D boxCollider;
	public virtual long SkillTid => 0;

	public abstract IdleNumber Hp { get; set; }
	public abstract IdleNumber MaxHp { get; }

	protected SkillEffectObject normalSkillObject = null;

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

	public void Init()
	{
		// 스킬초기화
		var skillData = ScriptableObject.CreateInstance<DefaultAttackData>();
		skillData.cooltime = AttackTime;

		skillModule = new SkillModule(this, new DefaultAttack(skillData));
		InitSkill(skillModule);

		conditionModule = new ConditionModule(this);

		InitState();

		unitAnimation = model.GetComponent<UnitAnimation>();
		if (unitAnimation == null)
		{
			unitAnimation = model.AddComponent<UnitAnimation>();
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


	public void ShakeUnit()
	{
		model.transform.localPosition = UnityEngine.Random.insideUnitCircle * 0.3f;
		model.transform.DOLocalMove(Vector3.zero, 0.1f);
	}



	public void AttackStart(SkillBase _skillBase)
	{
		usingSkillBase = _skillBase;
		usingSkillBase.SetCooltime();
		usingSkillBase.coolDowning = true;
		PlayAnimation(StateType.ATTACK);
	}
	public override void ResetDefaultAttack()
	{
		if (normalSkillObject == null)
		{
			return;
		}

		normalSkillObject.Reset();
		normalSkillObject.Release();
	}

	public override void SetAttack()
	{

		if (normalSkillObject == null)
		{
			SkillEffectData data = GetSkillEffectData();

			normalSkillObject = SkillEffectObjectPool.it.Get();
			normalSkillObject.SetData(data);
		}

		if (target != null)
		{
			normalSkillObject.OnSkillStart(this, target.unitAnimation.CenterPivot.position, AttackPower);
		}
	}
	public override void DefaultAttack(float time)
	{
		if (target != null)
		{

		}
		normalSkillObject.UpdateFromOutSide(time);
	}

	public void ChangeState(StateType stateType)
	{
		if (currentState == StateType.DEATH)
		{
			return;
		}

		if (currentState == stateType)
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


		VLog.AILog($"{NameAndId} StateChange {currentState} -> {stateType}");
		OnChangeState(stateType);


	}
	protected virtual void OnChangeState(StateType stateType)
	{
		if (currentState == stateType)
		{
			return;
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



	public abstract void Spawn(UnitData data, int _level = 1);

	public override void Hit(HitInfo _hitInfo)
	{
		//base.Hit(_hitInfo);

		if (Hp > 0)
		{
			if (_hitInfo.ShakeCamera)
			{
				if (SceneCameraV2.it != null)
				{
					SceneCameraV2.it.ShakeCamera();
				}
				else
				{
					SceneCamera.it.ShakeCamera();
				}
			}
			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower.ToString(), _hitInfo.fontColor, unitAnimation.CenterPivot.position, _hitInfo.criticalType, isPlayer: _hitInfo.IsPlayerAttack == false);
			ShakeUnit();
		}
		Hp -= _hitInfo.TotalAttackPower;

		VGameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}
	}

	public override void Heal(HealInfo _healInfo)
	{
		//base.Heal(_healInfo);

		if (currentState != StateType.DEATH)
		{
			IdleNumber newHP = Hp + _healInfo.healRecovery;
			IdleNumber rawHP = MaxHp;
			if (rawHP < newHP)
			{
				newHP = rawHP;
			}

			IdleNumber addHP = newHP - Hp;
			Hp += addHP;
			VGameManager.it.battleRecord.RecordHeal(_healInfo, addHP);
			GameUIManager.it.ShowFloatingText(_healInfo.healRecovery.ToString(), _healInfo.color, unitAnimation.CenterPivot.position, CriticalType.Normal, isPlayer: _healInfo.IsPlayerHeal == false);
		}
	}


	public override bool IsAlive()
	{
		return Hp > 0;
	}

	public void Kill()
	{
		Hp = (IdleNumber)0;
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
		if (Hp <= 0)
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
		if (SkillTid != 0)
		{
			if (DataManager.SkillMeta.dic.ContainsKey(SkillTid) == false)
			{
				VLog.SkillLogError($"스킬 타입이 테이블에 없음. tid: {SkillTid}");
				return;
			}

			SkillBaseData skillBaseData = DataManager.SkillMeta.dic[SkillTid];

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
			_skillModule.RegistMainSkill(skill);
		}
	}

	public void ReleaseSkillEffectObject()
	{
		if (normalSkillObject != null)
		{
			normalSkillObject.Release();
		}
	}

	public virtual void KnockbackStart()
	{
		ChangeState(StateType.IDLE);
	}

	public virtual void StunStart()
	{
		// 애니메이션 시작
		ChangeState(StateType.IDLE);
	}

	public virtual void StunFinish()
	{
		// 애니메이션 종료
	}

	public virtual bool ConditionApplicable(ConditionBase _condition)
	{
		return true;
	}

	public virtual void Debuff(List<StatInfo> debufflist)
	{

	}
}
