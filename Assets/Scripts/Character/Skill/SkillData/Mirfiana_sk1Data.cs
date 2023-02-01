using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 체력이 가장 많은 적에게 공격력 1142%만큼 피해를 입히고 치명타 적중시 2초 동안 기절 상태로 만든다.
/// </summary>
[Serializable]
public class Mirfiana_sk1Data : SkillBaseData
{
	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	[Tooltip("대미지 증가배율")]
	public float attackPowerMul = 1;
}


public class Mirfiana_sk1 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private Mirfiana_sk1Data skillData;




	public Mirfiana_sk1(Mirfiana_sk1Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		base.Action();

		if (owner.target != null)
		{
			SkillUtility.SimpleAttack(owner, owner.target, AttackType.SKILL, owner.info.AttackPower() * skillData.attackPowerMul, fontColor);

			string[] resList = new string[] { "FX_Lightning_Attacked", "FX_Magic_Fire_01_Attacked", "FX_Water_Attacked" };
			var res = Resources.Load<GameObject>($"B/FX_Magic_Fire_01_Attacked");
			var effect = GameObject.Instantiate(res, SpawnManager.it.enemyRoot.parent);

			effect.transform.position = owner.target.transform.position;
		}
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
	}
}
