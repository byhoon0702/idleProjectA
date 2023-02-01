using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITrainingItem : MonoBehaviour
{
	[SerializeField] private AbilityType userAbilityType;

	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;

	public void SetData(AbilityType _userAbilityType)
	{
		userAbilityType = _userAbilityType;
	}

	public void SetUI()
	{
		textTitle.text = $"{UserInfo.GetAbiltyTitle(userAbilityType)} LV.{UserInfo.training.GetTrainingLevel(userAbilityType)}";
		textCurrentStat.text = $"{UserInfo.training.GetCurrentTrainingValue(userAbilityType).ToString("N1")}";

		// 최대 레벨일경우 다음 훈련 벨류는 비활성화하도록
		textNextStat.text = $"{UserInfo.training.GetTrainingValue(userAbilityType, UserInfo.training.GetTrainingLevel(userAbilityType) + 1).ToString("N1")}";

		textPrice.text = $"{UserInfo.training.GetCurrentTrainingValue(userAbilityType)}";

		SetButton();
	}

	public void SetButton()
	{
		upgradeButton.repeatCallback = () => AbilityLevelUp();
	}

	private void AbilityLevelUp()
	{
		var cost = (IdleNumber)UserInfo.training.GetCurrentTrainingValue(userAbilityType);

		if(Inventory.it.CheckMoney("gold", cost).Fail())
		{
			return;
		}

		if (Inventory.it.ConsumeItem("gold", cost).Ok())
		{
			UserInfo.training.LevelUpTraining(userAbilityType);
			SetUI();
		}
	}
}
