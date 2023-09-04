using UnityEngine;
using System.Collections.Generic;

namespace RuntimeData
{
	public class StageMonsterInfo
	{
		public int phase;
		public EnemyUnitData data;
	}
	[System.Serializable]
	public class StageInfo
	{

		public int playCount;
		public bool isClear;
		public int totalSpawnCount;
		public int totalBossSpawnCount;

		#region Static Property
		public StageType StageType => stageData.stageType;
		public int AreaNumber => stageData.areaNumber;
		public int StageNumber => stageListData.stageNumber;

		public string StageName => PlatformManager.Language[Name];
		public string Name => stageData.name;
		public float TimeLimit => stageData.enemyInfo.timelimit;
		public int DisplayUnitCount => stageData.enemyInfo.enemyDisplayCount;
		public int SpawnPerWave => stageData.enemyInfo.spawnPerWave > 0 ? stageData.enemyInfo.spawnPerWave : 1;
		public int CountLimit => stageData.enemyInfo.enemyCountLimit;
		public int BossCountLimit => stageData.enemyInfo.bossCountLimit;
		#endregion
		public float SpawnMinDistance = 3;
		public float SpawnMaxDistance = 5;

		public List<StageMonsterInfo> spawnEnemyInfos { get; private set; }
		public List<StageMonsterInfo> spawnBoss { get; private set; }
		public StageListData stageListData { get; private set; }
		public StageData stageData { get; private set; }


		public List<RuntimeData.RewardInfo> MonsterReward { get; private set; }
		public List<RuntimeData.RewardInfo> StageClearReward { get; private set; }

		public RuntimeData.RewardInfo MonsterExp { get; private set; }
		public RuntimeData.RewardInfo MonsterGold { get; private set; }

		public IdleNumber StageExp { get; private set; }
		public IdleNumber StageGold { get; private set; }

		public StageRule Rule
		{
			get
			{
				if (itemObject == null || itemObject.rule == null)
				{
					Debug.LogError($"====!!!! {stageData.tid} HAS NO RULE !!!!====");
					return null;
				}

				return itemObject.rule;

			}
		}
		public Sprite Icon
		{
			get
			{
				if (itemObject == null)
				{
					return null;
				}

				return itemObject.ItemIcon;
			}
		}


		public StageMapObject itemObject { get; private set; }


		public StageInfo(long tid, int stageNumber)
		{
			stageData = DataManager.Get<StageDataSheet>().Get(tid);

			if (stageData.stageType == StageType.Normal)
			{
				var list = DataManager.Get<StageDataSheet>().GetListByType(StageType.Normal);
				for (int i = 0; i < list.Count; i++)
				{
					for (int ii = 0; ii < list[i].stageListData.Length; ii++)
					{
						if (list[i].stageListData[ii].stageNumber == stageNumber)
						{
							stageData = list[i];
							stageListData = list[i].stageListData[ii];
						}
					}
				}
			}
			else
			{
				stageListData = stageData.stageListData[stageNumber - 1];
			}

			UpdateData();
			itemObject = PlatformManager.UserDB.stageContainer.GetScriptableObject<StageMapObject>(stageData.tid);
		}

		public StageInfo(StageData _dungeonData, StageListData _data)
		{
			stageData = _dungeonData;
			stageListData = _data;
			UpdateData();
			itemObject = PlatformManager.UserDB.stageContainer.GetScriptableObject<StageMapObject>(stageData.tid);
		}

		public void UpdateData()
		{
			spawnEnemyInfos = new List<StageMonsterInfo>();
			spawnBoss = new List<StageMonsterInfo>();
			for (int i = 0; i < stageData.spawnList.Count; i++)
			{
				var unit = DataManager.Get<EnemyUnitDataSheet>().Get(stageData.spawnList[i].tid);
				if (unit == null)
				{
					continue;
				}
				if (stageData.spawnList[i].isBoss)
				{
					spawnBoss.Add(new StageMonsterInfo() { phase = stageData.spawnList[i].maxPhase, data = unit });
				}
				else
				{
					spawnEnemyInfos.Add(new StageMonsterInfo() { phase = 0, data = unit });
				}

			}

			if (stageListData.overrideSpawnList != null && stageListData.overrideSpawnList.Count > 0)
			{

				spawnBoss.Clear();
				for (int i = 0; i < stageListData.overrideSpawnList.Count; i++)
				{
					var unit = DataManager.Get<EnemyUnitDataSheet>().Get(stageListData.overrideSpawnList[i].tid);
					if (stageListData.overrideSpawnList[i].isBoss)
					{
						spawnBoss.Add(new StageMonsterInfo() { phase = stageListData.overrideSpawnList[i].maxPhase, data = unit });
					}

				}
			}

			isClear = PlatformManager.UserDB.stageContainer.IsCleared(StageType, stageData.dungeonTid, StageNumber);
		}

		void SetMonsterReward(IdleNumber multi)
		{
			if (stageData.monsterReward != null)
			{
				MonsterReward = new List<RuntimeData.RewardInfo>();
				for (int i = 0; i < stageData.monsterReward.Count; i++)
				{
					RuntimeData.RewardInfo info = new RuntimeData.RewardInfo(stageData.monsterReward[i]);
					info.Multiply(multi);
					//경험치와 골드는 따로 처리
					if (info.Category == RewardCategory.EXP)
					{

						MonsterExp = info;
						continue;
					}
					else if (info.Category == RewardCategory.Currency)
					{
						var data = DataManager.Get<CurrencyDataSheet>().Get(info.Tid);
						if (data.type == CurrencyType.GOLD)
						{

							MonsterGold = info;
							continue;
						}
						else
						{

							MonsterReward.Add(info);
						}
					}
					else
					{

						MonsterReward.Add(info);
					}
				}
			}
		}

		public void SetStageReward(IdleNumber step)
		{
			if (stageData.stageReward != null)
			{
				StageClearReward = new List<RuntimeData.RewardInfo>();
				for (int i = 0; i < stageData.stageReward.Count; i++)
				{
					RuntimeData.RewardInfo info = new RuntimeData.RewardInfo(stageData.stageReward[i]);
					info.UpdateCount(step);
					StageClearReward.Add(info);
				}
			}
		}
		public void SetReward(IdleNumber multi)
		{
			SetMonsterReward(multi);
			SetStageReward((IdleNumber)0);
		}

		public IdleNumber GetMonsterExp()
		{
			if (MonsterExp == null)
			{
				return (IdleNumber)0;
			}
			MonsterExp.UpdateCount(StageNumber);
			return MonsterExp.fixedCount;

		}

		public IdleNumber GetMonsterGold()
		{
			if (MonsterGold == null)
			{
				return (IdleNumber)0;
			}
			MonsterGold.UpdateCount(StageNumber);
			return MonsterGold.fixedCount;
		}

		public List<RuntimeData.RewardInfo> GetStageRewardList()
		{
			List<RuntimeData.RewardInfo> infoList = new List<RewardInfo>();

			if (StageClearReward == null)
			{
				return infoList;
			}

			int minChance = 0;
			for (int i = 0; i < StageClearReward.Count; i++)
			{
				var chance = RandomLogic.Reward.Next(0, RandomLogic.maxChance);
				var reward = StageClearReward[i];
				int maxChance = (int)(reward.Chance * 100);
				if (reward.Chance == 100 || chance >= minChance && chance < maxChance)
				{
					infoList.Add(reward);
				}
				//minChance += maxChance;
			}

			return infoList;
		}

		public List<RuntimeData.RewardInfo> GetMonsterRewardList()
		{
			List<RuntimeData.RewardInfo> infoList = new List<RewardInfo>();

			if (MonsterReward == null)
			{
				return infoList;
			}

			int minChance = 0;
			for (int i = 0; i < MonsterReward.Count; i++)
			{
				var chance = RandomLogic.Reward.Next(0, RandomLogic.maxChance);
				var reward = MonsterReward[i];
				int maxChance = (int)(reward.Chance * 100);
				if (reward.Chance == 100 || chance >= minChance && chance < maxChance)
				{
					infoList.Add(reward);
				}
				//minChance += maxChance;
			}

			return infoList;
		}

		public RuntimeData.RewardInfo GetMonsterReward()
		{
			RuntimeData.RewardInfo info = null;
			var chance = RandomLogic.Reward.Next(0, RandomLogic.maxChance);

			if (MonsterReward == null)
			{
				return null;
			}

			int minChance = 0;
			for (int i = 0; i < MonsterReward.Count; i++)
			{
				var reward = MonsterReward[i];
				int maxChance = (int)(reward.Chance * 100);

				if (reward.Chance == 100 || chance >= minChance && chance < maxChance)
				{
					info = reward;
					break;
				}
			}

			return info;
		}

		public IdleNumber UnitAttackPower(bool isBoss)
		{
			//int difficultLv = ((int)Difficulty + 1);
			float stageweight = GameManager.Config.STAGE_WEIGHT;
			float levelweight = GameManager.Config.LEVEL_WEIGHT;
			float bossweight = GameManager.Config.BOSS_WEIGHT;
			float atkWeight = GameManager.Config.UNIT_STATE_ATTACK_POWER_WEIGHT;

			IdleNumber defaultValue = (IdleNumber)stageData.monsterStats.attackPower;


			float bossWeight = (isBoss ? bossweight : 0) * (1 + stageListData.bossWeight);

			var result = defaultValue + ((defaultValue * (stageListData.stageNumber - 1) * stageweight) * (stageListData.levelWeight * levelweight) * atkWeight);

			var bossresult = result + (result * bossWeight);

			return isBoss ? bossresult : result;
		}

		public IdleNumber UnitHP(bool isBoss)
		{
			//int difficultLv = ((int)Difficulty + 1);
			float stageweight = GameManager.Config.STAGE_WEIGHT;
			float levelweight = GameManager.Config.LEVEL_WEIGHT;
			float bossweight = GameManager.Config.BOSS_WEIGHT;
			float hpWeight = GameManager.Config.UNIT_STATE_HP_WEIGHT;

			IdleNumber defaultValue = (IdleNumber)stageData.monsterStats.hp;

			float bossWeight = (isBoss ? bossweight : 0) * (1 + stageListData.bossWeight);

			var result = defaultValue + ((defaultValue * (stageListData.stageNumber - 1) * stageweight) * (stageListData.levelWeight * levelweight) * hpWeight);

			var bossresult = result + (result * bossWeight);

			return isBoss ? bossresult : result;
		}


	}
}
