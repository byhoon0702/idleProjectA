using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementVeterancy : UIBase
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

	protected override void OnEnable()
	{
		base.OnEnable();
		VeterancyContainer.OnGetPoints += OnUpdate;
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		VeterancyContainer.OnGetPoints -= OnUpdate;
	}

	public void OnUpdate()
	{
		UpdateItem();
		UpdateMoney();
	}

	public void UpdateItem()
	{
		var list = PlatformManager.UserDB.veterancyContainer.veterancyInfos;
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

	public UIItemVeterancy Find(StatsType type)
	{
		for (int i = 0; i < itemRoot.childCount; i++)
		{
			var itemTraining = itemRoot.GetChild(i).GetComponent<UIItemVeterancy>();
			if (itemTraining.VeterancyInfo.type == type)
			{
				return itemTraining;
			}
		}
		return null;
	}
	public void UpdateMoney()
	{
		propertyPointText.text = PlatformManager.UserDB.veterancyContainer.VeterancyPoint.ToString();
	}

	public void OnResetButtonClick()
	{
		PlatformManager.UserDB.veterancyContainer.ResetPoint();
		OnUpdate();
	}
}
