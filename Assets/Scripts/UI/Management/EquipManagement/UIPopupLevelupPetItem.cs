using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class UIPopupLevelupPetItem : UIPopupLevelupBaseItem<RuntimeData.PetInfo>
{
	[SerializeField] private UIPetSlot uiPetSlot;


	public override void OnUpdate(UIManagementEquip _parent, RuntimeData.PetInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;
		itemInfo = info;
		textMeshProName.text = itemInfo.itemObject.ItemName;

		uiPetSlot.OnUpdate(null, itemInfo, null);
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
	}

	public bool ItemLevelupable()
	{


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
