using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class Pet : UnitBase
{
	public SkillModule skillModule;

	public PetInfo info;
	public PetIdleState idleState;
	public PetAttackState attackState;

	public PetMoveState petMoveState;

	public float acceleration = 1;
	public override string CharName => info.data.name;
	protected override string ModelResourceName => info.data.resource;
	public override string defaultAttackSoundPath => info.data.attackSound;
	public override ControlSide ControlSide => ControlSide.PLAYER;
	public override IdleNumber AttackPower => info.AttackPower();
	public override float AttackSpeedMul => info.AttackSpeedMul();
	public override float CriticalDamageMultifly => info.CriticalDamageMultifly();
	public override float CriticalX2DamageMultifly => info.CriticalX2DamageMultifly();

	public override float SearchRange => 10;
	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.right;

	private SkillEffectObject normalSkillObject = null;



	public void InitState()
	{
		idleState = new PetIdleState();
		petMoveState = new PetMoveState();
		attackState = new PetAttackState();


		idleState.Init(this);
		petMoveState.Init(this);
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
				currentfsm = petMoveState;
				break;
			case StateType.ATTACK:
				currentfsm = attackState;
				break;
		}
		currentfsm?.OnEnter();
	}
	int positionIndex = 0;
	public void Init(int index)
	{

		positionIndex = index;

		InitState();

		unitAnimation = model.GetComponent<UnitAnimation>();
		if (unitAnimation == null)
		{
			unitAnimation = model.AddComponent<UnitAnimation>();
		}

		if (SpawnManagerV2.it != null)
		{
			petPosition = SpawnManagerV2.it.GetPartyPos(positionIndex);
		}
		else
		{
			petPosition = UnitManager.it.GetPartyPos(positionIndex);
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
		if (SpawnManagerV2.it != null)
		{
			petPosition = SpawnManagerV2.it.GetPartyPos(positionIndex);
		}
		else
		{
			petPosition = UnitManager.it.GetPartyPos(positionIndex);
		}

		// idle상태이면 move로 이동 시도
		if (currentState == StateType.IDLE)
		{
			ChangeState(StateType.MOVE);
		}

		currentfsm?.OnUpdate(delta);
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
		if (normalSkillObject == null)
		{
			return;
		}
		normalSkillObject.UpdateFromOutSide(time);
		//normalSkillObject.Attack(time);
	}
	public override void ResetDefaultAttack()
	{
		if (normalSkillObject == null)
		{
			return;
		}

		normalSkillObject.Reset();
	}
	public void AttackStart()
	{

		PlayAnimation(StateType.ATTACK);
	}
	public void Spawn(PetData _data, int index)
	{
		info = new PetInfo(this, _data);

		if (model == null)
		{
			LoadModel();
		}

		Init(index);

	}

	Vector3 petPosition = Vector3.zero;

	public override void Move(float _delta)
	{
		Vector3 toward = Vector3.MoveTowards(position, petPosition, _delta * info.MoveSpeed() * acceleration);

		transform.SetPositionAndRotation(toward, transform.rotation);

		float distance = petPosition.x - position.x;
		if (distance > 0.1f)
		{
			acceleration = 1.5f;
		}
		else
		{
			acceleration = 1f;
		}
	}

	public override SkillEffectData GetSkillEffectData()
	{
		return info.normalSkillEffectData;
	}
}
