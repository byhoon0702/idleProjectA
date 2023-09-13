
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class GachaInfo : ItemInfo
	{
		public override string ItemName => PlatformManager.Language[rawData.name];
		[SerializeField] private int viewAdsCount = 0;
		public int ViewAdsCount => viewAdsCount;
		[SerializeField] private int exp;
		public int Exp => exp;

		[SerializeField] private int rewardIndex;
		public int RewardIndex => rewardIndex;

		public GachaData rawData { get; private set; }

		public override Sprite IconImage => itemObject != null ? itemObject.ItemIcon : null;
		public GachaObject itemObject { get; private set; }


		public GachaDataSummonInfo gacha10 { get; private set; }
		public GachaDataSummonInfo gacha30 { get; private set; }
		public GachaDataSummonInfo gachaAds { get; private set; }

		public GachaLevelInfo currentLevelInfo { get; private set; }
		public int MaxLevel { get; private set; }

		public event System.Action OnLevelUp;

		private struct InternalChance
		{
			public Grade grade;
			public int chance;

		}
		private class GachaResult
		{
			public long tid;
			public int count;
			public Grade grade;
			public RewardCategory category;
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);

			GachaInfo gacha = info as GachaInfo;
			exp = gacha.exp;

			viewAdsCount = gacha.viewAdsCount;
			rewardIndex = gacha.rewardIndex;
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as GachaData;
			tid = rawData.tid;
			exp = 0;
			_level = 0;
			for (int i = 0; i < rawData.summonInfos.Length; i++)
			{
				if (rawData.summonInfos[i].summonType == GachaButtonType.Gacha10)
				{
					gacha10 = rawData.summonInfos[i];
				}
				if (rawData.summonInfos[i].summonType == GachaButtonType.Gacha100)
				{
					gacha30 = rawData.summonInfos[i];
				}
				if (rawData.summonInfos[i].summonType == GachaButtonType.Ads)
				{
					gachaAds = rawData.summonInfos[i];
				}
			}

			itemObject = PlatformManager.UserDB.gachaContainer.GetScriptableObject<GachaObject>(tid);

			MaxLevel = rawData.gachaLevelInfos[rawData.gachaLevelInfos.Count - 1].level;
		}


		public override void UpdateData()
		{
			if (currentLevelInfo == null || currentLevelInfo.level != _level)
			{
				if (_level >= rawData.gachaLevelInfos.Count)
				{
					_level = rawData.gachaLevelInfos.Count;
				}
				currentLevelInfo = rawData.gachaLevelInfos.Find(x => x.level == _level);
			}

			if (currentLevelInfo == null)
			{
				_level = 0;
			}
		}

		public void Reset()
		{
			viewAdsCount = 0;
		}

		public bool CanGetReward()
		{

			if (rewardIndex + 1 >= rawData.gachaLevelInfos.Count)
			{
				return false;
			}
			if (rewardIndex + 1 >= _level)
			{
				return false;
			}

			if (currentLevelInfo != null && currentLevelInfo.reward.tid == 0)
			{
				return false;
			}

			return true;
		}

		public void OnReceiveReward()
		{
			if (rewardIndex >= rawData.gachaLevelInfos.Count)
			{
				return;
			}
			if (rewardIndex >= _level)
			{
				return;
			}

			var gachaReward = rawData.gachaLevelInfos[rewardIndex];
			if (gachaReward == null)
			{
				return;
			}
			if (gachaReward.reward.tid == 0)
			{
				rewardIndex++;
				return;
			}
			List<RewardInfo> rewardList = new List<RewardInfo>();
			RewardInfo info = new RewardInfo(gachaReward.reward);

			info.UpdateCount();
			rewardList.Add(info);

			rewardIndex++;

			PlatformManager.UserDB.AddRewards(rewardList, true, true);
			OnLevelUp?.Invoke();

		}


		private void OnInternalUpdateExp(int _exp)
		{
			if (_level >= MaxLevel)
			{
				return;
			}
			exp += _exp;

			if (currentLevelInfo != null && exp >= currentLevelInfo.exp)
			{
				_level++;
				exp -= currentLevelInfo.exp;
			}

			UpdateData();
		}

		public void RollingGacha(GachaDataSummonInfo info)
		{
			int count = info.defaultCount;

			if (info.summonType == GachaButtonType.Ads)
			{
				count = Mathf.Min(info.defaultCount + viewAdsCount, info.maxCount);
			}

			Dictionary<long, GachaResult> gachaResults = new Dictionary<long, GachaResult>();
			for (int i = 0; i < count; i++)
			{
				OnInternalUpdateExp(1);
				var result = InternalRollingGacha();
				if (gachaResults.ContainsKey(result.tid))
				{
					long tid = result.tid;
					gachaResults[tid].count += result.count;
				}
				else
				{
					gachaResults.Add(result.tid, result);
				}
			}

			List<RewardInfo> rewardList = new List<RewardInfo>();
			foreach (var result in gachaResults.Values)
			{
				RewardInfo reward = new RewardInfo(result.tid, result.category, result.grade, result.count);
				rewardList.Add(reward);
			}
			OnLevelUp?.Invoke();
			PlatformManager.UserDB.AddRewards(rewardList, true);
			QuestGoalType questType = QuestGoalType.SUMMON_EQUIP;
			switch (rawData.gachaType)
			{
				case GachaType.Equip:
					questType = QuestGoalType.SUMMON_EQUIP;
					break;
				case GachaType.Pet:
					questType = QuestGoalType.SUMMON_PET;
					break;
				case GachaType.Skill:
					questType = QuestGoalType.SUMMON_SKILL;
					break;
				case GachaType.Relic:
					questType = QuestGoalType.SUMMON_RELIC;
					break;

			}
			PlatformManager.UserDB.questContainer.ProgressAdd(questType, 0, (IdleNumber)count);

		}

		private GachaResult InternalRollingGacha()
		{
			GachaResult result = new GachaResult();

			List<InternalChance> internalChances = new List<InternalChance>();
			for (int i = 0; i < rawData.chances.Count; i++)
			{
				if (rawData.chances[i].grade == Grade.SSS)
				{
					continue;
				}
				InternalChance ic = new InternalChance();
				ic.grade = rawData.chances[i].grade;
				if (_level < rawData.chances[i].chances.Count)
				{
					ic.chance = rawData.chances[i].chances[_level];
				}

				internalChances.Add(ic);
			}

			int chance = RandomLogic.Gacha.Get(1);
			int low = 0;
			Grade winGrade = Grade.D;
			for (int i = 0; i < internalChances.Count; i++)
			{
				var internalChance = internalChances[i];
				if (internalChance.chance == 0)
				{
					continue;
				}
				if (low <= chance && chance < low + internalChance.chance)
				{
					winGrade = internalChance.grade;
					break;
				}
				else
				{
					low += internalChance.chance;
				}
			}

			int winStar = 1;
			chance = RandomLogic.Gacha.Get(0, 100);
			low = 0;
			for (int i = 0; i < PlatformManager.UserDB.gachaContainer.gachaStarChance.Length; i++)
			{
				if (low <= chance && chance < low + PlatformManager.UserDB.gachaContainer.gachaStarChance[i])
				{
					winStar = i + 1;
					break;
				}
				else
				{
					low += PlatformManager.UserDB.gachaContainer.gachaStarChance[i];
				}
			}

			switch (rawData.gachaType)
			{
				case GachaType.Equip:
					{
						EquipType type = (EquipType)Random.Range((int)EquipType.WEAPON, (int)EquipType._END);
						var list = DataManager.Get<EquipItemDataSheet>().GetByItemType(type).FindAll(x => x.itemGrade == winGrade);
						var reward = list.Find(x => x.starLevel == winStar);

						if (reward == null)
						{
							reward = list[Random.Range(0, list.Count)];
						}
						result.tid = reward.tid;
						result.count += 1;

						result.grade = reward.itemGrade;
						result.category = RewardCategory.Equip;
					}
					break;
				case GachaType.Pet:
					{
						var list = DataManager.Get<PetDataSheet>().GetInfosClone().FindAll(x => x.itemGrade == winGrade);
						if (list.Count == 0)
						{
							return null;
						}

						var reward = list[Random.Range(0, list.Count)];

						result.tid = reward.tid;
						result.count += 1;
						result.grade = reward.itemGrade;
						result.category = RewardCategory.Pet;
					}
					break;
				case GachaType.Relic: /*임시*/
					{
						var list = DataManager.Get<RelicItemDataSheet>().GetInfosClone().FindAll(x => x.itemGrade == winGrade);
						if (list.Count == 0)
						{
							return null;
						}
						var reward = list[Random.Range(0, list.Count)];

						result.tid = reward.tid;
						result.count += 1;
						result.grade = reward.itemGrade;
						result.category = RewardCategory.Relic;
					}
					break;
				case GachaType.Skill:
					{
						var list = DataManager.Get<SkillDataSheet>().GetInfosClone().FindAll(x => x.itemGrade == winGrade);
						var sublist = list.FindAll(x => x.hideInUI == false);
						if (sublist.Count == 0)
						{
							return null;
						}
						var reward = sublist[Random.Range(0, sublist.Count)];

						result.tid = reward.tid;
						result.count += 1;
						result.grade = reward.itemGrade;
						result.category = RewardCategory.Skill;
					}
					break;
			}
			return result;
		}

		public void OnClickGacha(GachaButtonType type)
		{
			var summonInfo = gacha10;
			switch (type)
			{
				case GachaButtonType.Gacha100:
					summonInfo = gacha30;
					break;
				case GachaButtonType.Ads:
					summonInfo = gachaAds;
					break;
			}

			//var currency = PlatformManager.UserDB.inventory.FindCurrency(summonInfo.itemTid);
			//if (currency.Value < summonInfo.cost)
			//{
			//	return;
			//}

			//currency.Pay((IdleNumber)summonInfo.cost);
			RollingGacha(summonInfo);
		}


		public void OnClickGachaAds()
		{
			viewAdsCount++;
		}
	}
}


