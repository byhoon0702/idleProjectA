using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class UIPopupLevelupPetItem : MonoBehaviour
{
	[SerializeField] private UIPetSlot uiPetSlot;

	[SerializeField] Image imageCurrency;
	[SerializeField] protected TextMeshProUGUI textMeshCurrency;
	[SerializeField] Image imageButtonCurrency;
	[SerializeField] protected TextMeshProUGUI textMeshButtonCurrency;

	[SerializeField] protected Button buttonExit;
	[SerializeField] protected Button buttonUpgrade;

	[SerializeField] protected TextMeshProUGUI textMeshProName;
	[SerializeField] protected TextMeshProUGUI textEquipBuff;
	[SerializeField] protected TextMeshProUGUI textOwnedBuff;

	private RuntimeData.PetInfo itemInfo;
	private UIManagementPet parent;

	protected virtual void Awake()
	{
		buttonExit.onClick.RemoveAllListeners();
		buttonExit.onClick.AddListener(OnClose);

		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClickLevelUp);
	}

	public void OnClose()
	{
		gameObject.SetActive(false);
	}
	public void OnUpdate(UIManagementPet _parent, RuntimeData.PetInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;
		itemInfo = info;
		textMeshProName.text = itemInfo.itemObject.ItemName;
		OnUpdateInfo();

	}

	public void OnUpdateInfo()
	{
		uiPetSlot.OnUpdate(null, itemInfo, null);
		UpdateItemLevelupInfo();

		var currencyItem = GameManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);
		textMeshCurrency.text = currencyItem.Value.ToString();
		imageCurrency.sprite = currencyItem.IconImage;
		imageButtonCurrency.sprite = currencyItem.IconImage;

		IdleNumber value = itemInfo.LevelUpNeedCount();
		textMeshButtonCurrency.text = value.ToString();

		if (value > currencyItem.Value)
		{
			textMeshButtonCurrency.color = Color.red;
		}
		else
		{
			textMeshButtonCurrency.color = Color.white;
		}


	}

	public void UpdateItemLevelupInfo()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < itemInfo.equipAbilities.Count; i++)
		{
			string tail = itemInfo.equipAbilities[i].tailChar;
			IdleNumber nextValue = itemInfo.equipAbilities[i].GetNextValue(itemInfo.level + 1);
			sb.Append($"{itemInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{itemInfo.equipAbilities[i].Value.ToString()}{tail}</color><color=green> > {nextValue.ToString()}{tail}</color>");
			sb.Append('\n');


		}
		textEquipBuff.text = sb.ToString();

		sb.Clear();
		for (int i = 0; i < itemInfo.ownedAbilities.Count; i++)
		{
			string tail = itemInfo.ownedAbilities[i].tailChar;
			IdleNumber nextValue = itemInfo.ownedAbilities[i].GetNextValue(itemInfo.level + 1);
			sb.Append($"{itemInfo.ownedAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{itemInfo.ownedAbilities[i].Value.ToString()}{tail}</color><color=green> > {nextValue.ToString()}{tail}</color>");
			sb.Append('\n');
		}
		textOwnedBuff.text = sb.ToString();
		//ownedBuffs[0].OnUpdate().text = $"{sb.ToString()}";
	}

	public void OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return;
		}


		var currencyitem = GameManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);

		if (currencyitem.Pay(itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.it.Enqueue("펫 먹이가 부족합니다");
			return;
		}

		GameManager.UserDB.petContainer.LevelUpPet(ref itemInfo);

		parent.OnUpdate(false);
		OnUpdateInfo();
	}

	public bool ItemLevelupable()
	{
		if (itemInfo.unlock == false)
		{
			return false;
		}
		if (itemInfo.CanLevelUp() == false)
		{
			ToastUI.it.Enqueue("최대 레벨입니다.");
			return false;
		}
		var currencyitem = GameManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);

		if (currencyitem == null)
		{
			return false;
		}

		if (currencyitem.Check(itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.it.Enqueue("펫 먹이가 부족합니다");
			return false;
		}

		return true;
	}

}
