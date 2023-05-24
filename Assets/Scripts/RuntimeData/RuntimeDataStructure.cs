
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using Newtonsoft.Json;

public class ModifyInfo
{
	protected bool isDirty;


	protected IdleNumber _value;
	protected IdleNumber lastBaseValue;
	[JsonIgnore]
	public virtual IdleNumber Value
	{
		get
		{
			if (isDirty || BaseValue != lastBaseValue)
			{
				lastBaseValue = BaseValue;
				_value = Calculate();
				isDirty = false;
			}

			return _value;
		}
	}
	[JsonIgnore] public virtual IdleNumber BaseValue { get; protected set; } = (IdleNumber)0;

	[JsonIgnore] protected readonly List<StatsModifier> modifiers;
	[JsonIgnore] public readonly ReadOnlyCollection<StatsModifier> Modifiers;

	public ModifyInfo()
	{
		modifiers = new List<StatsModifier>();
		Modifiers = modifiers.AsReadOnly();
	}
	public virtual void SetDirty()
	{
		isDirty = true;
	}
	public virtual void AddModifiers(StatsModifier modifier)
	{
		isDirty = true;
		modifiers.Add(modifier);
		modifiers.Sort((a, b) => { return a.Order.CompareTo(b.Order); });

	}

	public virtual void UpdateModifiers(StatsModifier modifier)
	{

		int index = modifiers.FindIndex(x => x.Type == modifier.Type && x.Source == modifier.Source && x.Order == modifier.Order);
		if (index == -1)
		{
			AddModifiers(modifier);
			return;
		}
		isDirty = true;

		modifiers[index] = modifier;

	}

	public virtual bool RemoveAllModifiersFromSource(object source)
	{
		bool didRemove = false;
		for (int i = modifiers.Count - 1; i >= 0; i--)
		{
			if (modifiers[i].Source == source)
			{
				isDirty = true;
				didRemove = true;
				modifiers.RemoveAt(i);
			}
		}

		return didRemove;
	}
	public virtual bool RemoveModifiers(StatsModifier modifier)
	{
		if (modifiers.Remove(modifier))
		{
			isDirty = true;
			return true;
		}


		return false;
	}


	public IdleNumber Calculate()
	{
		if (modifiers == null || modifiers.Count == 0)
		{

			return BaseValue;
		}

		IdleNumber finalValue = BaseValue;
		IdleNumber sumFlatAdd = (IdleNumber)0;
		IdleNumber sumPerentAdd = (IdleNumber)0;

		for (int i = 0; i < modifiers.Count; i++)
		{
			var modifier = modifiers[i];
			if (modifier.Type == StatModeType.Flat)
			{
				finalValue += modifier.Value;
			}
			else if (modifier.Type == StatModeType.FlatAdd)
			{
				sumFlatAdd += modifier.Value;
				if (i >= modifiers.Count - 1 || modifiers[i + 1].Type != modifier.Type)
				{
					if (sumFlatAdd == 0)
					{
						continue;
					}
					finalValue *= 1 + (sumFlatAdd / 100f);
				}
			}
			else if (modifier.Type == StatModeType.PercentAdd)
			{
				sumPerentAdd += modifier.Value;
				if (i >= modifiers.Count - 1 || modifiers[i + 1].Type != modifier.Type)
				{
					if (sumPerentAdd == 0)
					{
						continue;
					}
					finalValue *= 1 + (sumPerentAdd / 100f);
				}
			}
			else
			{
				if (modifier.Value == 0)
				{
					continue;
				}
				finalValue *= 1 + (modifier.Value / 100f);
			}
		}

		return finalValue;
	}

}

namespace RuntimeData
{
	public interface IDataInfo
	{
		void SetRawData<T>(T data) where T : class;

	}
	public abstract class ItemInfo : ModifyInfo, IDataInfo
	{
		public long tid;
		public bool unlock;
		public int level;
		public int count;

		public virtual void SetRawData<T>(T data) where T : class
		{

		}

		public virtual void Load(ItemInfo info)
		{
			tid = info.tid;
			unlock = info.unlock;
			level = info.level;
			count = info.count;
		}
	}


	[System.Serializable]
	public struct Veterancy
	{

	}

	[System.Serializable]
	public struct ItemData
	{

		public ItemStats[] buffs;
	}

	[System.Serializable]
	public class EquipItemInfo : ItemInfo
	{
		public EquipType type => rawData.equipType;

		public Grade grade => rawData.itemGrade;
		public EquipItemData rawData { get; private set; }

		public EquipItemObject itemObject { get; private set; }

		public List<AbilityInfo> equipAbilities { get; private set; } = new List<AbilityInfo>();
		public List<AbilityInfo> ownedAbilities { get; private set; } = new List<AbilityInfo>();

		public override void SetRawData<T>(T data)
		{
			rawData = data as EquipItemData;
			tid = rawData.tid;

			itemObject = GameManager.UserDB.equipContainer.GetScriptableObject<EquipItemObject>(rawData.tid);
			UpdateAbilities();
		}

		public override void Load(ItemInfo info)
		{
			base.Load(info);
			SetDirty();
			UpdateAbilities();
		}

		public void UpdateAbilities()
		{
			equipAbilities.Clear();
			for (int i = 0; i < rawData.equipValues.Count; i++)
			{
				ItemStats buff = rawData.equipValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(level);
				equipAbilities.Add(info);
			}
			ownedAbilities.Clear();
			for (int i = 0; i < rawData.ownValues.Count; i++)
			{
				ItemStats buff = rawData.ownValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(level);
				ownedAbilities.Add(info);
			}
		}

		public void LevelUP()
		{
			level++;
			UpdateAbilities();
		}
		public void SetLevel(int _level)
		{
			level = _level;
			SetDirty();
			UpdateAbilities();
		}

		public bool CanLevelUp()
		{
			return level < 100;
		}

		public int LevelUpNeedCount()
		{
			//var requirement = VGameManager.UserDB.commonData.LEVELUP_REQUIREMENT.Find(x => x.level <= level);

			//return requirement.requirement;
			return 0;
		}
	}

	[System.Serializable]
	public class CostumeInfo : ItemInfo
	{
		[JsonIgnore] public CostumeType Type => rawData.costumeType;

		[JsonIgnore] public CostumeItemObject itemObject { get; private set; }
		[JsonIgnore] public CostumeData rawData { get; private set; }
		[JsonIgnore] public string ItemName => itemObject != null ? itemObject.ItemName : "No Data";

		[JsonIgnore] public List<AbilityInfo> equipAbilities { get; private set; } = new List<AbilityInfo>();
		[JsonIgnore] public List<AbilityInfo> ownedAbilities { get; private set; } = new List<AbilityInfo>();

		public bool CanLevelUp()
		{
			return false;
		}
		public override void SetRawData<T>(T data)
		{
			rawData = data as CostumeData;
			tid = rawData.tid;

			itemObject = GameManager.UserDB.costumeContainer.GetScriptableObject<CostumeItemObject>(tid);
			UpdateAbilities();

		}

		public void UpdateAbilities()
		{
			equipAbilities.Clear();
			for (int i = 0; i < rawData.equipValues.Count; i++)
			{
				ItemStats buff = rawData.equipValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(level);
				equipAbilities.Add(info);
			}
			ownedAbilities.Clear();
			for (int i = 0; i < rawData.ownValues.Count; i++)
			{
				ItemStats buff = rawData.ownValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(level);
				ownedAbilities.Add(info);
			}
		}
	}


	public struct ItemBuffOption
	{
		public short index;
		public Grade grade;
		public AbilityInfo ability;
		public bool isLock;
	}

	[System.Serializable]
	public class PetInfo : ItemInfo
	{
		public int evolutionLevel;
		public Grade grade => rawData.itemGrade;

		public string ItemName { get; private set; }

		//런타임에서 로드 할것

		public PetItemObject itemObject { get; private set; }

		public PetData rawData { get; private set; }
		public Sprite Icon => itemObject != null ? itemObject.Icon : null;
		public GameObject PetObject => itemObject != null ? itemObject.PetObject : null;

		public List<AbilityInfo> equipAbilities { get; private set; } = new List<AbilityInfo>();
		public List<AbilityInfo> ownedAbilities { get; private set; } = new List<AbilityInfo>();

		public List<ItemBuffOption> options { get; private set; } = new List<ItemBuffOption>();

		public PetInfo Clone()
		{
			PetInfo clone = new PetInfo();

			clone.tid = tid;
			clone.unlock = unlock;
			clone.level = level;
			clone.count = count;
			clone.itemObject = UnityEngine.Object.Instantiate(itemObject);
			clone.rawData = rawData;
			clone.equipAbilities = equipAbilities;
			clone.ownedAbilities = ownedAbilities;
			clone.evolutionLevel = evolutionLevel;
			return clone;
		}

		public bool CanLevelUp()
		{
			return false;
		}
		public override void SetRawData<T>(T data)
		{
			rawData = data as PetData;
			tid = rawData.tid;
			itemObject = GameManager.UserDB.petContainer.GetScriptableObject<PetItemObject>(tid);
			UpdateAbilities();
			ItemName = rawData.name;
		}

		public void Evolution()
		{
			evolutionLevel++;
			UpdateAbilities();
		}

		public void AddOptions(ItemBuffOption option)
		{
			options.Add(option);
		}

		public void UpdateOption(int index, ItemBuffOption option)
		{
			options[index] = option;
		}

		public void UpdateAbilities()
		{
			equipAbilities.Clear();
			for (int i = 0; i < rawData.equipValues.Count; i++)
			{
				ItemStats buff = rawData.equipValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(level);
				equipAbilities.Add(info);
			}
			ownedAbilities.Clear();
			for (int i = 0; i < rawData.ownValues.Count; i++)
			{
				ItemStats buff = rawData.ownValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(level);
				ownedAbilities.Add(info);
			}
		}
	}


	[System.Serializable]
	public class TrainingInfo : StatInfo
	{
		public Sprite icon => itemObject.Icon;
		public StatsType type => rawData.buff.type;

		public bool isLock;

		public StatsType PreconditionType => rawData.preconditionType;
		public long PreconditionLevel => rawData.preconditionLevel;
		public StatModeType modeType => rawData.buff.modeType;

		public IdleNumber basicValue => (IdleNumber)rawData.buff.value;
		public IdleNumber perLevelValue => (IdleNumber)rawData.buff.perLevel;

		public delegate void TrainingInfoDelegate();
		public event TrainingInfoDelegate OnClickLevelup;


		public TrainingItemObject itemObject;
		public TrainingData rawData { get; private set; }

		public bool isOpen
		{
			get
			{
				if (rawData.preconditionType == StatsType.None)
				{
					return true;
				}

				return GameManager.UserDB.training.Find(rawData.preconditionType).level >= rawData.preconditionLevel;
			}
		}
		public bool isMaxLevel
		{
			get
			{
				if (rawData.maxLevel == 0)
				{
					return false;
				}

				return level >= rawData.maxLevel;
			}
		}

		public TrainingInfo()
		{
			level = 1;
		}

		public TrainingInfo(TrainingData _data) : this(_data, 1)
		{

		}

		public TrainingInfo(TrainingData _data, int _level)
		{
			rawData = _data;
			tid = rawData.tid;
			itemObject = GameManager.UserDB.training.GetScriptableObject<TrainingItemObject>(rawData.tid);
			SetLevel(_level);
		}
		public override void SetRawData<T>(T data)
		{
			rawData = data as TrainingData;
			tid = rawData.tid;
			itemObject = GameManager.UserDB.training.GetScriptableObject<TrainingItemObject>(rawData.tid);

			if (level == 0)
			{
				level = 1;
			}
			SetLevel(level);
		}

		~TrainingInfo()
		{
			OnClickLevelup = null;
		}

		public override void Load(StatInfo info)
		{
			base.Load(info);
			SetLevel(level);
		}
		public void SetLevel(int _level)
		{
			level = _level;

			GetCost();
			GetValue();
			GetNextValue();
		}

		public IdleNumber GetCost()
		{
			cost = (IdleNumber)(rawData.basicCost + ((Mathf.Max(0, (level - 1)) * rawData.basicCostInc) * (rawData.basicCostWeight * (level - 1))));
			return cost;
		}

		public IdleNumber GetValue()
		{
			currentValue = (basicValue) + (perLevelValue * Mathf.Max(0, (level - 1)));
			return currentValue;
		}
		public IdleNumber GetNextValue()
		{
			nextValue = (basicValue) + (perLevelValue * level);
			return nextValue;
		}

		public override void AddModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}
			userDB.AddModifiers(rawData.buff.isHyper, type, new StatsModifier(currentValue, modeType, this));
		}
		public override void UpdateModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}
			userDB.UpdateModifiers(rawData.buff.isHyper, type, new StatsModifier(currentValue, modeType, this));
		}
		public override void RemoveModifier(UserDB userDB)
		{
			userDB.RemoveModifiers(rawData.buff.isHyper, type, this);
		}


		public void ClickLevelup()
		{
			if (isMaxLevel)
			{
				return;
			}

			if (rawData.maxLevel > 0)
			{
				level = Mathf.Min(++level, rawData.maxLevel);
			}
			else
			{
				level++;
			}
			SetLevel(level);

			RemoveModifier(GameManager.UserDB);
			AddModifier(GameManager.UserDB);

			if (OnClickLevelup != null)
			{
				OnClickLevelup();
			}
			//GameManager.UserDB.Save();
		}


	}
	public abstract class StatInfo : IDataInfo
	{
		[SerializeField] protected long tid;
		public long Tid => tid;

		[SerializeField] protected int level;
		public int Level => level;
		public IdleNumber cost
		{
			get;
			protected set;
		}

		public IdleNumber currentValue { get; protected set; }
		public IdleNumber nextValue { get; protected set; }

		public abstract void AddModifier(UserDB userDB);
		public abstract void UpdateModifier(UserDB userDB);
		public abstract void RemoveModifier(UserDB userDB);
		public abstract void SetRawData<T>(T data) where T : class;

		public virtual void Load(StatInfo info)
		{
			tid = info.tid;
			level = info.level;

		}
	}


	[System.Serializable]
	public class VeterancyInfo : StatInfo
	{
		public Sprite icon => itemObject.Icon;
		public StatsType type => rawData.buff.type;
		public string Name => rawData.name;
		public string Description => rawData.description;

		public StatModeType modeType => rawData.buff.modeType;

		public int MaxLevel => rawData.maxLevel;
		public Cost basicCost => rawData.basicCost;
		public IdleNumber basicValue => (IdleNumber)rawData.buff.value;
		public IdleNumber perLevelValue => (IdleNumber)rawData.buff.perLevel;

		public delegate void LevelUpDelegate();
		public event LevelUpDelegate OnClickLevelup;

		public VeterancyObject itemObject { get; protected set; }
		private VeterancyData rawData;
		[JsonIgnore]
		public bool isOpen
		{
			get
			{
				return true;
			}
		}

		public VeterancyInfo()
		{

		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as VeterancyData;
			tid = rawData.tid;
			itemObject = GameManager.UserDB.veterancyContainer.GetScriptableObject<VeterancyObject>(tid);

			SetLevel(level);
		}

		public override void Load(StatInfo info)
		{
			base.Load(info);
			SetLevel(level);
		}
		public void SetLevel(int _level)
		{

			level = _level;

			GetCost();
			GetValue();
			GetNextValue();
		}
		public VeterancyInfo(VeterancyObject _itemObject)
		{
			itemObject = _itemObject;
			tid = itemObject.Tid;
			SetLevel(level);
		}

		~VeterancyInfo()
		{
			OnClickLevelup = null;
		}

		public IdleNumber GetCost()
		{
			cost = (IdleNumber)basicCost.cost + ((Mathf.Max(0, (level - 1)) * basicCost.costIncrease) * (basicCost.costWeight * level));
			return cost;
		}

		public IdleNumber GetValue()
		{
			currentValue = (basicValue) + (perLevelValue * Mathf.Max(0, (level - 1)));
			return currentValue;
		}
		public IdleNumber GetNextValue()
		{
			nextValue = (basicValue) + (perLevelValue * level);
			return nextValue;
		}

		public void Get(ref IdleNumber value)
		{
			value += currentValue;
		}

		public bool IsMax()
		{
			if (MaxLevel == 0)
			{
				return false;
			}


			return MaxLevel == level;

		}
		public override void AddModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}

			userDB.AddModifiers(rawData.buff.isHyper, type, new StatsModifier(currentValue, modeType, this));
		}
		public override void UpdateModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}
			userDB.UpdateModifiers(rawData.buff.isHyper, type, new StatsModifier(currentValue, modeType, this));
		}


		public override void RemoveModifier(UserDB userDB)
		{
			userDB.RemoveModifiers(rawData.buff.isHyper, type, this);
		}
		public void ClickLevelup()
		{
			if (IsMax())
			{
				return;
			}

			if (MaxLevel > 0)
			{
				level = Mathf.Min(++level, MaxLevel);
			}
			else
			{
				level++;
			}
			SetLevel(level);

			//RemoveModifier(GameManager.UserDB);
			//AddModifier(GameManager.UserDB);

			if (OnClickLevelup != null)
			{
				OnClickLevelup();
			}

			//if (OnAbilityUpdate != null)
			//{
			//	OnAbilityUpdate(currentValue);
			//}
		}


	}
}




[System.Serializable]
public class AbilityInfo
{
	public StatsType type { get; private set; }
	public StatusData rawData { get; private set; }

	[SerializeField] private IdleNumber value;
	[SerializeField] private IdleNumber inc;
	public IdleNumber Value => modifiedValue;
	public IdleNumber Inc => inc;

	private IdleNumber modifiedValue;

	public StatModeType modeType { get; private set; }

	public bool isPercentage { get; private set; }
	public bool isHyper { get; private set; }

	public string tailChar
	{
		get
		{
			if (isPercentage == false)
			{
				return "";
			}
			return "%";
		}
	}
	public AbilityInfo Clone()
	{
		AbilityInfo info = new AbilityInfo();
		info.type = type;
		info.rawData = rawData;
		info.value = value;
		info.inc = inc;
		info.modifiedValue = modifiedValue;
		info.modeType = modeType;
		info.isPercentage = isPercentage;
		info.isHyper = isHyper;

		return info;
	}
	public AbilityInfo()
	{

	}

	public AbilityInfo(StatsType _type, IdleNumber _value) : this(_type, _value, (IdleNumber)0, StatModeType.Flat, false, false)
	{

	}
	public AbilityInfo(StatsType _type, IdleNumber _value, IdleNumber _inc) : this(_type, _value, _inc, StatModeType.Flat, false, false)
	{

	}

	public AbilityInfo(StatsType _type, IdleNumber _value, IdleNumber _inc, StatModeType _modeType, bool _isPercentage, bool isHyper)
	{
		type = _type;
		value = _value;
		inc = _inc;
		modeType = _modeType;
		isPercentage = _isPercentage;
		this.isHyper = isHyper;
		//rawData = DataManager.Get<StatusDataSheet>().GetData(type);
	}

	public AbilityInfo(ItemStats stats) : this(stats.type, (IdleNumber)stats.value, (IdleNumber)stats.perLevel, stats.modeType, stats.isPercentage, stats.isHyper)
	{
	}

	public string Description()
	{
		//if (rawData == null)
		//{
		//	rawData = DataManager.Get<StatusDataSheet>().GetData(type);
		//}
		//return rawData.description;
		return "";
	}

	public IdleNumber GetValue(int _level = 1)
	{

		int relative = _level - 1;
		if (relative < 0)
		{
			relative = 0;
		}
		modifiedValue = value + (inc * relative);

		return modifiedValue;
	}

	public IdleNumber GetNextValue(int _level = 1)
	{
		int relative = _level - 1;
		if (relative < 0)
		{
			relative = 0;
		}
		IdleNumber nextValue = value + (inc * relative);

		return nextValue;
	}

	public override string ToString()
	{
		return $"{type}. +{value.ToString()}";
	}


}
