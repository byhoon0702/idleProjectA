using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInfo : RuntimeData.UnitInfo
{
	new public Pet owner;
	new public PetData data;

	public PetInfo(Pet _owner, PetData _data)
	{
		this.owner = _owner;
		this.data = _data.Clone();
	}

	public override IdleNumber AttackPower()
	{
		// 공식 (★★★ 변경시 같이 업데이트해주세요 ★★★)
		//(unitPower) * (합 버프) * (blessingBuffRatio) * (hyperBonus)

		// 캐릭터기본공격력
		IdleNumber unitPower = new IdleNumber();




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
		//float hyperBonusMul = 1 + UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Ability.Attackpower);


		// 합 연산
		IdleNumber multifly = (IdleNumber)1 + driveBuffRatio + chainBuffRatio + bookBuffRatio + specificityRatio + recordBuffRatio + supportBuffRatio + adBuffRatio;
		// 곱 연산
		//multifly = multifly * blessingBuffMul * hyperBonusMul;

		IdleNumber total = unitPower * multifly;

		return total;
	}

	public override float AttackSpeed()
	{
		//float hyperBonusMul = UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Ability.AttackSpeed);
		float total = 1;

		total = Mathf.Clamp(total, GameManager.Config.ATTACK_SPEED_MIN, GameManager.Config.ATTACK_SPEED_MAX);

		return total;
	}

	public override float CriticalChanceRatio()
	{
		//전체 능력치를 가지고 계산 해야함
		//float conditionTotalRatio = owner.conditionModule.ability.criticalChanceUpRatio - owner.conditionModule.ability.criticalChanceDownRatio;
		//float total = data.criticalRate + conditionTotalRatio;
		float total = 1;

		if (total > GameManager.Config.CRITICAL_CHANCE_MAX_RATIO)
		{
			total = GameManager.Config.CRITICAL_CHANCE_MAX_RATIO;
		}

		return total;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public override float CriticalDamageMultifly()
	{
		//	float hyperBonusMul = 1 + UnitGlobal.it.hyperModule.GetHyperAbilityRatio(owner, Ability.CriticalDamage);
		float total = 1;//+ data.criticalPowerRate;

		return total;
	}

	public override float MoveSpeed()
	{
		return 6;
	}
}
