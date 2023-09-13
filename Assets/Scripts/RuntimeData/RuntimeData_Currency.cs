using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;

namespace RuntimeData
{
	[System.Serializable]
	public class CurrencyInfo : ItemInfo
	{
		public override string ItemName => PlatformManager.Language[rawData.name];
		[SerializeField] private IdleNumber value;
		public IdleNumber Value => value;
		public IdleNumber max { get; private set; }
		public IdleNumber refill { get; private set; }
		public CurrencyType type { get; private set; }
		public CurrencyData rawData { get; private set; }

		public override Sprite IconImage => itemObject != null ? itemObject.ItemIcon : null;
		public CurrencyItemObject itemObject { get; private set; }
		public CurrencyInfo()
		{

		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);


			CurrencyInfo currencyInfo = info as CurrencyInfo;

			value = currencyInfo.value;
			value.NormalizeSelf();
			value.Turncate();
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as CurrencyData;
			tid = rawData.tid;
			value = (IdleNumber)0;
			max = (IdleNumber)rawData.maxValue;
			refill = (IdleNumber)rawData.refillValue;
			type = rawData.type;

			itemObject = PlatformManager.UserDB.inventory.GetScriptableObject<CurrencyItemObject>(tid);
		}


		public bool Check(int cost)
		{
			return Check((IdleNumber)cost);
		}
		public bool Check(IdleNumber cost)
		{
			if (value - cost < 0)
			{
				return false;
			}

			return true;
		}

		public bool Pay(int cost)
		{
			return Pay((IdleNumber)cost);
		}
		public bool Pay(IdleNumber cost)
		{
			if (Check(cost) == false)
			{
				return false;
			}

			value -= cost;
			value.NormalizeSelf();
			EventCallbacks.CallCurrencyChanged(type);
			return true;
		}

		public bool Earn(IdleNumber money)
		{
			if (value + money > max)
			{
				value = max;
				return true;
			}

			value += money;
			value.NormalizeSelf();
			EventCallbacks.CallCurrencyChanged(type);
			return true;
		}
		public void ResetCount(IdleNumber money)
		{
			if (value >= money)
			{
				return;
			}
			value = money;
			value.NormalizeSelf();
			EventCallbacks.CallCurrencyChanged(type);
		}
	}

	[System.Serializable]
	public class RewardBoxInfo : ItemInfo
	{
		public override string ItemName => PlatformManager.Language[rawData.name];
		public List<RewardInfo> rewardInfo;

		public override Sprite IconImage => itemObject != null ? itemObject.ItemIcon : null;
		public RewardBoxData rawData { get; private set; }

		public RewardBoxItemObject itemObject { get; private set; }
		public RewardBoxInfo()
		{ }
		public RewardBoxInfo(long _tid)
		{
			rawData = DataManager.Get<RewardBoxDataSheet>().Get(_tid);
			tid = rawData.tid;

			itemObject = PlatformManager.UserDB.inventory.GetScriptableObject<RewardBoxItemObject>(tid);

		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as RewardBoxData;
			tid = rawData.tid;

			itemObject = PlatformManager.UserDB.inventory.GetScriptableObject<RewardBoxItemObject>(tid);
		}

		public void SetReward()
		{
			rewardInfo = new List<RewardInfo>();
			for (int i = 0; i < rawData.rewards.Count; i++)
			{
				rewardInfo.Add(new RewardInfo(rawData.rewards[i]));
			}
		}
		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
		}

		public void AddRewardBox(int _count)
		{
			_count += _count;
		}

		public List<RewardInfo> Open()
		{
			List<RewardInfo> rewardInfos = new List<RewardInfo>();
			List<RewardInfo> result = new List<RewardInfo>();
			for (int i = 0; i < rewardInfo.Count; i++)
			{
				RuntimeData.RewardInfo reward = rewardInfo[i];
				reward.UpdateCount();
				rewardInfos.Add(reward);

				result = RewardUtil.RandomReward(rewardInfos, RandomLogic.RewardBox);

			}


			return result;
		}
	}


	[System.Serializable]
	public class RewardInfo
	{
		[SerializeField] private long tid;
		public long Tid => tid;
		[SerializeField] private RewardCategory category;
		public RewardCategory Category => category;
		[SerializeField] private float chance;
		public float Chance => chance;
		[SerializeField] private IdleNumber countMin;
		[SerializeField] private IdleNumber countMax;
		[SerializeField] private IdleNumber countPerStage;

		public int Star { get; private set; }
		public IdleNumber fixedCount { get; private set; }

		public Sprite iconImage { get; private set; }

		public Grade grade { get; private set; }

		private bool isFixed = false;
		public string name { get; private set; }



		public RewardInfo Clone()
		{
			RewardInfo clone = new RewardInfo();
			clone.tid = tid;
			clone.category = category;
			clone.chance = chance;
			clone.countMin = countMin;
			clone.countMax = countMax;
			clone.countPerStage = countPerStage;
			clone.iconImage = iconImage;
			clone.grade = grade;
			clone.name = name;
			clone.Star = Star;
			clone.fixedCount = fixedCount;
			return clone;
		}
		public RewardInfo()
		{

		}
		public RewardInfo(ChanceReward _reward)
		{
			tid = _reward.tid;
			category = _reward.category;
			chance = _reward.chance;

			countMin = (IdleNumber)_reward.countMin;
			countMax = (IdleNumber)_reward.countMax;
			countPerStage = (IdleNumber)_reward.countPerStage;

			iconImage = null;
			grade = Grade.D;
			isFixed = false;
			SetReward();
		}
		public RewardInfo(Reward _reward)
		{
			tid = _reward.tid;
			category = _reward.category;
			chance = 100;

			countMin = (IdleNumber)_reward.count;
			countMax = (IdleNumber)0;
			countPerStage = (IdleNumber)0;

			iconImage = null;
			grade = Grade.D;
			isFixed = false;
			SetReward();
		}

		/// <summary>
		/// 웬만하면 가챠 결과로만 사용할것 
		/// </summary>
		/// <param name="_tid"></param>
		/// <param name="_category"></param>
		/// <param name="_grade"></param>
		/// <param name="_count"></param>
		/// 
		public RewardInfo(long _tid, RewardCategory _category) : this(_tid, _category, Grade.D, (IdleNumber)1)
		{

		}

		public RewardInfo(long _tid, RewardCategory _category, Grade _grade, int _count) : this(_tid, _category, _grade, (IdleNumber)_count)
		{

		}
		public RewardInfo(long _tid, RewardCategory _category, IdleNumber _count) : this(_tid, _category, Grade.D, _count)
		{

		}
		public RewardInfo(long _tid, RewardCategory _category, Grade _grade, IdleNumber _count)
		{
			tid = _tid;
			category = _category;
			grade = _grade;

			_count.NormalizeSelf();
			fixedCount = _count;
			countMin = _count;
			countMax = (IdleNumber)0;
			countPerStage = (IdleNumber)0;
			iconImage = null;
			isFixed = true;
			SetReward();
		}


		private void SetReward()
		{
			countMin.NormalizeSelf();
			fixedCount = countMin;
			switch (category)
			{
				case RewardCategory.Equip:
					{
						var data = DataManager.Get<EquipItemDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Equip");
							return;
						}

						PlatformManager.UserDB.equipContainer.GetScriptableObject(tid, out EquipItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.ItemIcon;
						}
						grade = data.itemGrade;

						Star = data.starLevel;

					}
					break;


				case RewardCategory.Costume:
					{
						//Debug.Log("Reward Costume");
						var data = DataManager.Get<CostumeDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Costume");
							return;
						}
						name = data.name;
						PlatformManager.UserDB.costumeContainer.GetScriptableObject(tid, out CostumeItemObject scriptableObject);

						if (scriptableObject != null)
						{
							iconImage = scriptableObject.ItemIcon;
						}
						grade = data.itemGrade;

					}
					break;
				case RewardCategory.Pet:
					{
						//Debug.Log("Reward Pet");//
						var data = DataManager.Get<PetDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Pet");
							return;
						}
						PlatformManager.UserDB.petContainer.GetScriptableObject(tid, out PetItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.ItemIcon;
						}
						grade = data.itemGrade;
						//Star = data.starLevel;
					}
					break;
				case RewardCategory.Event_Currency:
				case RewardCategory.Currency:
					{
						//Debug.Log("Reward Currency");
						var data = DataManager.Get<CurrencyDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Currency");
							return;
						}
						PlatformManager.UserDB.inventory.GetScriptableObject(tid, out CurrencyItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.ItemIcon;
						}
						grade = data.itemGrade;
					}

					break;
				case RewardCategory.Relic:
					{
						var data = DataManager.Get<RelicItemDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Relic");
							return;
						}
						PlatformManager.UserDB.relicContainer.GetScriptableObject(tid, out RelicItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.ItemIcon;
						}
						grade = data.itemGrade;
					}
					break;
				case RewardCategory.Skill:
					{
						//Debug.Log("Reward Skil");
						var data = DataManager.Get<SkillDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Skill");
							return;
						}

						PlatformManager.UserDB.skillContainer.GetScriptableObject(tid, out SkillCore scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.Icon;
						}
						grade = data.itemGrade;
					}
					break;
				case RewardCategory.EXP:
					grade = Grade.D;
					iconImage = GameUIManager.it.spriteExp;
					name = "경험치";
					break;
				case RewardCategory.RewardBox:
					{
						var data = DataManager.Get<RewardBoxDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in RewardBox");
							return;
						}
						PlatformManager.UserDB.inventory.GetScriptableObject(tid, out RewardBoxItemObject scriptableObject);

						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.ItemIcon;
						}


						grade = data.itemGrade;
					}
					break;
				case RewardCategory.Persistent:
					{
						var data = DataManager.Get<PersistentItemDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in RewardBox");
							return;
						}

						name = data.name;

						iconImage = Resources.Load<Sprite>($"Icons/{data.resource}");

					}
					break;
			}
		}

		public void Multiply(IdleNumber multi)
		{
			if (multi == 0)
			{
				return;
			}
			multi.NormalizeSelf();
			fixedCount *= multi;
			countMin *= multi;
		}

		public void AddCount(IdleNumber count)
		{
			count.NormalizeSelf();
			fixedCount += count;
			countMin += count;
		}
		public void UpdateCount(IdleNumber _step)
		{
			IdleNumber count = new IdleNumber();
			IdleNumber step = _step;
			if (countMax == 0)
			{
				count = countMin + (countPerStage * step);
			}
			else
			{
				count = IdleRandom.Random(countMin, countMax) + (countPerStage * step);
			}

			count.NormalizeSelf();
			fixedCount = count;
			isFixed = true;
		}

		public void UpdateCount(int stage = 0)
		{
			UpdateCount((IdleNumber)stage);
		}
	}

	[System.Serializable]
	public class PersistentItemInfo : ItemInfo
	{
		public override string ItemName => PlatformManager.Language[RawData.name];
		public Sprite icon;
		public PersistentItemData RawData { get; private set; }
		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
		}

		public override void SetRawData<T>(T data)
		{
			RawData = data as PersistentItemData;
			tid = RawData.tid;

			if (!RawData.resource.IsNullOrEmpty())
			{
				icon = Resources.Load<Sprite>($"Icons/{RawData.resource}");
			}
		}
	}

}


