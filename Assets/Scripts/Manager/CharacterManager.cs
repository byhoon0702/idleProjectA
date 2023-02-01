using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	private static CharacterManager instance;
	public static CharacterManager it => instance;

	private PlayerCharacter player;
	public PlayerCharacter Player
	{
		get
		{
			if (player == null)
			{
				player = GameObject.FindObjectOfType<PlayerCharacter>();
			}

			return player;
		}
	}

	[SerializeField] private GameObject playerGroup;
	[SerializeField] private GameObject enemyGroup;

	List<Unit> characters = new List<Unit>();


	private void Awake()
	{
		instance = this;
	}

	public void AddCharacters()
	{
		characters = new List<Unit>();

		characters.AddRange(GetPlayerCharacters());
		characters.AddRange(GetEnemyCharacters());
	}

	public Unit GetCharacter(Int32 _charID, bool _includeDeathChar = false)
	{
		var characters = GetCharacters(_includeDeathChar);

		foreach (var character in characters)
		{
			if (character.charID == _charID)
			{
				return character;
			}
		}

		return null;
	}

	public List<Unit> GetCharacters(bool _includeDeathChar = false)
	{
		List<Unit> outCharacters = new List<Unit>();

		outCharacters.AddRange(GetPlayerCharacters(_includeDeathChar));
		outCharacters.AddRange(GetEnemyCharacters(_includeDeathChar));

		return outCharacters;
	}

	public List<Unit> GetPlayerCharacters(bool _includeDeathChar = false)
	{
		List<Unit> outCharacters = new List<Unit>();


		outCharacters.AddRange(playerGroup.GetComponentsInChildren<PlayerCharacter>(_includeDeathChar));

		return outCharacters;
	}
	public List<Companion> GetCompanions(bool _includeDeathChar = false)
	{
		List<Companion> outCharacters = new List<Companion>();


		outCharacters.AddRange(playerGroup.GetComponentsInChildren<Companion>(_includeDeathChar));

		return outCharacters;
	}


	public List<Unit> GetEnemyCharacters(bool _includeDeathChar = false)
	{
		List<Unit> outCharacters = new List<Unit>();
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
			outCharacters.Add(unit);
		}

		return outCharacters;
	}


	public void Avoid()
	{

	}
}
