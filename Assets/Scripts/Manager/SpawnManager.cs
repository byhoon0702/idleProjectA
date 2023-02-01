using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private static SpawnManager instance;
	public static SpawnManager it => instance;

	[Header("Prototype")]
	//사용안함
	//public CharacterDataSheet characterDataSheet;

	[Header("==========")]
	public Transform playerRoot;
	public Transform enemyRoot;

	public PlayerCharacter playerCharacterPrefab;
	public Companion companionPrefab;
	public EnemyCharacter enemyCharacterPrefab;
	public BossCharacter bossCharacterPrefab;

	public Rect spawnArea;

	public PlayerCharacter playerCharacter = null;
	public List<Unit> enemyList = new List<Unit>();
	public List<Companion> companionList = new List<Companion>();
	public BossCharacter bossCharacter = null;

	public int gridSize = 4;
	public float lineDiff = 0.5f;

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

	public bool IsAllPlayerDead
	{
		get
		{
			if (playerCharacter == null)
			{
				return true;
			}
			return playerCharacter.IsAlive() == false;
		}
	}

	public bool IsBossClear
	{
		get
		{
			// 아얘 생성된 상태가 아니면 클리어할 수 없음
			if (bossCharacter == null)
			{
				return false;
			}
			return bossCharacter.IsAlive() == false;
		}
	}

	public bool IsBossDead
	{
		get
		{
			if (bossCharacter == null)
			{
				return true;
			}
			return bossCharacter.IsAlive() == false;
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
		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(0f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnHeight = Mathf.Abs(spawnArea.height);
		float spawnY = spawnArea.y - spawnHeight;

		float cellX = spawnArea.width / gridSize;
		float cellY = spawnArea.height / gridSize;

		List<CompanionData> userPartyDataList = new List<CompanionData>();
		var friendList = DataManager.it.Get<CompanionDataSheet>().GetInfos();

		UnitData mainUnit = DataManager.it.Get<UnitDataSheet>().GetData(2);

		//임시로 랜덤 
		for (int i = 0; i < (friendList.Count > 5 ? 5 : friendList.Count);)
		{
			userPartyDataList.Add(friendList[0]);
			friendList.RemoveAt(0);
		}

		float offset = 1.5f;
		Vector3 posCenter = new Vector3(edge.x + (spawnArea.width / 2), 0, spawnY + (spawnHeight / 2));
		Vector3[] pos = new Vector3[6]
		{
			posCenter ,
			new Vector3(posCenter.x, posCenter.y, posCenter.z + offset ), // 캐릭터 위
			new Vector3(posCenter.x, posCenter.y, posCenter.z - offset ), // 캐릭터 아래 
			new Vector3(posCenter.x - offset , posCenter.y, posCenter.z + offset ), // 캐릭터 뒤쪽 위
			new Vector3(posCenter.x - offset , posCenter.y, posCenter.z), // 캐릭터 뒤쪽 
			new Vector3(posCenter.x - offset  , posCenter.y, posCenter.z - offset ), // 캐릭터 뒤쪽 아래
		};

		for (int i = 0; i < (userPartyDataList.Count > 5 ? 5 : userPartyDataList.Count); i++)
		{
			Companion companion = MakeCompanion(userPartyDataList[i], pos[i + 1]);
			if (companion == null)
			{
				// 오류는 생성함수에서 표시
				yield break;
			}

			companion.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.1f);
		}


		PlayerCharacter player = MakePlayer(mainUnit, pos[0]);
		if (player == null)
		{
			// 오류는 생성함수에서 표시
			yield break;
		}

		player.gameObject.SetActive(true);

		yield return new WaitForSeconds(0.1f);

		SceneCamera.it.FindPlayers();
		onComplete?.Invoke();

	}

	private PlayerCharacter MakePlayer(UnitData _characterData, Vector3 _pos)
	{
		PlayerCharacter player;

		player = Instantiate(playerCharacterPrefab);
		playerCharacter = player;

		if (player == null)
		{
			VLog.LogError($"PlayerCharacter Spawn Fail. Can not find Resource");
			return null;
		}
		player.transform.SetParent(playerRoot);
		player.transform.position = _pos;
		player.Spawn(_characterData);

		return player;
	}
	private Companion MakeCompanion(CompanionData _companionData, Vector3 _pos)
	{
		Companion companion;

		companion = Instantiate(companionPrefab);

		if (companion == null)
		{
			VLog.LogError($"PlayerCharacter Spawn Fail. Can not find Resource");
			return null;
		}
		companion.transform.SetParent(playerRoot);
		companion.transform.position = _pos;
		companion.Spawn(_companionData);

		companionList.Add(companion);

		return companion;
	}

	public bool SpawnEnemies()
	{
		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;

		int waveUnitCount = StageManager.it.CurrentStageInfo.waveUnitCount;
		var unitTidList = StageManager.it.CurrentStageInfo.enemyTidList;

		List<int> randomTidList = new List<int>();
		for (int i = 0; i < waveUnitCount; i++)
		{
			int index = Random.Range(0, unitTidList.Count);
			randomTidList.Add(unitTidList[index]);
		}

		List<UnitData> unitDataList = new List<UnitData>();
		for (int i = 0; i < randomTidList.Count; i++)
		{
			int tid = randomTidList[i];
			UnitData unitdata = DataManager.it.Get<UnitDataSheet>().GetData(tid);
			unitDataList.Add(unitdata);
		}

		MakeEnemy(unitDataList, new Rect(spawnX, spawnY, spawnArea.width, spawnArea.height));

		return true;
	}

	public bool SpawnBoss()
	{
		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;

		UnitData bossData = DataManager.it.Get<UnitDataSheet>().GetData(StageManager.it.CurrentStageInfo.bossTid);

		MakeBoss(bossData, new Rect(spawnX, spawnY, spawnArea.width, spawnArea.height));

		return true;
	}

	private void MakeEnemy(List<UnitData> _unitDataList, Rect _rect)
	{
		for (int i = 0; i < _unitDataList.Count; i++)
		{
			UnitData unitdata = _unitDataList[i];

			EnemyCharacter enemyCharacter;

			Vector3 pos = new Vector3(Random.Range(_rect.x, _rect.x + _rect.width), 0, Random.Range(_rect.y, _rect.y + _rect.height));

			enemyCharacter = Instantiate(enemyCharacterPrefab);
			enemyList.Add(enemyCharacter);

			if (enemyCharacter == null)
			{
				VLog.LogError($"EnemyCharacter Spawn Fail. Can not find Resource");
				continue;
			}
			enemyCharacter.name = unitdata.name;
			enemyCharacter.transform.SetParent(enemyRoot);
			enemyCharacter.transform.position = pos;
			enemyCharacter.Spawn(unitdata);

			enemyCharacter.gameObject.SetActive(true);
		}
	}

	private void MakeBoss(UnitData _bossData, Rect _rect)
	{
		BossCharacter BossCharacter;

		Vector3 pos = new Vector3(Random.Range(_rect.x, _rect.x + _rect.width), 0, Random.Range(_rect.y, _rect.y + _rect.height));

		BossCharacter = Instantiate(bossCharacterPrefab);
		bossCharacter = BossCharacter;

		if (BossCharacter == null)
		{
			VLog.LogError($"EnemyCharacter Spawn Fail. Can not find Resource");
			return;
		}
		BossCharacter.name = _bossData.name;
		BossCharacter.transform.SetParent(enemyRoot);
		BossCharacter.transform.position = pos;
		BossCharacter.Spawn(_bossData);

		BossCharacter.gameObject.SetActive(true);
	}

	public void ClearCharacters()
	{
		if (playerCharacter != null)
		{
			Destroy(playerCharacter.gameObject);
		}

		if (bossCharacter != null)
		{
			Destroy(bossCharacter.gameObject);
		}

		foreach (var unit in enemyList)
		{
			if (unit == null)
			{
				continue;
			}
			Destroy(unit.gameObject);
		}

		foreach (var unit in companionList)
		{
			if (unit == null)
			{
				continue;
			}
			Destroy(unit.gameObject);
		}

		enemyList.Clear();
		companionList.Clear();
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

	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying == false)
		{
			return;
		}
		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
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
