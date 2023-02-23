using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerV2 : MonoBehaviour
{
	private static SpawnManagerV2 instance;
	public static SpawnManagerV2 it => instance;

	[Header("==========")]
	public Transform playerRoot;
	public Transform enemyRoot;

	public PlayerUnit playerUnitPrefab;
	public Pet petPrefab;
	public EnemyUnit enemyUnitPrefab;
	public ChasingDungeonBossUnit chasingDungeonBossUnitPrefab;

	public Rect spawnArea;

	public PlayerUnit playerUnit = null;
	public List<Unit> enemyList = new List<Unit>();
	public List<Pet> petList = new List<Pet>();
	public Unit bossUnit = null;

	public int gridSize = 4;
	public float lineDiff = 0.5f;
	float offset = 1.5f;


	public bool IsAllEnemyDead
	{
		get
		{
			if (enemyList == null)
			{
				return true;
			}

			bool isEnemyDead = true;

			for (int i = 0 ; i < enemyList.Count ; i++)
			{
				var enemy = enemyList[i];

				if (enemy.IsAlive() == true)
				{
					isEnemyDead = false;
					break;
				}
			}
			return isEnemyDead && IsBossDead;
		}
	}

	public bool IsAllPlayerDead
	{
		get
		{
			if (playerUnit == null)
			{
				return true;
			}
			return playerUnit.IsAlive() == false;
		}
	}

	public bool IsBossClear
	{
		get
		{
			// 아얘 생성된 상태가 아니면 클리어할 수 없음
			if (bossUnit == null)
			{
				return false;
			}
			return bossUnit.IsAlive() == false;
		}
	}

	public bool IsBossDead
	{
		get
		{
			if (bossUnit == null)
			{
				return true;
			}
			return bossUnit.IsAlive() == false;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void SpawnCoroutine(System.Action onComplete)
	{
		StartCoroutine(SpawnPlayers(onComplete));
	}

	public IEnumerator SpawnPlayers(System.Action onComplete)
	{
		var edge = SceneCameraV2.it.GetCameraEdgePosition(new Vector2(0f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnHeight = Mathf.Abs(spawnArea.height);
		float spawnY = spawnArea.y - spawnHeight;

		float cellX = spawnArea.width / gridSize;
		float cellY = spawnArea.height / gridSize;

		List<PetData> userPartyDataList = new List<PetData>();

		var unitItemData = DataManager.Get<ItemDataSheet>().Get(UserInfo.EquipUnitItemTid);
		UnitData mainUnit = DataManager.Get<UnitDataSheet>().GetData(unitItemData.unitTid);

		foreach (var petTid in UserInfo.pets)
		{
			if (petTid == 0)
			{
				continue;
			}

			var petItemData = DataManager.Get<ItemDataSheet>().Get(petTid);

			if (petItemData == null)
			{
				VLog.ScheduleLogError($"PetItemData is null. ItemDataSheet. itemTid: {petTid}");
				continue;
			}

			var petData = DataManager.Get<PetDataSheet>().GetData(petItemData.petTid);
			if (petData == null)
			{
				VLog.ScheduleLogError($"PetData is null. PetDataSheet. petTid: {petItemData.petTid}");
				continue;
			}

			userPartyDataList.Add(petData);
		}

		Vector3 posCenter = new Vector3(edge.x + (spawnArea.width / 2), 0, spawnY + (spawnHeight / 2));

		PlayerUnit player = MakePlayer(mainUnit, GetPartyPos(0, posCenter));
		if (player == null)
		{
			// 오류는 생성함수에서 표시
			yield break;
		}

		player.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.1f);

		for (int i = 0 ; i < (userPartyDataList.Count > 5 ? 5 : userPartyDataList.Count) ; i++)
		{
			Pet pet = MakePet(userPartyDataList[i], GetPartyPos(i + 1, posCenter), i + 1);
			if (pet == null)
			{
				// 오류는 생성함수에서 표시
				yield break;
			}

			pet.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.1f);
		}

		SceneCameraV2.it.FindPlayers();
		onComplete?.Invoke();

	}

	public Vector3 GetPartyPos(int index, Vector3 _centerPos = default)
	{
		Vector3 centerPos = _centerPos;
		if (UnitManager.it.Player != null)
		{
			centerPos = UnitManager.it.Player.position;
		}
		float zPos = 0;

		if(index == 0)
		{
			zPos = 0;
		}
		else
		{
			if(index % 2 == 0)
			{
				zPos = 1;
			}
			else
			{
				zPos = -1;
			}
		}

		return new Vector3(centerPos.x + (-1 * index), 0, zPos);
	}

	private PlayerUnit MakePlayer(UnitData _unitData, Vector3 _pos)
	{
		PlayerUnit player;

		player = Instantiate(playerUnitPrefab);
		playerUnit = player;

		if (player == null)
		{
			VLog.LogError($"PlayerUnit Spawn Fail. Can not find Resource");
			return null;
		}
		player.transform.SetParent(playerRoot);
		player.transform.position = _pos;
		player.Spawn(_unitData);

		return player;
	}

	private Pet MakePet(PetData _petData, Vector3 _pos, int index)
	{
		Pet pet;

		pet = Instantiate(petPrefab);

		if (pet == null)
		{
			VLog.LogError($"PlayerUnit Spawn Fail. Can not find Resource");
			return null;
		}
		pet.transform.SetParent(playerRoot);
		pet.transform.position = _pos;
		pet.Spawn(_petData, index);

		petList.Add(pet);

		return pet;
	}

	public bool SpawnEnemies()
	{
		var edge = SceneCameraV2.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;

		int waveUnitCount = StageManager.it.CurrentNormalStageInfo.waveUnitCount;
		var enemyInfoList = StageManager.it.CurrentNormalStageInfo.enemyInfos;

		if (enemyInfoList.Count == 0)
		{
			VLog.LogError("Stage Enemy Info is Empty");
			return false;
		}

		for (int i = 0 ; i < waveUnitCount ; i++)
		{
			int index = Random.Range(0, enemyInfoList.Count);
			var info = enemyInfoList[index];

			MakeEnemy(info, new Vector3(spawnX + (i * 2), 0, 0));
		}


		return true;
	}

	public bool SpawnBoss()
	{
		var edge = SceneCameraV2.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;

		EnemyInfo bossInfo = new EnemyInfo();
		bossInfo.tid = StageManager.it.CurrentStageInfo.bossTid;
		bossInfo.level = StageManager.it.CurrentStageInfo.bossLevel;

		MakeBoss(bossInfo, new Rect(spawnX, spawnY, spawnArea.width, spawnArea.height));

		return true;
	}

	private void MakeEnemy(EnemyInfo _info, Vector3 _pos)
	{
		UnitData data = DataManager.Get<UnitDataSheet>().GetData(_info.tid);

		EnemyUnit enemyUnit;

		Vector3 pos = new Vector3(_pos.x, 0, 0);

		enemyUnit = Instantiate(enemyUnitPrefab);
		enemyList.Add(enemyUnit);

		if (enemyUnit == null)
		{
			VLog.LogError($"EnemyUnit Spawn Fail. Can not find Resource");
			return;
		}

		enemyUnit.name = data.name;
		enemyUnit.transform.SetParent(enemyRoot);
		enemyUnit.transform.position = pos;
		enemyUnit.Spawn(data, _info.level);

		enemyUnit.gameObject.SetActive(true);
	}

	private void MakeBoss(EnemyInfo _bossInfo, Rect _rect)
	{
		Unit bossUnit = null;
		UnitData bossData = DataManager.Get<UnitDataSheet>().GetData(_bossInfo.tid);

		Vector3 pos = new Vector3(Random.Range(_rect.x, _rect.x + _rect.width), 0, 0);

		switch (StageManager.it.CurrentStageType)
		{
			case StageType.NORMAL:
				bossUnit = Instantiate(enemyUnitPrefab);
				break;
			case StageType.CHASING:
				bossUnit = Instantiate(chasingDungeonBossUnitPrefab);
				break;
		}
		this.bossUnit = bossUnit;

		if (bossUnit == null)
		{
			VLog.LogError($"EnemyUnit Spawn Fail. Can not find Resource");
			return;
		}
		bossUnit.name = bossData.name;
		bossUnit.transform.SetParent(enemyRoot);
		bossUnit.transform.position = pos;
		bossUnit.Spawn(bossData, _bossInfo.level);
		bossUnit.isBoss = true;

		bossUnit.gameObject.SetActive(true);
	}

	public void ClearUnits()
	{
		if (playerUnit != null)
		{
			Destroy(playerUnit.gameObject);
			playerUnit = null;
		}

		if (bossUnit != null)
		{
			Destroy(bossUnit.gameObject);
			bossUnit = null;
		}

		foreach (var unit in enemyList)
		{
			if (unit == null)
			{
				continue;
			}
			Destroy(unit.gameObject);
		}

		foreach (var unit in petList)
		{
			if (unit == null)
			{
				continue;
			}
			Destroy(unit.gameObject);
		}

		enemyList.Clear();
		petList.Clear();
	}

	public void ClearDeadEnemy()
	{
		List<Unit> deleteEnemy = new List<Unit>();

		foreach (var unit in enemyList)
		{
			if (unit.IsAlive() == false)
			{
				deleteEnemy.Add(unit);
			}
		}
		enemyList.RemoveAll(deleteEnemy.Contains);

		foreach (var unit in deleteEnemy)
		{
			Destroy(unit.gameObject);
		}
		deleteEnemy.Clear();
	}

	// 보스전 진행용 메소드. 보상지급은 제외
	public void KillAllEnemy()
	{
		for (int i = 0 ; i < enemyList.Count ; i++)
		{
			var enemy = enemyList[i];
			enemy.Kill();
		}
	}

	public void KillPlayer()
	{
		playerUnit.Kill();
	}

	public void ResetPlayer()
	{
		ProjectileManager.it.ClearPool();
		playerUnit.ResetUnit();
	}

	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying == false)
		{
			return;
		}
		var edge = SceneCameraV2.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		Gizmos.color = Color.red;
		float startX = edge.x + spawnArea.x;

		var left_top = new Vector3(startX, 0, spawnArea.y);
		var right_top = new Vector3(startX + spawnArea.width, 0, spawnArea.y);
		var left_bottom = new Vector3(startX, 0, spawnArea.height);
		var right_bottom = new Vector3(startX + spawnArea.width, 0, spawnArea.height);
		Gizmos.DrawLine(left_top, right_top);
		Gizmos.DrawLine(right_top, right_bottom);
		Gizmos.DrawLine(right_bottom, left_bottom);
		Gizmos.DrawLine(left_bottom, left_top);
	}
}
