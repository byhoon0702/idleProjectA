using System;
using System.Collections.Generic;
using System.Data;


public enum Grade
{
	D,
	C,
	B,
	A,
	S,
	SS,
	SSS,
	_END,

}

/// <summary>
/// Raw 데이터
/// </summary>
[Serializable]
public class UnitData : BaseData
{
	//데이터 테이블에만 표시되는 설명 

	public UnitType type;
	public string resource;

	public Int64 skillTid = 0;

	public UnitData()
	{
		//기본적으로 무조건 추가 되어야 할 데이터들

	}

	public virtual T Clone<T>() where T : UnitData
	{
		UnitData data = new UnitData();
		data.name = name;
		data.resource = resource;

		data.skillTid = skillTid;
		data.tid = tid;

		return data as T;
	}
}

public enum UnitType
{
	_NONE,
	Player,
	NormalEnemy,
	BossEnemy,
	TreasureBox,
	Pet,
}
