using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AdBuffData : BaseData
{
	public int maxLevel;
	public int BaseExp;
	public int ExpPerLevel;
	public ItemStats stats;
	public int duration;
}

[Serializable]
public class AdBuffDataSheet : DataSheetBase<AdBuffData>
{

}
