﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPetGrid : UIBaseGrid<RuntimeData.PetInfo>
{
	public override void Init(UIManagementEquip _parent)
	{
		parent = _parent;
	}
	public override void OnUpdate(List<RuntimeData.PetInfo> itemList)
	{
		int countForMake = itemList.Count - itemRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot);
			}
		}

		for (int i = 0; i < itemRoot.childCount; i++)
		{

			var child = itemRoot.GetChild(i);
			if (i >= itemList.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UIPetSlot slot = child.GetComponent<UIPetSlot>();

			var info = itemList[i];
			slot.OnUpdate(parent, info, () =>
			{
				parent.SelectPetItem(info.tid);
				parent.UpdateInfo();
			});
		}
	}
}
