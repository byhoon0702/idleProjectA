using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class JuvenescenceElementStat
	{
		[SerializeField] private int point;
		public int Point => point;
		public AbilityInfo stats { get; private set; }

		public void SetStats(ItemStats stats)
		{
			this.stats = new AbilityInfo(stats);
		}

		public void SetPoint(int _point)
		{
			point = _point;
		}

		public IdleNumber GetValue()
		{
			return stats.GetValue(point + 1);
		}
	}
	[System.Serializable]
	public class JuvenescenceElementInfo
	{
		[SerializeField] private int point;
		public int Point => point;


		[SerializeField] private List<JuvenescenceElementStat> statsInfo;
		public List<JuvenescenceElementStat> StatsInfo => statsInfo;
		public JuvenescenceElement rawData { get; private set; }

		public void Load(JuvenescenceElementInfo _info)
		{
			point = _info.point;
			for (int i = 0; i < statsInfo.Count; i++)
			{
				statsInfo[i].SetPoint(_info.statsInfo[i].Point);
			}
		}

		public void SetRawData(JuvenescenceElement _data)
		{
			rawData = _data;
			statsInfo = new List<JuvenescenceElementStat>();

			for (int i = 0; i < rawData.stats.Count; i++)
			{
				JuvenescenceElementStat stat = new JuvenescenceElementStat();
				stat.SetPoint(0);
				stat.SetStats(rawData.stats[i]);
				statsInfo.Add(stat);
			}
		}

		public void LevelUp()
		{
			if (point >= rawData.maxPoint)
			{
				return;
			}
			point++;
			int randomIndex = Random.Range(0, statsInfo.Count);

			statsInfo[randomIndex].SetPoint(Mathf.Min(rawData.maxLevel, (statsInfo[randomIndex].Point + Random.Range(rawData.minLevelUp, rawData.maxLevelUp + 1))));

		}

		public void SetPoint(int _point)
		{
			point = _point;
		}

		public void SetList(List<JuvenescenceElementStat> _statsInfo)
		{
			statsInfo = _statsInfo;

		}
	}

	[System.Serializable]
	public class JuvenescenceInfo : StatInfo
	{
		public bool unlock { get; private set; }
		public int point;

		public int page => rawData.level;
		public List<JuvenescenceElementInfo> infos;
		public JuvenescenceData rawData { get; private set; }

		public List<AbilityInfo> modifiedAbilities { get; private set; } = new List<AbilityInfo>();
		public override void AddModifier(UserDB userDB)
		{
			var enumer = infos.GetEnumerator();
			while (enumer.MoveNext())
			{
				var info = enumer.Current;
				var subEnumer = info.StatsInfo.GetEnumerator();
				while (subEnumer.MoveNext())
				{
					var data = subEnumer.Current;
					userDB.AddModifiers(data.stats.isHyper, data.stats.type, new StatsModifier(data.stats.Value, data.stats.modeType, this));
				}

			}
		}

		public override void RemoveModifier(UserDB userDB)
		{
			var enumer = infos.GetEnumerator();
			while (enumer.MoveNext())
			{
				var info = enumer.Current;
				var subEnumer = info.StatsInfo.GetEnumerator();
				while (subEnumer.MoveNext())
				{
					var data = subEnumer.Current;
					userDB.RemoveModifiers(data.stats.isHyper, data.stats.type, this);
				}

			}
		}
		public override void UpdateModifier(UserDB userDB)
		{
			var enumer = infos.GetEnumerator();
			while (enumer.MoveNext())
			{
				var info = enumer.Current;
				var subEnumer = info.StatsInfo.GetEnumerator();
				while (subEnumer.MoveNext())
				{
					var data = subEnumer.Current;
					userDB.UpdateModifiers(data.stats.isHyper, data.stats.type, new StatsModifier(data.stats.Value, data.stats.modeType, this));
				}

			}
		}
		public void LevelUp(JuvenescenceElementInfo info)
		{
			if (point >= rawData.maxPoint)
			{
				return;
			}
			point++;
			info.LevelUp();
			UpdateModifier(GameManager.UserDB);
		}

		public void UpdateStats()
		{

		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as JuvenescenceData;

			infos = new List<JuvenescenceElementInfo>();

			for (int i = 0; i < rawData.elements.Count; i++)
			{
				JuvenescenceElementInfo info = new JuvenescenceElementInfo();
				info.SetRawData(rawData.elements[i]);
				infos.Add(info);
			}
			unlock = false;
			point = 0;
		}

		public void Load(JuvenescenceInfo _info)
		{
			unlock = _info.unlock;
			point = _info.point;
			//infos = new List<JuvenescenceElementInfo>(_info.infos);

			for (int i = 0; i < infos.Count; i++)
			{
				infos[i].Load(_info.infos[i]);
			}
		}


	}
}
