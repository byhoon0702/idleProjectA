using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionInfo : UnitInfo
{

	public Companion owner;
	public CompanionData data;
	public CompanionData rawData;
	public string charName => rawData.name;

	public IdleNumber attackPower;
	public string charNameAndCharId => $"{data.name}({owner.charID})";
	public CompanionInfo(Companion owner, CompanionData _data)
	{
		this.owner = owner;
		this.data = _data.Clone();
		this.rawData = _data;

		attackPower = (IdleNumber)data.attackPower;

	}
	public override IdleNumber AttackPower(bool _random = true)
	{
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.AttackPower);
		float multifly = 1 + hyperBonus;

		IdleNumber total = attackPower * multifly;

		if (_random)
		{
			total += total * Random.Range(-ConfigMeta.it.ATTACK_POWER_RANGE, ConfigMeta.it.ATTACK_POWER_RANGE);
		}

		return total;
	}
	public override float AttackSpeedMul()
	{
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.AttackSpeed);
		float total = 1 + hyperBonus;

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
		float hyperBonus = UnitGlobal.it.hyperModule.GetHyperAbility(owner, AbilityType.CriticalAttackPower);
		float total = 1 + hyperBonus;//+ data.criticalPowerRate;

		return total;
	}
}
