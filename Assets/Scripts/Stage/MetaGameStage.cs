using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaGameStage
{
	public Dictionary<WaveType, List<GameStageInfo>> stages = new Dictionary<WaveType, List<GameStageInfo>>();



	public VResult Setup()
	{
		VResult _result = new VResult();
		stages.Clear();

		// 데이터 초기화
		foreach (var stageInfo in DataManager.Get<StageInfoDataSheet>().infos)
		{
			var waveInfo = DataManager.Get<StageWaveDataSheet>().Get(stageInfo.stageWaveTid);
			if (waveInfo == null)
			{
				VLog.LogError($"no metadata. StageWaveDataSheet. stageinfoTid: {stageInfo.tid}, stagewavetid: {stageInfo.stageWaveTid}");
				continue;
			}

			GameStageInfo gameStageInfo;
			switch (waveInfo.waveType)
			{
				case WaveType.Normal:
				case WaveType.NormalBoss:
					gameStageInfo = new GameNormalStageInfo();
					break;
				case WaveType.BossRush_Dark:
				case WaveType.BossRush_Fire:
				case WaveType.BossRush_Ice:
				case WaveType.BossRush_Ship:
				case WaveType.BossRush_Stone:
				case WaveType.BossRush_Volcano:
					gameStageInfo = new GameBossRushStageInfo();
					break;
				case WaveType.NightmareTower:
					gameStageInfo = new GameNightmareTowerStageInfo();
					break;
				case WaveType.ImmortalHub:
					gameStageInfo = new GameImmortalHubStageInfo();
					break;
				case WaveType.Vitality:
					gameStageInfo = new GameVitalityStageInfo();
					break;
				default:
					gameStageInfo = new GameDungeonStageInfo();
					break;
			}

			_result = gameStageInfo.Setup(stageInfo, waveInfo);
			if (_result.Fail())
			{
				return _result;
			}

			if(stages.ContainsKey(gameStageInfo.WaveType) == false)
			{
				stages.Add(gameStageInfo.WaveType, new List<GameStageInfo>());
			}

			stages[gameStageInfo.WaveType].Add(gameStageInfo);
		}

		// 정렬
		foreach (var gameStageInfo in stages.Values)
		{
			gameStageInfo.Sort((a, b) =>
			{
				return a.StageLv.CompareTo(b.StageLv);
			});
		}

		return _result.SetOk();
	}

	public GameStageInfo GetStage(WaveType _waveType, int _stageLevel)
	{
		if(stages.ContainsKey(_waveType) == false)
		{
			return null;
		}


		foreach(var stage in stages[_waveType])
		{
			if(stage.StageLv == _stageLevel)
			{
				return stage;
			}
		}

		return null;
	}

	public List<GameStageInfo> GetStages(WaveType _waveType)
	{
		if (stages.ContainsKey(_waveType) == false)
		{
			return null;
		}

		return stages[_waveType];
	}

	/// <summary>
	/// areaIndex가 -1이면 체크하지않음
	/// </summary>
	public List<GameStageInfo> GetStages(WaveType _waveType, StageDifficulty _difficult, int _areaIndex = -1)
	{
		var stages = GetStages(_waveType);
		List<GameStageInfo> outStages = new List<GameStageInfo>();
		foreach (var stage in stages)
		{
			if (stage.Difficult != _difficult)
			{
				continue;
			}

			if(_areaIndex != -1 && _areaIndex != stage.AreaIndex)
			{
				continue;
			}

			outStages.Add(stage);
		}

		return outStages;
	}

	/// <summary>
	/// 마지막 AreaIndex 반환
	/// </summary>
	public int GetLastAreaIndex(WaveType _waveType, StageDifficulty _difficult)
	{
		var stages = GetStages(_waveType, _difficult);

		return stages[stages.Count - 1].AreaIndex;
	}
}
