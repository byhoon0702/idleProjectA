﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VeterancyItemData : ItemData
{
	public long userPropertyTid;
	public List<StatsValue> ownValues;
}

[Serializable]
public class PropertyItemDataSheet : DataSheetBase<VeterancyItemData>
{


}
