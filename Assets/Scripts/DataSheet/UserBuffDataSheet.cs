using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserBuffData : BaseData
{
	public string name;

	public int maxLevel;
}

[Serializable]
public class UserBuffDataSheet : DataSheetBase<UserBuffData>
{
}
