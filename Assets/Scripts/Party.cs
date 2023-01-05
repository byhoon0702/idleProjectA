using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyInfo
{
	public RaceType raceType;
	public long characterTid;
	public int level;


	public Vector2Int coord;
}
[Serializable]
public class PartyData
{
	public List<PartyInfo> partySlots = new List<PartyInfo>();

	public void SetCharacterToParty(int _slotIndex, long _data)
	{
		int x = _slotIndex / 5;
		int y = _slotIndex % 5;

		partySlots[_slotIndex].characterTid = _data;
	}

	public void SetSlotElement(int _slotIndex, RaceType _raceType)
	{
		int x = _slotIndex / 5;
		int y = _slotIndex % 5;

		partySlots[_slotIndex].raceType = _raceType;
	}


	public void AddPartyslot()
	{
		partySlots.Add(new PartyInfo());
	}
	public void RemovePartySlot()
	{
		if (partySlots.Count == 0)
		{
			return;
		}
		partySlots.RemoveAt(partySlots.Count - 1);
	}

	public void SetSlotLevel()
	{

	}

	private bool CheckTypeMatch(int x, int y)
	{
		var slot = partySlots[x * 5 + y];
		if (slot.characterTid == 0)
		{
			return false;
		}

		if (slot.raceType == RaceType.NONE)
		{
			return false;
		}


		return true;
	}
}
