using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuffItemData : ItemData
{
	public long userBuffTid;
}

[Serializable]
public class BuffItemDataSheet : DataSheetBase<BuffItemData>
{

}
