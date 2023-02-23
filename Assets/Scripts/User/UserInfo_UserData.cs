using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class UserData
	{
		public string userName = "VIVID";
		public int uid = 12345678;
		public long playTicks;
		public Grade userGrade;

		public long equipUnitItemTid;

		public long EquipWeaponTid = 1701301001;
		public long EquipArmerTid;
		public long EquipAccessoryTid;


		public long[] skillSlots = new long[SKILL_SLOT_COUNT];
		public long[] petSlots = new long[PET_SLOT_COUNT];

		public long userExp = 0;

		// 진급능력
		public CoreAbilitySave coreAbil = new CoreAbilitySave();

		public List<InstantItem> instantItems = new List<InstantItem>();


		private void RefreshData()
		{
			instantItems.Clear();
			foreach(var item in Inventory.it.Items)
			{
				instantItems.Add(item.instantItem.DeepClone());
			}
		}

		public void SaveUserData()
		{
			RefreshData();
			// 실제 저장기능은 추후에....
			VLog.Log($"데이터 저장완료!. PlayTime: {UserInfo.PlayTicksToString}, Server Now: {TimeManager.it.m_now}");
		}

		public void MakeTestInstantItems()
		{
			/* 
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 *  재화, 일반 아이템 생성
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 */

			// 골드
			var goldData = DataManager.Get<ItemDataSheet>().GetByHashTag("gold");
			var goldItem = ItemCreator.MakeInstantItem(goldData);
			goldItem.count = new IdleNumber(10000);
			instantItems.Add(goldItem);

			// 다이아
			var diaData = DataManager.Get<ItemDataSheet>().GetByHashTag("dia");
			var diaItem = ItemCreator.MakeInstantItem(diaData);
			diaItem.count = new IdleNumber(10000);
			instantItems.Add(diaItem);

			//코어 성장재료
			var cData = DataManager.Get<ItemDataSheet>().GetByHashTag("corepoint");
			var cItem = ItemCreator.MakeInstantItem(cData);
			cItem.count = new IdleNumber(100);
			instantItems.Add(cItem);

			//코어어빌 재료
			var caData = DataManager.Get<ItemDataSheet>().GetByHashTag("coreabilitypoint");
			var caItem = ItemCreator.MakeInstantItem(caData);
			caItem.count = new IdleNumber(100);
			instantItems.Add(caItem);

			// 유닛 레벨업재화
			var unitLvMoney = DataManager.Get<ItemDataSheet>().GetByHashTag("unitlevelup");
			var unitLvItem = ItemCreator.MakeInstantItem(unitLvMoney);
			unitLvItem.count = new IdleNumber(5);
			instantItems.Add(unitLvItem);

			// 하트(테스트)
			var heartData = DataManager.Get<ItemDataSheet>().Get(1701100003);
			var heartItem = ItemCreator.MakeInstantItem(heartData);
			heartItem.count = new IdleNumber(5);
			instantItems.Add(heartItem);

			// 보스열쇠(테스트)
			var bossData = DataManager.Get<ItemDataSheet>().Get(1701100004);
			var bossItem = ItemCreator.MakeInstantItem(bossData);
			bossItem.count = new IdleNumber(2);
			instantItems.Add(bossItem);

			// 광고소환 3종
			for (long i = 1701105031; i <= 1701105033; i++)
			{
				var comData = DataManager.Get<ItemDataSheet>().Get(i);
				var comitem = ItemCreator.MakeInstantItem(comData);
				comitem.count = new IdleNumber(3);
				instantItems.Add(comitem);
			}


			///* 
			// * ★★★★★★★★★★★★★★★★★★★★★★★
			// *  경험치 아이템 생성
			// * ★★★★★★★★★★★★★★★★★★★★★★★
			// */
			// 광고경험치 3종
			for (long i = 1702300001; i <= 1702300006; i += 2)
			{
				var comData = DataManager.Get<ItemDataSheet>().Get(i);
				var comitem = ItemCreator.MakeInstantItem(comData);
				comitem.count = new IdleNumber(1);
				instantItems.Add(comitem);
			}

			/* 
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 *  유닛 아이템 생성
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 */

			// 기본 유닛 생성
			var mageData = DataManager.Get<ItemDataSheet>().GetByHashTag("defaultunit");
			var mageItem = ItemCreator.MakeInstantItem(mageData);
			mageItem.count = new IdleNumber(1);
			instantItems.Add(mageItem);

			var charData = DataManager.Get<ItemDataSheet>().Get(1701204001);
			var charItem = ItemCreator.MakeInstantItem(charData);
			charItem.count = new IdleNumber(1);
			instantItems.Add(charItem);


			/* 
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 *  무기 아이템 생성
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 */
			var defaultWpData = DataManager.Get<ItemDataSheet>().Get(1701301001);
			var defauitititem = ItemCreator.MakeInstantItem(defaultWpData);
			defauitititem.count = new IdleNumber(10);
			instantItems.Add(defauitititem);

			var rareWpData = DataManager.Get<ItemDataSheet>().Get(1701301006);
			var rareititem = ItemCreator.MakeInstantItem(rareWpData);
			rareititem.count = new IdleNumber(1);
			instantItems.Add(rareititem);



			/* 
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 *  스킬 아이템 생성
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 */
			// 매직미사일
			var skData = DataManager.Get<ItemDataSheet>().Get(1701600001);
			var skitem = ItemCreator.MakeInstantItem(skData);
			skitem.count = new IdleNumber(1);
			instantItems.Add(skitem);



			/* 
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 *  유물 아이템 생성
			 * ★★★★★★★★★★★★★★★★★★★★★★★
			 */
			// 공격유물
			var atkData = DataManager.Get<ItemDataSheet>().Get(1701807001);
			var atkitem = ItemCreator.MakeInstantItem(atkData);
			atkitem.count = new IdleNumber(20);
			instantItems.Add(atkitem);

			// 동료 5명
			for (long i = 1701708001; i <= 1701708005; i++)
			{
				var comData = DataManager.Get<ItemDataSheet>().Get(i);
				var comitem = ItemCreator.MakeInstantItem(comData);
				comitem.count = new IdleNumber(30);
				instantItems.Add(comitem);
			}
			// 훈련
			for (long i = 1701900001; i <= 1701900010; i++)
			{
				var trainingData = DataManager.Get<ItemDataSheet>().Get(i);
				var trItem = ItemCreator.MakeInstantItem(trainingData);
				trItem.count = new IdleNumber(1);
				instantItems.Add(trItem);
			}
			// 특성
			for (long i = 1702000001; i <= 1702000008; i++)
			{
				var propData = DataManager.Get<ItemDataSheet>().Get(i);
				var propItem = ItemCreator.MakeInstantItem(propData);
				propItem.count = new IdleNumber(1);
				instantItems.Add(propItem);
			}
			// 마스터리
			for (long i = 1702100001; i <= 1702100004; i++)
			{
				var propData = DataManager.Get<ItemDataSheet>().Get(i);
				var propItem = ItemCreator.MakeInstantItem(propData);
				propItem.count = new IdleNumber(1);
				instantItems.Add(propItem);
			}
			// 코어
			for (long i = 1702200001; i <= 1702200004; i++)
			{
				var propData = DataManager.Get<ItemDataSheet>().Get(i);
				var propItem = ItemCreator.MakeInstantItem(propData);
				propItem.count = new IdleNumber(1);
				instantItems.Add(propItem);
			}
		}

		public void MakeTestSkillSlots()
		{
			skillSlots[0] = 1701600001;
		}

		public void MakeTestPetSlots()
		{
			if (PlayerPrefs.GetString("GameSceneName") != "SampleScene")
			{
				petSlots[0] = 1701708001;
				petSlots[1] = 1701708002;
				petSlots[2] = 1701708003;
			}
		}
	}
}


[Serializable]
public class AbilityInfo
{
	public Stats type;
	public IdleNumber value;
	public IdleNumber inc;


	public AbilityInfo()
	{

	}

	public AbilityInfo(Stats _type, IdleNumber _value)
	{
		type = _type;
		value = _value;
		inc = new IdleNumber(0);
	}

	public AbilityInfo(Stats _type, IdleNumber _value, IdleNumber _inc)
	{
		type = _type;
		value = _value;
		inc = _inc;
	}

	public IdleNumber GetValue(int _level)
	{
		IdleNumber returnValue = value + (inc * (_level - 1));

		return returnValue;
	}

	public override string ToString()
	{
		return $"{type}. {value.ToString()}";
	}
}
