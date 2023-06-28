



[System.Serializable]
public class PreConditionData
{
	public StatsType preconditionType;
	public long preconditionLevel;
	public bool isSum;
}
[System.Serializable]
public class Cost
{
	public CurrencyType currency;
	public string cost;
	public float costIncrease;
	public float costWeight;
}



[System.Serializable]
public class VeterancyData : BaseData
{
	public string name;
	public ItemStats buff;

	public int maxLevel;

	public PreConditionData preCondition;

	public Cost basicCost;
}

[System.Serializable]
public class VeterancyDataSheet : DataSheetBase<VeterancyData>
{

}
