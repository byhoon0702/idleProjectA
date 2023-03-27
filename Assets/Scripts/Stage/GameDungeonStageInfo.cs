using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDungeonStageInfo : GameStageInfo
{
	public DungeonInfoData dungeonInfo;
	private ItemData consumeItem;

	public string DungeonName => dungeonInfo.name;
	public string ConsumeItemIcon => consumeItem.Icon;
	public override long ConsumeItemTid => consumeItem.tid;
	public IdleNumber consumeItemCount;



	public override VResult Setup(StageInfoData _stageInfo, StageWaveData _waveInfo)
	{
		var result = base.Setup(_stageInfo, _waveInfo);

		if(result.Fail())
		{
			return result;
		}

		dungeonInfo = DataManager.Get<DungeonInfoDataSheet>().Get(WaveType);
		if (dungeonInfo == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"DungeonInfoDataSheet. stageinfoTid: {stageInfo.tid}, WaveType: {WaveType}");
		}

		// 소비재화
		if (dungeonInfo.itemTidNeed != 0)
		{
			consumeItem = DataManager.GetFromAll<ItemData>(dungeonInfo.itemTidNeed);
			if(consumeItem == null)
			{
				return result.SetFail(VResultCode.NO_META_DATA, $"ItemData. stageinfoTid: {stageInfo.tid}, dungeonInfo.itemTidNeed: {dungeonInfo.itemTidNeed}");
			}
			consumeItemCount = (IdleNumber)dungeonInfo.itemCount;
		}
		else
		{
			consumeItem = null;
		}

		return result.SetOk();
	}

	public override VResult IsPlayable()
	{
		if (IsStageOpend() == false)
		{
			return new VResult().SetFail(VResultCode.STAGE_NO_OPEND);
		}

		if (ConsumeItemTid != 0)
		{
			var checkMoneyResult = Inventory.it.CheckMoney(ConsumeItemTid, consumeItemCount);
			if (checkMoneyResult.Fail())
			{
				return checkMoneyResult;
			}
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

		// 재화 소비는 스테이지 클리어 시점에 처리
		//var consumeResult = Inventory.it.ConsumeItem(ConsumeItemTid, consumeItemCount);
		//if (consumeResult.Fail())
		//{
		//	return consumeResult;
		//}

		UserInfo.stage.SetPlayingStageLv(WaveType, StageLv);
		StageManager.it.PlayStage(this);

		return playableResult.SetOk();
	}

	public override VResult ClearStage()
	{
		var consumeResult = Inventory.it.ConsumeItem(ConsumeItemTid, consumeItemCount);
		if (consumeResult.Fail())
		{
			return consumeResult;
		}

		int beforeRecentLv = UserInfo.stage.RecentStageLv(WaveType);
		if (UserInfo.stage.RecentStageLv(WaveType) == StageLv)
		{
			UserInfo.stage.SetRecentStageLv(WaveType, beforeRecentLv + 1);
		}

		VLog.Log($"[{StageSubTitle}({stageInfo.tid}) - {WaveType}] 스테이지 클리어. recent: {beforeRecentLv} -> {UserInfo.stage.RecentStageLv(WaveType)}");
		return new VResult().SetOk();
	}
}
