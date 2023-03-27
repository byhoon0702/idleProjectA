﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVitalityStageInfo : GameStageInfo
{
	public DungeonInfoData dungeonInfo;
	private ItemData consumeItem;


	public string DungeonName => dungeonInfo.name;
	public string ConsumeItemIcon => consumeItem.Icon;
	public override bool IsShowKillUI => true;
	public override long ConsumeItemTid => consumeItem.tid;
	public IdleNumber consumeItemCount;


	/// <summary>
	/// 현재 최대 처치수
	/// </summary>
	public int KillCount => VGameManager.it.battleRecord.killCount;
	
	/// <summary>
	/// 최대 처치수(UI)
	/// </summary>
	public int MaxKillCount => UserInfo.stage.VitalityKillCount;

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

		// 소비재화
		if (dungeonInfo.itemTidNeed != 0)
		{
			consumeItem = DataManager.GetFromAll<ItemData>(dungeonInfo.itemTidNeed);
			if (consumeItem == null)
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

		var checkMoneyResult = Inventory.it.CheckMoney(ConsumeItemTid, consumeItemCount);
		if (checkMoneyResult.Fail())
		{
			return checkMoneyResult;
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

		if (ConsumeItemTid != 0)
		{
			var consumeResult = Inventory.it.ConsumeItem(ConsumeItemTid, consumeItemCount);
			if (consumeResult.Fail())
			{
				return consumeResult;
			}
		}

		UserInfo.stage.SetPlayingStageLv(WaveType, StageLv);
		StageManager.it.PlayStage(this);

		return playableResult.SetOk();
	}

	public override VResult ClearStage()
	{
		UserInfo.stage.SetVitalityKillCount(VGameManager.it.battleRecord.killCount);
		VLog.Log($"[{StageSubTitle}({stageInfo.tid}) - {WaveType}] 스테이지 클리어. 처치수: {VGameManager.it.battleRecord.killCount}");

		return new VResult().SetOk();
	}
}
