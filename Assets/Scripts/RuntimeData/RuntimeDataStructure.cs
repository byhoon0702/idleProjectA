
using Unity.VisualScripting;
using UnityEngine;

namespace RuntimeData
{
	public interface IItemInfo
	{
		void SetRawData();
	}

	[System.Serializable]
	public struct TrainingAbility
	{
		public Ability type;
		public IdleNumber value;
		public IdleNumber cost;

		public TrainingItemData currentData;
		public TrainingItemData nextData;
	}

	[System.Serializable]
	public struct EquipSlot
	{
		public EquipType type;
	}

	[System.Serializable]
	public struct Veterancy
	{

	}

	[System.Serializable]
	public struct ItemData
	{

		public ItemBuff[] buffs;
	}

	[System.Serializable]
	public class EquipItemInfo : IItemInfo
	{
		public long tid;
		public int level;
		public int count;
		public EquipItemData rawData { get; private set; }
		public EquipItemObject itemObject;

		public void SetRawData()
		{
			rawData = DataManager.Get<EquipItemDataSheet>().Get(tid);
		}
		public bool CanLevelUp()
		{
			return false;
		}
	}

	[System.Serializable]
	public class CostumeInfo : IItemInfo
	{
		public long tid;
		public int level;
		public int count;

		public CostumeItemObject itemObject;
		public CostumeData rawData { get; private set; }
		public string ItemName => itemObject != null ? itemObject.ItemName : "No Data";

		public AbilityInfo[] ownedAbilities => itemObject != null ? itemObject.OwnedAbilities : new AbilityInfo[0];

		public bool CanLevelUp()
		{
			return false;
		}
		public void SetRawData()
		{
			rawData = DataManager.Get<CostumeDataSheet>().Get(tid);
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
	public class PetInfo : IItemInfo
	{
		public long tid;
		public int level;
		public int count;

		public PetItemObject itemObject;

		public PetData rawData { get; private set; }
		public Sprite Icon => itemObject != null ? itemObject.Icon : null;
		public GameObject PetObject => itemObject != null ? itemObject.PetObject : null;
		public string ItemName => itemObject != null ? itemObject.ItemName : "No Data";
		public AbilityInfo[] OwnedAbilities => itemObject != null ? itemObject.OwnedAbilities : new AbilityInfo[0];

		public System.Collections.Generic.List<ItemBuffOption> options { get; private set; } = new System.Collections.Generic.List<ItemBuffOption>();
		public bool CanLevelUp()
		{
			return false;
		}
		public void SetRawData()
		{
			rawData = DataManager.Get<PetDataSheet>().Get(tid);
		}

		public void AddOptions(ItemBuffOption option)
		{
			options.Add(option);
		}

		public void UpdateOption(int index, ItemBuffOption option)
		{
			options[index] = option;
		}
	}



	[System.Serializable]
	public class TrainingInfo
	{
		private const string formula = "({0}+(({1}-1)*{2}))*({3}*{1})";

		public Ability type => data.type;

		public bool isLock;

		public int level;

		public IdleNumber cost
		{
			get;
			private set;
		}

		public IdleNumber currentValue;
		public IdleNumber nextValue;

		public string Name => data.name;
		public string Description => data.description;

		private TrainingData data;


		private IdleNumber basicValue;
		public IdleNumber perLevelValue;

		public delegate void TrainingInfoDelegate();
		public event TrainingInfoDelegate OnClickLevelup;

		public event OnAbilityUpate OnAbilityUpdate;

		public TrainingInfo(TrainingData _data, int _level)
		{
			data = _data;
			level = _level;

			basicValue = (IdleNumber)data.basicValue;
			perLevelValue = (IdleNumber)data.perLevelValue;

			GetCost();
			GetValue();
			GetNextValue();
		}

		~TrainingInfo()
		{
			OnClickLevelup = null;
		}

		public IdleNumber GetCost()
		{
			cost = (IdleNumber)((data.basicCost + (Mathf.Max(0, (level - 1)) * data.basicCostInc)) * (data.basicCostWeight * level));
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

		public void Click()
		{
			level++;
			GetCost();
			GetValue();
			GetNextValue();
			if (OnClickLevelup != null)
			{
				OnClickLevelup();
			}

			if (OnAbilityUpdate != null)
			{
				OnAbilityUpdate(currentValue);
			}
		}
	}



	[System.Serializable]
	public class SkillInfo : IItemInfo
	{
		public long tid;
		public int level;
		public int count;

		public Sprite icon => objectInfo != null ? objectInfo.iconResource : null;
		public SkillData rawData { get; private set; }
		public SkillInfoObject objectInfo;
		public SkillItemObject itemObject;
		public void SetRawData()
		{
			rawData = DataManager.Get<SkillDataSheet>().Get(tid);
		}
		public void AddLevel()
		{
			level++;
		}

		public void Get(UnitStats stats)
		{
			//objectInfo.mainSkillInfo.Calculate(stats, )
		}
	}
}


public delegate void Training(ref IdleNumber a);
public delegate void Equip(ref IdleNumber a, Ability type);
public delegate void Buff(ref IdleNumber a, Ability type);

[System.Serializable]
public class UserAbility
{
	[SerializeField] private Ability type;
	public Ability Type => type;
	[SerializeField] private IdleNumber value;

	[SerializeField] private IdleNumber baseValue;
	[SerializeField] private IdleNumber modifiedValue;

	public IdleNumber min;
	public IdleNumber max;

	private event Training OnTraining;
	private event Buff OnBuff;
	private event Equip OnEquip;

	public StatusData rawData
	{
		get;
		private set;
	}

	public UserAbility(Ability type, IdleNumber value, IdleNumber min, IdleNumber max)
	{
		this.type = type;
		this.baseValue = value;
		this.min = min;
		this.max = max;

		rawData = DataManager.Get<StatusDataSheet>().GetData(type);
	}

	~UserAbility()
	{
		OnTraining = null;
		OnBuff = null;
		OnEquip = null;
	}

	public void SetValue(IdleNumber _value)
	{
		modifiedValue = _value;
	}

	public void RegisterEquipEvent(Equip onEquip)
	{
		if (OnEquip.IsRegistered(onEquip) == false)
		{
			OnEquip += onEquip;
		}
	}

	public void RegisterTrainingEvent(Training onAdd)
	{
		if (OnTraining.IsRegistered(onAdd) == false)
		{
			OnTraining += onAdd;
		}

	}

	public void RegisterBuffEvent(Buff onAdd)
	{
		if (OnBuff.IsRegistered(onAdd) == false)
		{
			OnBuff += onAdd;
		}
	}

	public IdleNumber GetValue()
	{
		return modifiedValue;
	}

	public void UpdateValue()
	{
		IdleNumber add = (IdleNumber)0;
		IdleNumber multi = (IdleNumber)1;

		if (OnTraining != null)
		{
			OnTraining(ref add);
		}
		if (OnBuff != null)
		{
			OnBuff(ref multi, type);
		}
		if (OnEquip != null)
		{
			OnEquip(ref multi, type);
		}

		modifiedValue = (baseValue + add) * (multi);
	}
}

public struct Economy
{
	[SerializeField] private long tid;
}
