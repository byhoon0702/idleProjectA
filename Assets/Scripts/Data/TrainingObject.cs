using System;
using System.Collections.Generic;
using UnityEngine;



public delegate IdleNumber OnAbilityUpate(IdleNumber value);

[CreateAssetMenu]
public class TrainingObject : ScriptableObject
{
	[NonSerialized]
	public List<RuntimeData.TrainingInfo> trainingInfos;

	private UserDB parent;
	/// <summary>
	/// 서버에는 각 훈련의 레벨 정보만 저장
	/// </summary>
	public void LoadFromServer()
	{

	}

	public void InitTrainingInfo(UserDB _parent)
	{
		parent = _parent;
		trainingInfos = new List<RuntimeData.TrainingInfo>();
		var list = DataManager.Get<TrainingDataSheet>().GetInfosClone();

		for (int i = 0; i < list.Count; i++)
		{
			RuntimeData.TrainingInfo info = new RuntimeData.TrainingInfo(list[i], 1);
			trainingInfos.Add(info);
		}
	}

	public RuntimeData.TrainingInfo Find(Ability type)
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


	public void AddEvent(Ability type, RuntimeData.TrainingInfo.TrainingInfoDelegate delega)
	{
		Find(type).OnClickLevelup += delega;
	}
	public void RemoveEvent(Ability type, RuntimeData.TrainingInfo.TrainingInfoDelegate delega)
	{
		Find(type).OnClickLevelup -= delega;
	}

	public void OnClickLevelUp(Ability type)
	{
		Find(type).Click();
	}
}
