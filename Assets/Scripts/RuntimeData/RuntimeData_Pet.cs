using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class PetInfo : ItemInfo
	{

		public int evolutionLevel;
		public Grade grade => rawData.itemGrade;
		public long SkillTid
		{
			get
			{
				if (rawData == null)
				{
					return 0;

				}
				return rawData.skillTid;
			}
		}


		public string ItemName { get; private set; }

		//런타임에서 로드 할것
		public PetItemObject itemObject { get; private set; }

		public PetData rawData { get; private set; }
		public Sprite Icon => itemObject != null ? itemObject.ItemIcon : null;
		public GameObject PetObject => itemObject != null ? itemObject.PetObject : null;

		public List<AbilityInfo> equipAbilities { get; private set; } = new List<AbilityInfo>();
		public List<AbilityInfo> ownedAbilities { get; private set; } = new List<AbilityInfo>();

		public List<ItemBuffOption> options { get; private set; } = new List<ItemBuffOption>();

		public RuntimeData.SkillInfo PetSkill { get; private set; }
		private int maxEvolutionLevel = 7;
		public PetInfo Clone()
		{
			PetInfo clone = new PetInfo();

			clone.tid = tid;
			clone.unlock = unlock;
			clone._level = _level;
			clone._count = _count;
			clone.itemObject = UnityEngine.Object.Instantiate(itemObject);
			clone.rawData = rawData;
			clone.equipAbilities = equipAbilities;
			clone.ownedAbilities = ownedAbilities;
			clone.evolutionLevel = evolutionLevel;

			clone.PetSkill = new SkillInfo(PetSkill.Tid);
			return clone;
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);

			if (info == null)
			{
				return;
			}
			PetInfo temp = info as PetInfo;
			evolutionLevel = temp.evolutionLevel;

			UpdatePetSkill();
		}
		public bool CanLevelUp()
		{
			return _level < 100;
		}
		public override void SetRawData<T>(T data)
		{
			rawData = data as PetData;
			tid = rawData.tid;
			itemObject = PlatformManager.UserDB.petContainer.GetScriptableObject<PetItemObject>(tid);
			UpdateAbilities();
			ItemName = rawData.name;
		}

		public bool Evolution(bool applyToUserDb = true)
		{
			var data = PlatformManager.CommonData.PetEvolutionLevelDataList[evolutionLevel];
			if (_count < data.consumeCount)
			{
				return false;
			}

			if (evolutionLevel >= maxEvolutionLevel)
			{
				evolutionLevel = maxEvolutionLevel;
				return false;
			}

			_count -= data.consumeCount;
			evolutionLevel++;

			if (applyToUserDb)
			{
				UpdateAbilities();
				UpdatePetSkill();
			}


			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EVOLUTION_PET, tid, (IdleNumber)1);

			return true;
		}

		public void AddOptions(ItemBuffOption option)
		{
			options.Add(option);
		}

		public void UpdateOption(int index, ItemBuffOption option)
		{
			options[index] = option;
		}

		public IdleNumber LevelUpNeedCount()
		{
			var requirement = PlatformManager.CommonData.LevelUpConsumeDataList.Find(x => x.grade == grade);
			var first = requirement.basicConsume;
			var second = ((_level - 1) * requirement.increaseRange) * (1 + (_level * requirement.gradeWeight));
			//return requirement.requirement;
			return (IdleNumber)(first + second);
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

			UpdateModifiers(PlatformManager.UserDB);
			//PlatformManager.UserDB.petContainer.UpdateData();
		}

		public void LevelUP()
		{
			_level++;
			UpdateAbilities();
			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.LEVELUP_PET, tid, (IdleNumber)1);
		}

		public void UpdatePetSkill()
		{
			if (PetSkill == null)
			{
				PetSkill = PlatformManager.UserDB.skillContainer.FindSKill(SkillTid);
				if (PetSkill == null)
				{
					return;
				}
			}
			PetSkill.SetLevel(evolutionLevel + 1);
			PetSkill.SetEvolutionLevel(evolutionLevel);
		}

		public void UpdateModifiers(UserDB userDB)
		{
			if (unlock == false)
			{
				return;
			}
			for (int i = 0; i < ownedAbilities.Count; i++)
			{
				userDB.UpdateModifiers(ownedAbilities[i].type, new StatsModifier(ownedAbilities[i].Value, ownedAbilities[i].modeType, this));
			}
		}
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
	}



}

