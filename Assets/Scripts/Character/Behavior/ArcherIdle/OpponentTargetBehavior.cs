using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Opponent Targeting Behavior", menuName = "Unit/Behavior/Opponent Targeting", order = 2)]
public class OpponentTargetBehavior : UnitBehavior
{
	public GameObject OnTarget(UnitBase character, bool ignoreSearchDelay)
	{

		if (character is PlayerCharacter || character is Companion)
		{
			character.FindTarget(Time.deltaTime, CharacterManager.it.GetEnemyCharacters(), ignoreSearchDelay);
		}
		else
		{
			character.FindTarget(Time.deltaTime, CharacterManager.it.Player, ignoreSearchDelay);
		}

		return null;
	}
}
