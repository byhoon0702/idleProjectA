using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITraining : MonoBehaviour
{
	[SerializeField] private UIItemTraining itemPrefab;
	[SerializeField] private Transform itemRoot;

	private List<UIItemTraining> items = new List<UIItemTraining>();


	private void OnEnable()
	{
		EventCallbacks.onItemChanged += OnItemChanged;
	}

	private void OnDisable()
	{
		EventCallbacks.onItemChanged -= OnItemChanged;
	}

	private void OnItemChanged(List<long> _changedItems)
	{
		foreach (var tid in _changedItems)
		{
			if (tid == Inventory.it.GoldTid)
			{
				UpdateItem(true);
			}
		}
	}

	public void OnUpdate(bool _refreshGrid)
	{
		UpdateItem(_refreshGrid);
	}

	public void UpdateItem(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemTraining>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Training))
			{
				UITrainingData uiData = new UITrainingData();
				var result = uiData.Setup(v);
				if(result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, uiData);
			}
		}


		foreach (var v in itemRoot.GetComponentsInChildren<UIItemTraining>())
		{
			v.OnRefresh();
		}
	}
}
