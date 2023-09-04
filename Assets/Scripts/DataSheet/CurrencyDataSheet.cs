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

	/// <summary>
	/// 스킬 진화석
	/// </summary>
	SKILL_EVOLUTION_STONE_D = 16,
	SKILL_EVOLUTION_STONE_C = 17,
	SKILL_EVOLUTION_STONE_B = 18,
	SKILL_EVOLUTION_STONE_A = 19,
	SKILL_EVOLUTION_STONE_S = 20,
	SKILL_EVOLUTION_STONE_SS = 21,
	SKILL_EVOLUTION_STONE_SSS = 22,

	AWAKENING_WEAPON_A = 101,
	AWAKENING_WEAPON_B = 102,
	AWAKENING_WEAPON_C = 103,
	AWAKENING_WEAPON_D = 104,

	VITALITY_DUNGEON_TICKET = 500,
	GUARDIAN_DUNGEON_TICKET = 501,

	COSTUME_TICKET = 600,

	EQUIP_GACHA_TICKET = 650,
	PET_GACHA_TICKET = 651,
	SKILL_GACHA_TICKET = 652,
	RELIC_GACHA_TICKET = 653,

	CASH = 1000,

	ADS = 2000,
	ADS_A = 2001,
	ADS_B = 2002,
	ADS_C = 2003,
	ADS_D = 2004,
	ADS_E = 2005,
	ADS_F = 2006,
	ADS_G = 2007,
	ADS_H = 2008,
	ADS_I = 2009,
	ADS_J = 2010,
	ADS_K = 2011,
	ADS_L = 2012,
	ADS_M = 2013,
	ADS_N = 2014,
	ADS_O = 2015,

	BREAKTHROUGHT_ITEM_D = 20001,
	BREAKTHROUGHT_ITEM_C = 20002,
	BREAKTHROUGHT_ITEM_B = 20003,
	BREAKTHROUGHT_ITEM_A = 20004,
	BREAKTHROUGHT_ITEM_S = 20005,
	BREAKTHROUGHT_ITEM_SS = 20006,
	BREAKTHROUGHT_ITEM_SSS = 20007,

	FREE = 3000,
	END
}
[System.Serializable]
public class Cost
{
	public CurrencyType currency;
	public long tid;
	public string cost;
	public float costIncrease;
	public float costWeight;
}

[System.Serializable]
public class CurrencyData : ItemData
{
	public CurrencyType type;
	public string refillValue;
	public string maxValue;
}

[System.Serializable]
public class CurrencyDataSheet : DataSheetBase<CurrencyData>
{

}
