

public class ConditionAbility
{
	/// <summary>
	/// 공격력 증가 비율
	/// </summary>
	public float attackPowerUpRatio;
	/// <summary>
	/// 공격력 감소 비율
	/// </summary>
	public float attackPowerDownRatio;

	/// <summary>
	/// 공격속도 증가 비율
	/// </summary>
	public float attackSpeedUpRatio;
	/// <summary>
	/// 공격속도 감소 비율
	/// </summary>
	public float attackSpeedDownRatio;

	/// <summary>
	/// 크리티컬 증가 비율
	/// </summary>
	public float criticalChanceUpRatio;
	/// <summary>
	/// 크리티컬 감소 비율
	/// </summary>
	public float criticalChanceDownRatio;

	/// <summary>
	/// 이동속도 증가 비율
	/// </summary>
	public float moveSpeedUpRatio;
	/// <summary>
	/// 이동속도 감소 비율
	/// </summary>
	public float moveSpeedDownRatio;




	/// <summary>
	/// 컨디션 정보가 변화할때마다 호출해서 데이터를 최신화 시킬수 있어야 함
	/// </summary>
	public void Calculate(ConditionModule _module)
	{
		attackPowerUpRatio = CalculateAttackPowerUpRatio(_module);
		attackPowerDownRatio = CalculateAttackPowerDownRatio(_module);

		attackSpeedUpRatio = CalcutateAttackSpeedUpRatio(_module);
		attackSpeedDownRatio = CalculateAttackSpeedDownRatio(_module);

		criticalChanceUpRatio = CalculateCriticalChanceUpRatio(_module);
		criticalChanceDownRatio = CalculateCriticalChanceDownRatio(_module);

		moveSpeedUpRatio = CalculateMoveSpeedUpRatio(_module);
		moveSpeedDownRatio = CalculateMoveSpeedDownRatio(_module);
	}

	private float CalculateAttackPowerUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackPowerUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackPowerUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateAttackPowerDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackPowerDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackPowerDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalcutateAttackSpeedUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackSpeedUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackSpeedUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateAttackSpeedDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackSpeedDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackSpeedDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateCriticalChanceUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.CriticalChanceUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as CriticalChanceUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateCriticalChanceDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.CriticalChanceDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as CriticalChanceDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateMoveSpeedUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.MoveSpeedUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as MoveSpeedUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateMoveSpeedDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.MoveSpeedDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as MoveSpeedDownCondition).ratio;
		}

		return outTotal;
	}
}
