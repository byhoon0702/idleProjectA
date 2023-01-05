using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSheet", menuName = "CharacterDataSheet", order = 2)]
public class CharacterDataSheet : ScriptableObject
{
	public List<CharacterData> characterDataSheets = new List<CharacterData>();
	public List<CharacterData> enemyCharacterDataSheets = new List<CharacterData>();
	public CharacterData GetData(long _tid)
	{
		if (_tid == 0)
		{
			return null;
		}
		for (int i = 0; i < characterDataSheets.Count; i++)
		{
			if (characterDataSheets[i].tid == _tid)
			{
				return characterDataSheets[i];
			}
		}
		return null;
	}

	public CharacterData GetEnemyData(long _tid)
	{
		if (_tid == 0)
		{
			return null;
		}
		for (int i = 0; i < enemyCharacterDataSheets.Count; i++)
		{
			if (enemyCharacterDataSheets[i].tid == _tid)
			{
				return enemyCharacterDataSheets[i];
			}
		}
		return null;
	}
}
