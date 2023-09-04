using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Veterancy Container", menuName = "ScriptableObject/Container/Veterancy", order = 1)]
public class VeterancyContainer : BaseContainer
{
	public static event Action OnGetPoints;
	//[SerializeField] private int veterancyPoint;
	public IdleNumber VeterancyPoint => PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.VETERANCY_POINT).Value;
	public List<RuntimeData.VeterancyInfo> veterancyInfos;
	public override void Dispose()
	{

	}
	/// <summary>
	/// 서버에는 각 훈련의 레벨 정보만 저장
	/// </summary>
	public void LoadFromServer()
	{

	}
	public override void DailyResetData()
	{

	}


	public void ResetPoint()
	{
		IdleNumber point = (IdleNumber)0;
		for (int i = 0; i < veterancyInfos.Count; i++)
		{
			point += veterancyInfos[i].Level * veterancyInfos[i].cost;
			veterancyInfos[i].SetLevel(0);
			veterancyInfos[i].RemoveModifier(PlatformManager.UserDB);
			veterancyInfos[i].AddModifier(PlatformManager.UserDB);
		}
		PlatformManager.UserDB.UpdateUserStats();
		PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.VETERANCY_POINT).Earn(point);
	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void FromJson(string json)
	{
		VeterancyContainer temp = CreateInstance<VeterancyContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref veterancyInfos, temp.veterancyInfos);

		//veterancyPoint = temp.veterancyPoint;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < veterancyInfos.Count; i++)
		{
			veterancyInfos[i].RemoveModifier(PlatformManager.UserDB);
			veterancyInfos[i].AddModifier(PlatformManager.UserDB);
		}
	}
	public void AddVeterancyPoint()
	{
		if (parent.inventory.FindCurrency(CurrencyType.VETERANCY_POINT) != null)
		{
			parent.inventory.FindCurrency(CurrencyType.VETERANCY_POINT).Earn((IdleNumber)1);
		}

		OnGetPoints?.Invoke();
	}

	public bool Check(IdleNumber cost)
	{
		if (VeterancyPoint - cost < 0)
		{
			return false;
		}


		return true;
	}

	public bool ConsumePoint(IdleNumber cost)
	{
		return parent.inventory.FindCurrency(CurrencyType.VETERANCY_POINT).Pay(cost);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();

		SetListRawData(ref veterancyInfos, DataManager.Get<VeterancyDataSheet>().GetInfosClone());
	}

	public RuntimeData.VeterancyInfo Find(StatsType type)
	{
		for (int i = 0; i < veterancyInfos.Count; i++)
		{
			if (veterancyInfos[i].type == type)
			{
				return veterancyInfos[i];
			}
		}
		return null;
	}


	public void AddEvent(StatsType type, RuntimeData.VeterancyInfo.LevelUpDelegate delega)
	{
		Find(type).OnClickLevelup += delega;
	}
	public void RemoveEvent(StatsType type, RuntimeData.VeterancyInfo.LevelUpDelegate delega)
	{
		Find(type).OnClickLevelup -= delega;
	}

	public void OnClickLevelUp(StatsType type)
	{
		Find(type).ClickLevelup();
	}


	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var veterancylist = Resources.LoadAll<VeterancyObject>("RuntimeDatas/Veterancy");
		AddDictionary(scriptableDictionary, veterancylist);
	}
}

