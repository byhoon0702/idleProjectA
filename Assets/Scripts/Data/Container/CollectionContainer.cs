using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class CollectionReward
	{
		[SerializeField] private bool isGet;
		public bool IsGet => isGet;
		public int level { get; private set; }
		public RuntimeData.AbilityInfo reward { get; private set; }
		public CollectionReward(CollectionCondition data)
		{
			level = data.level;
			reward = new AbilityInfo(data.reward);
			isGet = false;
		}
		public CollectionReward()
		{

		}

		public void GetReward(bool isTrue)
		{
			isGet = isTrue;
		}

		public CollectionReward Clone()
		{
			CollectionReward temp = new CollectionReward();
			temp.level = level;
			temp.reward = reward;
			temp.isGet = isGet;
			temp.reward = reward.Clone();
			return temp;

		}
	}
	[System.Serializable]
	public class CollectionInfo : BaseInfo
	{
		public CollectionData rawData { get; private set; }
		public List<CollectionReward> rewardList;

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			CollectionInfo temp = info as CollectionInfo;
			if (temp.rewardList != null)
			{
				for (int i = 0; i < rewardList.Count; i++)
				{
					if (i < temp.rewardList.Count)
					{
						rewardList[i].GetReward(temp.rewardList[i].IsGet);
					}
				}
			}
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as CollectionData;
			tid = rawData.tid;

			rewardList = new List<CollectionReward>();
			for (int i = 0; i < rawData.rewards.Count; i++)
			{
				rewardList.Add(new CollectionReward(rawData.rewards[i]));
			}
		}

		public override void UpdateData()
		{
			base.UpdateData();
		}

		public void GetReward(CollectionReward reward)
		{
			var info = rewardList.Find(x => x.level == reward.level);
			info.GetReward(true);

			PlatformManager.UserDB.collectionContainer.UpdateData();
			PlatformManager.UserDB.Save();
		}
	}
}

public class CollectionContainer : BaseContainer
{
	[SerializeField] private List<RuntimeData.CollectionInfo> equipCollectionList;
	[SerializeField] private List<RuntimeData.CollectionInfo> skillCollectionList;
	[SerializeField] private List<RuntimeData.CollectionInfo> petCollectionList;

	public Dictionary<StatsType, RuntimeData.AbilityInfo> CollectionBuff { get; private set; } = new Dictionary<StatsType, RuntimeData.AbilityInfo>();

	public List<RuntimeData.CollectionInfo> this[CollectionTab tab]
	{
		get
		{
			switch (tab)
			{
				case CollectionTab.EQUIP: return equipCollectionList;
				case CollectionTab.SKILL: return skillCollectionList;
				case CollectionTab.PET: return petCollectionList;
			}
			return new List<RuntimeData.CollectionInfo>();
		}
	}

	private const string CollectionBuffKey = "CollectionBuff";
	public override void Dispose()
	{

	}
	public override void FromJson(string json)
	{
		CollectionContainer temp = CreateInstance<CollectionContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref equipCollectionList, temp.equipCollectionList);
		LoadListTidMatch(ref skillCollectionList, temp.skillCollectionList);
		LoadListTidMatch(ref petCollectionList, temp.petCollectionList);

	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();
		SetListRawData(ref equipCollectionList, DataManager.Get<CollectionDataSheet>().GetByType(CollectionTab.EQUIP));
		SetListRawData(ref skillCollectionList, DataManager.Get<CollectionDataSheet>().GetByType(CollectionTab.SKILL));
		SetListRawData(ref petCollectionList, DataManager.Get<CollectionDataSheet>().GetByType(CollectionTab.PET));
	}
	public override void DailyResetData()
	{

	}
	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}


	void AddCollectionBuff(List<RuntimeData.CollectionInfo> collectionList)
	{
		for (int i = 0; i < collectionList.Count; i++)
		{
			var collection = collectionList[i];
			for (int ii = 0; ii < collection.rewardList.Count; ii++)
			{
				var reward = collection.rewardList[ii];
				if (reward.IsGet)
				{

					if (CollectionBuff.ContainsKey(reward.reward.type))
					{
						CollectionBuff[reward.reward.type].AddModifiers(new StatsModifier(reward.reward.Value, StatModeType.Add));
					}
					else
					{
						CollectionBuff.Add(reward.reward.type, reward.reward.Clone());
					}
				}
			}
		}
	}
	public override void UpdateData()
	{
		CollectionBuff = new Dictionary<StatsType, RuntimeData.AbilityInfo>();
		AddCollectionBuff(equipCollectionList);
		AddCollectionBuff(skillCollectionList);
		AddCollectionBuff(petCollectionList);


		ApplyCollectionBuff();
	}

	public void ApplyCollectionBuff()
	{
		PlatformManager.UserDB.RemoveAllModifiers(CollectionBuffKey);
		foreach (var buff in CollectionBuff)
		{
			var ability = buff.Value;
			PlatformManager.UserDB.AddModifiers(ability.type, new StatsModifier(ability.Value, StatModeType.Buff, CollectionBuffKey));
		}
	}
}
