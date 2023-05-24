using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class YouthOptionInfo : StatInfo
	{
		public Grade grade;
		public StatsType type;
		public StatModeType modeType;
		public bool isHyper;
		public YouthOptionInfo(Grade _grade, StatsType _type, StatModeType _modeType, IdleNumber _value)
		{
			grade = _grade;
			type = _type;
			modeType = _modeType;
			currentValue = _value;
		}

		public override void AddModifier(UserDB userDB)
		{
			userDB.AddModifiers(isHyper, type, new StatsModifier(currentValue, modeType, this));
		}

		public override void RemoveModifier(UserDB userDB)
		{
			userDB.RemoveModifiers(isHyper, type, this);
		}

		public override void SetRawData<T>(T data)
		{

		}

		public override void UpdateModifier(UserDB userDB)
		{

		}
	}

	[System.Serializable]
	public class YouthInfo : StatInfo
	{
		[JsonIgnore] public Sprite icon => itemObject.Icon;
		[JsonIgnore] public StatsType type => rawData.buff.type;

		//public string Name => itemObject.ItemName;
		//public string Description => _data.Description;

		[JsonIgnore] public StatModeType modeType => rawData.buff.modeType;

		[JsonIgnore] public IdleNumber basicValue => (IdleNumber)rawData.buff.value;
		[JsonIgnore] public IdleNumber perLevelValue => (IdleNumber)rawData.buff.perLevel;

		[JsonIgnore] private int maxLevel;
		[JsonIgnore]
		public int MaxLevel
		{
			get
			{
				if (isDirty)
				{
					isDirty = false;
					maxLevel = GameManager.UserDB.youthContainer.MaxLevel();
				}
				return maxLevel;
			}
		}
		public delegate void LevelUpDelegate();
		public event LevelUpDelegate OnClickLevelup;
		[JsonIgnore] private bool isDirty = false;

		[JsonIgnore] public YouthBuffObject itemObject { get; protected set; }
		[JsonIgnore] public YouthBuffData rawData { get; protected set; }
		[JsonIgnore]
		public bool isOpen
		{
			get
			{
				return true;
			}
		}

		public void SetDirty()
		{
			isDirty = true;
		}

		public override void Load(StatInfo info)
		{
			base.Load(info);
			SetLevel(level);
		}
		public void SetLevel(int _level)
		{
			level = _level;
			SetDirty();
			GetCost();
			GetValue();
			GetNextValue();
		}

		public YouthInfo(YouthBuffData _data)
		{
			rawData = _data;
			level = 0;
			SetLevel(level);
		}
		public YouthInfo(YouthBuffObject _itemObject)
		{
			itemObject = _itemObject;
			tid = itemObject.Tid;
			SetLevel(level);
		}

		~YouthInfo()
		{
			OnClickLevelup = null;
		}

		public IdleNumber GetCost()
		{
			cost = (IdleNumber)100 + (Mathf.Max(0, (level)) * 100);
			return cost;
		}

		public IdleNumber GetValue()
		{
			currentValue = (basicValue) + (perLevelValue * Mathf.Max(0, (level)));
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
			return MaxLevel <= level;
		}

		public void AddEvent(LevelUpDelegate listener)
		{
			if (OnClickLevelup.IsRegistered(listener) == false)
			{
				OnClickLevelup += listener;
			}
		}
		public void RemoveEvent(LevelUpDelegate listener)
		{
			if (OnClickLevelup.IsRegistered(listener))
			{
				OnClickLevelup -= listener;
			}
		}
		public override void AddModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}
			SetDirty();
			userDB.AddModifiers(rawData.buff.isHyper, type, new StatsModifier(currentValue, modeType, this));
		}
		public override void UpdateModifier(UserDB userDB)
		{
			if (isOpen == false)
			{
				return;
			}
			SetDirty();
			userDB.UpdateModifiers(rawData.buff.isHyper, type, new StatsModifier(currentValue, modeType, this));
		}

		public override void RemoveModifier(UserDB userDB)
		{
			SetDirty();
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
			SetDirty();
			GetCost();
			GetValue();
			//GetNextValue();

			UpdateModifier(GameManager.UserDB);
			//RemoveModifier(GameManager.UserDB.UserStats);
			//AddModifier(GameManager.UserDB.UserStats);

			if (OnClickLevelup != null)
			{
				OnClickLevelup();
			}
		}

		public override void SetRawData<T>(T data)
		{

		}



	}


}
