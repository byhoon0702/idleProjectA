using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSheet", menuName = "CharacterDataSheet", order = 2)]
public class CharacterDataSheet : ScriptableObject
{
	public List<CharacterData> characterDataSheets = new List<CharacterData>();
}
