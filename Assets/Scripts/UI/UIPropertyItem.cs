using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPropertyItem : MonoBehaviour
{
	[SerializeField] private AbilityType userAbilityType;

	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;

	public void SetUI()
	{
		textTitle.text = $"{UserInfo.GetAbiltyTitle(userAbilityType)} LV.{UserInfo.prop.GetCurrentPropertyLevel(userAbilityType)}";
		textCurrentStat.text = $"{UserInfo.prop.GetCurrentPropertyValue(userAbilityType).ToString("N1")}";

		// 최대 레벨일경우 다음 훈련 벨류는 비활성화하도록
		textNextStat.text = $"{UserInfo.prop.GetPropertyValue(userAbilityType, UserInfo.prop.GetCurrentPropertyLevel(userAbilityType) + 1).ToString("N1")}";

		textPrice.text = $"{UserInfo.prop.GetPropertyLevelupConsumeCount(userAbilityType)}";

		SetButton();
	}

	public void SetButton()
	{
		upgradeButton.repeatCallback = () => PropertyLevelUp();
	}

	private void PropertyLevelUp()
	{
		var cost = (IdleNumber)UserInfo.training.GetCurrentTrainingValue(userAbilityType);

		if (Inventory.it.CheckMoney("gold", cost).Fail())
		{
			return;
		}

		if (Inventory.it.ConsumeItem("gold", cost).Ok())
		{
			UserInfo.prop.LevelUpProperty(userAbilityType);
			SetUI();
		}
	}
}
