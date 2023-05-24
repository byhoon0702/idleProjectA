

using UnityEngine;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using JetBrains.Annotations;

[System.Serializable]
public class YouthOptionSlot
{
	public Grade grade;
	public bool isLock = false;
	public RuntimeData.YouthOptionInfo info;

	public void UpdateSlot(RuntimeData.YouthOptionInfo _info)
	{
		if (info != null)
		{
			info.RemoveModifier(GameManager.UserDB);
		}
		info = _info;
		info.AddModifier(GameManager.UserDB);
	}
}


[CreateAssetMenu(fileName = "Youth Container", menuName = "ScriptableObject/Container/Youth Container", order = 1)]
public class YouthContainer : BaseContainer
{
	[SerializeField] private int youthLevel;

	public int YouthLevel
	{
		get
		{

			return Mathf.Max(youthLevel, 1);
		}
	}

	[SerializeField] private int youthCostumeIndex;
	public int YouthCostumeIndex
	{
		get
		{
			return Mathf.Max(youthCostumeIndex, 0);
		}
	}

	public Grade currentGrade;
	public YouthOptionSlot[] slots;

	public List<RuntimeData.YouthInfo> info;
	public YouthData youthData { get; private set; }
	public List<int[]> probRange { get; private set; } = new List<int[]>();

	[NonSerialized] public bool toggleOptionSS = false;
	[NonSerialized] public bool toggleOptionSSS = false;

	public override void Load(UserDB _parent)
	{
		youthData = DataManager.Get<YouthDataSheet>().GetInfosClone()[0];
		SetItemListRawData(ref info, youthData);

		if (slots == null)
		{
			slots = new YouthOptionSlot[(int)(Grade._END - 1)];
			for (int i = 0; i < 6; i++)
			{
				slots[i] = new YouthOptionSlot();
				slots[i].grade = (Grade)i + 1;
				slots[i].isLock = false;
			}
		}

		int sum = 0;
		for (int i = 0; i < youthData.probabilityDatas.Length - 1; i++)
		{
			var probData = youthData.probabilityDatas[i];
			probRange.Add(new int[2] { sum, sum + (int)probData.probability });
			sum += (int)youthData.probabilityDatas[i].probability;
		}
	}
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		YouthContainer temp = CreateInstance<YouthContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		for (int i = 0; i < info.Count; i++)
		{
			info[i].Load(temp.info[i]);
		}
	}

	public int MaxLevel()
	{
		for (int i = 0; i < youthData.hyperBuffs.Length; i++)
		{
			if (youthData.hyperBuffs[i].grade == currentGrade)
			{
				return youthData.hyperBuffs[i].levelLimit;
			}
		}

		return 0;
	}

	public bool RollYouthOption()
	{
		bool stop = false;
		int optionDatacount = youthData.optionDatas.Length;
		for (int i = 0; i < slots.Length; i++)
		{
			var slot = slots[i];
			if (slot.isLock)
			{
				continue;
			}

			if (slot.grade > currentGrade)
			{
				continue;
			}


			int gradePercent = UnityEngine.Random.Range(0, 100);
			Grade selectedGrade = GetGrade(gradePercent);

			var optionData = youthData.optionDatas[UnityEngine.Random.Range(0, optionDatacount)];

			int[] percentRange = new int[2];
			for (int ii = 0; ii < optionData.percentageData.Length; ii++)
			{
				var percentageData = optionData.percentageData[ii];
				if (percentageData.grade == selectedGrade)
				{
					if (ii == 0)
					{
						percentRange[0] = 0;
						percentRange[1] = (int)percentageData.percentage;
					}
					else
					{
						percentRange[0] = (int)optionData.percentageData[ii - 1].percentage;
						percentRange[1] = (int)percentageData.percentage;
					}
					break;
				}
			}

			int selectedPercentage = UnityEngine.Random.Range(percentRange[0], percentRange[1]);
			float value = optionData.min + ((optionData.max - optionData.min) * selectedPercentage / 100f);
			value = Mathf.Floor(value * 10f) / 10f;

			RuntimeData.YouthOptionInfo info = new RuntimeData.YouthOptionInfo(selectedGrade, optionData.type, optionData.modeType, (IdleNumber)value);
			slot.UpdateSlot(info);


			if (toggleOptionSS)
			{
				if (slot.info.grade == Grade.SS)
				{
					stop = true;
				}
			}

			if (toggleOptionSSS)
			{

				if (slot.info.grade == Grade.SSS)
				{
					stop = true;
				}
			}
		}

		if (stop)
		{
			return false;
		}

		return true;
	}

	public Grade GetGrade(int percentage)
	{
		int index = 0;
		for (int i = 0; i < probRange.Count; i++)
		{
			if (percentage >= probRange[i][0] && percentage <= probRange[i][1])
			{
				index = i;
				break;
			}
		}

		return youthData.probabilityDatas[index].grade;

	}

	public bool UpdateGrade()
	{
		for (int i = 0; i < info.Count; i++)
		{
			if (info[i].Level < MaxLevel())
			{
				return false;
			}
		}
		currentGrade++;

		return true;
	}

	public YouthOptionProbData GetData(Grade grade)
	{
		for (int i = 0; i < youthData.optionDatas.Length; i++)
		{
			if (youthData.probabilityDatas[i].grade == grade)
			{
				return youthData.probabilityDatas[i];
			}
		}

		return null;
	}

	public override void LoadScriptableObject()
	{


	}


	private void SetItemListRawData(ref List<RuntimeData.YouthInfo> infos, YouthData datas)
	{
		infos.Clear();
		if (infos == null || infos.Count == 0)
		{
			infos = new List<RuntimeData.YouthInfo>();
			for (int i = 0; i < datas.buffList.Length; i++)
			{
				RuntimeData.YouthInfo data = new RuntimeData.YouthInfo(datas.buffList[i]);
				infos.Add(data);
			}
			return;
		}
		for (int i = 0; i < infos.Count; i++)
		{

			infos[i].SetRawData(datas.buffList[i]);
		}

	}
}
