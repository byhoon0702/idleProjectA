using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RuntimeData
{

	[System.Serializable]
	public class HyperClassStat
	{
		public AbilityInfo stats;
	}

	[System.Serializable]
	public class HyperClassGradeInfo
	{
		public List<AbilityInfo> stats;
		public HyperRewardInfo rewardInfo;
		public HyperClassGradeInfo()
		{
			stats = new List<AbilityInfo>();
		}
	}

	[System.Serializable]

	public class HyperClassInfo : StatInfo
	{

		public List<int> subLevels;

		#region 저장 안되는 값
		public HyperClass hyperClass => rawData.hyperClass;
		public List<HyperClassStat> classStats { get; private set; }

		public HyperClassGradeInfo gradeInfo { get; private set; }

		public HyperData rawData { get; private set; }
		#endregion

		public override void AddModifier(UserDB userDB)
		{
			for (int i = 0; i < classStats.Count; i++)
			{
				var ability = classStats[i].stats;
				userDB.AddModifiers(ability.isHyper, ability.type, new StatsModifier(ability.Value, ability.modeType, this)); ;
			}
		}
		public override void UpdateModifier(UserDB userDB)
		{
			for (int i = 0; i < classStats.Count; i++)
			{
				var ability = classStats[i].stats;
				userDB.UpdateModifiers(ability.isHyper, ability.type, new StatsModifier(ability.Value, ability.modeType, this));
			}
		}

		public override void RemoveModifier(UserDB userDB)
		{
			for (int i = 0; i < classStats.Count; i++)
			{
				var ability = classStats[i].stats;
				userDB.RemoveModifiers(ability.isHyper, ability.type, this);
			}

		}

		public void LevelUp()
		{

			if (level >= rawData.maxLevel)
			{
				return;
			}

			bool canUpgrade = true;
			for (int i = 0; i < subLevels.Count; i++)
			{
				if (subLevels[i] == level)
				{
					canUpgrade = false;
					subLevels[i]++;
					break;
				}
			}


			if (canUpgrade)
			{
				level++;
			}


			UpdateStats();
		}

		public void UpdateStats()
		{
			classStats = new List<HyperClassStat>();

			if (gradeInfo != null && gradeInfo.stats != null)
			{
				for (int i = 0; i < gradeInfo.stats.Count; i++)
				{
					HyperClassStat stat = new HyperClassStat();

					stat.stats = gradeInfo.stats[i];
					stat.stats.GetValue(level);
					classStats.Add(stat);

				}
			}

			for (int i = 0; i < rawData.stats.Count; i++)
			{
				HyperClassStat stat = new HyperClassStat();

				stat.stats = new AbilityInfo(rawData.stats[i]);
				stat.stats.GetValue(subLevels[i]);
				classStats.Add(stat);
			}

			gradeInfo = new HyperClassGradeInfo();

			for (int i = 0; i < rawData.rewardStats.Count; i++)
			{
				gradeInfo.stats.Add(new AbilityInfo(rawData.rewardStats[i]));
			}
			gradeInfo.rewardInfo = rawData.rewardinfo.Find(x => x.level == level);

			UpdateModifier(GameManager.UserDB);
		}

		public void Load(RuntimeData.HyperClassInfo _info)
		{
			subLevels = _info.subLevels;
			level = _info.level;

			UpdateStats();
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as HyperData;
			tid = rawData.tid;
			subLevels = new List<int>();
			for (int i = 0; i < rawData.stats.Count; i++)
			{
				subLevels.Add(0);
			}

			//stats = new List<HyperClassStat>();

			//for (int i = 0; i < rawData.stats.Count; i++)
			//{
			//	stats.Add(new HyperClassStat() { level = 0, stats = new AbilityInfo(rawData.stats[i]) });
			//}
			UpdateStats();
		}
	}
}
