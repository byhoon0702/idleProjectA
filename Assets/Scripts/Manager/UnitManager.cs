using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	private static UnitManager instance;
	public static UnitManager it => instance;

	private List<Unit> playerToLIst = new List<Unit>(1);
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

	public List<Unit> PlayerToList
	{ 
		get
		{
			playerToLIst.Clear();
			playerToLIst.Add(Player);

			return playerToLIst;
		}
	}

	[SerializeField] private GameObject playerGroup;
	[SerializeField] private GameObject enemyGroup;

	List<Unit> units = new List<Unit>();

	private void Awake()
	{
		instance = this;
	}

	public void AddUnits()
	{
		units = new List<Unit>();

		units.AddRange(GetPlayerUnit());
		units.AddRange(GetEnemyUnit());
	}

	public Unit GetUnits(Int32 _charID, bool _includeDeathChar = false)
	{
		var units = GetUnits(_includeDeathChar);

		foreach (var unit in units)
		{
			if (unit.CharID == _charID)
			{
				return unit;
			}
		}

		return null;
	}

	public List<Unit> GetUnits(bool _includeDeathChar = false)
	{
		List<Unit> outUnits = new List<Unit>();

		outUnits.AddRange(GetPlayerUnit(_includeDeathChar));
		outUnits.AddRange(GetEnemyUnit(_includeDeathChar));

		return outUnits;
	}

	public List<Unit> GetPlayerUnit(bool _includeDeathChar = false)
	{
		List<Unit> outUnits = new List<Unit>();


		outUnits.AddRange(playerGroup.GetComponentsInChildren<PlayerUnit>(_includeDeathChar));

		return outUnits;
	}
	public List<Pet> GetPets(bool _includeDeathChar = false)
	{
		List<Pet> outUnits = new List<Pet>();


		outUnits.AddRange(playerGroup.GetComponentsInChildren<Pet>(_includeDeathChar));

		return outUnits;
	}


	public List<Unit> GetEnemyUnit(bool _includeDeathChar = false)
	{
		List<Unit> outUnits = new List<Unit>();
		var allEnemyUnits = enemyGroup.GetComponentsInChildren<Unit>();

		for (int i = 0; i < allEnemyUnits.Length; i++)
		{
			var unit = allEnemyUnits[i];
			if (_includeDeathChar == false)
			{
				if (unit.currentState == StateType.DEATH)
				{
					continue;
				}
			}
			outUnits.Add(unit);
		}

		return outUnits;
	}

	private float offset = 1.5f;

	public Vector2[] offsetVectors;

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

	public void Avoid()
	{

	}
}
