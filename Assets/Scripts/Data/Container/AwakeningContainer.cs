using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Awakening Container", menuName = "ScriptableObject/Container/Awakening", order = 1)]
[System.Serializable]
public class AwakeningContainer : BaseContainer
{
	[SerializeField] private long selectedTid = 0;

	public bool HyperActivate { get; private set; }

	[SerializeField] private List<RuntimeData.AwakeningInfo> _infoList = new List<RuntimeData.AwakeningInfo>();
	public List<RuntimeData.AwakeningInfo> InfoList => _infoList;

	[SerializeField] private RuntimeData.AwakeningLevelInfo[] _runeInfoList = new RuntimeData.AwakeningLevelInfo[5];
	public RuntimeData.AwakeningLevelInfo[] RuneInfoList => _runeInfoList;

	public RuntimeData.AwakeningInfo SelectedInfo { get; private set; }


	public override void Dispose()
	{

	}
	public override void FromJson(string json)
	{
		AwakeningContainer temp = CreateInstance<AwakeningContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);


		LoadListTidMatch(ref _infoList, temp.InfoList);
		LoadListIndexMatch(ref _runeInfoList, temp.RuneInfoList);

		selectedTid = temp.selectedTid;

		UpdateSelectedInfo();

		UpdateHyperStat();
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();

		SetListRawData(ref _infoList, DataManager.Get<AwakeningDataSheet>().GetInfosClone());


		if (_runeInfoList == null)
		{
			_runeInfoList = new RuntimeData.AwakeningLevelInfo[5];
		}
		for (int i = 0; i < _infoList[0].RawData.awakeningLevels.Count; i++)
		{
			var levelData = _infoList[0].RawData.awakeningLevels[i];
			_runeInfoList[i] = new RuntimeData.AwakeningLevelInfo(levelData);
		}
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var awakeninglist = Resources.LoadAll<AwakeningItemObject>("RuntimeDatas/Awakening");
		AddDictionary(scriptableDictionary, awakeninglist);
		var hyperClassObjectList = Resources.LoadAll<HyperClassObject>("RuntimeDatas/HyperClass");
		AddDictionary(scriptableDictionary, hyperClassObjectList);
	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void UpdateData()
	{
		ContentsContainer.AddEvent(SetHyperActivate);
		for (int i = 0; i < InfoList.Count; i++)
		{
			InfoList[i].UpdateData();
		}
	}
	public override void DailyResetData()
	{

	}
	public void RuneLevelUp(RuntimeData.AwakeningLevelInfo info)
	{
		if (info.AddLevel() == false)
		{
			return;
		}

		UpdateHyperStat();

	}

	public bool CheckCanAwaken()
	{
		bool canAwaken = true;
		for (int i = 0; i < RuneInfoList.Length; i++)
		{
			if (RuneInfoList[i].IsMax() == false)
			{
				canAwaken = false;
				break;
			}
		}
		return canAwaken;
	}
	public bool CanBeAwaken(RuntimeData.AwakeningInfo info, out string message)
	{
		message = "";
		if (info.IsAwaken)
		{
			message = "이미 각성 완료 되었습니다.";
			return false;
		}



		bool canAwaken = true;

		var runeList = _runeInfoList;

		for (int i = 0; i < runeList.Length; i++)
		{
			if (runeList[i].CanIAwaken(info.RawData.awakeningLevels[i].MaxLevel) == false)
			{
				message = $"모든 룬이 {info.RawData.awakeningLevels[i].MaxLevel} 레벨을 달성해야 합니다.";
				canAwaken = false;
				break;
			}
		}

		int index = _infoList.FindIndex(x => x.Tid == info.Tid);
		index++;

		if (index > _infoList.Count)
		{
			message = "마지막 각성 단계입니다.";
			canAwaken = false;
		}

		return canAwaken;
	}

	public bool Awaken(RuntimeData.AwakeningInfo info)
	{
		if (CanBeAwaken(info, out string message) == false)
		{
			ToastUI.Instance.Enqueue(message);
			return false;
		}

		int index = _infoList.FindIndex(x => x.Tid == info.Tid);
		index++;
		long costumetid = 0;
		if (index < _infoList.Count)
		{
			costumetid = _infoList[index].RawData.costumeTid;
			RuntimeData.RewardInfo rewardInfo = new RuntimeData.RewardInfo(costumetid, RewardCategory.Costume);
			parent.AddRewards(new List<RuntimeData.RewardInfo>() { rewardInfo }, true);
		}

		info.Awaken();

		UpdateSelectedInfo();

		UpdateHyperStat();

		parent.costumeContainer.Equip(costumetid);
		//PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.LEVELUP_AWAKENING, info.Tid, (IdleNumber)1);
		return true;
	}


	public void UpdateSelectedInfo()
	{
		for (int i = 0; i < _infoList.Count; i++)
		{
			if (_infoList[i].IsAwaken == false)
			{
				SelectedInfo = _infoList[i];
				break;
			}
		}

		if (SelectedInfo == null)
		{
			SelectedInfo = _infoList[_infoList.Count - 1];
		}

		for (int i = 0; i < _runeInfoList.Length; i++)
		{
			_runeInfoList[i].Upgrade(SelectedInfo.RawData.awakeningLevels[i]);
		}
	}

	public void UpdateHyperStat()
	{
		parent.RemoveHyperAbility(this);
		parent.AddHyperAbilityInfo(SelectedInfo.AbilityInfos, this);

		for (int i = 0; i < RuneInfoList.Length; i++)
		{
			if (RuneInfoList[i].Ability != null)
			{
				parent.AddHyperAbilityInfo(RuneInfoList[i].Ability, this);
			}
		}
	}

	public void SetHyperActivate(List<ContentsInfo> infos)
	{
		HyperActivate = PlatformManager.UserDB.contentsContainer.IsOpen(ContentType.HERO_AWAKENING);
	}
}
