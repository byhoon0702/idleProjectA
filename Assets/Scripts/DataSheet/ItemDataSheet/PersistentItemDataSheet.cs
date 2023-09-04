using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersistentItemData : BaseData
{
	public string resource;
}

[System.Serializable]
public class PersistentItemDataSheet : DataSheetBase<PersistentItemData>
{

}
