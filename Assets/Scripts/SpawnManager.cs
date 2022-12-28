using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private static SpawnManager instance;
	public static SpawnManager it => instance;

	public CharacterDataSheet characterDataSheet;
	public Transform playerRoot;
	public Transform enemyRoot;
	public EnemyCharacter enemyCharacter;
	public PlayerCharacter playerCharacter;

	public int playerCount;
	public int enemyCount;
	public Rect spawnArea;

	private void Awake()
	{
		instance = this;
	}
	// Start is called before the first frame update
	void Start()
	{
		SpawnPlayers();

		Invoke("SpawnEnemies", 1);

	}

	private void SpawnPlayers()
	{
		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(0f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;

		float cellX = spawnArea.width / 5;
		float cellY = spawnArea.height / 5;
		Vector2[] grid = new Vector2[25];
		for (int y = 0; y < 5; y++)
		{
			for (int x = 0; x < 5; x++)
			{
				grid[(y * 5) + x] = new Vector2(cellX * x + spawnX, cellY * y + spawnY);
			}
		}

		List<int> indexlist = new List<int>();
		for (int i = 0; i < playerCount; i++)
		{
			PlayerCharacter player = Instantiate(playerCharacter);
			player.transform.SetParent(playerRoot);
			player.Spawn(characterDataSheet.characterDataSheets[Random.Range(0, 3)]);
			player.data.moveSpeed = Random.Range(1f, 1.2f);
			int index = Random.Range(0, 25);

			while (indexlist.Contains(index))
			{
				index = Random.Range(0, 25);
			}
			player.gameObject.SetActive(true);
			player.transform.position = grid[index];
			indexlist.Add(index);
		}

		SceneCamera.it.FindPlayers();
	}
	private void SpawnEnemies()
	{
		var edge = SceneCamera.it.GetCameraEdgePosition(new Vector2(1f, 0.5f));
		float spawnX = edge.x + spawnArea.x;
		float spawnY = spawnArea.y;


		float cellX = spawnArea.width / 5;
		float cellY = spawnArea.height / 5;
		Vector2[] grid = new Vector2[25];
		for (int y = 0; y < 5; y++)
		{
			for (int x = 0; x < 5; x++)
			{
				grid[(y * 5) + x] = new Vector2(cellX * x + spawnX, cellY * y + spawnY);
			}
		}


		List<int> indexlist = new List<int>();
		for (int i = 0; i < enemyCount; i++)
		{
			EnemyCharacter enemy = Instantiate(enemyCharacter);
			enemy.transform.SetParent(enemyRoot);
			enemy.Spawn(characterDataSheet.characterDataSheets[Random.Range(0, 3)]);

			enemy.data.moveSpeed = Random.Range(1f, 1.2f);
			int index = Random.Range(0, 25);

			while (indexlist.Contains(index))
			{
				index = Random.Range(0, 25);
			}
			enemy.gameObject.SetActive(true);
			enemy.transform.position = grid[index];
			indexlist.Add(index);
		}
	}

	// Update is called once per frame
	void Update()
	{

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
