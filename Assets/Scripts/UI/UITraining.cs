using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITraining : MonoBehaviour
{
	[SerializeField] private UITrainingItem[] trainingItems;

	public void Init()
	{
		UpdateStatUpgrdadeUI();
	}

	private void UpdateStatUpgrdadeUI()
	{
		for (int i = 0; i < trainingItems.Length; i++)
		{
			var item = trainingItems[i];
			item.SetUI();
		}
	}

	void Test()
	{
		foreach (UserAbilityType abilityType in UserInfo.trainingTypes)
		{
			// 어빌리티 레벨
			int abilityLv = UserInfo.training.GetTrainingLevel(abilityType);
			bool isMaxLv = UserInfo.training.GetTrainingMaxLevel(abilityType) == abilityLv;
			float currentAbilityValue = UserInfo.training.GetTrainingValue(abilityType, abilityLv);
			float nextAbilityValue = UserInfo.training.GetTrainingValue(abilityType, abilityLv + 1);
			IdleNumber consume = UserInfo.training.GetTrainingLevelupConsumeCount(abilityType, abilityLv + 1);
		}
	}
}
