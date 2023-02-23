using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UIManagementRelic : MonoBehaviour
{
	[Header("아이템리스트")]
	[SerializeField] private UIItemRelic itemPrefab;
	[SerializeField] private RectTransform itemRoot;

	[Header("Button")]
	[SerializeField] private Button upgradeAllButton;


	private void Awake()
	{
		upgradeAllButton.onClick.RemoveAllListeners();
		upgradeAllButton.onClick.AddListener(OnUpgradeAllButtonClick);
	}

	public void OnUpdate(bool _refreshGrid)
	{
		UpdateItems(_refreshGrid);
		UpdateButton();
	}

	public void UpdateItems(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemRelic>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Relic))
			{
				UIRelicData uiData = new UIRelicData();
				VResult result = uiData.Setup(v);
				if(result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(uiData);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemRelic>())
		{
			v.OnRefresh();
		}
	}

	public void UpdateButton()
	{
		upgradeAllButton.interactable = false;
	}

	private void OnUpgradeAllButtonClick()
	{

	}
}
