using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;



public delegate IdleNumber OnAbilityUpate(IdleNumber value);

[CreateAssetMenu(fileName = "Training Container", menuName = "ScriptableObject/Container/Training", order = 1)]
public class TrainingContainer : BaseContainer
{
	public List<RuntimeData.TrainingInfo> trainingInfos;
	public override void Dispose()
	{
		trainingInfos = null;
	}
	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();
		SetListRawData(ref trainingInfos, DataManager.Get<TrainingDataSheet>().GetInfosClone());
	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		TrainingContainer temp = CreateInstance<TrainingContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref trainingInfos, temp.trainingInfos);
	}

	public override void UpdateData()
	{
		for (int i = 0; i < trainingInfos.Count; i++)
		{
			trainingInfos[i].RemoveModifier(PlatformManager.UserDB);
			trainingInfos[i].AddModifier(PlatformManager.UserDB);
		}


	}
	public override void DailyResetData()
	{

	}
	public RuntimeData.TrainingInfo Find(StatsType type)
	{
		for (int i = 0; i < trainingInfos.Count; i++)
		{
			if (trainingInfos[i].type == type)
			{
				return trainingInfos[i];
			}
		}
		return null;
	}
	public RuntimeData.TrainingInfo Find(long tid)
	{
		for (int i = 0; i < trainingInfos.Count; i++)
		{
			if (trainingInfos[i].Tid == tid)
			{
				return trainingInfos[i];
			}
		}
		return null;
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var trainingList = Resources.LoadAll<TrainingItemObject>("RuntimeDatas/Trainings");
		AddDictionary(scriptableDictionary, trainingList);
	}
}
