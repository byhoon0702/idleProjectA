using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementVeterancy : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI propertyPointText;
	[SerializeField] private UIItemVeterancy itemPrefab;
	[SerializeField] private Transform itemRoot;
	[SerializeField] private Button resetButton;




	private void Awake()
	{
		resetButton.onClick.RemoveAllListeners();
		resetButton.onClick.AddListener(OnResetButtonClick);
	}

	public void OnUpdate(bool _refreshGrid)
	{
		UpdateItem(_refreshGrid);
		UpdateMoney();
	}

	public void UpdateItem(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemVeterancy>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<PropertyItemDataSheet>().GetInfosClone())
			{
				UIVeterancyData uiData = new UIVeterancyData();
				VResult result = uiData.Setup(v);
				if (result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, uiData);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemVeterancy>())
		{
			v.OnRefresh();
		}
	}

	public void UpdateMoney()
	{
		propertyPointText.text = UserInfo.info.RemainPropertyPoint.ToString("N0");
	}

	public void OnResetButtonClick()
	{
		UserInfo.info.ResetProperty();
		UserInfo.SaveUserData();
		OnUpdate(true);
	}
}
