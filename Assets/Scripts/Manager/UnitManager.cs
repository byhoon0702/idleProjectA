using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	private static UnitManager instance;
	public static UnitManager it => instance;

	private List<Unit> playerToList = new List<Unit>(1);
	public Pet[] pets
	{
		get
		{
			return SpawnManager.it.petList;
		}
	}
	private PlayerUnit player;
	public PlayerUnit Player
	{
		get
		{
			if (player == null)
			{
				player = GameObject.FindObjectOfType<PlayerUnit>();
			}

			return player;
		}
	}
	public EnemyUnit Boss;
	public List<Unit> PlayerToList
	{
		get
		{
			playerToList.Clear();
			playerToList.Add(Player);

			return playerToList;
		}
	}

	[SerializeField] private GameObject playerGroup;
	[SerializeField] private GameObject enemyGroup;


	public Vector2[] offsetVectors;


	private void Awake()
	{
		instance = this;
	}

	public List<Pet> GetPets()
	{
		List<Pet> outUnits = new List<Pet>();

		outUnits.AddRange(playerGroup.GetComponentsInChildren<Pet>());

		return outUnits;
	}

	public List<HittableUnit> GetBosses()
	{
		List<HittableUnit> outUnits = new List<HittableUnit>();
		var allEnemyUnits = enemyGroup.GetComponentsInChildren<HittableUnit>();

		for (int i = 0; i < allEnemyUnits.Length; i++)
		{
			var unit = allEnemyUnits[i];
			if (unit.currentState == StateType.DEATH)
			{
				continue;
			}
			if (unit is EnemyUnit && (unit as EnemyUnit).isBoss)
			{
				outUnits.Add(unit);
			}

		}

		return outUnits;
	}

	public List<HittableUnit> GetEnemies()
	{
		List<HittableUnit> outUnits = new List<HittableUnit>();
		var allEnemyUnits = enemyGroup.GetComponentsInChildren<HittableUnit>();

		for (int i = 0; i < allEnemyUnits.Length; i++)
		{
			var unit = allEnemyUnits[i];
			if (unit.currentState == StateType.DEATH)
			{
				continue;
			}

			outUnits.Add(unit);
		}

		return outUnits;
	}

	public List<HittableUnit> GetRandomEnemies(Vector3 pos, float range = 1f, int filterCount = 1)
	{
		List<HittableUnit> outUnits = new List<HittableUnit>();
		List<HittableUnit> allEnemyUnits = new List<HittableUnit>(enemyGroup.GetComponentsInChildren<HittableUnit>());

		List<HittableUnit> inRange = new List<HittableUnit>();
		inRange = allEnemyUnits.FindAll(x => Vector3.Distance(pos, x.position) <= range);

		int count = 0;
		int index = -1;

		if (filterCount == -1)
		{
			return inRange;
		}
		while (count < filterCount && inRange.Count > 0)
		{
			index = UnityEngine.Random.Range(0, inRange.Count);
			var unit = inRange[index];
			if (unit.currentState == StateType.DEATH)
			{
				inRange.RemoveAt(index);
				continue;
			}
			outUnits.Add(inRange[index]);
			inRange.RemoveAt(index);
			count++;
		}

		return outUnits;
	}

	public Vector3 GetPartyPos(int index, Vector3 _centerPos = default)
	{
		Vector3 centerPos = _centerPos;
		if (Player != null)
		{
			centerPos = Player.position;
		}

		switch (index)
		{
			case 0: //0번 인덱스는 메인 캐릭터 위치
			case 1: //캐릭터 위
			case 2: // 캐릭터 아래
			case 3: // 캐릭터 뒤쪽 위
			case 4: // 캐릭터 뒤쪽
			case 5: //캐릭터 뒤쪽 아래 
				{
					if (offsetVectors.Length > index)
					{
						return new Vector3(centerPos.x + offsetVectors[index].x, 0, centerPos.z + offsetVectors[index].y);
					}
					else
					{
						//자리 개수가 안맞을 경우 오류를 방지 하기 위해 임시로 위치 지정
						return centerPos;
					}

				}

			default:
				return Vector3.zero;
		}
	}

	/// <summary>
	/// 제일 가까운 적의 위치
	/// </summary>
	public float GetRecentEnemyPos()
	{
		float min = float.MaxValue;
		for (int i = 0; i < enemyGroup.transform.childCount; i++)
		{
			Transform tr = enemyGroup.transform.GetChild(i);
			if (tr.gameObject.activeInHierarchy == false)
			{
				continue;
			}

			var unit = tr.GetComponent<Unit>();
			if (unit != null && unit.IsAlive() == false)
			{
				continue;
			}

			min = Mathf.Min(tr.position.x, min);
		}

		return min;
	}

	public void EnemyDissovle(float value)
	{
		var enemies = GetEnemies();
		for (int i = 0; i < enemies.Count; i++)
		{
			var enemy = enemies[i] as EnemyUnit;
			enemy.ChangeState(StateType.IDLE, true);
			enemy.unitAnimation.StopAnimation();
			enemy.unitAnimation.PlayDissolve(value * 1.5f);
		}

	}
}
