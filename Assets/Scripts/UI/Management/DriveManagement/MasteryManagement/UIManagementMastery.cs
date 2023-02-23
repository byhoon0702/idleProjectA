using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementMastery : MonoBehaviour
{
	[SerializeField] private Button resetButton;
	[SerializeField] private TextMeshProUGUI corePointText;

	[Header("아이템리스트")]
	[SerializeField] private UIItemMasterySlotGroup itemPrefab;
	[SerializeField] private RectTransform itemRoot;


	[Header("info")]
	[SerializeField] private GameObject infoRoot;
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI masteryTitleText;
	[SerializeField] private TextMeshProUGUI masteryAbilityText;
	[SerializeField] private TextMeshProUGUI masteryCostText;
	[SerializeField] private RepeatButton upgradeButton;

	private UIMasteryData selectedMasteryData;



	private void Awake()
	{
		upgradeButton.repeatCallback += OnUpgradeButtonClick;
		resetButton.onClick.RemoveAllListeners();
		resetButton.onClick.AddListener(OnResetButtonClick);
	}

	public void OnUpdate(bool _refreshGrid)
	{
		UpdateItems(_refreshGrid);
		UpdatePoint();
		ShowMasteryInfo(null);
	}

	public void UpdatePoint()
	{
		corePointText.text = $"{UserInfo.RemainMasteryPoint}";
	}

	public void UpdateItems(bool _refresh)
	{
		if (_refresh == false)
		{
			var sheet = DataManager.Get<UserMasteryDataSheet>();
			int lastStep = sheet.GetStepMax();

			foreach (var v in itemRoot.GetComponentsInChildren<UIItemMasterySlotGroup>())
			{
				Destroy(v.gameObject);
			}

			for(int i=0 ; i<lastStep ; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, i + 1);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemMasterySlotGroup>())
		{
			v.OnRefresh();
		}
	}


	public void ShowMasteryInfo(UIMasteryData _uiData)
	{
		if (_uiData == null)
		{
			infoRoot.SetActive(false);
		}
		else
		{
			infoRoot.SetActive(true);
			icon.sprite = Resources.Load<Sprite>($"Icon/{_uiData.Icon}");

			masteryTitleText.text = _uiData.MasteryName;
			masteryCostText.text = _uiData.ConsumePoint.ToString("N0");
			masteryAbilityText.text = _uiData.MasteryAbilityText;

			bool levelupable = _uiData.Levelupable();
			selectedMasteryData = _uiData;
			upgradeButton.SetInteractable(levelupable);
		}
	}

	

	private void OnUpgradeButtonClick()
	{
		selectedMasteryData.LevelupMastery(() => 
		{
			Inventory.it.abilityCalculator.GetCalculator(ItemType.Mastery).Calculate(Inventory.it.Items);
			Inventory.it.abilityCalculator.RefreshAbilityTotal();

			UpdateItems(true);
			UpdatePoint();

			ShowMasteryInfo(selectedMasteryData);
		});
	}

	public void OnResetButtonClick()
	{
		UserInfo.ResetMastery();
		OnUpdate(true);
	}
}
