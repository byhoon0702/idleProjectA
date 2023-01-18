using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Opponent Targeting Behavior", menuName = "Unit/Behavior/Opponent Targeting", order = 2)]
public class OpponentTargetBehavior : UnitBehavior
{
	public GameObject OnTarget(Character character)
	{

		if (character is PlayerCharacter)
		{
			character.FindTarget(Time.deltaTime, CharacterManager.it.GetEnemyCharacters());
		}
		else
		{
			character.FindTarget(Time.deltaTime, CharacterManager.it.GetPlayerCharacters());
		}

		return null;
	}
}
