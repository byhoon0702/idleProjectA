
using UnityEngine;
using System.Collections;
using System;
using System.Linq;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RuntimeData
{

	[System.Serializable]
	public class SkillInfo : ItemInfo
	{
		#region 수정 가능한 값
		[SerializeField] private int evolutionLevel;
		public int EvolutionLevel => evolutionLevel;
		[SerializeField] private float cooldownValue;
		public float CooldownValue => cooldownValue;


		#endregion

		public string Description { get; private set; }
		public int TargetCount => levelSheet.targetCount;
		public float AttackRange => levelSheet.attackRange;
		public int AttackCount => levelSheet.attackCount;
		public float Interval => levelSheet.attackInterval;
		public float HitRange => levelSheet.hitRange;
		public int HitCount => levelSheet.hitCount;
		public float Duration => levelSheet.duration;
		public float KnockbackPower => levelSheet.knockbackPower;

		public bool isEquipped = false;

		#region 수정되면 안되는 필드
		public int maxLevel
		{
			get
			{
				if (levelSheet != null)
				{
					return levelSheet.maxLevel == 0 ? 100 : levelSheet.maxLevel;
				}
				return 100;
			}
		}

		public int EvolutionMaxLevel { get; private set; }


		public Sprite Icon
		{
			get
			{
				if (itemObject != null)
				{
					return itemObject.Icon;
				}
				return null;
			}

		}


		public bool Instant => itemObject.Instant;
		public bool IsSkillState => itemObject.IsChangeState;
		public Grade grade
		{
			get
			{

				return rawData.itemGrade;
			}
		}
		public override IdleNumber BaseValue
		{
			get
			{
				return skillAbility.Value;
			}
		}

		public SkillCooldownType CooldownType => rawData.detailData.cooldownType;
		public string Name
		{
			get
			{
				string name_key = rawData.name;

				string language = PlatformManager.Language[name_key];
				return language;
			}
		}

		public bool HideInUI => rawData.hideInUI;
		public Sprite icon => itemObject != null ? itemObject.Icon : null;
		public SkillData rawData { get; private set; }
		public SkillLevelSheet levelSheet { get; private set; }
		public SkillCore itemObject { get; private set; }

		public SkillActiveType activeType
		{
			get
			{
				if (rawData.detailData == null)
				{
					return SkillActiveType.ACTIVE;

				}
				return rawData.detailData.activeType;
			}
		}


		public AbilityInfo skillAbility { get; private set; }
		#endregion


		public SkillInfo()
		{

		}

		public SkillInfo(long _tid)
		{
			tid = _tid;
			SetRawData(DataManager.Get<SkillDataSheet>().Get(tid));
		}

		public SkillInfo Clone()
		{
			SkillInfo clone = new SkillInfo();

			clone._level = _level;
			clone.rawData = rawData;
			clone.tid = tid;
			clone._count = _count;
			clone._value = _value;
			clone.evolutionLevel = evolutionLevel;
			clone.levelSheet = levelSheet;
			clone.itemObject = itemObject;

			clone.skillAbility = skillAbility;
			clone.EvolutionMaxLevel = EvolutionMaxLevel;
			return clone;
		}
		public override void SetRawData<T>(T data)
		{
			rawData = data as SkillData;
			tid = rawData.tid;
			itemObject = PlatformManager.UserDB.skillContainer.GetScriptableObject<SkillCore>(tid);

			EvolutionMaxLevel = 0;
			if (rawData.levelSheet != null && rawData.levelSheet.Count > 0)
			{
				EvolutionMaxLevel = rawData.levelSheet[rawData.levelSheet.Count - 1].evolutionLevel;
			}

			UpdateAbilities();
			SetLevelSheetData();

			if (rawData.detailData != null)
			{
				cooldownValue = rawData.detailData.cooldownValue;
			}
		}

		private void SetLevelSheetData()
		{
			if (levelSheet == null)
			{
				levelSheet = new SkillLevelSheet();
			}

			if (rawData.levelSheet != null)
			{
				for (int i = 0; i < rawData.levelSheet.Count; i++)
				{
					var _levelsheet = rawData.levelSheet[i];
					if (_levelsheet.evolutionLevel > evolutionLevel)
					{
						break;
					}
					levelSheet = _levelsheet;


				}

				if (levelSheet.description_key.IsNullOrEmpty() == false)
				{
					string knockBack = levelSheet.knockbackPower > 0 ? PlatformManager.Language["str_skill_knockback"] : "";
					string status =
					Description = string.Format(PlatformManager.Language[levelSheet.description_key],
						skillAbility.type.ToUIString(),
						skillAbility.GetValue(Level).ToString(),
						levelSheet.targetCount,
						levelSheet.attackRange,
						levelSheet.attackCount,
						levelSheet.attackInterval,
						levelSheet.hitRange,
						levelSheet.hitCount,
						knockBack,
						levelSheet.duration,
						PlatformManager.Language[levelSheet.statusEffect.ToUIString()]);

				}
			}
		}

		public bool IsMax()
		{
			return _level >= maxLevel;
		}

		public bool CanLevelUp(out string message)
		{
			message = "";
			if (_count < LevelUpNeedCount())
			{
				message = "필요 수량이 부족합니다";
				return false;
			}
			if (_level >= maxLevel)
			{
				_level = maxLevel;
				message = "최대 레벨입니다";
				return false;
			}

			return true;
		}
		public void LevelUp()
		{
			if (_level >= maxLevel)
			{
				_level = maxLevel;
				return;
			}

			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.LEVELUP_SKILL, tid, (IdleNumber)1);
			_count -= LevelUpNeedCount();
			SetLevel(++_level);
		}
		public void Evoltion(bool useQuest = true)
		{
			if (evolutionLevel >= EvolutionMaxLevel)
			{
				return;
			}

			evolutionLevel++;
			SetLevelSheetData();

			if (useQuest)
			{
				PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EVOLUTION_SKILL, tid, (IdleNumber)1);
			}
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}

			base.Load(info);
			if (_level >= maxLevel)
			{
				_level = maxLevel;
			}

			SkillInfo temp = info as SkillInfo;
			evolutionLevel = temp.evolutionLevel;

			if (evolutionLevel > EvolutionMaxLevel)
			{
				evolutionLevel = EvolutionMaxLevel;

			}
			SetLevel(_level);
			SetLevelSheetData();
		}

		public override void UpdateData()
		{
			//unlock = true;
			//if (_level == 0)
			//{
			//	_level = 1;
			//}
			SetLevel(_level);
			SetLevelSheetData();
		}
		public override void SetLevel(int level)
		{
			_level = level;
			if (_level >= maxLevel)
			{
				_level = maxLevel;
			}
			UpdateAbilities();
		}

		public void SetEvolutionLevel(int _evolutionLevel)
		{
			if (_evolutionLevel > EvolutionMaxLevel)
			{
				return;
			}

			evolutionLevel = _evolutionLevel;
			SetLevelSheetData();
		}


		public void UpdateAbilities()
		{
			skillAbility = new AbilityInfo(rawData.useValue);
			skillAbility.GetValue(_level);
		}

		public bool IsSkillOpen()
		{
			return true;
		}

		public int EvolutionNeedCount()
		{
			var data = PlatformManager.CommonData.SkillEvolutionNeedsList.Find(x => x.level == evolutionLevel + 1);
			if (data.Equals(default))
			{
				return 100;
			}
			return data.count;
		}
		public int LevelUpNeedCount()
		{
			return 5 * (1 + (Mathf.FloorToInt(_level / 10f)));
		}

		public string MakeDescription(string color = "white")
		{
			string desc = "";
			if (levelSheet.description_key.IsNullOrEmpty() == false)
			{
				desc = string.Format(PlatformManager.Language[levelSheet.description_key],
					skillAbility.type.ToUIString(),
					$"<color={color}>{skillAbility.GetValue(Level).ToString()}</color>",
					$"<color={color}>{levelSheet.targetCount}</color>",
					$"<color={color}>{levelSheet.attackRange}</color>",
					$"<color={color}>{levelSheet.attackCount}</color>",
					$"<color={color}>{levelSheet.attackInterval}</color>",
					$"<color={color}>{levelSheet.hitRange}</color>",
					$"<color={color}>{levelSheet.hitCount}</color>",
					$"<color={color}>{levelSheet.knockbackPower}</color>",
					$"<color={color}>{levelSheet.duration}</color>",
					$"<color={color}>{PlatformManager.Language[levelSheet.statusEffect.ToUIString()]}</color>");
			}
			return desc;
		}

		public void MakeCompareDescritption(SkillInfo nextInfo, out string currentDesc, out string nextDesc)
		{
			string[] currentColor = Enumerable.Repeat("white", 10).ToArray();
			string[] nextColor = Enumerable.Repeat("white", 10).ToArray();

			var nextSheet = nextInfo.levelSheet;
			var nextSkillAbility = nextInfo.skillAbility;

			List<bool> compareList = new List<bool>();

			compareList.Add(skillAbility.GetValue(Level).Equals(nextSkillAbility.GetValue(nextInfo.Level)) == false);
			compareList.Add(levelSheet.targetCount != nextSheet.targetCount);
			compareList.Add(levelSheet.attackRange != nextSheet.attackRange);
			compareList.Add(levelSheet.attackCount != nextSheet.attackCount);
			compareList.Add(levelSheet.attackInterval != nextSheet.attackInterval);
			compareList.Add(levelSheet.hitRange != nextSheet.hitRange);
			compareList.Add(levelSheet.hitCount != nextSheet.hitCount);
			compareList.Add(levelSheet.knockbackPower != nextSheet.knockbackPower);
			compareList.Add(levelSheet.duration != nextSheet.duration);
			compareList.Add(levelSheet.statusEffect != nextSheet.statusEffect);
			for (int i = 0; i < compareList.Count; i++)
			{
				if (compareList[i])
				{
					{ currentColor[i] = "red"; nextColor[i] = "green"; }
				}
			}

			currentDesc = string.Format(PlatformManager.Language[levelSheet.description_key],
				skillAbility.type.ToUIString(),
				$"<color={currentColor[0]}>{skillAbility.GetValue(Level).ToString()}</color>",
				$"<color={currentColor[1]}>{levelSheet.targetCount}</color>",
				$"<color={currentColor[2]}>{levelSheet.attackRange}</color>",
				$"<color={currentColor[3]}>{levelSheet.attackCount}</color>",
				$"<color={currentColor[4]}>{levelSheet.attackInterval}</color>",
				$"<color={currentColor[5]}>{levelSheet.hitRange}</color>",
				$"<color={currentColor[6]}>{levelSheet.hitCount}</color>",
				$"<color={currentColor[7]}>{(levelSheet.knockbackPower > 0 ? PlatformManager.Language["str_skill_knockback"] : "")}</color>",
				$"<color={currentColor[8]}>{levelSheet.duration}</color>",
				$"<color={currentColor[9]}>{PlatformManager.Language[levelSheet.statusEffect.ToUIString()]}</color>");

			nextDesc = string.Format(PlatformManager.Language[nextSheet.description_key],
				nextSkillAbility.type.ToUIString(),
				$"<color={nextColor[0]}>{skillAbility.GetValue(nextInfo.Level).ToString()}</color>",
				$"<color={nextColor[1]}>{nextSheet.targetCount}</color>",
				$"<color={nextColor[2]}>{nextSheet.attackRange}</color>",
				$"<color={nextColor[3]}>{nextSheet.attackCount}</color>",
				$"<color={nextColor[4]}>{nextSheet.attackInterval}</color>",
				$"<color={nextColor[5]}>{nextSheet.hitRange}</color>",
				$"<color={nextColor[6]}>{nextSheet.hitCount}</color>",
				$"<color={nextColor[7]}>{(nextSheet.knockbackPower > 0 ? PlatformManager.Language["str_skill_knockback"] : "")}</color>",
				$"<color={nextColor[8]}>{nextSheet.duration}</color>",
				$"<color={nextColor[9]}>{PlatformManager.Language[nextSheet.statusEffect.ToUIString()]}</color>");
		}
	}
}




