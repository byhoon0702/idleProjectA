using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
public class UIPopupLevelupEquipItem : UIPopupLevelupBaseItem<RuntimeData.EquipItemInfo>
{

	[SerializeField] private Image imageCost;
	[SerializeField] private TextMeshProUGUI textMeshCost;

	[SerializeField] private Image imageButtonCost;
	[SerializeField] private TextMeshProUGUI textMeshButtonCost;

	[SerializeField] private UIEquipSlot uiEquipSlot;


	public override void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;
		itemInfo = info;

		textMeshProName.text = itemInfo.itemObject.ItemName;
		OnUpdateInfo();


	}
	public void OnUpdateInfo()
	{
		uiEquipSlot.OnUpdate(null, itemInfo, null);
		UpdateItemLevelupInfo();
		var currencyitem = GameManager.UserDB.inventory.FindCurrency(CurrencyType.UPGRADE_ITEM);
		textMeshCost.text = currencyitem.Value.ToString();
		imageCost.sprite = currencyitem.IconImage;
		imageButtonCost.sprite = currencyitem.IconImage;

		IdleNumber value = itemInfo.LevelUpNeedCount();
		textMeshButtonCost.text = value.ToString();

		if (value > currencyitem.Value)
		{
			textMeshButtonCost.color = Color.red;
		}
		else
		{
			textMeshButtonCost.color = Color.white;
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

	}
	public override void OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return;
		}

		var currencyitem = GameManager.UserDB.inventory.FindCurrency(CurrencyType.UPGRADE_ITEM);

		if (currencyitem.Pay((IdleNumber)itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.it.Enqueue("강화석이 부족합니다");
			return;
		}

		GameManager.UserDB.equipContainer.LevelUpEquipItem(ref itemInfo);

		parent.OnUpdateEquip(itemInfo.type, itemInfo.Tid);
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
		var currencyitem = GameManager.UserDB.inventory.FindCurrency(CurrencyType.UPGRADE_ITEM);

		if (currencyitem == null)
		{
			return false;
		}

		if (currencyitem.Check(itemInfo.LevelUpNeedCount()) == false)
		{
			ToastUI.it.Enqueue("강화석이 부족합니다");
			return false;
		}

		return true;
	}

}
