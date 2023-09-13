using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class RelicInfo : ItemInfo
	{
		public const float maxChance = 100f;
		public float chance
		{
			get
			{
				return maxChance - _level;
			}
		}
		public override string ItemName => PlatformManager.Language[rawData.name];
		public IdleNumber BaseValue
		{
			get => (IdleNumber)rawData.ownValue.value;
		}
		public IdleNumber perLevel
		{
			get => (IdleNumber)rawData.ownValue.perLevel;
		}
		public IdleNumber Value
		{
			get
			{
				return BaseValue + (_level * perLevel);
			}
		}

		public override Sprite IconImage => itemObject != null ? itemObject.ItemIcon : null;

		public AbilityInfo ownedAbility;

		public RelicItemData rawData { get; private set; }

		public RelicItemObject itemObject { get; private set; }

		public string Description()
		{
			return $"{ownedAbility.type.ToUIString()} <color=green>+{Value.ToFloatingString()}%</color>";
		}
		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}

			var temp = info as RuntimeData.RelicInfo;
			_level = temp._level;
			_count = temp._count;
			unlock = true;

			UpdateAbility();
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as RelicItemData;
			tid = rawData.tid;

			itemObject = PlatformManager.UserDB.relicContainer.GetScriptableObject<RelicItemObject>(rawData.tid);
			UpdateAbility();
		}

		public void UpdateAbility()
		{
			ownedAbility = new AbilityInfo(rawData.ownValue);
			ownedAbility.GetValue(_level);
		}

		public override void UpdateData()
		{
			RemoveModifiers(PlatformManager.UserDB);
			AddModifiers(PlatformManager.UserDB);
		}

		public void OnLevelUp()
		{
			if (_count == 0)
			{
				ToastUI.Instance.Enqueue("[실패] 개수 부족");
				return;
			}
			if (_level >= rawData.maxLevel)
			{
				ToastUI.Instance.Enqueue("[실패] 최대 레벨");
				return;
			}
			int localChance = Random.Range(0, 100);
			if (localChance > chance)
			{
				ToastUI.Instance.Enqueue("강화 실패");
				_count--;
				return;
			}

			_level++;
			_count--;

			UpdateAbility();
			UpdateData();
			ToastUI.Instance.Enqueue("강화 성공");
			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.LEVELUP_RELIC, tid, (IdleNumber)1);
		}

		public void AddItem(int count)
		{
			_count += count;
		}

		public void AddModifiers(UserDB userDB)
		{
			userDB.AddModifiers(ownedAbility.type, new StatsModifier(Value, ownedAbility.modeType, this));
		}
		public void UpdateModifiers(UserDB userDB)
		{
			userDB.UpdateModifiers(ownedAbility.type, new StatsModifier(Value, ownedAbility.modeType, this));
		}
		public void RemoveModifiers(UserDB userDB)
		{
			userDB.RemoveModifiers(ownedAbility.type, this);
		}
	}
}
