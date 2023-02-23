using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemMasterySlotGroup : MonoBehaviour
{
	[SerializeField] private UIItemMasterySlot[] slots;

	private UIManagementMastery owner;



	public void OnUpdate(UIManagementMastery _owner, int _step)
	{
		owner = _owner;

		var masteryList = DataManager.Get<UserMasteryDataSheet>().GetByStep(_step);
		var itemSheet = DataManager.Get<ItemDataSheet>();

		for (int i=0; i < slots.Length; i++)
		{
			if(masteryList.Count <= i)
			{
				slots[i].gameObject.SetActive(false);
			}
			else
			{
				UIMasteryData uiData = new UIMasteryData();
				VResult result = uiData.Setup(masteryList[i]);
				if(result.Fail())
				{
					slots[i].gameObject.SetActive(false);
					VLog.LogError(result.ToString());
					continue;
				}

				slots[i].gameObject.SetActive(true);
				slots[i].OnUpdate(owner, uiData);
			}
		}
	}

	public void OnRefresh()
	{
		for (int i = 0 ; i < slots.Length ; i++)
		{
			if (slots[i].gameObject.activeInHierarchy)
			{
				slots[i].OnRefresh();
			}
		}
	}
}
