using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemProperty : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;


	private UIManagementProperty owner;
	private UIPropertyData uiData;



	private void Awake()
	{
		upgradeButton.repeatCallback = () => AbilityLevelUp();
	}

	public void OnUpdate(UIManagementProperty _owner, UIPropertyData _uiData)
	{
		owner = _owner;
		uiData = _uiData;

		icon.sprite = Resources.Load<Sprite>($"Icon/{uiData.Icon}");

		UpdateLevelInfo();
	}

	public void OnRefresh()
	{
		UpdateLevelInfo();
	}

	public void UpdateLevelInfo()
	{
		var sheet = DataManager.Get<UserPropertyDataSheet>();
		textTitle.text = uiData.ItemName;
		textCurrentStat.text = uiData.CurrentStatText;
		textNextStat.text = uiData.NextStatText;
		textPrice.text = uiData.CostText;

		bool levelupable = uiData.Levelupable();
		upgradeButton.SetInteractable(levelupable);
	}

	

	private void AbilityLevelUp()
	{
		uiData.LevelupItem(() =>
		{
			Inventory.it.abilityCalculator.GetCalculator(ItemType.Property).Calculate(Inventory.it.Items);
			Inventory.it.abilityCalculator.RefreshAbilityTotal();

			owner.UpdateItem(true);
			owner.UpdateMoney();
		});
	}
}
