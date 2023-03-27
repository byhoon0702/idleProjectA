using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameNormalStageInfo : GameStageInfo
{
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
		if (WaveType == WaveType.NormalBoss)
		{
			int beforeRecentLv = UserInfo.stage.RecentStageLv(WaveType.Normal);
			int beforePlayingLv = UserInfo.stage.PlayingStageLv(WaveType.Normal);

			if (UserInfo.stage.RecentStageLv(WaveType.Normal) == StageLv)
			{
				UserInfo.stage.SetRecentStageLv(WaveType.Normal, beforeRecentLv + 1);
			}
			UserInfo.stage.SetPlayingStageLv(WaveType.Normal, StageLv + 1);

			VLog.Log($"[{StageSubTitle}({stageInfo.tid}) - {WaveType}] 스테이지 클리어. recent: {beforeRecentLv} -> {UserInfo.stage.RecentStageLv(WaveType.Normal)}, playing: {beforePlayingLv} -> {UserInfo.stage.PlayingStageLv(WaveType.Normal)}");
		}
		else if (WaveType == WaveType.Normal)
		{
			VLog.Log($"[{StageSubTitle}({stageInfo.tid}) - {WaveType}] 스테이지 클리어");
		}

		return new VResult().SetOk();
	}
}
