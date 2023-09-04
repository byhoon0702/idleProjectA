using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ContentType
{
	NONE = 0,

	HERO = 1,
	HERO_TRAINING = 10,
	HERO_SKILL = 20, // 2
	HERO_ADVANCEMENT = 30,
	HERO_VETERANCY = 40,
	HERO_AWAKENING = 50,

	HERO_COSTUME = 100,
	HERO_COSTUME_WEAPON = HERO_COSTUME + 1,
	HERO_COSTUME_BODY = HERO_COSTUME + 2,
	HERO_COSTUME_HEAD = HERO_COSTUME + 3,
	HERO_COSTUME_HYPER = HERO_COSTUME + 4,


	EQUIP = 200,
	EQUIP_WEAPON = EQUIP + 1,
	EQUIP_ARMOR = EQUIP + 2,
	EQUIP_RING = EQUIP + 3,
	EQUIP_NECKLACE = EQUIP + 4,

	PET = 300,

	BATTLE = 400,
	BATTLE_DUNGEON_SESAME = BATTLE + 1,
	BATTLE_DUNGEON_HATCHERY = BATTLE + 2,
	BATTLE_DUNGEON_IMMORTAL = BATTLE + 3,
	BATTLE_DUNGEON_TOMB = BATTLE + 4,
	BATTLE_DUNGEON_VITALITY = BATTLE + 5,

	BATTLE_TOWER = BATTLE + 6,

	BATTLE_GUARDIAN_FIRE = BATTLE + 7,
	BATTLE_GUARDIAN_FROST = BATTLE + 8,
	BATTLE_GUARDIAN_EARTH = BATTLE + 9,
	BATTLE_GUARDIAN_SEA = BATTLE + 10,

	BATTLE_BOSS = BATTLE + 20,

	SHOP = 500,
	SHOP_PACKAGE = SHOP + 1,
	SHOP_SALE = SHOP + 2,
	SHOP_DIA = SHOP + 3,
	SHOP_NORMAL = SHOP + 4,
	SHOP_ADS = SHOP + 5,

	GACHA = 600,
	GACHA_EQUIP = GACHA + 1,
	GACHA_PET = GACHA + 2,
	GACHA_SKILL = GACHA + 3,
	GACHA_RELIC = GACHA + 4,

	RELIC = 700,

	QUEST = 800,

	ATTENDANCE = 900,
	MONTHLY_ATTENDANCE = ATTENDANCE + 1,
	WEEKLY_ATTENDANCE = ATTENDANCE + 2,

	AUTO_SKILL = 1000,
	AUTO_HYPER = 1001,
	ADS_BUFF = 1002,

	COLLECTION = 1100,
	COLLECTION_EQUIP = COLLECTION + 1,
	COLLECTION_SKILL = COLLECTION + 2,
	COLLECTION_PET = COLLECTION + 3,

	MAIL = 1200,

	EVENT = 4000,

	OFFLINE_REWARD = 9000,

}

public enum ConditionType
{
	NONE,
	STAGE,
	USELEVEL,
	QUEST,
	CONTENT,
	GUIDE,
	DATETIME,
	END,

}

[System.Serializable]
public class OpenCondition
{
	public ConditionType type;
	public long tid;
	public int parameter;
	public ContentType content;
	public string dateTime;
}

[System.Serializable]
public class ContentsData : BaseData
{

	[SerializeField] private ContentType type;
	public ContentType Type => type;
	[SerializeField] private OpenCondition condition;
	public OpenCondition Condition => condition;

	[SerializeField] private List<ChanceReward> rewardList;
	public List<ChanceReward> RewardList => rewardList;
}

[System.Serializable]
public class ContentsDataSheet : DataSheetBase<ContentsData>
{

	public ContentsData GetByType(ContentType type)
	{
		return infos.Find(x => x.Type == type);

	}
}
