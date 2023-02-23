using System.Collections;
using System.Collections.Generic;
using IniParser.Model;
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
			//	return ConfigMeta.it.PLAYER_TARGET_RANGE_CLOSE;
			//}
			//else
			//{
			//	return ConfigMeta.it.PLAYER_TARGET_RANGE_FAR;
			//}
			return ConfigMeta.it.PLAYER_TARGET_RANGE_CLOSE;
		}
	}

	public EditorUnitInfo(EditorUnit _owner, UnitData _data) : base(_data)
	{
		owner = _owner;

		CalculateBaseAttackPowerAndHp();

		maxHP = CalculateMaxHP();
		hp = maxHP;
		SetProjectile(data.skillEffectTidNormal);
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

		IdleNumber total = rawAttackPower * multifly;

		return total;
	}

	/// <summary>
	/// 최대체력은 실시간으로 계산하지 않는다.
	/// (아이템 장착등의 변경이 있거나, 어빌리티를 올렸을때만 체크해줄 예정)
	/// </summary>
	public IdleNumber CalculateMaxHP()
	{
		// 공식 (★★★ 변경시 같이 업데이트해주세요 ★★★)
		//(hp) * (합 버프)

		// 아이템 장착(무기, 악세, 방어구)버프, 아이템 보유버프
		IdleNumber itemBuffRatio = new IdleNumber();

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

		// 합 연산
		IdleNumber multifly = (IdleNumber)1 + itemBuffRatio + driveBuffRatio + chainBuffRatio + bookBuffRatio + specificityRatio + recordBuffRatio + supportBuffRatio + adBuffRatio;

		IdleNumber total = rawHp * multifly;

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
	public override float AttackSpeedMul()
	{
		float condition = 1;
		float hyperBonusRatio = 1;// UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, AbilityType.AttackSpeed);
		float total = 1 + condition + hyperBonusRatio;

		total = Mathf.Clamp(total, ConfigMeta.it.ATTACK_SPEED_MIN, ConfigMeta.it.ATTACK_SPEED_MAX);

		return total;
	}

	/// <summary>
	/// 크리티컬 발동여부. true면 크리티컬로 처리하면 됨
	/// </summary>
	public override CriticalType IsCritical()
	{
		float total = CriticalChanceRatio();
		bool isCritical = SkillUtility.Cumulative(total);
		bool isCriticalX2 = SkillUtility.Cumulative(total);

		if (isCriticalX2 && isCritical)
		{
			return CriticalType.CriticalX2;
		}
		else if (isCritical)
		{
			return CriticalType.Critical;
		}
		else
		{
			return CriticalType.Normal;
		}
	}

	public float attackTime
	{
		get
		{
			return 1;
		}
	}



	public PlayerAttackType GetAttackType()
	{
		if (TripleAttack())
		{
			return PlayerAttackType.TripleAttack;
		}
		else if (DoubleAttack())
		{
			return PlayerAttackType.DoubleAttack;
		}
		else
		{
			return PlayerAttackType.Attack;
		}
	}

	private bool TripleAttack()
	{
		return false;
		//IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.TripleAttack) * 0.01f;
		//bool result = SkillUtility.Cumulative(itemBuffRatio.GetValueFloat());

		//return result;
	}

	private bool DoubleAttack()
	{
		return false;
		//IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.DoubleAttack) * 0.01f;
		//bool result = SkillUtility.Cumulative(itemBuffRatio.GetValueFloat());

		//return result;
	}

	public override float CriticalChanceRatio()
	{
		return 1;
		float conditionTotalRatio = 1;
		float hyperBonusRatio = UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.CriticalChance);
		float total = data.criticalRate + conditionTotalRatio + hyperBonusRatio;


		if (total > ConfigMeta.it.CRITICAL_CHANCE_MAX_RATIO)
		{
			total = ConfigMeta.it.CRITICAL_CHANCE_MAX_RATIO;
		}

		return total;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public override float CriticalDamageMultifly()
	{
		float total = 1 + data.criticalPowerRate;

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public override float MoveSpeed()
	{
		return 1;

	}

	/// <summary>
	/// true면 컨디션 적용 안됨
	/// </summary>
	public bool ConditionApplicable(ConditionBase _condition)
	{
		switch (_condition.conditionType)
		{
			case UnitCondition.Knockback:
				return false;

		}

		return true;
	}
}
public class EditorUnit : Unit
{
	public GameObject modelView;
	public UnitData unitData;
	public PetData petData;

	public EditorUnitInfo info;
	[HideInInspector]
	public SkillEffectData skilleffectData;

	public bool isDummy;

	protected override string ModelResourceName => "";


	AttackStateBehavior attackStateBehavior;
	public void CreateUnit<T>(string resource)
	{
		var model = UnitModelPoolManager.it.Get(resource);
		model.transform.SetParent(transform);
		model.transform.localPosition = Vector3.zero;
		model.transform.localScale = Vector3.one;

		modelView = model.gameObject;
		unitAnimation = modelView.GetComponent<UnitAnimation>();
		if (unitAnimation == null)
		{
			unitAnimation = modelView.AddComponent<UnitAnimation>();
		}

		attackStateBehavior = unitAnimation.animator.GetBehaviour<AttackStateBehavior>();
	}
	public void Spawn(UnitData _data)
	{
		info = new EditorUnitInfo(this, _data);
		unitData = _data;
		CreateUnit<Unit>(_data.resource);
	}

	public void Spawn(PetData _data)
	{
		petData = _data;
		CreateUnit<Pet>(_data.resource);
	}

	public void SetAttackAnimationLoop(bool isLoop)
	{
		attackStateBehavior.isLoop = isLoop;
	}

	public override ControlSide ControlSide => ControlSide.NO_CONTROL;

	public override IdleNumber AttackPower => info.AttackPower();

	public override float AttackSpeedMul => info.AttackSpeedMul();
	public override float MoveSpeed => 1;

	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }

	public override IdleNumber MaxHp => info.maxHP;

	public TargetFilterBehaviorSO targetFilterBehaviorSO;


	public void SetSkillEffectObject(SkillEffectObject skillObject)
	{
		normalSkillObject = skillObject;
	}

	protected override void Update()
	{

	}

	protected override void LateUpdate()
	{

	}
	public override void SetAttack()
	{
		if (normalSkillObject == null)
		{
			return;
		}
		Vector3 firePos = unitAnimation.CenterPivot.position;
		target = UnitEditor.it.FindTarget();
		Vector3 targetPos = new Vector3(firePos.x + 6, firePos.y, firePos.z);
		if (target != null)
		{
			targetPos = target.position;
		}


		normalSkillObject.OnSkillStart(this, firePos, targetPos, AttackPower);

	}

	public override void DefaultAttack(float time)
	{
		if (normalSkillObject == null)
		{
			return;
		}
		normalSkillObject.UpdateFromOutSide(time);
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

	public override bool IsAlive()
	{
		return info.hp > 0;
	}

	public override void Hit(HitInfo _hitInfo)
	{
		GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower.ToString(), _hitInfo.fontColor, unitAnimation.CenterPivot.position, _hitInfo.criticalType, isPlayer: _hitInfo.IsPlayerAttack == false);
	}

	public override SkillEffectData GetSkillEffectData()
	{
		return skilleffectData;
	}

	public override void Spawn(UnitData data, int _level = 1)
	{

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
			GameUIManager.it.ShowFloatingText(_healInfo.healRecovery.ToString(), _healInfo.color, unitAnimation.CenterPivot.position, CriticalType.Normal, isPlayer: _healInfo.IsPlayerHeal == false);
		}
	}
	public override void Debuff(List<StatInfo> debufflist)
	{
		foreach (var debuff in debufflist)
		{
			if (info.status.ContainsKey(debuff.type))
			{
				Debug.Log($"디버프 적용 수치 : {debuff.type} / {info.status[debuff.type].value}");
			}
		}

	}
}
