using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;

namespace RuntimeData
{
	[System.Serializable]
	public class CurrencyInfo : ItemInfo
	{
		[SerializeField] private IdleNumber value;
		public override IdleNumber Value => value;
		public IdleNumber max { get; private set; }
		public CurrencyType type { get; private set; }
		public CurrencyData rawData { get; private set; }

		public Sprite IconImage
		{
			get
			{
				if (itemObject == null)
				{
					return null;
				}
				return itemObject.Icon;
			}
		}
		public CurrencyItemObject itemObject { get; private set; }
		public CurrencyInfo()
		{

		}

		public override void Load<T>(T info)
		{
			base.Load(info);
			SetDirty();

			CurrencyInfo currencyInfo = info as CurrencyInfo;

			value = currencyInfo.value;
			value.Turncate();
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as CurrencyData;
			tid = rawData.tid;
			value = (IdleNumber)0;
			max = (IdleNumber)rawData.maxValue;
			type = rawData.type;

			itemObject = GameManager.UserDB.inventory.GetScriptableObject<CurrencyItemObject>(tid);
		}


		public bool Check(IdleNumber cost)
		{
			if (value - cost < 0)
			{
				return false;
			}

			return true;
		}

		public bool Pay(IdleNumber cost)
		{
			if (Check(cost) == false)
			{
				return false;
			}

			value -= cost;
			value.Turncate();
			return true;
		}

		public bool Earn(IdleNumber money)
		{
			if (value + money > max)
			{
				value = max;
				return true;
			}

			value += money;
			value.Turncate();
			return true;
		}
	}

	[System.Serializable]
	public class RewardBoxInfo : ItemInfo
	{
		public List<RewardInfo> rewardInfo;
		public RewardBoxData rawData { get; private set; }
		public RewardBoxInfo()
		{ }
		public RewardBoxInfo(long _tid)
		{
			rawData = DataManager.Get<RewardBoxDataSheet>().Get(_tid);
			tid = rawData.tid;



		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as RewardBoxData;
			tid = rawData.tid;


		}

		public void SetReward()
		{
			rewardInfo = new List<RewardInfo>();
			for (int i = 0; i < rawData.rewards.Count; i++)
			{
				rewardInfo.Add(new RewardInfo(rawData.rewards[i]));
			}
		}
		public override void Load<T>(T info)
		{
			base.Load(info);
			SetDirty();
		}

		public void AddRewardBox(int _count)
		{
			count += _count;
		}


	}


	[System.Serializable]
	public class RewardInfo
	{
		[SerializeField] private long tid;
		public long Tid => tid;
		[SerializeField] private RewardCategory category;
		public RewardCategory Category => category;
		[SerializeField] private float chance;
		public float Chance => chance;
		[SerializeField] private IdleNumber countMin;
		[SerializeField] private IdleNumber countMax;
		[SerializeField] private IdleNumber countPerStage;

		public IdleNumber fixedCount { get; private set; }

		public Sprite iconImage { get; private set; }

		public Grade grade { get; private set; }

		private bool isFixed = false;
		public string name { get; private set; }
		public RewardInfo(Reward _reward)
		{
			tid = _reward.tid;
			category = _reward.category;
			chance = _reward.chance;

			countMin = (IdleNumber)_reward.countMin;
			countMax = (IdleNumber)_reward.countMax;
			countPerStage = (IdleNumber)_reward.countPerStage;

			iconImage = null;
			grade = Grade.D;
			isFixed = false;
			SetReward();
		}

		/// <summary>
		/// 웬만하면 가챠 결과로만 사용할것 
		/// </summary>
		/// <param name="_tid"></param>
		/// <param name="_category"></param>
		/// <param name="_grade"></param>
		/// <param name="_count"></param>
		public RewardInfo(long _tid, RewardCategory _category, Grade _grade, int _count) : this(_tid, _category, _grade, (IdleNumber)_count)
		{

		}
		public RewardInfo(long _tid, RewardCategory _category, Grade _grade, IdleNumber _count)
		{
			tid = _tid;
			category = _category;
			grade = _grade;
			fixedCount = _count;
			countMin = _count;
			countMax = (IdleNumber)0;
			countPerStage = (IdleNumber)0;
			iconImage = null;
			isFixed = true;
			SetReward();
		}

		private void SetReward()
		{
			fixedCount = countMin;
			switch (category)
			{
				case RewardCategory.Ability: break;
				case RewardCategory.Equip:
					{
						//Debug.Log("Reward Equip");
						var data = DataManager.Get<EquipItemDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Equip");
							return;
						}

						GameManager.UserDB.equipContainer.GetScriptableObject(tid, out EquipItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.Icon;
						}
						grade = data.itemGrade;
					}
					break;
				case RewardCategory.Costume:
					{
						//Debug.Log("Reward Costume");
						var data = DataManager.Get<CostumeDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Costume");
							return;
						}
						name = data.name;
						GameManager.UserDB.costumeContainer.GetScriptableObject(tid, out CostumeItemObject scriptableObject);

						if (scriptableObject != null)
						{
							iconImage = scriptableObject.Icon;
						}
						grade = data.itemGrade;
					}
					break;
				case RewardCategory.Pet:
					{
						//Debug.Log("Reward Pet");//
						var data = DataManager.Get<PetDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Pet");
							return;
						}
						GameManager.UserDB.petContainer.GetScriptableObject(tid, out PetItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.Icon;
						}
						grade = data.itemGrade;
					}
					break;
				case RewardCategory.Currency:
					{
						//Debug.Log("Reward Currency");
						var data = DataManager.Get<CurrencyDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Currency");
							return;
						}
						GameManager.UserDB.inventory.GetScriptableObject(tid, out CurrencyItemObject scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.Icon;
						}
						grade = data.itemGrade;
					}

					break;
				case RewardCategory.Skill:
					{
						//Debug.Log("Reward Skil");
						var data = DataManager.Get<SkillDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in Skill");
							return;
						}

						GameManager.UserDB.skillContainer.GetScriptableObject(tid, out SkillCore scriptableObject);
						name = data.name;
						if (scriptableObject != null)
						{
							iconImage = scriptableObject.Icon;
						}
						grade = data.itemGrade;
					}
					break;
				case RewardCategory.EXP:
					grade = Grade.D;
					iconImage = Resources.Load<Sprite>("ExpIcon");
					name = "경험치";
					break;
				case RewardCategory.RewardBox:
					{
						var data = DataManager.Get<RewardBoxDataSheet>().Get(tid);
						if (data == null)
						{
							Debug.LogError($"{tid} not in RewardBox");
							return;
						}

						name = data.name;
						iconImage = Resources.Load<Sprite>("RandomBox");
						//var scriptableObject = GameManager.UserDB.costumeContainer.GetScriptableObject(tid);

						//if (scriptableObject != null)
						//{
						//	iconImage = scriptableObject.Icon;
						//}
						grade = data.itemGrade;
					}
					break;
			}

		}

		public void UpdateCount(IdleNumber _step)
		{
			IdleNumber count = new IdleNumber();
			IdleNumber step = _step;
			if (countMax == 0)
			{
				count = countMin + (countPerStage * step);
			}
			else
			{
				count = IdleRandom.Random(countMin, countMax) + (countPerStage * step);
			}

			count.Turncate();
			fixedCount = count;
			isFixed = true;
		}

		public void UpdateCount(int stage = 0)
		{
			UpdateCount((IdleNumber)stage);
		}
	}
}


