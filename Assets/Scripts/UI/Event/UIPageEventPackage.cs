using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageEventPackage : UIShopBase<RuntimeData.EventShopInfo>
{

	public override void OnUpdate(ShopType type)
	{
		currentType = type;
		var eventInfo = PlatformManager.UserDB.eventContainer.GetCurrentEvent();
		if (eventInfo != null)
		{
			infoList = eventInfo.Get(currentType);
		}
		else
		{
			infoList = new List<RuntimeData.EventShopInfo>();
		}
		SetGrid();
	}

	public override void Refresh()
	{
		var eventInfo = PlatformManager.UserDB.eventContainer.GetCurrentEvent();
		if (eventInfo != null)
		{
			infoList = eventInfo.Get(currentType);
		}
		else
		{
			infoList = new List<RuntimeData.EventShopInfo>();
		}
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
				UIListCellEventPackage packageItem = child.GetComponent<UIListCellEventPackage>();
				packageItem.OnUpdate(this, infoList[i]);
			}
		}

	}
}
