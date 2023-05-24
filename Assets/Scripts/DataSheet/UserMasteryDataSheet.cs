using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UserMasteryData : BaseData
{
	public long preTid;
	public int step;
	public int maxLevel;
	public int consumePoint;
}

[Serializable]
public class UserMasteryDataSheet : DataSheetBase<UserMasteryData>
{


	public List<UserMasteryData> GetByStep(int _step)
	{
		List<UserMasteryData> outMastery = new List<UserMasteryData>();

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].step == _step)
			{
				outMastery.Add(infos[i]);
			}
		}

		return outMastery;
	}

	/// <summary>
	/// 마지막 스텝값 가져옴
	/// </summary>
	/// <returns></returns>
	public int GetStepMax()
	{
		int maxStep = 0;

		for (int i = 0; i < infos.Count; i++)
		{
			maxStep = Mathf.Max(maxStep, infos[i].step);
		}

		return maxStep;
	}
}
