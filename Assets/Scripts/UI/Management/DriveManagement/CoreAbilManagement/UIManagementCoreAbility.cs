using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementCoreAbility : MonoBehaviour
{
	[SerializeField] private UICoreAbilityProbabilityPopup probabilityPopup;
	[SerializeField] private UIItemCoreAbilitySlot[] items;
	[SerializeField] private Button helpButton;
	[SerializeField] private Button randomAbilButton;
	[SerializeField] private TextMeshProUGUI costText;
	[SerializeField] private TextMeshProUGUI currentItemCountText;




	private void Awake()
	{
		helpButton.onClick.RemoveAllListeners();
		helpButton.onClick.AddListener(OnHelpButtonClick);

		randomAbilButton.onClick.RemoveAllListeners();
		randomAbilButton.onClick.AddListener(OnRandomAbilityButtonClick);
	}

	public void OnUpdate()
	{
		int index = 0;
		foreach(Grade grade in Enum.GetValues(typeof(Grade)))
		{
			items[index].OnUpdate(this, grade);
			items[index].SetLock(UserInfo.coreAbil.GetAbilityLock(grade));

			index++;
		}

		UpdateButton();
		UpdateItemCount();
	}

	public void UpdateItemCount()
	{
		currentItemCountText.text = Inventory.it.ItemCount("coreabilitypoint").ToString();
	}

	public void UpdateButton()
	{
		IdleNumber cost = GetCost();
		VResult checkMoneyResult = Inventory.it.CheckMoney("coreabilitypoint", cost);
		bool isAllLock = true;

		foreach (var v in items)
		{
			if (v.NoPossessed)
			{
				continue;
			}

			if (v.IsLock)
			{
				continue;
			}

			isAllLock = false;
			break;
		}

		randomAbilButton.interactable = checkMoneyResult.Ok() && (isAllLock == false);
		costText.text = $"{cost.ToString()}";
	}

	private void OnHelpButtonClick()
	{
		probabilityPopup.gameObject.SetActive(true);
		probabilityPopup.OnUpdate();
	}

	public IdleNumber GetCost()
	{
		int lockCount = 0;
		foreach (var v in items)
		{
			if (v.IsLock)
			{
				lockCount++;
			}
		}

		return new IdleNumber(UserInfo.coreAbil.ConsumeMedalCount(lockCount));
	}

	private void OnRandomAbilityButtonClick()
	{
		IdleNumber cost = GetCost();
		if (Inventory.it.ConsumeItem("coreabilitypoint", cost).Fail())
		{
			return;
		}


		for (int i = 0; i<items.Length ; i++)
		{
			if (items[i].NoPossessed)
			{
				continue;
			}

			if (items[i].IsLock)
			{
				continue;
			}

			UserInfo.coreAbil.SetRandomAbility(items[i].SlotGrade);
		}

		OnUpdate();
	}
}
