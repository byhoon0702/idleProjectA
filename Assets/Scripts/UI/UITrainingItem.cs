using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITrainingItem : MonoBehaviour
{
	[SerializeField] private UserAbilityType userAbilityType;

	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;

	public void SetData(UserAbilityType _userAbilityType)
	{
		userAbilityType = _userAbilityType;
	}

	public void SetUI()
	{
		textTitle.text = $"{SetTitle(userAbilityType)} LV.{UserInfo.training.GetTrainingLevel(userAbilityType)}";
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
		if (Inventory.it.ConsumeItem(Inventory.it.GoldItem.tid, (IdleNumber)UserInfo.training.GetCurrentTrainingValue(userAbilityType)) == true)
		{
			UserInfo.training.LevelUpTraining(userAbilityType);
			SetUI();
		}
	}

	private string SetTitle(UserAbilityType _userAbilityType)
	{
		switch (_userAbilityType)
		{
			case UserAbilityType.AttackPower:
				return "공격력";
			case UserAbilityType.Hp:
				return "HP";
			case UserAbilityType.CriticalChance:
				return "치명타 확률";
			case UserAbilityType.CriticalAttackPower:
				return "치명타 피해";
			case UserAbilityType.GoldUp:
				return "골드 획득량";
			case UserAbilityType.ExpUp:
				return "경험치 획득량";
			case UserAbilityType.ItemUp:
				return "아이템 획득량";
			case UserAbilityType.MoveSpeed:
				return "이동속도";
			case UserAbilityType.AttackSpeed:
				return "공격속도";
			case UserAbilityType.SkillAttackPower:
				return "스킬피해";
			case UserAbilityType.BossAttackPower:
				return "보스피해";
			default:
				return "";
		}
	}
}
