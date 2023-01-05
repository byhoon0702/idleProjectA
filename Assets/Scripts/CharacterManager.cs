using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	private static CharacterManager instance;
	public static CharacterManager it => instance;

	[SerializeField] private GameObject playerGroup;
	[SerializeField] private GameObject enemyGroup;

	List<Character> characters = new List<Character>();


	private void Awake()
	{
		instance = this;
	}

	public void AddCharacters()
	{
		characters = new List<Character>();

		characters.AddRange(GetPlayerCharacters());
		characters.AddRange(GetEnemyCharacters());
	}

	public Character GetCharacter(Int32 _charID, bool _includeDeathChar = false)
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

	public List<Character> GetCharacters(bool _includeDeathChar = false)
	{
		List<Character> outCharacters = new List<Character>();

		outCharacters.AddRange(GetPlayerCharacters(_includeDeathChar));
		outCharacters.AddRange(GetEnemyCharacters(_includeDeathChar));

		return outCharacters;
	}

	public List<Character> GetPlayerCharacters(bool _includeDeathChar = false)
	{
		List<Character> outCharacters = new List<Character>();

		outCharacters.AddRange(playerGroup.GetComponentsInChildren<Character>(_includeDeathChar));

		return outCharacters;
	}


	public List<Character> GetEnemyCharacters(bool _includeDeathChar = false)
	{
		List<Character> outCharacters = new List<Character>();

		outCharacters.AddRange(enemyGroup.GetComponentsInChildren<Character>(_includeDeathChar));

		return outCharacters;
	}


	public void Avoid()
	{

	}
}
