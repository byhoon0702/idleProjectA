using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class UIPopupLevelupPetItem : UIBase
{
	[SerializeField] private UIPetSlot uiPetSlot;

	[SerializeField] protected Button buttonExit;
	[SerializeField] protected Button buttonMax;
	[SerializeField] protected UIEconomyButton buttonUpgrade;
	public UIEconomyButton ButtonUpgrade => buttonUpgrade;

	[SerializeField] protected TextMeshProUGUI textMeshProName;
	[SerializeField] protected TextMeshProUGUI textEquipBuff;
	[SerializeField] protected TextMeshProUGUI textOwnedBuff;

	private RuntimeData.PetInfo itemInfo;
	private UIManagementPet parent;

	protected virtual void Awake()
	{
		buttonExit.onClick.RemoveAllListeners();
		buttonExit.onClick.AddListener(OnClose);

		buttonMax.SetButtonEvent(OnClickMax);
		buttonUpgrade.SetButtonEvent(OnClickLevelUp, null);
	}

	private void OnClickMax()
	{
		ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_max_level"]);
	}

	public void OnUpdate(UIManagementPet _parent, RuntimeData.PetInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;
		itemInfo = info;
		textMeshProName.text = PlatformManager.Language[itemInfo.rawData.name];
		OnUpdateInfo();

	}

	public void OnUpdateInfo()
	{
		uiPetSlot.OnUpdate(null, itemInfo, null);
		UpdateItemLevelupInfo();

		var currencyItem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);

		IdleNumber value = itemInfo.LevelUpNeedCount();


		bool isMax = itemInfo.IsMaxLevel();

		buttonUpgrade.gameObject.SetActive(!isMax);
		buttonMax.gameObject.SetActive(isMax);
		buttonUpgrade.SetButton(currencyItem.IconImage, $"{currencyItem.Value.ToString()}/{value.ToString()}", value <= currencyItem.Value);
	}

	public void UpdateItemLevelupInfo()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < itemInfo.equipAbilities.Count; i++)
		{
			string tail = itemInfo.equipAbilities[i].tailChar;
			IdleNumber nextValue = itemInfo.equipAbilities[i].GetNextValue(itemInfo.Level + 1);
			sb.Append($"{itemInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{itemInfo.equipAbilities[i].GetValue(itemInfo.Level).ToString()}{tail}</color><color=green> > {nextValue.ToString()}{tail}</color>");
			sb.Append('\n');


		}
		textEquipBuff.text = sb.ToString();

		sb.Clear();
		for (int i = 0; i < itemInfo.ownedAbilities.Count; i++)
		{
			string tail = itemInfo.ownedAbilities[i].tailChar;
			IdleNumber nextValue = itemInfo.ownedAbilities[i].GetNextValue(itemInfo.Level + 1);
			sb.Append($"{itemInfo.ownedAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{itemInfo.ownedAbilities[i].GetValue(itemInfo.Level).ToString()}{tail}</color><color=green> > {nextValue.ToString()}{tail}</color>");
			sb.Append('\n');
		}
		textOwnedBuff.text = sb.ToString();
		//ownedBuffs[0].OnUpdate().text = $"{sb.ToString()}";
	}

	public bool OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return false;
		}


		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);

		if (currencyitem.Pay(itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.Instance.Enqueue("펫 먹이가 부족합니다");
			return false;
		}

		PlatformManager.UserDB.petContainer.LevelUpPet(ref itemInfo);

		parent.OnUpdate(false);
		OnUpdateInfo();
		return true;
	}

	public bool ItemLevelupable()
	{
		if (itemInfo.unlock == false)
		{
			return false;
		}
		if (itemInfo.IsMaxLevel())
		{
			ToastUI.Instance.Enqueue("최대 레벨입니다.");
			return false;
		}
		var currencyitem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);

		if (currencyitem == null)
		{
			return false;
		}

		if (currencyitem.Check(itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.Instance.Enqueue("펫 먹이가 부족합니다");
			return false;
		}

		return true;
	}

}
