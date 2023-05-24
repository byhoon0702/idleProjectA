using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
public class UIPopupLevelupEquipItem : UIPopupLevelupBaseItem<RuntimeData.EquipItemInfo>
{
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
	public override void OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return;
		}

		GameManager.UserDB.equipContainer.LevelUpEquipItem(ref itemInfo);

		parent.OnUpdateEquip(itemInfo.type, itemInfo.tid);
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
			return false;
		}
		//if (equipInfo == null && equipInfo.count == 0)
		//{
		//	return false;
		//}
		//if (equipInfo.CanLevelUp() == false)
		//{
		//	return false;
		//}
		//if (Inventory.it.CheckMoney(item.Tid, new IdleNumber(item.nextExp)).Fail())
		//{
		//	return false;
		//}

		return true;
	}

}
