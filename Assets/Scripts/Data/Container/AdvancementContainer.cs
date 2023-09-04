using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Advancement Container", menuName = "ScriptableObject/Container/Advancement Container", order = 1)]
[System.Serializable]
public class AdvancementContainer : BaseContainer
{

	[SerializeField] private List<RuntimeData.AdvancementInfo> _infoList;
	public List<RuntimeData.AdvancementInfo> InfoList => _infoList;

	[SerializeField] private int _advancementLevel;
	public int AdvancementLevel => _advancementLevel;

	private RuntimeData.AdvancementInfo _lastInfo;
	public bool IsDirty;
	public RuntimeData.AdvancementInfo Info
	{
		get
		{
			if (_lastInfo == null || _lastInfo.Tid == 0 || IsDirty)
			{
				for (int i = 0; i < _infoList.Count; i++)
				{
					if (_infoList[i].GotAdvancement == false)
					{
						break;
					}
					_lastInfo = _infoList[i];
				}
			}

			return _lastInfo;
		}
	}

	public override void Dispose()
	{

	}
	public override void FromJson(string json)
	{

		AdvancementContainer temp = CreateInstance<AdvancementContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListIndexMatch(ref _infoList, temp.InfoList);
		_advancementLevel = temp.AdvancementLevel;


	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		SetListRawData(ref _infoList, DataManager.Get<AdvancementDataSheet>().GetInfosClone());

	}
	public override void DailyResetData()
	{

	}
	public override void UpdateData()
	{
		for (int i = 0; i < InfoList.Count; i++)
		{
			InfoList[i].UpdateData();
		}

		RemoveModifiers(parent.UserStats);
		AddModifiers(parent.UserStats);
		StageManager.StageClearEvent += OnStageClear;
	}

	private void OnStageClear(RuntimeData.StageInfo stageInfo)
	{

		if (stageInfo.StageType != StageType.Youth)
		{
			return;
		}

		for (int i = 0; i < InfoList.Count; i++)
		{
			var _info = InfoList[i];
			if (_info.rawData.battleTid == stageInfo.stageData.dungeonTid && _info.rawData.stageNumber == stageInfo.StageNumber)
			{
				AddItemInfo itemInfo = new AddItemInfo(_info.Costume.Tid, (IdleNumber)1, RewardCategory.Costume);
				GameUIManager.it.AddContentOpenMessage(new ContentOpenMessage($"{PlatformManager.Language[_info.rawData.name]}", itemInfo));
				IsDirty = true;
				_info.Advancement();
				RemoveModifiers(parent.UserStats);
				AddModifiers(parent.UserStats);

				parent.costumeContainer.Buy(_info.Costume.Tid);
				break;
			}
		}
	}

	public void RemoveModifiers(UnitStats userStats)
	{
		if (Info == null)
		{
			return;
		}
		for (int i = 0; i < Info.AbilityList.Count; i++)
		{
			var stat = Info.AbilityList[i];
			userStats.RemoveModifier(stat.type, this);
		}
	}

	public void AddModifiers(UnitStats userStats)
	{
		if (Info == null)
		{
			return;
		}
		for (int i = 0; i < Info.AbilityList.Count; i++)
		{
			var stat = Info.AbilityList[i];
			userStats.AddModifier(stat.type, new StatsModifier((IdleNumber)stat.Value, StatModeType.Multi, this));
		}
	}

	public bool IsCompletedPreAdvancement(RuntimeData.AdvancementInfo info)
	{
		int index = InfoList.FindIndex(x => x == info);

		if (index < 0)
		{
			return false;
		}

		if (index == 0)
		{
			return true;
		}
		var prev = InfoList[index - 1];
		return prev.GotAdvancement;
	}

	//public void LevelUp(Unit owner, RuntimeData.AdvancementInfo info)
	//{
	//	int index = InfoList.FindIndex(x => x == info);

	//	if (index < 1)
	//	{
	//		return;
	//	}

	//	var prev = InfoList[index - 1];
	//	if (prev.GotAdvancement == false)
	//	{
	//		ToastUI.Instance.Enqueue("이전 승급 단계를 먼저 진행하세요.");
	//		return;
	//	}

	//	info.Advancement();

	//	RemoveModifiers(parent.UserStats);
	//	AddModifiers(parent.UserStats);

	//	(owner as PlayerUnit).UpdateMaxHp();
	//	owner.Heal(new HealInfo(owner.gameObject.layer, owner, owner.MaxHp));
	//}

	public void ChangeCostume(Unit owner)
	{
		//info.ChangeCostume(owner, _info);
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
}
