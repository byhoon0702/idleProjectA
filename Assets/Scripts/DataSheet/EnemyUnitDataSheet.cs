using System.Collections.Generic;

[System.Serializable]
public class EnemyPhaseData
{

}

[System.Serializable]
public class EnemyUnitData : UnitData
{
	//데이터 테이블에만 표시되는 설명 

	public int maxPhase;
	public List<ItemStats> phaseBuff;


	public override T Clone<T>()
	{
		EnemyUnitData clone = new EnemyUnitData();

		clone.name = name;
		clone.resource = resource;

		clone.skillTid = skillTid;
		clone.tid = tid;

		clone.maxPhase = maxPhase;
		clone.phaseBuff = new List<ItemStats>(phaseBuff);

		return clone as T;
	}
}


[System.Serializable]
public class EnemyUnitDataSheet : DataSheetBase<EnemyUnitData>
{
}
