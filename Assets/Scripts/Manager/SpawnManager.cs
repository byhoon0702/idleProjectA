using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private static SpawnManager instance;
	public static SpawnManager it => instance;


	[Header("Prefab")]
	[SerializeField] private PlayerUnit playerUnitPrefab;
	[SerializeField] private EnemyUnit enemyUnitPrefab;
	[SerializeField] private ImmortalUnit immotalUnitPrefab;
	[SerializeField] private TreasureBox treasureBoxPrefab;
	[SerializeField] private Pet petPrefab;

	[Header("==========")]
	[SerializeField] private Transform playerRoot;
	[SerializeField] private Transform enemyRoot;

	[SerializeField] private Rect spawnArea;

	private PlayerUnit playerUnit = null;
	private List<Unit> enemyList = new List<Unit>();
	public List<Pet> petList { get; private set; } = new List<Pet>();
	private HittableUnit lastUnit = null;

	[SerializeField] private int gridSize = 4;

	public HittableUnit LastUnit => lastUnit;


	public bool IsAllEnemyDead
	{
		get
		{
			if (enemyList == null)
			{
				return true;
			}

			bool isEnemyDead = true;

			for (int i = 0; i < enemyList.Count; i++)
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

	public bool PlayerDead
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

	// 처치조건 이걸로 하면 안됨. by myoung1 2023/03/06.
	// Spawn에 문제가 있어서 실패하면 게임 클리어된걸로 처리될 수 있음
	public bool IsKillLastUnit
	{
		get
		{
			if (lastUnit == null)
			{
				return true;
			}

			return lastUnit.IsAlive() == false;
		}
	}

	public bool IsBossDead
	{
		get
		{
			if (lastUnit == null)
			{
				return true;
			}
			return lastUnit.IsAlive() == false;
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
		Vector3 posCenter = Vector3.zero;
		// 유닛정보
		//var unitItemData = DataManager.Get<UnitItemDataSheet>().Get(UserInfo.equip.EquipUnitItemTid);
		UnitData mainUnit = DataManager.Get<UnitDataSheet>().GetData(2600000002);

		// 플레이어 생성
		playerUnit = MakePlayer(mainUnit, Vector3.zero, out var playerSpawnResult);
		if (playerSpawnResult.Fail())
		{
			PopAlert.Create(playerSpawnResult);
			yield break;
		}

		playerUnit.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.1f);

		SpawnPet();

		SceneCamera.it.FindPlayers();
		onComplete?.Invoke();
	}

	public void SpawnPet()
	{
		var petslot = GameManager.UserDB.petContainer.PetSlots;
		// 펫 생성

		foreach (var unit in petList)
		{
			if (unit == null)
			{
				continue;
			}
			Destroy(unit.gameObject);
		}
		petList.Clear();
		int index = 0;
		for (int i = 0; i < petslot.Length; i++)
		{
			if (petslot[i].item == null || petslot[i].item.itemObject == null)
			{
				continue;
			}
			Pet pet = MakePet(petslot[i], index + 1, playerUnit, out var petSpawnResult);
			if (pet == null)
			{
				return;
			}

			pet.gameObject.SetActive(true);

			petList.Add(pet);
			pet.transform.SetParent(playerRoot);
			index++;
		}
	}

	public void AddPet(int index)
	{
		if (playerUnit == null)
		{
			return;
		}
		var petslot = GameManager.UserDB.petContainer.PetSlots;
		Pet pet = MakePet(petslot[index], index + 1, playerUnit, out var petSpawnResult);
		if (pet == null)
		{
			return;
		}

		pet.gameObject.SetActive(true);

		petList[index] = pet;
		pet.transform.SetParent(playerRoot);
	}

	public void RemovePet(int index)
	{
		Destroy(petList[index].gameObject);
	}

	public void ChangePet(int index)
	{
		var petslot = GameManager.UserDB.petContainer.PetSlots;
		Vector3 pos = petList[index].transform.position;
		Destroy(petList[index].gameObject);

		if (playerUnit == null)
		{
			return;
		}
		Pet pet = MakePet(petslot[index], index + 1, playerUnit, out var petSpawnResult);
		if (pet == null)
		{
			return;
		}

		pet.gameObject.SetActive(true);

		petList[index] = pet;
		pet.transform.SetParent(playerRoot);
		pet.transform.position = pos;
	}


	private List<PetData> GetEquipPetData()
	{
		List<PetData> outData = new List<PetData>();

		//foreach (var petTid in UserInfo.equip.pets)
		//{
		//	if (petTid == 0)
		//	{
		//		continue;
		//	}

		//	var petItemData = DataManager.Get<PetItemDataSheet>().Get(petTid);

		//	if (petItemData == null)
		//	{
		//		VLog.ScheduleLogError($"PetItemData is null. ItemDataSheet. itemTid(equipPetTid) {petTid}");
		//		continue;
		//	}

		//	var petData = DataManager.Get<PetDataSheet>().GetData(petItemData.petTid);
		//	if (petData == null)
		//	{
		//		VLog.ScheduleLogError($"PetData is null. PetDataSheet. petTid: {petItemData.petTid}");
		//		continue;
		//	}

		//	outData.Add(petData);
		//}

		return outData;
	}

	public Vector3 GetPartyPos(int index, Vector3 _centerPos = default)
	{
		Vector3 centerPos = _centerPos;
		if (UnitManager.it.Player != null)
		{
			centerPos = UnitManager.it.Player.position;
		}
		float zPos = 0;

		if (index == 0)
		{
			zPos = 0;
		}
		else
		{
			if (index % 2 == 0)
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

	private PlayerUnit MakePlayer(UnitData _unitData, Vector3 _pos, out VResult outResult)
	{
		outResult = new VResult();
		var player = Instantiate(playerUnitPrefab);

		if (player == null)
		{
			outResult.SetFail(VResultCode.MAKE_FAIL, "Spawn Fail. SpawnManager.playerUnitPrefab");
			return null;
		}

		player.transform.SetParent(playerRoot);
		player.transform.position = _pos;
		player.Spawn(_unitData);

		outResult.SetOk();
		return player;
	}

	private Pet MakePet(PetSlot _petSlot, int _index, PlayerUnit _follow, out VResult outResult)
	{
		outResult = new VResult();

		if (_petSlot == null)
		{
			return null;
		}
		if (_follow == null)
		{
			return null;
		}

		Pet pet = Instantiate(petPrefab);

		if (pet == null)
		{
			outResult.SetFail(VResultCode.MAKE_FAIL, "Spawn Fail. SpawnManager.petPrefab");
			return null;
		}

		pet.Spawn(_petSlot, _index, _follow);

		outResult.SetOk();
		return pet;
	}

	public bool SpawnEnemies(float minDistance, float maxDistance)
	{
		int waveUnitCount = StageManager.it.CurrentStage.DisplayUnitCount;
		var enemyInfoList = StageManager.it.CurrentStage.spawnEnemyInfos;

		int totalSpawnCount = StageManager.it.CurrentStage.totalSpawnCount;

		int countLimit = StageManager.it.CurrentStage.CountLimit;
		int perWaveCount = Mathf.Min(4, waveUnitCount - UnitManager.it.GetEnemies().Count);

		if (countLimit > 0)
		{
			int leftCount = countLimit - totalSpawnCount;
			perWaveCount = Mathf.Min(leftCount, perWaveCount);
		}


		if (enemyInfoList.Count == 0)
		{
			VLog.LogError("Stage Enemy Info is Empty");
			return false;
		}

		for (int i = 0; i < perWaveCount; i++)
		{
			int index = UnityEngine.Random.Range(0, enemyInfoList.Count);
			var info = enemyInfoList[index];

			Vector3 random = UnityEngine.Random.insideUnitSphere;
			int minusX = (random.x < 0) ? -1 : 1;
			int minusZ = (random.z < 0) ? -1 : 1;
			Vector2 pos = new Vector2(UnityEngine.Random.Range(minDistance, maxDistance) + Mathf.Abs(random.x), UnityEngine.Random.Range(minDistance, maxDistance) + Mathf.Abs(random.z));

			var enemyUnit = MakeEnemy(info, new Vector3(pos.x * minusX, pos.y * minusZ, 0), out VResult enemySpawnResult);
			if (enemySpawnResult.Fail())
			{
				// 적 스폰에 실패한경우엔 스테이지에 문제가 있으니 클리어가 되게 하지 않기 위해 다시 시작필요
				PopAlert.Create(enemySpawnResult);
				StageManager.it.RetryStage();
				return false;
			}
			enemyList.Add(enemyUnit);
			StageManager.it.CurrentStage.totalSpawnCount++;
		}


		return true;
	}

	public bool SpawnLast(UnitData _spawnInfo, float _distance)
	{
		float xPosition = UnitManager.it.Player.position.x + _distance;

		if (_spawnInfo.type == UnitType.TreasureBox)
		{
			MakeTreasureBox(_spawnInfo, xPosition, out var result);
			if (result.Fail())
			{
				PopAlert.Create(result);
				StageManager.it.RetryStage();
				return false;
			}
		}
		else
		{
			MakeBoss(_spawnInfo, out var result);
			if (result.Fail())
			{
				PopAlert.Create(result);
				StageManager.it.RetryStage();
				return false;
			}
		}

		return true;
	}

	public bool SpawnImmotal(UnitData _spawnInfo, float _distance)
	{
		float xPosition = UnitManager.it.Player.position.x + _distance;
		MakeImmotal(_spawnInfo, xPosition);

		return true;
	}

	private EnemyUnit MakeEnemy(UnitData _spawnInfo, Vector3 _pos, out VResult _outResult)
	{
		_outResult = new VResult();
		EnemyUnit enemyUnit = Instantiate(enemyUnitPrefab);

		if (enemyUnit == null)
		{
			_outResult.SetFail(VResultCode.MAKE_FAIL, "EnemyUnit Spawn Fail. SpawnManager.enemyUnitPrefab");
			return null;
		}

		enemyUnit.name = _spawnInfo.name;
		enemyUnit.transform.SetParent(enemyRoot);
		enemyUnit.transform.position = _pos;
		enemyUnit.Spawn(_spawnInfo);

		enemyUnit.gameObject.SetActive(true);

		_outResult.SetOk();
		return enemyUnit;
	}

	private TreasureBox MakeTreasureBox(UnitData _spawnInfo, float _xPosition, out VResult _outResult)
	{
		_outResult = new VResult();
		TreasureBox treasureBox = Instantiate(treasureBoxPrefab);
		Vector3 pos = new Vector3(UnityEngine.Random.Range(2, 6), UnityEngine.Random.Range(2, 6), 0);

		lastUnit = treasureBox;

		if (treasureBox == null)
		{
			_outResult.SetFail(VResultCode.MAKE_FAIL, "TreasureBox Spawn Fail. SpawnManager.treasureBoxPrefab");
			return null;
		}

		treasureBox.name = _spawnInfo.name;
		treasureBox.transform.SetParent(enemyRoot);
		treasureBox.transform.position = pos;
		treasureBox.Spawn(_spawnInfo, StageManager.it.CurrentStage);

		treasureBox.gameObject.SetActive(true);

		_outResult.SetOk();
		return treasureBox;
	}

	public EnemyUnit MakeBoss(UnitData _bossInfo, out VResult _outResult)
	{
		_outResult = new VResult();
		EnemyUnit bossUnit = Instantiate(enemyUnitPrefab);
		Vector3 pos = new Vector3(0, 3f, 0);

		lastUnit = bossUnit;

		if (bossUnit == null)
		{
			_outResult.SetFail(VResultCode.MAKE_FAIL, "EnemyUnit Spawn Fail.. SpawnManager.treasureBoxPrefab");
			return null;
		}

		bossUnit.name = _bossInfo.name;
		bossUnit.transform.SetParent(enemyRoot);
		bossUnit.transform.position = pos;
		bossUnit.isBoss = true;
		bossUnit.Spawn(_bossInfo);


		bossUnit.gameObject.SetActive(true);

		StageManager.it.CurrentStage.totalBossSpawnCount++;
		_outResult.SetOk();
		return bossUnit;
	}

	public EnemyUnit MakeImmotal(UnitData _bossInfo, float _xPosition)
	{
		EnemyUnit bossUnit = Instantiate(immotalUnitPrefab);
		Vector3 pos = new Vector3(0, 3f, 0);

		lastUnit = bossUnit;

		if (bossUnit == null)
		{
			VLog.LogError($"EnemyUnit Spawn Fail. Can not find Resource");
			return null;
		}
		bossUnit.name = _bossInfo.name;
		bossUnit.transform.SetParent(enemyRoot);
		bossUnit.transform.position = pos;
		bossUnit.isBoss = true;
		bossUnit.Spawn(_bossInfo);


		bossUnit.gameObject.SetActive(true);
		return bossUnit;
	}

	public void ClearEnemies()
	{
		foreach (var unit in enemyList)
		{
			if (unit == null)
			{
				continue;
			}
			Destroy(unit.gameObject);
		}
		enemyList.Clear();
	}

	public void ClearUnits()
	{
		if (playerUnit != null)
		{
			Destroy(playerUnit.gameObject);
			playerUnit = null;
		}

		if (lastUnit != null)
		{
			Destroy(lastUnit.gameObject);
			lastUnit = null;
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
		for (int i = 0; i < enemyList.Count; i++)
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

		playerUnit.ResetUnit();
	}
}
