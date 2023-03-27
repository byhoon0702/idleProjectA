using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class GachaInfo
	{
		/*
		 * 장비 가챠
		 */
		public const string EQUIP_EXP_ITEM_HASH_TAG = "gachaexp_equip";
		public const string EQUIP_AD_ITEM_HASH_TAG = "gachaexp_equipad";
		public int EquipLv => Inventory.it.FindItemByHashTag(EQUIP_EXP_ITEM_HASH_TAG).Level;
		public long EquipExp => Inventory.it.FindItemByHashTag(EQUIP_EXP_ITEM_HASH_TAG).Exp;
		public long NextEquipExp => (Inventory.it.FindItemByHashTag(EQUIP_EXP_ITEM_HASH_TAG) as ItemGachaExp).nextExp;
		public int EquipAdSummonCount => Inventory.it.ItemCount(EQUIP_AD_ITEM_HASH_TAG).GetValueToInt();
		public void IncEquipAdSummonCount()
		{
			Inventory.it.AddItem(EQUIP_AD_ITEM_HASH_TAG, new IdleNumber(1));
		}
		public void AddEquipExp(int _summonCount)
		{
			Inventory.it.FindItemByHashTag(EQUIP_EXP_ITEM_HASH_TAG).AddExp(_summonCount);
		}



		/*
		 * 스킬 가챠
		 */
		public const string SKILL_EXP_ITEM_HASH_TAG = "gachaexp_skill";
		public const string SKILL_AD_ITEM_HASH_TAG = "gachaexp_skillad";
		public int SkillLv => Inventory.it.FindItemByHashTag(SKILL_EXP_ITEM_HASH_TAG).Level;
		public long SkillExp => Inventory.it.FindItemByHashTag(SKILL_EXP_ITEM_HASH_TAG).Exp;
		public long NextSkillExp => (Inventory.it.FindItemByHashTag(SKILL_EXP_ITEM_HASH_TAG) as ItemGachaExp).nextExp;
		public int SkillAdSummonCount => Inventory.it.ItemCount(SKILL_AD_ITEM_HASH_TAG).GetValueToInt();
		public void IncSkillAdSummonCount()
		{
			Inventory.it.AddItem(SKILL_AD_ITEM_HASH_TAG, new IdleNumber(1));
		}
		public void AddSkillExp(int _summonCount)
		{
			Inventory.it.FindItemByHashTag(SKILL_EXP_ITEM_HASH_TAG).AddExp(_summonCount);
		}





		/*
		 * 펫 가챠
		 */
		public const string PET_EXP_ITEM_HASH_TAG = "gachaexp_pet";
		public const string PET_AD_ITEM_HASH_TAG = "gachaexp_petad";
		public int PetLv => Inventory.it.FindItemByHashTag(PET_EXP_ITEM_HASH_TAG).Level;
		public long PetExp => Inventory.it.FindItemByHashTag(PET_EXP_ITEM_HASH_TAG).Exp;
		public long NextPetExp => (Inventory.it.FindItemByHashTag(PET_EXP_ITEM_HASH_TAG) as ItemGachaExp).nextExp;
		public int PetAdSummonCount => Inventory.it.ItemCount(PET_AD_ITEM_HASH_TAG).GetValueToInt();
		public void IncPetAdSummonCount()
		{
			Inventory.it.AddItem(PET_AD_ITEM_HASH_TAG, new IdleNumber(1));
		}
		public void AddPetExp(int _summonCount)
		{
			Inventory.it.FindItemByHashTag(PET_EXP_ITEM_HASH_TAG).AddExp(_summonCount);
		}




		/*
		 * 골동품 가챠
		 */
		public const string RELIC_EXP_ITEM_HASH_TAG = "gachaexp_relic";
		public const string RELIC_AD_ITEM_HASH_TAG = "gachaexp_relicad";
		public int RelicLv => Inventory.it.FindItemByHashTag(RELIC_EXP_ITEM_HASH_TAG).Level;
		public long RelicExp => Inventory.it.FindItemByHashTag(RELIC_EXP_ITEM_HASH_TAG).Exp;
		public long NextRelicExp => (Inventory.it.FindItemByHashTag(RELIC_EXP_ITEM_HASH_TAG) as ItemGachaExp).nextExp;
		public int RelicAdSummonCount => Inventory.it.ItemCount(RELIC_AD_ITEM_HASH_TAG).GetValueToInt();
		public void IncRelicAdSummonCount()
		{
			Inventory.it.AddItem(RELIC_AD_ITEM_HASH_TAG, new IdleNumber(1));
		}
		public void AddRelicExp(int _summonCount)
		{
			Inventory.it.FindItemByHashTag(RELIC_EXP_ITEM_HASH_TAG).AddExp(_summonCount);
		}
	}
}
