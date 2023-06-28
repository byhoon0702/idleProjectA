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





		public HyperData rawData { get; private set; }

		public HyperClassObject itemObject { get; private set; }
		#endregion


		public override void AddModifier(UserDB userDB)
		{

		}
		public override void UpdateModifier(UserDB userDB)
		{

		}

		public override void RemoveModifier(UserDB userDB)
		{


		}

		public void LevelUp()
		{


		}

		public void UpdateStats()
		{

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

			UpdateStats();
		}
	}
}
