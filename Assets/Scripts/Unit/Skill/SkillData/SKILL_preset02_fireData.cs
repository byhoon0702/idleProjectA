using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// 전방에 돌덩이를 던져 공격력의 {100+(a * {스킬레벨}}%의 피해를 준다.
/// </summary>
public class SKILL_preset02_fireData : SkillBaseData
{
	/// <summary>
	/// 발사체
	/// </summary>
	[Tooltip("발사체")]
	public string projectile;

	/// <summary>
	/// 발사체 시작 위치
	/// </summary>
	[Tooltip("발사체 시작 위치")]
	public DirectionType projectileStartPos;

	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	[Tooltip("대미지")]
	public float attackPower = 1;

	/// <summary>
	/// 대미지 증가배율 계산식
	/// </summary>
	[FourArithmetic]
	[Tooltip("대미지 증가배율 계산식")]
	public string attackPowerOperator;
}

public class SKILL_preset02_fire : SkillBase
{
	public override float skillUseTime => 1;

	private SKILL_preset02_fireData skillData;

	private double totalAttackPowerMul;



	public SKILL_preset02_fire(SKILL_preset02_fireData _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		SkillEffectData data = DataManager.Get<SkillEffectDataSheet>().GetData(skillEffectTid);
		GameObject go = new GameObject(data.name);
		var normalSkillObject = go.AddComponent<SkillEffectObject>();
		normalSkillObject.SetData(data);
		normalSkillObject.OnSkillStart(owner, owner.target.unitAnimation.CenterPivot.position, owner.AttackPower * totalAttackPowerMul);

		base.Action();
	}

	public override void CalculateSkillLevelData(int _skillLevel)
	{
		totalAttackPowerMul = FourArithmeticCalculator.Calculate(skillData.attackPowerOperator, skillData.attackPower, _skillLevel);
	}
}
