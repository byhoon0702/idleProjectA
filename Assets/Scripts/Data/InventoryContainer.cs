using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory Container", menuName = "ScriptableObject/Container/Inventory", order = 1)]
[System.Serializable]
public class InventoryContainer : BaseContainer
{
	public int ddd;
	[SerializeField]
	public List<RuntimeData.CurrencyInfo> currencyList;

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		SetItemListRawData(ref currencyList, DataManager.Get<CurrencyDataSheet>().GetInfosClone());
	}


	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		InventoryContainer temp = CreateInstance<InventoryContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		for (int i = 0; i < currencyList.Count; i++)
		{
			if (i < temp.currencyList.Count)
			{
				currencyList[i].Load(temp.currencyList[i]);
			}

		}
	}
	public RuntimeData.CurrencyInfo FindCurrency(CurrencyType type)
	{
		for (int i = 0; i < currencyList.Count; i++)
		{
			if (currencyList[i].type == type)
			{
				return currencyList[i];
			}
		}
		return null;
	}


	public override void LoadScriptableObject()
	{

	}
}
