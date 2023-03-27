using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static partial class UserInfo
{
	public class EquipInfo
	{
		/*
		 * 유닛
		 */
		public long EquipUnitItemTid
		{
			get
			{
				if (userData.equipUnitItemTid == 0)
				{
					var defaultUnit = DataManager.Get<UnitItemDataSheet>().GetByHashTag("defaultunit");
					if (defaultUnit == null)
					{
						PopAlert.Create(new VResult().SetFail(VResultCode.NO_DEFINED_DEFAULT_CHAR), PopupCallback.GoToIntro);
						return 0;
					}

					userData.equipUnitItemTid = defaultUnit.tid;
				}

				return userData.equipUnitItemTid;
			}
		}
		public void EquipUnit(long _itemTid)
		{
			userData.equipUnitItemTid = _itemTid;
		}



		/*
		 * 스킬
		 */
		public long[] skills => userData.skillSlots;
		public bool IsEquipSkill(long _itemTid)
		{
			foreach (var v in userData.skillSlots)
			{
				if (v == _itemTid)
				{
					return true;
				}
			}

			return false;
		}
		public void EquipSkill(int _index, long _itemTid)
		{
			userData.skillSlots[_index] = _itemTid;
		}




		/*
		 * 동료
		 */
		public long[] pets => userData.petSlots;
		public bool IsEquipPet(long _itemTid)
		{
			foreach (var v in userData.petSlots)
			{
				if (v == _itemTid)
				{
					return true;
				}
			}

			return false;
		}
		public void EquipPet(int _index, long _itemTid)
		{
			userData.petSlots[_index] = _itemTid;
		}




		/*
		 * 장착 아이템
		 */
		public long EquipWeaponTid => userData.EquipWeaponTid;
		public long EquipArmorTid => userData.EquipArmerTid;
		public long EquipAccessoryTid => userData.EquipAccessoryTid;
		public long EquipNecklaceTid => userData.EquipNecklaceTid;
		public void EquipWeapon(long _itemTid)
		{
			userData.EquipWeaponTid = _itemTid;
		}
		public void EquipArmor(long _itemTid)
		{
			userData.EquipArmerTid = _itemTid;
		}
		public void EquipAccessory(long _itemTid)
		{
			userData.EquipAccessoryTid = _itemTid;
		}
		public long GetUserEquipItem(ItemType _itemType)
		{
			switch (_itemType)
			{
				case ItemType.Unit:
					return EquipUnitItemTid;
				case ItemType.Weapon:
					return EquipWeaponTid;
				case ItemType.Armor:
					return EquipArmorTid;
				case ItemType.Ring:
					return EquipAccessoryTid;
				default:
					return 0;
			}
		}
	}
}
