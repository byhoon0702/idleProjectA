﻿
[System.Serializable]
public class MoveSpeedDownConditionData
{
	/// <summary>
	/// 이동속도 감소비율
	/// </summary>
	public float ratio = 0.5f;

	/// <summary>
	/// 지속시간
	/// </summary>
	public float duration = 5;

	public MoveSpeedDownConditionData()
	{

	}

	public MoveSpeedDownConditionData(float _ratio, float _duration)
	{
		ratio = _ratio;
		duration = _duration;
	}
}

public class MoveSpeedDownCondition : ConditionBase
{
	public override CharacterCondition conditionType => CharacterCondition.MoveSpeedDown;

	public override string iconPath => "";

	public override string effectPath => "";

	private MoveSpeedDownConditionData conditionData;

	/// <summary>
	/// 이동속도 감소비율
	/// </summary>
	public float ratio => conditionData.ratio;


	public MoveSpeedDownCondition(Character _attacker, MoveSpeedDownConditionData _conditionData) : base(_attacker, _conditionData.duration)
	{
		conditionData = _conditionData;
	}
}
