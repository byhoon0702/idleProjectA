using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIShopPackage : UIShopBase<RuntimeData.ShopInfo>
{

	public override void OnUpdate(ShopType type)
	{
		currentType = type;
		infoList = PlatformManager.UserDB.shopContainer.GetNormal(type);
		SetGrid();
	}
	public override void Refresh()
	{
		infoList = PlatformManager.UserDB.shopContainer.GetNormal(currentType);
		SetGrid();
	}
	protected override void SetGrid()
	{
		content.CreateListCell(infoList.Count, itemPrefab, null);
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < infoList.Count)
			{
				child.gameObject.SetActive(true);
				UIShopItemPackage packageItem = child.GetComponent<UIShopItemPackage>();
				packageItem.OnUpdate(this, infoList[i]);
			}
		}

	}
}
