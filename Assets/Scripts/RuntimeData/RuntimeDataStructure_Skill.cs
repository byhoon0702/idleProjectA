
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuntimeData
{

	[System.Serializable]
	public class SkillInfo : ItemInfo
	{
		#region 수정 가능한 값
		[SerializeField] private int hitCount;
		public int HitCount => hitCount;
		[SerializeField] private float hitRange;
		public float HitRange => hitRange;

		[SerializeField] private float cooldownValue;


		[SerializeField] private float interval = 0.2f;
		public float Interval => interval;
		public float CooldownValue => cooldownValue;

		public bool isEquipped = false;

		public int maxLevel => rawData.maxLevel;
		#endregion

		#region 수정되면 안되는 필드

		public override IdleNumber BaseValue
		{
			get
			{
				return skillAbility.Value;
			}
		}

		public SkillCooldownType CooldownType => rawData.cooldownType;
		public string Name
		{
			get
			{
				string name_key = rawData.name;

				string language = GameManager.Language[name_key];
				return language;
			}
		}

		public string Description
		{
			get
			{
				string description_key = rawData.description;

				string language = GameManager.Language[description_key];
				return language;
			}
		}
		public bool HideInUI => rawData.hideInUI;
		public Sprite icon => itemObject != null ? itemObject.Icon : null;
		public SkillData rawData { get; private set; }
		public SkillTree skillTree { get; private set; }
		public NewSkill itemObject { get; private set; }
		public AbilityInfo skillAbility;
		#endregion

		public SkillInfo Clone()
		{
			SkillInfo clone = new SkillInfo();
			clone.tid = tid;
			clone.level = level;
			clone.count = count;
			clone.unlock = unlock;
			clone.hitCount = hitCount;
			clone.cooldownValue = cooldownValue;
			clone.hitRange = hitRange;
			clone.isEquipped = isEquipped;
			clone.rawData = rawData;
			clone.itemObject = itemObject;
			clone.skillAbility = skillAbility;

			return clone;
		}

		public SkillInfo()
		{

		}

		public SkillInfo(long _tid)
		{
			tid = _tid;
			SetRawData(DataManager.Get<SkillDataSheet>().Get(tid));
		}


		public override void SetRawData<T>(T data)
		{
			rawData = data as SkillData;
			tid = rawData.tid;
			itemObject = GameManager.UserDB.skillContainer.GetScriptableObject<NewSkill>(tid);
			SetLevelData();
			UpdateAbilities();
			cooldownValue = rawData.cooldownValue;
		}


		private void SetLevelData()
		{
			hitCount = 1;
			interval = 0;
			hitRange = 1;

			for (int i = 0; i < rawData.levelSheet.Count; i++)
			{
				var levelsheet = rawData.levelSheet[i];
				if (levelsheet.level > level)
				{
					break;
				}
				hitCount = levelsheet.attackCount;
				interval = levelsheet.attackInterval;
				hitRange = levelsheet.attackRange;
			}
		}

		public bool IsMax()
		{
			return level >= maxLevel;
		}

		public void LevelUp()
		{
			if (level >= maxLevel)
			{
				level = maxLevel;
				return;
			}
			SetLevel(++level);
		}

		public override void Load(ItemInfo info)
		{
			base.Load(info);
			if (level >= maxLevel)
			{
				level = maxLevel;
			}

			SetLevelData();
			UpdateAbilities();
			UpdateModifier();
		}

		public void SetLevel(int _level)
		{
			level = _level;
			if (level >= maxLevel)
			{
				level = maxLevel;
			}

			SetLevelData();
			UpdateAbilities();
			UpdateModifier();
		}

		public void UpdateModifier()
		{
			switch (rawData.valueModifyTarget)
			{
				case ValueModifyTarget.SKILL:
					{
						var baseSkill = GameManager.UserDB.skillContainer.Get(rawData.rootSkillTid);
						if (baseSkill != null)
						{
							baseSkill.RemoveAllModifiersFromSource(this);
							baseSkill.AddModifiers(new StatsModifier(Value, skillAbility.modeType, this));
						}
					}
					break;
				case ValueModifyTarget.CHARACTER:
					{
						if (level > 0)
						{
							GameManager.UserDB.UserStats.RemoveModifier(skillAbility.type, this);
							GameManager.UserDB.UserStats.AddModifier(skillAbility.type, new StatsModifier(Value, skillAbility.modeType, this));
						}
					}
					break;
			}
		}

		public void Reset()
		{
			level = 0;
			switch (rawData.valueModifyTarget)
			{
				case ValueModifyTarget.SKILL:
					{
						var baseSkill = GameManager.UserDB.skillContainer.Get(rawData.rootSkillTid);
						if (baseSkill != null)
						{
							baseSkill.RemoveAllModifiersFromSource(this);

						}
					}
					break;
				case ValueModifyTarget.CHARACTER:
					{
						GameManager.UserDB.UserStats.RemoveModifier(skillAbility.type, this);

					}
					break;
			}
		}


		public void UpdateAbilities()
		{
			skillAbility = new AbilityInfo(rawData.useValue);
			skillAbility.GetValue(level);
		}

		public bool IsSkillOpen()
		{
			return true;
		}
	}
}




