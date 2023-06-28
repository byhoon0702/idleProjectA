using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class UIManagementAdvancement : UIBase
{

	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform itemRoot;

	private UnitAdvancementInfo currentInfo;
	private UIManagement parent;
	private UnitCostume unitCostume;


	public void Init(UIManagement _parent)
	{
		parent = _parent;
	}

	public void OnUpdate()
	{
		currentInfo = GameManager.UserDB.advancementContainer.Info.advancementInfo;
		UpdateInfo();

	}


	public void Refresh()
	{
		for (int i = 0; i < itemRoot.childCount; i++)
		{
			var child = itemRoot.GetChild(i);
			if (child.gameObject.activeInHierarchy == false)
			{
				continue;
			}

			UIItemAdvancement slot = child.GetComponent<UIItemAdvancement>();

			slot.UpdateInfo();
		}
	}

	public void UpdateInfo()
	{
		var list = GameManager.UserDB.advancementContainer.Info.rawData.upgradeInfoList;

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

			var info = list[i];
			if (info.level == 0)
			{
				continue;
			}
			child.gameObject.SetActive(true);
			UIItemAdvancement slot = child.GetComponent<UIItemAdvancement>();

			slot.OnUpdate(this, info);
		}
	}

}
