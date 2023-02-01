using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThrowData : SkillBaseData
{
	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	[Tooltip("대미지 증가배율")]
	public float attackPowerMul = 1;
}

public class SkillThrow : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private SkillThrowData skillData;




	public SkillThrow(SkillThrowData _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		base.Action();

		if (owner.target != null)
		{
			VLog.SkillLog("돌던지기 스킬 사용");
		}

		SetCooltime();
		coolDowning = true;
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
	}
}
