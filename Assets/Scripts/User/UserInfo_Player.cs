using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class PlayerInfo
	{
		public int UserLv
		{
			get
			{
				var itemExp = Inventory.it.FindItemByTid(Inventory.it.UserExpTid) as ItemUserExp;
				if (itemExp == null)
				{
					return 1;
				}
				else
				{
					return itemExp.Level;
				}
			}
		}
		public long ExpTotal
		{
			get
			{
				var itemExp = Inventory.it.FindItemByTid(Inventory.it.UserExpTid) as ItemUserExp;
				if (itemExp == null)
				{
					return 0;
				}
				else
				{
					return itemExp.Exp;
				}
			}
		}
		public long CurrExp
		{
			get
			{
				var itemExp = Inventory.it.FindItemByTid(Inventory.it.UserExpTid) as ItemUserExp;
				if (itemExp == null)
				{
					return 0;
				}
				else
				{
					return ExpTotal - itemExp.levelInfo.beginExp;
				}
			}
		}
		public long NextExp
		{
			get
			{
				var itemExp = Inventory.it.FindItemByTid(Inventory.it.UserExpTid) as ItemUserExp;
				if (itemExp == null)
				{
					return 0;
				}
				else
				{
					return itemExp.levelInfo.nextExp - itemExp.levelInfo.beginExp;
				}
			}
		}
		public float ExpRatio
		{
			get
			{
				if (NextExp == 0)
				{
					return 0;
				}
				else
				{
					return (float)System.Math.Clamp((double)CurrExp / NextExp, 0, 1);
				}
			}
		}
		private UserGradeData userGrade;
		public UserGradeData UserGrade
		{
			get
			{
				if (userGrade == null || userGrade.grade != userData.userGrade)
				{
					userGrade = DataManager.Get<UserGradeDataSheet>().Get(userData.userGrade);
				}

				return userGrade;
			}
		}
		public int TotalPropertyPoint
		{
			get
			{
				int total = (UserLv - 1) * VGameManager.Config.PROPERTY_POINT_PER_LEVEL;
				return Mathf.Abs(total);
			}
		}
		public int RemainPropertyPoint
		{
			get
			{
				int result = TotalPropertyPoint - UsingPropertyPoint;

				return result;
			}
		}
		public int UsingPropertyPoint
		{
			get
			{
				var itemList = Inventory.it.FindItemsByType(ItemType.Property);
				int total = 0;
				foreach (var item in itemList)
				{
					total += (item as ItemProperty).GetUsingPropertyPoint();
				}

				return total;
			}
		}
		public int TotalMasteryPoint
		{
			get
			{
				var unitList = Inventory.it.FindItemsByType(ItemType.Unit);
				int total = 0;
				foreach (var unit in unitList)
				{
					total += (unit.Level - 1);
				}
				return total;
			}
		}
		public int RemainMasteryPoint
		{
			get
			{
				int result = TotalMasteryPoint - UsingMasteryPoint;

				return result;
			}
		}
		public int UsingMasteryPoint
		{
			get
			{
				var itemList = Inventory.it.FindItemsByType(ItemType.Mastery);
				int total = 0;
				foreach (var item in itemList)
				{
					total += (item as ItemMastery).GetUsingMasteryPoint();
				}

				return total;
			}
		}


		public IdleNumber TotalCombatPower()
		{
			IdleNumber outNumber = new IdleNumber(info.UserLv * 100);
			return outNumber;
		}
		public void UserGradeUp()
		{
			var sheet = DataManager.Get<UserGradeDataSheet>();
			Grade nextGrade = sheet.NextGrade(UserGrade.grade);

			if (nextGrade < userGrade.grade)
			{
				return;
			}

			userData.userGrade = nextGrade;
		}

		public void ResetProperty()
		{
			Inventory.it.ResetProperty();
		}

		public void ResetMastery()
		{
			Inventory.it.ResetMastery();
		}
	}
}
