using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PresetDataSheet", menuName = "PresetDataSheet", order = 2)]
public class PresetDataSheet : ScriptableObject
{
	public CharacterDataSheet characterDataSheet;

	public List<PartyData> playerPartyPresetData = new List<PartyData>();

	public List<PartyData> enemypartyPresetDatas = new List<PartyData>();


	public void AddPlayerParty()
	{
		playerPartyPresetData.Add(new PartyData());

	}

	public void RemovePlayerParty()
	{
		if (playerPartyPresetData.Count == 0)
		{
			return;
		}
		playerPartyPresetData.RemoveAt(playerPartyPresetData.Count - 1);
	}

	public void AddEnemyParty()
	{
		enemypartyPresetDatas.Add(new PartyData());
	}
	public void RemoveEnemyParty()
	{
		if (enemypartyPresetDatas.Count == 0)
		{
			return;
		}
		enemypartyPresetDatas.RemoveAt(enemypartyPresetDatas.Count - 1);
	}
}
