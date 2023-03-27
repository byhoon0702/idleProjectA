using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNightmareTowerStageInfo : GameStageInfo
{
	public DungeonInfoData dungeonInfo;

	public int CurrentFloor => UserInfo.stage.RecentStageLv(WaveType.NightmareTower);



	public override VResult Setup(StageInfoData _stageInfo, StageWaveData _waveInfo)
	{
		var result = base.Setup(_stageInfo, _waveInfo);

		if (result.Fail())
		{
			return result;
		}

		dungeonInfo = DataManager.Get<DungeonInfoDataSheet>().Get(WaveType);
		if (dungeonInfo == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"DungeonInfoDataSheet. stageinfoTid: {stageInfo.tid}, WaveType: {WaveType}");
		}

		return result.SetOk();
	}
	public override VResult IsPlayable()
	{
		if (IsStageOpend() == false)
		{
			return new VResult().SetFail(VResultCode.STAGE_NO_OPEND);
		}

		return new VResult().SetOk();
	}

	public override VResult Play()
	{
		var playableResult = IsPlayable();
		if (playableResult.Fail())
		{
			return playableResult;
		}

		UserInfo.stage.SetPlayingStageLv(WaveType, StageLv);
		StageManager.it.PlayStage(this);

		return playableResult.SetOk();
	}


	public override VResult ClearStage()
	{
		int beforeRecentLv = UserInfo.stage.RecentStageLv(WaveType);
		if (UserInfo.stage.RecentStageLv(WaveType) == StageLv)
		{
			UserInfo.stage.SetRecentStageLv(WaveType, beforeRecentLv + 1);
		}

		VLog.Log($"[{StageSubTitle}({stageInfo.tid}) - {WaveType}] 스테이지 클리어. recent: {beforeRecentLv} -> {UserInfo.stage.RecentStageLv(WaveType)}");
		return new VResult().SetOk();
	}
}
