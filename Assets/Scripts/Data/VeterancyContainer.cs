using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
[CreateAssetMenu(fileName = "Veterancy Container", menuName = "ScriptableObject/Container/Veterancy", order = 1)]
public class VeterancyContainer : BaseContainer
{
	public List<RuntimeData.VeterancyInfo> veterancyInfos;


	/// <summary>
	/// 서버에는 각 훈련의 레벨 정보만 저장
	/// </summary>
	public void LoadFromServer()
	{

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
		for (int i = 0; i < veterancyInfos.Count; i++)
		{
			if (i < temp.veterancyInfos.Count)
			{
				veterancyInfos[i].Load(temp.veterancyInfos[i]);
			}
		}
	}
	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();

		SetStatListRawData(ref veterancyInfos, DataManager.Get<VeterancyDataSheet>().GetInfosClone());
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

