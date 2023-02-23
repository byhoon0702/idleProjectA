using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIItemTraining : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;

	[SerializeField] private Image iconImage;

	private UITraining owner;
	private UITrainingData uiData;


	private void Awake()
	{
		upgradeButton.repeatCallback = () => AbilityLevelUp();
	}

	public void OnUpdate(UITraining _owner, UITrainingData _uiData)
	{
		owner = _owner;
		uiData = _uiData;

		UpdateIcon();
		UpdateLevelInfo();
		UpdateButton();
	}

	private void UpdateIcon()
	{
		iconImage.sprite = Resources.Load<Sprite>($"Icon/{uiData.itemData.Icon}");
	}

	public void OnRefresh()
	{
		UpdateLevelInfo();
		UpdateButton();
	}

	private void UpdateLevelInfo()
	{
		textTitle.text = uiData.TitleText;
		textCurrentStat.text = uiData.CurrentStatText;
		textNextStat.text = uiData.NextStatText;
		textPrice.text = uiData.LevelupCostText;
	}

	private void UpdateButton()
	{
		bool levelupable = uiData.Levelupable();
		upgradeButton.SetInteractable(levelupable);
	}

	private void AbilityLevelUp()
	{
		uiData.AbilityLevelUp(() =>
		{
			Inventory.it.abilityCalculator.GetCalculator(ItemType.Training).Calculate(Inventory.it.Items);
			Inventory.it.abilityCalculator.RefreshAbilityTotal();

			if (UnitManager.it.Player != null)
			{
				UnitManager.it.Player.PlayLevelupEffect();
			}
		});
	}
}
