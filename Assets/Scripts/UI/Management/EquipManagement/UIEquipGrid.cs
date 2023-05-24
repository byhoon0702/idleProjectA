﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipGrid : UIBaseGrid<RuntimeData.EquipItemInfo>

{
	public override void Init(UIManagementEquip _parent)
	{
		parent = _parent;
	}

	public override void OnUpdate(List<RuntimeData.EquipItemInfo> itemList)
	{
		var list = itemList;
		int countForMake = list.Count - itemRoot.childCount;

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
			if (i > list.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UIEquipSlot slot = child.GetComponent<UIEquipSlot>();

			var info = list[i];
			slot.OnUpdate(parent, info, () =>
			{
				parent.SelectEquipItem(info.tid);
				parent.UpdateInfo();
			});
		}
	}
}
