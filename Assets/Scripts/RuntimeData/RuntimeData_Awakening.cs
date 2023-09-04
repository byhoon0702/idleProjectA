using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class AwakeningLevelInfo : ItemInfo
	{
		public AwakeningLevel RawData { get; private set; }
		public RuntimeData.AbilityInfo Ability { get; private set; }

		public int MaxLevel { get; private set; }
		public int BaseCost { get; private set; }
		public int IncPerLevel { get; private set; }
		public AwakeningLevelInfo(AwakeningLevel levelData)
		{
			RawData = levelData;
			MaxLevel = RawData.MaxLevel;
			BaseCost = RawData.BaseCost;
			IncPerLevel = RawData.IncPerLevel;
			SetAbility();
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			SetAbility();
		}


		public void SetAbility()
		{
			Ability = new AbilityInfo(RawData.Stat);
			Ability.RemoveAllModifiersFromSource(this);
			IdleNumber value = (IdleNumber)RawData.Stat.perLevel;
			Ability.AddModifiers(new StatsModifier(value * Level, StatModeType.Add, this));
		}

		public bool AddLevel()
		{
			if (MaxLevel <= _level)
			{
				return false;
			}
			_level++;

			Ability.RemoveAllModifiersFromSource(this);
			IdleNumber value = (IdleNumber)RawData.Stat.perLevel;
			Ability.AddModifiers(new StatsModifier(value * Level, StatModeType.Add, this));


			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.LEVELUP_AWAKENRUNE, tid, (IdleNumber)1);
			return true;
		}
		public void Upgrade(AwakeningLevel levelData)
		{
			BaseCost = levelData.BaseCost;
			MaxLevel = levelData.MaxLevel;
			IncPerLevel = levelData.IncPerLevel;
		}

		public IdleNumber GetCost()
		{
			int cost = BaseCost + (IncPerLevel * _level);

			return (IdleNumber)cost;
		}

		public IdleNumber GetNextInfo()
		{
			IdleNumber nextAbilities = (IdleNumber)0;
			int nextlevel = _level + 1;
			if (nextlevel > MaxLevel)
			{
				nextAbilities = CalculateAbility(MaxLevel);
			}
			else
			{
				nextAbilities = CalculateAbility(nextlevel);
			}
			return nextAbilities;
		}

		private IdleNumber CalculateAbility(int step)
		{
			AbilityInfo nextInfo = new AbilityInfo(RawData.Stat);

			return nextInfo.GetValueFromZero(step);

		}

		public bool IsMax()
		{
			return Level == MaxLevel;
		}
		public bool CanIAwaken(int maxLevel)
		{
			return Level >= maxLevel;
		}

	}

	[System.Serializable]
	public class AwakeningInfo : ItemInfo
	{
		[SerializeField] private bool _isAwaken;
		public bool IsAwaken => _isAwaken;
		public Sprite Icon => ItemObject != null ? ItemObject.ItemIcon : null;
		public Sprite CostIcon { get; private set; }
		public AwakeningItemObject ItemObject { get; private set; }
		public CostumeItemObject Costume { get; private set; }
		public AwakeningData RawData { get; private set; }

		public List<AbilityInfo> AbilityInfos { get; private set; }

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);

			UpdateData();
			AwakeningInfo temp = info as AwakeningInfo;
			_isAwaken = temp._isAwaken;
		}


		public override void SetRawData<T>(T data)
		{
			RawData = data as AwakeningData;
			tid = RawData.tid;

			AbilityInfos = new List<AbilityInfo>();
			if (RawData.awakeningStats != null)
			{
				for (int i = 0; i < RawData.awakeningStats.Length; i++)
				{
					AbilityInfos.Add(new AbilityInfo(RawData.awakeningStats[i]));
				}
			}
		}

		public bool IsMax()
		{

			return false;
		}

		public void Awaken()
		{
			_isAwaken = true;
		}

		public override void UpdateData()
		{
			ItemObject = PlatformManager.UserDB.awakeningContainer.GetScriptableObject<AwakeningItemObject>(RawData.tid);
			var currencyObject = PlatformManager.UserDB.inventory.GetScriptableObject<CurrencyItemObject>(RawData.costitemTid);
			CostIcon = currencyObject != null ? currencyObject.ItemIcon : null;
			Costume = PlatformManager.UserDB.costumeContainer.GetScriptableObject<CostumeItemObject>(RawData.costumeTid);

			if (IsAwaken)
			{
				var item = PlatformManager.UserDB.costumeContainer.FindCostumeItem(RawData.costumeTid, CostumeType.HYPER);
				item.unlock = true;
				item.SetCount(1);
			}
		}
	}
}
