using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class BattlePassTierInfo
	{
		public bool isUnlock;
		[SerializeField] private bool _isFreeRawardClaim;
		public bool isFreeRawardClaim => _isFreeRawardClaim;
		public RewardInfo freeReward { get; private set; }
		[SerializeField] private bool _isPassRawardClaim;
		public bool isPassRawardClaim => _isPassRawardClaim;
		public RewardInfo passReward { get; private set; }

		public RequirementInfo condition;
		public BattlePassTierInfo(RequirementInfo condition, BattlePassTier tierData)
		{
			this.condition = condition;
			freeReward = new RewardInfo(tierData.freeReward);
			passReward = new RewardInfo(tierData.passReward);
		}

		public void Load(BattlePassTierInfo info)
		{
			isUnlock = info.isUnlock;
			_isFreeRawardClaim = info.isFreeRawardClaim;
			_isPassRawardClaim = info.isPassRawardClaim;
		}

		public string CheckCondition()
		{
			if (condition.IsPassFulfill(out string message))
			{
				message = "";
			}
			else
			{
				if (message.IsNullOrEmpty())
				{
					message = "조건을 충족하지 못했습니다.";
				}
			}


			return message;
		}

		public void GetFreeReward()
		{
			PlatformManager.UserDB.AddRewards(new List<RuntimeData.RewardInfo>() { freeReward }, true, true);
			_isFreeRawardClaim = true;
		}
		public void GetPassReward()
		{
			PlatformManager.UserDB.AddRewards(new List<RuntimeData.RewardInfo>() { passReward }, true, true);
			_isPassRawardClaim = true;
		}
	}
	[System.Serializable]
	public class BattlePassInfo : BaseInfo
	{

		[SerializeField] private int _passXp;
		public int PassXP => _passXp;
		[SerializeField] private List<BattlePassTierInfo> _tierInfoList;
		public List<BattlePassTierInfo> TierInfoList => _tierInfoList;
		public BattlePassData rawData { get; private set; }

		//public RuntimeData.ShopInfo battlePassShopInfo { get; private set; }
		public RuntimeData.PersistentItemInfo PersistentItem { get; private set; }
		public RuntimeData.ShopInfo ShopInfo { get; private set; }
		public override void SetRawData<T>(T data)
		{
			rawData = data as BattlePassData;

			tid = rawData.tid;

			_tierInfoList = new List<BattlePassTierInfo>();
			for (int i = 0; i < rawData.tierList.Length; i++)
			{

				RequirementInfo info = new RequirementInfo();
				info.type = rawData.tierCondition.info.type;
				info.parameter1 = rawData.tierCondition.info.parameter1;
				info.parameter2 = rawData.tierCondition.info.parameter2 + (i * rawData.tierCondition.incValuePerTier);
				_tierInfoList.Add(new BattlePassTierInfo(info, rawData.tierList[i]));
			}
		}

		public override void UpdateData()
		{
			PersistentItem = PlatformManager.UserDB.inventory.GetPersistent(rawData.persistentItemTid);
			ShopInfo = PlatformManager.UserDB.shopContainer[ShopType.BATTLEPASS].Find(x => x.Tid == rawData.shopTid);
		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}

			BattlePassInfo temp = info as BattlePassInfo;

			for (int i = 0; i < _tierInfoList.Count; i++)
			{
				if (i < temp.TierInfoList.Count)
				{
					_tierInfoList[i].Load(temp.TierInfoList[i]);
				}
			}
			_passXp = temp._passXp;
		}

		public void GainXp(int xp)
		{
			_passXp = xp;
		}

		public void ClaimFreeReward(BattlePassTierInfo info)
		{

		}

		public void ClaimPassReward(BattlePassTierInfo info)
		{

		}

		public void Purchase()
		{
			ShopInfo.OnPurchaseSuccess();
		}
	}
}


public class BattlePassContainer : BaseContainer
{
	public List<RuntimeData.BattlePassInfo> infoList = new List<RuntimeData.BattlePassInfo>();

	public override void Dispose()
	{

	}

	public override void FromJson(string json)
	{
		BattlePassContainer temp = CreateInstance<BattlePassContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref infoList, temp.infoList);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		SetListRawData(ref infoList, DataManager.Get<BattlePassDataSheet>().GetInfosClone());

	}

	public override void DailyResetData()
	{

	}

	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < infoList.Count; i++)
		{
			infoList[i].UpdateData();
		}
	}
}
