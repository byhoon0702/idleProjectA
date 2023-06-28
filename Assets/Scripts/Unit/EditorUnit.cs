using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorUnitInfo : UnitInfo
{
	new public EditorUnit owner;
	public int skillLevel = 5;
	public float searchRange
	{
		get
		{
			//if (data.attackType == AttackType.MELEE)
			//{
			//	return VGameManager.Config.PLAYER_TARGET_RANGE_CLOSE;
			//}
			//else
			//{
			//	return VGameManager.Config.PLAYER_TARGET_RANGE_FAR;
			//}
			return GameManager.Config.PLAYER_TARGET_RANGE_CLOSE;
		}
	}

	public EditorUnitInfo(EditorUnit _owner, UnitData _data) : base(_data)
	{
		owner = _owner;

		//CalculateBaseAttackPowerAndHp();

		hp = maxHp;
		//SetProjectile(data.skillEffectTidNormal);
	}

	public override IdleNumber AttackPower()
	{
		// 공식 (★★★ 변경시 같이 업데이트해주세요 ★★★)
		//(unitPower) * (합 버프) * (blessingBuffRatio) * (hyperBonus)

		// 아이템 장착(무기, 악세, 방어구)버프, 아이템 보유버프
		IdleNumber itemBuffRatio = new IdleNumber();//Inventory.it.abilityCalculator.GetAbilityValue(AbilityType.AttackPower);

		// 드라이브 개조버프
		IdleNumber driveBuffRatio = new IdleNumber();

		// 회로 연결 버프
		IdleNumber chainBuffRatio = new IdleNumber();

		// 도감 버프
		IdleNumber bookBuffRatio = new IdleNumber();

		// 특성, 특성 시너지 버프
		IdleNumber specificityRatio = new IdleNumber();

		// 용사의 기록 버프
		IdleNumber recordBuffRatio = new IdleNumber();

		// 서포트 시스템 버프
		IdleNumber supportBuffRatio = new IdleNumber();

		// 광고 보너스 버프
		IdleNumber adBuffRatio = new IdleNumber();

		// 상태이상 적용
		IdleNumber conditionTotalRatio = new IdleNumber();

		// 축복 보너스 버프
		float blessingBuffMul = 1;

		// 하이퍼모드 보너스
		float hyperBonusMul = 1;// + UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, AbilityType.AttackPower);


		// 합 연산
		IdleNumber multifly = (IdleNumber)1 + itemBuffRatio + driveBuffRatio + chainBuffRatio + bookBuffRatio + specificityRatio + recordBuffRatio + supportBuffRatio + adBuffRatio + conditionTotalRatio;
		// 곱 연산
		multifly = multifly * blessingBuffMul * hyperBonusMul;

		IdleNumber total = multifly;// stats[Ability.Attackpower].GetValue() * multifly;

		return total;
	}

	/// <summary>
	/// 틱당 체력회복량
	/// </summary>
	public override IdleNumber HPRecovery()
	{
		IdleNumber itemBuffRatio = new IdleNumber();

		return itemBuffRatio;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public override float AttackSpeed()
	{
		float condition = 1;
		float hyperBonusRatio = 1;// UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, AbilityType.AttackSpeed);
		float total = 1 + condition + hyperBonusRatio;

		total = Mathf.Clamp(total, GameManager.Config.ATTACK_SPEED_MIN, GameManager.Config.ATTACK_SPEED_MAX);

		return 1;
	}


	public float attackTime
	{
		get
		{
			return 1;
		}
	}



}
public class EditorUnit : Unit
{
	public GameObject modelView;
	public UnitData unitData;
	public PetData petData;

	public EditorUnitInfo info;
	[HideInInspector]

	public bool isDummy;
	public override HitInfo HitInfo
	{
		get
		{
			return new HitInfo(AttackPower);
		}
	}

	public override UnitType UnitType => info.data.type;

	public void CreateUnit<T>(string resource)
	{
		var model = UnitModelPoolManager.it.Get(resource, "default");
		model.transform.SetParent(transform);
		model.transform.localPosition = Vector3.zero;
		model.transform.localScale = Vector3.one;

		modelView = model.gameObject;
		unitAnimation = modelView.GetComponent<UnitAnimation>();
		if (unitAnimation == null)
		{
			unitAnimation = modelView.AddComponent<UnitAnimation>();
		}


	}
	public void Spawn(UnitData _data)
	{
		info = new EditorUnitInfo(this, _data);
		unitData = _data;
		if (unitMode != null)
		{
			unitMode = Instantiate(unitMode);
		}
		if (unitHyperMode != null)
		{
			unitHyperMode = Instantiate(unitHyperMode);
		}
		unitMode?.OnInit(this);
		//unitMode?.OnSpawn(info.resource);

		unitHyperMode?.OnInit(this);
		//unitHyperMode?.OnSpawn(info.resource);

		unitHyperMode?.OnModeExit();
		unitMode?.OnModeEnter(StateType.IDLE);

		currentMode = unitMode;
	}

	public void Spawn(PetData _data)
	{
		petData = _data;
		//CreateUnit<Pet>(_data.resource);
	}

	public void SetAttackAnimationLoop(bool isLoop)
	{
#if UNITY_EDITOR
#endif
	}

	public override ControlSide ControlSide => ControlSide.NO_CONTROL;

	public override IdleNumber AttackPower => info.AttackPower();

	public override float AttackSpeed => info.AttackSpeed();
	public override float MoveSpeed => 1;

	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }

	public override IdleNumber MaxHp => info.maxHp;




	//public void SetSkillEffectObject(SkillEffectObject skillObject)
	//{
	//	defaultAttackObject = skillObject;
	//}

	protected override void Update()
	{

	}

	protected override void LateUpdate()
	{

	}



	public override bool IsAlive()
	{
		return info.hp > 0;
	}

	public override void Hit(HitInfo _hitInfo)
	{
		//GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower, CenterPosition, CenterPosition, _hitInfo.criticalType);
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
			//VGameManager.it.battleRecord.RecordHeal(_healInfo, addHP);
			//GameUIManager.it.ShowFloatingText(_healInfo.healRecovery, CenterPosition, CenterPosition, CriticalType.Normal);
		}
	}
	public override void Debuff(List<StatInfo> debufflist)
	{
		foreach (var debuff in debufflist)
		{
			if (info.rawStatus.ContainsKey(debuff.type))
			{
				Debug.Log($"디버프 적용 수치 : {debuff.type} / {info.rawStatus[debuff.type].value}");
			}
		}
	}

	public void ActivateHyperMode()
	{
		unitHyperMode?.OnModeEnter(currentState);
		unitMode?.OnModeExit();
		currentMode = unitHyperMode;
	}

	public void DeactivateHyperMode()
	{
		unitMode?.OnModeEnter(currentState);
		unitHyperMode?.OnModeExit();

		currentMode = unitMode;
	}
	public override void ChangeState(StateType stateType, bool force = false)
	{
		fsmModule?.ChangeState(stateType);
	}

	public override void ActiveHyperEffect()
	{
		throw new System.NotImplementedException();
	}

	public override void InactiveHyperEffect()
	{
		throw new System.NotImplementedException();
	}
}

