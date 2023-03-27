using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuffLevelData : BaseData
{
	public int level;
	public long nextExp;
}

[Serializable]
public class BuffLevelDataSheet : DataSheetBase<BuffLevelData>
{

}
