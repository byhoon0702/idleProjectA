using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemySpawnInfo
{
	public UnitData unitData;
	public int spawnLevel = 1;


	public EnemySpawnInfo(UnitData _unitData, int _spawnLevel)
	{
		unitData = _unitData;
		spawnLevel = _spawnLevel;
	}

	public override string ToString()
	{
		return $"[{unitData.name}({unitData.tid})] spawnLv: {spawnLevel}";
	}
}

/// <summary>
/// 스테이지 보상용 클래스
/// 원시데이터를 코드에서 바꾸지 못하게 하는 의도도 있음
/// </summary>
public class StageRewardInfo
{
	private MetaRewardInfo metaRewardInfo;

	public long Tid => metaRewardInfo.tid;
	public float DropRatio => metaRewardInfo.dropRatio;


	/// <summary>
	/// 계산된 보상 값
	/// </summary>
	public IdleNumber RewardCount(UnitType _unitType = UnitType._NONE)
	{
		IdleNumber totalCount = (IdleNumber)metaRewardInfo.count;

		if (Tid == Inventory.it.GoldTid || Tid == Inventory.it.UserExpTid)
		{
			// 골드, 유저 경험치는 유닛 타입별로 다름
			switch (_unitType)
			{
				case UnitType.BossEnemy:
					totalCount *= VGameManager.Config.STAGE_REWARD_BOSS_ENEMY_MUL;
					break;
				case UnitType.TreasureBox:
					totalCount *= VGameManager.Config.STAGE_REWARD_TREASURE_BOX_MUL;
					break;
				case UnitType.NormalEnemy:
				default:
					totalCount *= VGameManager.Config.STAGE_REWARD_NORMAL_ENEMY_MUL;
					break;
			}
		}

		return totalCount;
	}

	/// <summary>
	/// 보상 기본값(보통 UI에서 쓸듯)
	/// </summary>
	public IdleNumber RewardCountDefault()
	{
		return RewardCount(UnitType._NONE);
	}


	public StageRewardInfo(MetaRewardInfo _metaRewardInfo)
	{
		metaRewardInfo = _metaRewardInfo;
	}
}



public abstract class GameStageInfo
{
	private static VResult _result = new VResult();
	public StageMapData stageMap;
	public StageInfoData stageInfo;
	public StageWaveData waveInfo;

	public List<EnemySpawnInfo> spawnEnemyInfos = new List<EnemySpawnInfo>();
	public EnemySpawnInfo spawnLast = null; // null체크 필요

	public string StageName => stageMap.name;
	public string StageSubTitle => stageInfo.subTitle;
	public virtual bool IsShowStageNameUI => true;
	public virtual bool IsShowWaveGaugeUI => WaveCount > 1;
	public virtual bool IsShowNormalRewardIcon => WaveType == WaveType.Normal;
	public virtual bool IsShowSpecialRewardIcon => WaveType != WaveType.Normal;
	public virtual bool IsShowHPGaugeUI => OnlyOneBossBattle;
	public virtual bool IsShowTimeGauge => TimeLimit > 0;
	public virtual bool IsShowKillUI => false;
	public virtual bool IsShowDPSUI => false;
	public WaveType WaveType => waveInfo.waveType;
	public int WaveCount => waveInfo.waveCount;
	public int WaveUnitCount => stageInfo.enemyCount;
	public float WaveDistance => waveInfo.waveDistance;
	public StageDifficulty Difficult => stageInfo.difficulty;
	public int AreaIndex => stageMap.areaIndex;
	public long BgTid => stageMap.bgTid;
	public int StageLv => stageInfo.stageLevel;
	public float TimeLimit => waveInfo.timeLimit;
	public virtual long ConsumeItemTid => 0;

	public IdleNumber BestCombatPower => (IdleNumber)StageLv * 1000;

	/// <summary>
	/// 보스전 딱 한번만 하는경우 true(보스가 여러번 나오는경우 false)
	/// </summary>
	public bool OnlyOneBossBattle
	{
		get
		{
			if (WaveCount != 1)
			{
				return false;
			}

			if (waveInfo.spawnTreasureBox || waveInfo.spawnNormal)
			{
				return false;
			}

			return waveInfo.spawnBoss || waveInfo.spawnImmotal;
		}
	}
	public bool IsInfinityWave => waveInfo.waveCount == 0;

	public List<EnemySpawnInfo> SpawnEnemyInfos => spawnEnemyInfos;
	public List<StageRewardInfo> KillRewards { get; private set; }






	/// <summary>
	/// 스테이지 플레이가능.
	/// </summary>
	public abstract VResult IsPlayable();

	/// <summary>
	/// 스테이지 플레이
	/// </summary>
	public abstract VResult Play();

	/// <summary>
	/// 스테이지 클리어
	/// </summary>
	public abstract VResult ClearStage();

	public virtual VResult Setup(StageInfoData _stageInfo, StageWaveData _waveInfo)
	{
		stageInfo = _stageInfo;
		waveInfo = _waveInfo;
		spawnLast = null;


		stageMap = DataManager.Get<StageMapDataSheet>().Get(_stageInfo.stageMapTid);
		if (stageMap == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"StageMapDataSheet. stageInfoTid: {_stageInfo.tid}, mapTid: {_stageInfo.stageMapTid}");
		}


		// 적 스폰정보 초기화
		foreach (var enemy in stageMap.spawnEnemies)
		{
			var unitInfo = DataManager.Get<EnemyUnitDataSheet>().Get(enemy.enemyUnitTid);


			if (waveInfo.spawnBoss && unitInfo.type == UnitType.BossEnemy)
			{
				spawnLast = new EnemySpawnInfo(unitInfo, StageLv);
			}
			else if (waveInfo.spawnTreasureBox && unitInfo.type == UnitType.TreasureBox)
			{
				spawnLast = new EnemySpawnInfo(unitInfo, StageLv);
			}
			else if (waveInfo.spawnImmotal && unitInfo.type == UnitType.BossEnemy)
			{
				spawnLast = new EnemySpawnInfo(unitInfo, StageLv);
			}
			else if (waveInfo.spawnNormal && unitInfo.type == UnitType.NormalEnemy)
			{
				spawnEnemyInfos.Add(new EnemySpawnInfo(unitInfo, StageLv));
			}
		}

		// 보상정보 초기화
		KillRewards = new List<StageRewardInfo>();
		foreach (var reward in stageInfo.killRewards)
		{
			KillRewards.Add(new StageRewardInfo(reward));
		}

		return _result.SetOk();
	}

	/// <summary>
	/// 주요보상 표시용(UI)
	/// </summary>
	public List<ItemData> GetMainRewards()
	{
		List<ItemData> showItemList = new List<ItemData>();
		showItemList.AddRange(GetMainReward_internal(KillRewards));

		return showItemList;
	}

	protected List<ItemData> GetMainReward_internal(List<StageRewardInfo> _rewardInfos)
	{
		List<ItemData> showItemList = new List<ItemData>();
		foreach (var reward in _rewardInfos)
		{
			var itemData = DataManager.GetFromAll<ItemData>(reward.Tid);
			if (itemData == null)
			{
				VLog.LogError($"Invalid Reward tid. {reward.Tid}");
				continue;
			}

			if (itemData.itemType == ItemType.Weapon || itemData.itemType == ItemType.Armor || itemData.itemType == ItemType.Ring || itemData.itemType == ItemType.Necklace)
			{
				showItemList.Add(itemData);
			}
		}

		return showItemList;
	}

	/// <summary>
	/// 전체보상 표시용(UI)
	/// </summary>
	public List<StageRewardInfo> GetAllRewards()
	{
		return KillRewards;
	}

	/// <summary>
	/// 처치보상
	/// </summary>
	public List<StageRewardInfo> GenerateKillReward()
	{
		List<StageRewardInfo> outResult = new List<StageRewardInfo>();

		for (int i = 0; i < KillRewards.Count; i++)
		{
			var info = KillRewards[i];
			if (GetResult(info.DropRatio) == true)
			{
				outResult.Add(info);
			}
		}

		return outResult;
	}

	private bool GetResult(float _ratio)
	{
		float randomRatio = UnityEngine.Random.Range(0.0f, 1.0f);
		if (randomRatio <= _ratio)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// 스테이지에 맞는 유닛 공격력 기본값
	/// </summary>
	public IdleNumber UnitAttackPower(bool isBoss)
	{
		int difficultLv = ((int)Difficult + 1);
		float weight_1 = VGameManager.Config.UNIT_STATE_WEIGHT_1;
		float weight_2 = VGameManager.Config.UNIT_STATE_WEIGHT_2;
		float weight_3 = VGameManager.Config.UNIT_STATE_WEIGHT_3;
		float atkWeight = VGameManager.Config.UNIT_STATE_ATTACK_POWER_WEIGHT;

		IdleNumber defaultValue = waveInfo.DefaultAttackPower;
		float levelWeight = stageInfo.levelWeight;
		float bossWeight = isBoss ? stageInfo.bossWeight : 1;

		var result = ((defaultValue + (defaultValue * ((StageLv - 1) + (StageLv - 1) * ((difficultLv - 1) * weight_1))))) * (1 + ((levelWeight - 1) * weight_2)) * (bossWeight + ((bossWeight - 1)) * weight_3) * (StageLv + (StageLv - 1) * atkWeight);

		return result;
	}

	public IdleNumber UnitHP(bool isBoss)
	{
		int difficultLv = ((int)Difficult + 1);
		float weight_1 = VGameManager.Config.UNIT_STATE_WEIGHT_1;
		float weight_2 = VGameManager.Config.UNIT_STATE_WEIGHT_2;
		float weight_3 = VGameManager.Config.UNIT_STATE_WEIGHT_3;
		float hpWeight = VGameManager.Config.UNIT_STATE_HP_WEIGHT;

		IdleNumber defaultValue = waveInfo.DefaultHP;
		float levelWeight = stageInfo.levelWeight;
		float bossWeight = isBoss ? stageInfo.bossWeight : 1;

		var result = ((defaultValue + (defaultValue * ((StageLv - 1) + (StageLv - 1) * ((difficultLv - 1) * weight_1))))) * (1 + ((levelWeight - 1) * weight_2)) * (bossWeight + ((bossWeight - 1)) * weight_3) * (StageLv + (StageLv - 1) * hpWeight);

		return result;
	}

	/// <summary>
	/// 스테이지를 클리어했다
	/// </summary>
	public virtual bool IsStageCleared()
	{
		return UserInfo.stage.RecentStageLv(WaveType) > StageLv;
	}

	/// <summary>
	/// 스테이지가 열려있다(클리어 여부는 확인안함)
	/// </summary>
	public virtual bool IsStageOpend()
	{
		return UserInfo.stage.RecentStageLv(WaveType) >= StageLv;
	}

	public override string ToString()
	{
		return $"[{StageName}({stageInfo.tid}). Area: {AreaIndex}] wave: {WaveType}, stageLv: {StageLv}, IsOpen: {IsStageOpend()}, IsClear: {IsStageCleared()}";
	}

	public string ToStringSpawnEnemyInfos()
	{
		string outString = $"";
		foreach (var spawnInfo in spawnEnemyInfos)
		{
			outString += $"{spawnInfo.ToString()} / ATK: {UnitAttackPower(false).ToString()}, HP: {UnitHP(false).ToString()} \n";
		}

		if (spawnLast != null)
		{
			outString += $"Last: {spawnLast.ToString()} / ATK: {UnitAttackPower(true).ToString()}, HP: {UnitHP(true).ToString()} ";
		}

		return outString;
	}

	public string ToStringRewards()
	{
		string outString = $"";
		outString += "☆Rewards☆\n";
		foreach (var reward in KillRewards)
		{
			outString += reward.ToString() + "\n";
		}

		return outString;
	}
}
