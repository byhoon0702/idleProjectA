using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Unity.Serialization;

public class UIManagementAdvancement : UIBase
{
	public Sprite[] ImageGrade;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform itemRoot;

	private RuntimeData.AdvancementInfo currentInfo;
	private UIManagement parent;
	private UnitCostume unitCostume;

	public void Init(UIManagement _parent)
	{
		parent = _parent;
	}
	public Transform Find(int index)
	{
		return itemRoot.GetChild(index);
	}

	public void OnUpdate()
	{
		currentInfo = PlatformManager.UserDB.advancementContainer.Info;
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
		var list = PlatformManager.UserDB.advancementContainer.InfoList;

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

			child.gameObject.SetActive(true);
			UIItemAdvancement slot = child.GetComponent<UIItemAdvancement>();

			slot.OnUpdate(this, info);
		}
	}

}
