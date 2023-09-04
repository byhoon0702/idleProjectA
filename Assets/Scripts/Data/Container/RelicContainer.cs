using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Relic Container", menuName = "ScriptableObject/Container/Relic", order = 1)]
public class RelicContainer : BaseContainer
{
	[SerializeField] private List<RuntimeData.RelicInfo> relicInfos = new List<RuntimeData.RelicInfo>();
	public List<RuntimeData.RelicInfo> RelicInfos => relicInfos;
	public static event Action OnLevelUp;

	public override void Dispose()
	{

	}
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void FromJson(string json)
	{
		RelicContainer temp = CreateInstance<RelicContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref relicInfos, temp.relicInfos);
	}

	public override void UpdateData()
	{
		for (int i = 0; i < relicInfos.Count; i++)
		{
			relicInfos[i].RemoveModifiers(PlatformManager.UserDB);
			relicInfos[i].AddModifiers(PlatformManager.UserDB);
		}
	}


	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();

		SetListRawData(ref relicInfos, DataManager.Get<RelicItemDataSheet>().GetInfosClone());
	}

	public RuntimeData.RelicInfo Find(long tid)
	{
		for (int i = 0; i < relicInfos.Count; i++)
		{
			if (relicInfos[i].Tid == tid)
			{
				return relicInfos[i];
			}
		}
		return null;
	}

	public void AddItem(long _tid, int _count)
	{
		Find(_tid).AddItem(_count);
	}


	public void OnClickLevelUp(long _tid)
	{
		Find(_tid).OnLevelUp();
	}

	public override void DailyResetData()
	{

	}
	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var reliclist = Resources.LoadAll<RelicItemObject>("RuntimeDatas/RelicItems");
		AddDictionary(scriptableDictionary, reliclist);
	}
}
