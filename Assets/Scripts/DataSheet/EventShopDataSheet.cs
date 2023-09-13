using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EventShopDataSheet : DataSheetBase<ShopData>
{
	public List<ShopData> GetDatas(ShopType type)
	{
		return infos.FindAll(x => x.shopType == type);
	}

}
