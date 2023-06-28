using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
	NONE = 0,
	GOLD = 1,
	DIA = 2,

	/// <summary>
	/// 승급 아이템
	/// </summary>
	ADVANCEMENT_ITEM = 3,
	/// <summary>
	/// 강화 아이템
	/// </summary>
	UPGRADE_ITEM = 4,
	/// <summary>
	/// 진화 아이템
	/// </summary>
	PET_UPGRADE_ITEM = 5,
	/// <summary>
	/// 돌파 아이템
	/// </summary>
	BREAKTHROUGHT_ITEM = 6,

	DUNGEON_KEY = 7,
	SESAME_DUNGEON_KEY = 8,
	TOMB_DUNGEON_KEY = 9,
	HATCHERY_DUNGEON_KEY = 10,
	IMMORTAL_DUNGEON_KEY = 11,

	LEVELUP_ITEM = 12,
	/// <summary>
	/// 스킬 포인트
	/// </summary>
	SKILL_POINT = 13,
	/// <summary>
	/// 노련함 포인트
	/// </summary>
	VETERANCY_POINT = 14,
	/// <summary>
	/// 각성석
	/// </summary>
	AWAKENING_STONE = 15,

	AWAKENING_WEAPON_A = 101,
	AWAKENING_WEAPON_B = 102,
	AWAKENING_WEAPON_C = 103,
	AWAKENING_WEAPON_D = 104,

}


[System.Serializable]
public class CurrencyData : ItemData
{

	public CurrencyType type;
	public string maxValue;
}

[System.Serializable]
public class CurrencyDataSheet : DataSheetBase<CurrencyData>
{

}
