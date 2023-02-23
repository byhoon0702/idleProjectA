using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInfo : UnitInfo
{
	new public Pet owner;
	new public PetData data;
	new public PetData rawData;

	public PetInfo(Pet owner, PetData _data) : base()
	{
		this.owner = owner;
		this.data = _data.Clone();
		this.rawData = _data;

		rawAttackPower = (IdleNumber)data.attackPower;
		SetProjectile(data.skillEffectTidNormal);

	}

	public override IdleNumber AttackPower()
	{
		// 공식 (★★★ 변경시 같이 업데이트해주세요 ★★★)
		//(unitPower) * (합 버프) * (blessingBuffRatio) * (hyperBonus)

		// 캐릭터기본공격력
		IdleNumber unitPower = rawAttackPower;

		// 아이템 장착(무기, 악세, 방어구)버프, 아이템 보유버프
		IdleNumber itemBuffRatio = Inventory.it.abilityCalculator.GetAbilityValue(Stats.Attackpower);

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

		// 축복 보너스 버프
		float blessingBuffMul = 1;

		// 하이퍼모드 보너스
		float hyperBonusMul = 1 + UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.Attackpower);


		// 합 연산
		IdleNumber multifly = (IdleNumber)1 + itemBuffRatio + driveBuffRatio + chainBuffRatio + bookBuffRatio + specificityRatio + recordBuffRatio + supportBuffRatio + adBuffRatio;
		// 곱 연산
		multifly = multifly * blessingBuffMul * hyperBonusMul;

		IdleNumber total = unitPower * multifly;

		return total;
	}

	public override float AttackSpeedMul()
	{
		float hyperBonusMul = UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.AttackSpeed);
		float total = hyperBonusMul;

		total = Mathf.Clamp(total, ConfigMeta.it.ATTACK_SPEED_MIN, ConfigMeta.it.ATTACK_SPEED_MAX);

		return total;
	}
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
	public override float CriticalChanceRatio()
	{
		//전체 능력치를 가지고 계산 해야함
		//float conditionTotalRatio = owner.conditionModule.ability.criticalChanceUpRatio - owner.conditionModule.ability.criticalChanceDownRatio;
		//float total = data.criticalRate + conditionTotalRatio;
		float total = 1;

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
		float hyperBonusMul = 1 + UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Stats.CriticalDamage);
		float total = hyperBonusMul;//+ data.criticalPowerRate;

		return total;
	}

	public override float MoveSpeed()
	{
		return data.moveSpeed;
	}
}
