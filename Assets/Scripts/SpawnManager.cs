using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private static SpawnManager instance;
	public static SpawnManager it => instance;

	[Header("Prototype")]
	public CharacterDataSheet characterDataSheet;
	public PresetDataSheet presetDataSheet;
	[Header("==========")]
	public Transform playerRoot;
	public Transform enemyRoot;

	public PlayerCharacter playerCharacterPrefab;
	public EnemyCharacter enemyCharacterPrefab;


	public int enemyCount;
	public Rect spawnArea;

	public Dictionary<int/*slot index*/, PlayerCharacter> playerDictionary = new Dictionary<int, PlayerCharacter>();
	public Dictionary<int /*slot index*/, EnemyCharacter> enemyDictionary = new Dictionary<int, EnemyCharacter>();
	public EnemyCharacter rewardCharacter = null;
	public EnemyCharacter bossCharacter = null;

	public bool IsAllEnemyDead
	{
		get
		{
			if (enemyDictionary == null)
			{
				return true;
			}

			var keyList = new List<int>(enemyDictionary.Keys);

			for (int i = 0; i < keyList.Count; i++)
			{
				var key = keyList[i];

				var enemy = enemyDictionary[key];
				if (enemy.IsAlive() == true)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool IsAllPlayerDead
	{
		get
		{
			if (playerDictionary == null)
			{
				return true;
			}

			var keyList = new List<int>(playerDictionary.Keys);

			for (int i = 0; i < keyList.Count; i++)
			{
				var key = keyList[i];

				var player = playerDictionary[key];
				if (player.IsAlive() == true)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool IsBossDead
	{
		get
		{
			if (bossCharacter == null)
			{
				return false;
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
		float spawnY = spawnArea.y;

		float cellX = spawnArea.width / 5;
		float cellY = spawnArea.height / 5;
		Vector3[] grid = new Vector3[25];
		for (int y = 0; y < 5; y++)
		{
			for (int x = 0; x < 5; x++)
			{
				grid[(y * 5) + x] = new Vector3(cellX * x + spawnX, 0, cellY * y + spawnY);
			}
		}

		for (int i = 0; i < presetDataSheet.playerPartyPresetData[0].partySlots.Count; i++)
		{
			var slot = presetDataSheet.playerPartyPresetData[0].partySlots[i];
			if (slot.characterTid == 0)
			{
				continue;
			}

			CharacterData characterData = characterDataSheet.GetData(slot.characterTid);

			int index = slot.coord.y * 5 + slot.coord.x;
			PlayerCharacter player = MakePlayer(characterData, i, grid[index]);
			if (player == null)
			{
				// 오류는 생성함수에서 표시
				yield break;
			}

			player.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.1f);
		}

		SceneCamera.it.FindPlayers();
		onComplete?.Invoke();
	}

	private PlayerCharacter MakePlayer(CharacterData _characterData, int _slotIndex, Vector3 pos)
	{
		PlayerCharacter player;

		playerDictionary.TryGetValue(_slotIndex, out var characterObject);
		if (characterObject != null)
		{
			player = characterObject;
		}
		else
		{
			player = Instantiate(playerCharacterPrefab);
			playerDictionary.Add(_slotIndex, player);
		}

		if (player == null)
		{
			VLog.LogError($"PlayerCharacter Spawn Fail. Can not find Resource");
			return null;
		}
		player.transform.SetParent(playerRoot);
		player.transform.position = pos;
		player.Spawn(_characterData);
		player.info.data.moveSpeed = Random.Range(1f, 1.2f);

		return player;
	}

	public bool SpawnEnemies(int _waveCount = 0)
	{
		if (StageManager.it.CurrentStageInfo.listEnemyWavePreset.Count <= _waveCount)
		{
			return false;
		}

		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;

		float cellX = spawnArea.width / 5;
		float cellY = spawnArea.height / 5;
		Vector3[] grid = new Vector3[25];
		for (int y = 0; y < 5; y++)
		{
			for (int x = 0; x < 5; x++)
			{
				grid[(y * 5) + x] = new Vector3(cellX * x + spawnX, 0, cellY * y + spawnY);
			}
		}

		int enemyPartyPresetIndex = StageManager.it.CurrentStageInfo.listEnemyWavePreset[_waveCount];
		var enemyPartyDatas = presetDataSheet.enemypartyPresetDatas[enemyPartyPresetIndex];

		for (int i = 0; i < enemyPartyDatas.partySlots.Count; i++)
		{
			var slot = enemyPartyDatas.partySlots[i];
			if (slot.characterTid == 0)
			{
				continue;
			}
			CharacterData characterData = characterDataSheet.GetEnemyData(slot.characterTid);
			int index = slot.coord.y * 5 + slot.coord.x;

			EnemyCharacter enemy = MakeEnemy(characterData, _waveCount, i, grid[index]);

			enemy.gameObject.SetActive(true);
		}
		return true;
	}

	private EnemyCharacter MakeEnemy(CharacterData _characterData, int _waveCount, int _slotIndex, Vector3 pos)
	{
		EnemyCharacter enemyCharacter;

		enemyDictionary.TryGetValue(_waveCount * 10000 + _slotIndex, out var characterObject);
		if (characterObject != null)
		{
			enemyCharacter = characterObject;
			enemyCharacter.transform.position = pos;
		}
		else
		{
			enemyCharacter = Instantiate(enemyCharacterPrefab);
			enemyDictionary.Add(_waveCount * 10000 + _slotIndex, enemyCharacter);
		}

		if (enemyCharacter == null)
		{
			VLog.LogError($"EnemyCharacter Spawn Fail. Can not find Resource");
			return null;
		}
		enemyCharacter.name = _characterData.name;
		enemyCharacter.transform.SetParent(enemyRoot);
		enemyCharacter.transform.position = pos;
		enemyCharacter.Spawn(_characterData);

		enemyCharacter.info.data.moveSpeed = enemyCharacter.info.data.moveSpeed * Random.Range(1f, 1.2f);
		return enemyCharacter;
	}

	public void ClearCharacters()
	{
		// 체력 게이지 등도 같이 삭제해야 함.
		for (int i = 0; i < playerDictionary.Count; i++)
		{
			var player = playerDictionary[i];
			Destroy(player.gameObject);
		}

		for (int i = 0; i < enemyDictionary.Count; i++)
		{
			var enemy = enemyDictionary[i];
			Destroy(enemy.gameObject);
		}

		playerDictionary.Clear();
		enemyDictionary.Clear();

		SceneCamera.it.FindPlayers();
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

		var left_top = new Vector3(startX, spawnArea.y, 0);
		var right_top = new Vector3(startX + spawnArea.width, spawnArea.y, 0);
		var left_bottom = new Vector3(startX, spawnArea.height, 0);
		var right_bottom = new Vector3(startX + spawnArea.width, spawnArea.height, 0);
		Gizmos.DrawLine(left_top, right_top);
		Gizmos.DrawLine(right_top, right_bottom);
		Gizmos.DrawLine(right_bottom, left_bottom);
		Gizmos.DrawLine(left_bottom, left_top);
	}
}
