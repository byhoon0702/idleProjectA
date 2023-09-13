using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ShopContainer : BaseContainer
{
	public delegate void ShopEvent();
	public event ShopEvent ShopRefresh;

	[SerializeField] private List<RuntimeData.ShopInfo> packageList = new List<RuntimeData.ShopInfo>();
	[SerializeField] private List<RuntimeData.ShopInfo> salesList = new List<RuntimeData.ShopInfo>();
	[SerializeField] private List<RuntimeData.ShopInfo> diaList = new List<RuntimeData.ShopInfo>();
	[SerializeField] private List<RuntimeData.ShopInfo> normalList = new List<RuntimeData.ShopInfo>();
	[SerializeField] private List<RuntimeData.ShopInfo> adsList = new List<RuntimeData.ShopInfo>();
	[SerializeField] private List<RuntimeData.ShopInfo> battlePassList = new List<RuntimeData.ShopInfo>();


	public List<RuntimeData.ShopInfo> GetNormal(ShopType type)
	{
		switch (type)
		{
			case ShopType.PACKAGE: return packageList;
			case ShopType.SALE: return salesList;
			case ShopType.DIA: return diaList;
			case ShopType.NORMAL: return normalList;
			case ShopType.ADS: return adsList;
			case ShopType.BATTLEPASS: return battlePassList;
		}
		return new List<RuntimeData.ShopInfo>();
	}

	public override void Dispose()
	{

	}

	public override void FromJson(string json)
	{
		ShopContainer temp = CreateInstance<ShopContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref packageList, temp.packageList);
		LoadListTidMatch(ref salesList, temp.salesList);
		LoadListTidMatch(ref diaList, temp.diaList);
		LoadListTidMatch(ref normalList, temp.normalList);
		LoadListTidMatch(ref adsList, temp.adsList);
		LoadListTidMatch(ref battlePassList, temp.battlePassList);

	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		var sheet = DataManager.Get<ShopDataSheet>();
		SetListRawData(ref packageList, sheet.GetDatas(ShopType.PACKAGE));
		SetListRawData(ref salesList, sheet.GetDatas(ShopType.SALE));
		SetListRawData(ref diaList, sheet.GetDatas(ShopType.DIA));
		SetListRawData(ref normalList, sheet.GetDatas(ShopType.NORMAL));
		SetListRawData(ref adsList, sheet.GetDatas(ShopType.ADS));
		SetListRawData(ref battlePassList, sheet.GetDatas(ShopType.BATTLEPASS));

		PurchaseManager.Instance.Initialize();
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var shopItemObjects = Resources.LoadAll<ShopItemObject>("RuntimeDatas/ShopItems/Packages");
		AddDictionary(scriptableDictionary, shopItemObjects);
		shopItemObjects = Resources.LoadAll<ShopItemObject>("RuntimeDatas/ShopItems/Sales");
		AddDictionary(scriptableDictionary, shopItemObjects);
		shopItemObjects = Resources.LoadAll<ShopItemObject>("RuntimeDatas/ShopItems/Normals");
		AddDictionary(scriptableDictionary, shopItemObjects);
		shopItemObjects = Resources.LoadAll<ShopItemObject>("RuntimeDatas/ShopItems/Dias");
		AddDictionary(scriptableDictionary, shopItemObjects);
		shopItemObjects = Resources.LoadAll<ShopItemObject>("RuntimeDatas/ShopItems/Adss");
		AddDictionary(scriptableDictionary, shopItemObjects);

		//shopItemObjects = Resources.LoadAll<ShopItemObject>("RuntimeDatas/ShopItems/Events");
		//AddDictionary(scriptableDictionary, shopItemObjects);
	}
	public override void DailyResetData()
	{
		ResetList(packageList);
		ResetList(salesList);
		ResetList(diaList);
		ResetList(normalList);
		ResetList(adsList);
		ResetList(battlePassList);
	}

	public static void AddEvent(ShopEvent callback)
	{
		if (PlatformManager.UserDB == null || PlatformManager.UserDB.shopContainer == null)
		{
			return;
		}

		var container = PlatformManager.UserDB.shopContainer;
		if (container.ShopRefresh.IsRegistered(callback))
		{
			container.ShopRefresh -= callback;
		}
		container.ShopRefresh += callback;
	}

	public static void RemoveEvent(ShopEvent callback)
	{
		if (PlatformManager.UserDB == null || PlatformManager.UserDB.shopContainer == null)
		{
			return;
		}

		var container = PlatformManager.UserDB.shopContainer;
		if (container.ShopRefresh.IsRegistered(callback))
		{
			container.ShopRefresh -= callback;
		}
	}

	private void ResetList<T>(List<T> list) where T : RuntimeData.ShopInfo
	{
		for (int i = 0; i < list.Count; i++)
		{
			switch (list[i].LimitType)
			{
				case TimeLimitType.DAILY:
					list[i].DailyReset();
					break;
				case TimeLimitType.WEEKLY:
					list[i].WeeklyReset();
					break;
				case TimeLimitType.MONTHLY:
					list[i].MonthlyReset();
					break;
			}
		}
		ShopRefresh?.Invoke();
	}


	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;

	}

	public override void UpdateData()
	{
	}
}
