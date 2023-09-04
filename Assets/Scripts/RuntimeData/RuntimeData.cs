
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using Newtonsoft.Json;
using RuntimeData;

public class BaseInfo : IDataInfo
{
	[SerializeField][ReadOnly(false)] protected long tid;
	public long Tid => tid;

	public virtual void UpdateData()
	{

	}
	public virtual void Load<T>(T info) where T : BaseInfo
	{

	}

	public virtual void SetRawData<T>(T data) where T : BaseData
	{

	}
}

public class ModifyInfo : BaseInfo
{
	protected bool isDirty;


	protected IdleNumber _value;
	protected IdleNumber lastBaseValue;

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
	public virtual IdleNumber BaseValue { get; protected set; } = (IdleNumber)0;

	protected readonly List<StatsModifier> modifiers;
	public readonly ReadOnlyCollection<StatsModifier> Modifiers;

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
		modifiers.Add(modifier);
		modifiers.Sort((a, b) => { return a.Order.CompareTo(b.Order); });
		isDirty = true;
	}

	public virtual void UpdateModifiers(StatsModifier modifier)
	{
		RemoveModifiers(modifier);
		AddModifiers(modifier);
		isDirty = true;
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
	public virtual bool RemoveAllModifiersFromSource(StatModeType type)
	{
		bool didRemove = false;
		for (int i = modifiers.Count - 1; i >= 0; i--)
		{
			if (modifiers[i].Type == type)
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
		IdleNumber multi = (IdleNumber)0;
		IdleNumber buff = (IdleNumber)0;
		IdleNumber adsbuff = (IdleNumber)0;
		IdleNumber hyper = (IdleNumber)0;
		List<IdleNumber> debuff = new List<IdleNumber>();

		for (int i = 0; i < modifiers.Count; i++)
		{
			var modifier = modifiers[i];
			if (modifier.Type == StatModeType.Replace)
			{
				finalValue = modifier.Value;
			}
			else if (modifier.Type == StatModeType.Add)
			{
				finalValue += modifier.Value;
			}
			else if (modifier.Type == StatModeType.Multi)
			{
				multi += modifier.Value;
			}
			else if (modifier.Type == StatModeType.Buff)
			{
				buff += modifier.Value;
			}
			else if (modifier.Type == StatModeType.SkillBuff)
			{
				buff += modifier.Value;
			}
			else if (modifier.Type == StatModeType.AdsBuff)
			{
				adsbuff += modifier.Value;
			}
			else if (modifier.Type == StatModeType.Hyper)
			{
				hyper += modifier.Value;
			}
			else if (modifier.Type == StatModeType.SkillDebuff)
			{
				debuff.Add(modifier.Value);
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


		IdleNumber buffRatio = (IdleNumber)1;
		IdleNumber adsbuffRatio = (IdleNumber)1;
		IdleNumber hyperRatio = (IdleNumber)1;

		if (buff > 0)
		{
			buffRatio = (IdleNumber)(1 + (buff / 100f));
		}
		if (adsbuff > 0)
		{
			adsbuffRatio = (IdleNumber)(1 + (adsbuff / 100f));
		}
		if (hyper > 0)
		{
			hyperRatio = (IdleNumber)(1 + (hyper / 100f));
		}


		finalValue = (finalValue * (1 + (multi / 100f))) * buffRatio * adsbuffRatio * hyperRatio;

		for (int i = 0; i < debuff.Count; i++)
		{
			if (debuff[i] > 0)
			{
				IdleNumber minus = finalValue / 100f * debuff[i];
				finalValue -= minus;
			}
		}

		return finalValue;
	}

}

namespace RuntimeData
{
	public interface IDataInfo
	{
		void SetRawData<T>(T data) where T : BaseData;
		void Load<T>(T info) where T : BaseInfo;
		void UpdateData();
	}

	public class ItemInfo : ModifyInfo
	{
		public bool unlock;
		[SerializeField] protected int _level;
		public int Level => _level;
		/// <summary>
		/// 재화를 재외한 아이템의 개수, 재화의 경우 Value 를 사용할 것
		/// </summary>
		[SerializeField] protected int _count;
		public int Count => _count;

		public override void Load<T>(T info)
		{

			if (info == null)
			{
				return;
			}
			ItemInfo itemInfo = info as ItemInfo;
			unlock = itemInfo.unlock;
			_level = itemInfo._level;
			_count = itemInfo._count;
			if (unlock)
			{
				if (_level == 0)
				{
					_level = 1;
				}
			}
		}
		public virtual void SetLevel(int level)
		{
			_level = level;
		}
		public void SetCount(int count)
		{
			_count = count;
		}
		public void CalculateCount(int count, bool isPlus = true)
		{
			if (isPlus)
			{
				_count += count;
			}
			else
			{
				_count -= count;
			}

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
		private const int defaultMaxLevel = 100;
		public int MaxLevel { get; private set; }

		[SerializeField] private int breakthroughLevel;
		public int BreakthroughLevel => breakthroughLevel;

		public EquipType type => rawData.equipType;

		public Sprite Icon
		{
			get
			{
				if (itemObject != null)
				{
					return itemObject.ItemIcon;
				}
				return null;
			}
		}
		public int star => rawData.starLevel;
		public Grade grade => rawData.itemGrade;
		public EquipItemData rawData { get; private set; }

		public EquipItemObject itemObject { get; private set; }

		public List<AbilityInfo> equipAbilities { get; private set; } = new List<AbilityInfo>();
		public List<AbilityInfo> ownedAbilities { get; private set; } = new List<AbilityInfo>();

		public bool isLastItem { get; private set; }

		public override void SetRawData<T>(T data)
		{
			rawData = data as EquipItemData;
			tid = rawData.tid;

			itemObject = PlatformManager.UserDB.equipContainer.GetScriptableObject<EquipItemObject>(rawData.tid);
			UpdateAbilities();

			var equipList = DataManager.Get<EquipItemDataSheet>().GetByItemType(rawData.equipType);
			isLastItem = equipList[equipList.Count - 1].tid == tid;

		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			EquipItemInfo temp = info as EquipItemInfo;
			breakthroughLevel = temp.breakthroughLevel;
			SetDirty();
			UpdateAbilities();
			MaxLevel = PlatformManager.CommonData.EquipBreakThroughList[breakthroughLevel].maxLevel;
		}

		public void UpdateAbilities()
		{

			equipAbilities.Clear();
			for (int i = 0; i < rawData.equipValues.Count; i++)
			{
				ItemStats buff = rawData.equipValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(_level);
				equipAbilities.Add(info);
			}
			ownedAbilities.Clear();
			for (int i = 0; i < rawData.ownValues.Count; i++)
			{
				ItemStats buff = rawData.ownValues[i];
				AbilityInfo info = new AbilityInfo(buff);
				info.GetValue(_level);
				ownedAbilities.Add(info);
			}

			RemoveModifiers(PlatformManager.UserDB);
			AddModifiers(PlatformManager.UserDB);
		}
		//public void UpdateModifiers(UserDB userDB)
		//{
		//	if (unlock == false)
		//	{
		//		return;
		//	}
		//	for (int i = 0; i < ownedAbilities.Count; i++)
		//	{
		//		userDB.UpdateModifiers(ownedAbilities[i].type, new StatsModifier(ownedAbilities[i].Value, ownedAbilities[i].modeType, this));
		//	}
		//}

		public void AddModifiers(UserDB userDB)
		{
			if (unlock == false)
			{
				return;
			}
			for (int i = 0; i < ownedAbilities.Count; i++)
			{
				userDB.AddModifiers(ownedAbilities[i].type, new StatsModifier(ownedAbilities[i].Value, ownedAbilities[i].modeType, this));
			}
		}

		public void RemoveModifiers(UserDB userDB)
		{
			if (unlock == false)
			{
				return;
			}
			for (int i = 0; i < ownedAbilities.Count; i++)
			{
				userDB.RemoveModifiers(ownedAbilities[i].type, this);
			}
		}
		public void LevelUP()
		{
			_level++;
			UpdateAbilities();
			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.LEVELUP_WEAPON, tid, (IdleNumber)1);
		}

		public bool IsMaxBreakThrough()
		{
			if (breakthroughLevel >= PlatformManager.CommonData.EquipBreakThroughList.Count - 1)
			{
				return true;
			}
			return false;
		}
		public bool BreakThrough()
		{
			if (IsMaxBreakThrough())
			{
				return false;
			}
			breakthroughLevel++;
			MaxLevel = PlatformManager.CommonData.EquipBreakThroughList[breakthroughLevel].maxLevel;
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.BREAKTHROUGH_WEAPON, tid, (IdleNumber)1);
			return true;
		}

		public override void SetLevel(int level)
		{
			_level = level;
			SetDirty();
			UpdateAbilities();
		}

		public bool IsMaxLevel()
		{
			return _level == MaxLevel;
		}

		public IdleNumber LevelUpNeedCount()
		{
			var requirement = PlatformManager.CommonData.LevelUpConsumeDataList.Find(x => x.grade == grade);
			var first = (requirement.basicConsume + ((star - 1) * requirement.starWeight));
			var second = ((_level - 1) * requirement.increaseRange) * (1 + (_level * requirement.gradeWeight));
			//return requirement.requirement;
			return (IdleNumber)(first + second);
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
	public class TrainingInfo : StatInfo
	{
		public Sprite icon => itemObject.ItemIcon;
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

		public string tailChar
		{
			get
			{
				if (rawData.buff.isPercentage)
				{
					return "%";

				}
				return "";
			}
		}

		public bool isOpen
		{
			get
			{
				if (rawData.preconditionType == StatsType.None)
				{
					return true;
				}

				return PlatformManager.UserDB.training.Find(rawData.preconditionType)._level >= rawData.preconditionLevel;
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

				return _level >= rawData.maxLevel;
			}
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as TrainingData;
			tid = rawData.tid;
			itemObject = PlatformManager.UserDB.training.GetScriptableObject<TrainingItemObject>(rawData.tid);

			SetLevel(0);
		}

		~TrainingInfo()
		{
			OnClickLevelup = null;
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			SetLevel(_level);
		}

		public void AddLevel(int add)
		{
			_level += add;

			if (rawData.maxLevel > 0)
			{
				_level = Mathf.Min(_level, rawData.maxLevel);
			}

			GetCost();
			GetValue();
			GetNextValue();

		}
		public void SetLevel(int level)
		{
			if (level < _level)
			{
				///여기로 들어온다는건 문제가 있다는 뜻;
				Debug.LogError($"!!!!!!===Training {type}에 잘못된 정보가 들어옴. 현재 레벨{_level} 입력 값{level} ===!!!!!!");
				return;
			}

			_level = level;

			GetCost();
			GetValue();
			GetNextValue();
		}

		public IdleNumber GetCost()
		{
			cost = (IdleNumber)(rawData.basicCost + ((Mathf.Max(0, _level) * rawData.basicCostInc) * (rawData.basicCostWeight * _level)));
			return cost;
		}
		public IdleNumber GetCost(int level)
		{
			cost = (IdleNumber)(rawData.basicCost + ((Mathf.Max(0, level) * rawData.basicCostInc) * (rawData.basicCostWeight * level)));
			return cost;
		}



		public IdleNumber GetValue()
		{
			currentValue = (basicValue) + (perLevelValue * Mathf.Max(0, _level));
			return currentValue;
		}
		public IdleNumber GetNextValue()
		{
			nextValue = (basicValue) + (perLevelValue * _level);
			return nextValue;
		}

		public override void AddModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}
			userDB.AddModifiers(type, new StatsModifier(currentValue, modeType, this));
		}
		//public override void UpdateModifier(UserDB userDB)
		//{
		//	if (isOpen == false)
		//	{
		//		return;
		//	}
		//	userDB.UpdateModifiers(type, new StatsModifier(currentValue, modeType, this));
		//}
		public override void RemoveModifier(UserDB userDB)
		{
			userDB.RemoveModifiers(type, this);
		}


		public void ClickLevelup(int count)
		{
			if (isMaxLevel)
			{
				return;
			}


			AddLevel(count);

			RemoveModifier(PlatformManager.UserDB);
			AddModifier(PlatformManager.UserDB);

			OnClickLevelup?.Invoke();


			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.ABILITY_LEVEL, rawData.tid, (IdleNumber)_level);
			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.ABILITY_LEVELUP, rawData.tid, (IdleNumber)count);
		}
	}

	public abstract class StatInfo : BaseInfo
	{
		[SerializeField] protected int _level;
		public int Level => _level;
		public IdleNumber cost
		{
			get;
			protected set;
		}

		public IdleNumber currentValue { get; protected set; }
		public IdleNumber nextValue { get; protected set; }

		public abstract void AddModifier(UserDB userDB);
		//public abstract void UpdateModifier(UserDB userDB);
		public abstract void RemoveModifier(UserDB userDB);

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			StatInfo stat = info as StatInfo;
			tid = stat.tid;
			_level = stat.Level;

		}
	}


	[System.Serializable]
	public class VeterancyInfo : StatInfo
	{
		public Sprite icon => itemObject.ItemIcon;
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
		public VeterancyData rawData { get; protected set; }
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
			itemObject = PlatformManager.UserDB.veterancyContainer.GetScriptableObject<VeterancyObject>(tid);

			SetLevel(_level);
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			SetLevel(_level);
		}
		public void SetLevel(int _level)
		{

			base._level = _level;

			GetCost();
			GetValue();
			GetNextValue();


		}
		public VeterancyInfo(VeterancyObject _itemObject)
		{
			itemObject = _itemObject;
			tid = itemObject.Tid;
			SetLevel(_level);
		}

		~VeterancyInfo()
		{
			OnClickLevelup = null;
		}

		public IdleNumber GetCost()
		{
			cost = (IdleNumber)basicCost.cost + ((Mathf.Max(0, (_level)) * basicCost.costIncrease) * (basicCost.costWeight * _level));
			return cost;
		}

		public IdleNumber GetValue()
		{
			currentValue = (basicValue) + (perLevelValue * Mathf.Max(0, (_level)));
			return currentValue;
		}
		public IdleNumber GetNextValue()
		{
			nextValue = (basicValue) + (perLevelValue * (_level + 1));
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


			return MaxLevel == _level;

		}
		public override void AddModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}

			userDB.AddModifiers(type, new StatsModifier(currentValue, modeType, this));
		}
		//public override void UpdateModifier(UserDB userDB)
		//{
		//	if (isOpen == false)
		//	{
		//		return;
		//	}
		//	userDB.UpdateModifiers(type, new StatsModifier(currentValue, modeType, this));
		//}


		public override void UpdateData()
		{
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.LEVEL_VETERANCY, tid, (IdleNumber)_level);
		}
		public override void RemoveModifier(UserDB userDB)
		{
			userDB.RemoveModifiers(type, this);
		}
		public void ClickLevelup()
		{
			if (IsMax())
			{
				return;
			}

			if (MaxLevel > 0)
			{
				_level = Mathf.Min(++_level, MaxLevel);
			}
			else
			{
				_level++;
			}
			SetLevel(_level);
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.LEVEL_VETERANCY, tid, (IdleNumber)_level);
			RemoveModifier(PlatformManager.UserDB);
			AddModifier(PlatformManager.UserDB);
			PlatformManager.UserDB.veterancyContainer.ConsumePoint(cost);

			if (OnClickLevelup != null)
			{
				OnClickLevelup();
			}


		}
	}
	[System.Serializable]
	public class AbilityInfo : ModifyInfo
	{
		private IdleNumber inc;
		public StatsType type { get; private set; }
		public StatusData rawData { get; private set; }

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

		public AbilityInfo()
		{

		}

		public AbilityInfo(StatsType _type, IdleNumber _value) : this(_type, _value, (IdleNumber)0, StatModeType.Buff, false, false)
		{

		}
		public AbilityInfo(StatsType _type, IdleNumber _value, IdleNumber _inc) : this(_type, _value, _inc, StatModeType.Buff, false, false)
		{

		}

		public AbilityInfo(StatsType _type, IdleNumber _value, IdleNumber _inc, StatModeType _modeType, bool _isPercentage, bool isHyper)
		{
			type = _type;
			BaseValue = _value;
			inc = _inc;
			modeType = _modeType;
			isPercentage = _isPercentage;
			this.isHyper = isHyper;
		}

		public AbilityInfo(ItemStats stats) : this(stats.type, (IdleNumber)stats.value, (IdleNumber)stats.perLevel, stats.modeType, stats.isPercentage, stats.isHyper)
		{
		}

		public string Description()
		{

			return "";
		}
		public IdleNumber GetValueFromZero(int _level = 1)
		{

			int relative = _level;
			if (relative < 0)
			{
				relative = 0;
			}
			var _modified = BaseValue + (inc * relative);

			return _modified;
		}

		public IdleNumber GetValue(int _level = 1)
		{

			int relative = _level - 1;
			if (relative < 0)
			{
				relative = 0;
			}
			var _modified = BaseValue + (inc * relative);

			return _modified;
		}

		public IdleNumber GetNextValue(int _level = 1)
		{
			int relative = _level - 1;
			if (relative < 0)
			{
				relative = 0;
			}
			IdleNumber nextValue = BaseValue + (inc * relative);

			return nextValue;
		}

		public override string ToString()
		{
			return $"{type}. +{Value.ToString()}";
		}

		public AbilityInfo Clone()
		{
			AbilityInfo temp = new AbilityInfo();
			temp.tid = tid;
			temp.type = type;
			temp.BaseValue = BaseValue;
			temp.isDirty = isDirty;
			temp.inc = inc;
			temp.modeType = modeType;
			temp.isPercentage = isPercentage;

			return temp;
		}
	}




}





