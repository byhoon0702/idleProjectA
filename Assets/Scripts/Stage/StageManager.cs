using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private static StageManager instance;
	public static StageManager it => instance;

	public enum StageType
	{
		NONE,
		// 웨이브 전멸시 다음 웨이브
		NORMAL1,
		// 일정 주기로 웨이브 무조건 스폰
		NORMAL2,
		// 일정 주기로 무한 스폰
		INFINITE,
		BOSS
	}

	[Serializable]
	public class StageInfo
	{
		public StageType stageType;

		public string areaName;
		public int act;
		public int stage;

		public string bgCloseName;
		public string bgMiddleName;
		public string bgFarName;

		public List<int> listEnemyWavePreset = new List<int>();

		public StageInfo Clone()
		{
			StageInfo info = new StageInfo();

			info.stageType = stageType;

			info.areaName = areaName;
			info.act = act;
			info.stage = stage;

			info.bgCloseName = bgCloseName;
			info.bgMiddleName = bgMiddleName;
			info.bgFarName = bgFarName;

			info.listEnemyWavePreset.AddRange(listEnemyWavePreset);

			return info;
		}
	}

	[Header("임시 스테이지 데이터")]
	[SerializeField] private StageDataSheet normalStageDataSheet;
	[SerializeField] private StageDataSheet bossStageDataSheet;
	[Space]

	private int currentAct = 1;
	private int currentStage = 1;

	private Dictionary<int/*act*/, Dictionary<int/*stage*/, StageInfo>> dictionaryNormalStage = new Dictionary<int, Dictionary<int, StageInfo>>();
	private Dictionary<int/*act*/, Dictionary<int/*stage*/, StageInfo>> dictionaryBossStage = new Dictionary<int, Dictionary<int, StageInfo>>();
	private StageInfo currentStageInfo = null;

	public StageInfo CurrentStageInfo
	{
		get
		{
			if (currentStageInfo == null)
			{
				currentStageInfo = GetNormalStageInfo(1, 1);
			}
			return currentStageInfo;
		}
	}

	public StageType CurrentStageType
	{
		get
		{
			if (currentStageInfo == null)
			{
				return StageType.NONE;
			}
			return currentStageInfo.stageType;
		}
	}

	private void Start()
	{
		for (int i = 0; i < normalStageDataSheet.stageInfos.Count; i++)
		{
			var stageInfo = normalStageDataSheet.stageInfos[i];
			if (dictionaryNormalStage.ContainsKey(stageInfo.act) == true)
			{
				dictionaryNormalStage[stageInfo.act].Add(stageInfo.stage, stageInfo);
			}
			else
			{
				dictionaryNormalStage.Add(stageInfo.act, new Dictionary<int, StageInfo>());
				dictionaryNormalStage[stageInfo.act].Add(stageInfo.stage, stageInfo);
			}
		}

		for (int i = 0; i < bossStageDataSheet.stageInfos.Count; i++)
		{
			var stageInfo = bossStageDataSheet.stageInfos[i];
			if (dictionaryBossStage.ContainsKey(stageInfo.act) == true)
			{
				dictionaryBossStage[stageInfo.act].Add(stageInfo.stage, stageInfo);
			}
			else
			{
				dictionaryBossStage.Add(stageInfo.act, new Dictionary<int, StageInfo>());
				dictionaryBossStage[stageInfo.act].Add(stageInfo.stage, stageInfo);
			}
		}

		currentStageInfo = GetNormalStageInfo(currentAct, currentStage);
	}

	private void Awake()
	{
		instance = this;
	}

	public StageInfo GetNormalStageInfo(int _act, int _stage)
	{
		if (dictionaryNormalStage.ContainsKey(_act) == true)
		{
			var dic = dictionaryNormalStage[_act];
			if (dic.Count >= _stage)
			{
				return dic[_stage].Clone();
			}
		}
		return null;
	}

	public StageInfo GetBossStageInfo(int _act, int _stage)
	{
		if (dictionaryBossStage.ContainsKey(_act) == true)
		{
			var dic = dictionaryBossStage[_act];
			if (dic.Count >= _stage)
			{
				return dic[_stage].Clone();
			}
		}
		return null;
	}

	public StageInfo GetNextNormalStageInfo(StageInfo _currentStageInfo)
	{
		var currentAct = _currentStageInfo.act;
		var currentStage = _currentStageInfo.stage;

		StageInfo nextStageInfo = null;

		var dic = dictionaryNormalStage[currentAct];
		if (dic.ContainsKey(currentStage + 1) == true)
		{
			nextStageInfo = dictionaryNormalStage[currentAct][currentStage + 1].Clone();
		}
		else
		{
			if (dictionaryNormalStage.ContainsKey(currentAct + 1) == true)
			{
				nextStageInfo = dictionaryNormalStage[currentAct + 1][1].Clone();
			}
		}

		return nextStageInfo;
	}

	public void ClearStage()
	{
		switch (CurrentStageType)
		{
			case StageType.BOSS:
				{
					currentStageInfo = GetNextNormalStageInfo(currentStageInfo);
					currentAct = currentStageInfo.act;
					currentStage = currentStageInfo.stage;
					UnitModelPoolManager.it.ClearPool();
				}
				break;
			case StageType.NORMAL1:
			case StageType.NORMAL2:
				break;
		}
	}

	public void PlayNormalStage()
	{
		var stageInfo = GetNormalStageInfo(currentAct, currentStage);
		currentStageInfo = stageInfo;
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}

	public void PlayBossStage()
	{
		var stageInfo = GetBossStageInfo(currentAct, currentStage);
		currentStageInfo = stageInfo;
		VGameManager.it.ChangeState(GameState.BATTLEEND);
	}
}
