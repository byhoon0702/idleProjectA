using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILeftMenu : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI goldCount;
	private IdleNumber moneyCount = new IdleNumber(0, 0);

	[SerializeField] private Button openButton;
	[SerializeField] private Button closeButton;
	[SerializeField] private GameObject closedMenu;
	[SerializeField] private GameObject openMenu;

	[SerializeField] private Toggle[] toggleTab;
	[SerializeField] private GameObject[] listTabs;

	[SerializeField] private UITraining uiTraining;
	[SerializeField] private UIProperty uiProperty;

	private GameObject currentPage = null;

	public void Init()
	{
		openButton.onClick.RemoveAllListeners();
		openButton.onClick.AddListener(Open);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		SetToggles();

		uiTraining.Init();
		uiProperty.Init();
	}

	private void Open()
	{
		openMenu.SetActive(true);
		closedMenu.SetActive(false);
	}

	private void Close()
	{
		openMenu.SetActive(false);
		closedMenu.SetActive(true);
	}

	private void SetToggles()
	{
		for (int i = 0; i < toggleTab.Length; i++)
		{
			var index = i;
			var toggle = toggleTab[i];

			toggle.onValueChanged.AddListener((_isOn) =>
			{
				var text = toggle.GetComponentInChildren<TextMeshProUGUI>();
				if (_isOn == true)
				{
					text.color = Color.yellow;
				}
				else
				{
					text.color = Color.white;
				}

				if (currentPage != listTabs[index])
				{
					if (currentPage != null)
					{
						currentPage.gameObject.SetActive(false);
					}
					currentPage = listTabs[index];
					currentPage.gameObject.SetActive(true);
				}
			});
		}

		toggleTab[0].isOn = true;
	}

	private void UpdateGoldCount()
	{
		var currentMoneyCount = Inventory.it.ItemCount("gold");

		if (currentMoneyCount != moneyCount)
		{
			moneyCount = currentMoneyCount;
			goldCount.text = moneyCount.ToString();
		}
	}

	private void Update()
	{
		UpdateGoldCount();
	}
}
