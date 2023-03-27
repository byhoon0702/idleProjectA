using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameImmortalHubStageInfo : GameStageInfo
{
	public DungeonInfoData dungeonInfo;
	private ItemData consumeItem;

	public override bool IsShowDPSUI => true;
	public override long ConsumeItemTid => consumeItem.tid;
	public IdleNumber consumeItemCount;

	/// <summary>
	/// 현재 넣은 대미지 총량
	/// </summary>
	public IdleNumber TotalHitDamage => VGameManager.it.battleRecord.playerDPS.attackPower + VGameManager.it.battleRecord.petDPS.attackPower;

	/// <summary>
	/// 최대 스코어(UI용)
	/// </summary>
	public IdleNumber MaxHitDamage => UserInfo.stage.ImmortalDPS;


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

		var consumeResult = Inventory.it.ConsumeItem(ConsumeItemTid, consumeItemCount);
		if (consumeResult.Fail())
		{
			return consumeResult;
		}

		UserInfo.stage.SetPlayingStageLv(WaveType, StageLv);
		StageManager.it.PlayStage(this);

		return playableResult.SetOk();
	}


	public override VResult ClearStage()
	{
		var totalDPS = VGameManager.it.battleRecord.playerDPS.attackPower + VGameManager.it.battleRecord.petDPS.attackPower;
		UserInfo.stage.SetImmortalDPS(totalDPS);
		VLog.Log($"[{StageSubTitle}({stageInfo.tid}) - {WaveType}] 스테이지 클리어. 총 대미지: {totalDPS.ToString()}");

		return new VResult().SetOk();
	}
}
