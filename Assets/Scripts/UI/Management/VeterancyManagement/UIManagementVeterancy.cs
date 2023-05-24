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

	public void OnUpdate()
	{
		UpdateItem();
		UpdateMoney();
	}

	public void UpdateItem()
	{
		var list = GameManager.UserDB.veterancyContainer.veterancyInfos;
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
			UIItemVeterancy slot = child.GetComponent<UIItemVeterancy>();

			var info = list[i];
			slot.OnUpdate(this, info);
		}

	}

	public void UpdateMoney()
	{
		//propertyPointText.text = UserInfo.info.RemainPropertyPoint.ToString("N0");
	}

	public void OnResetButtonClick()
	{
		OnUpdate();
	}
}
