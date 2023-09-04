using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreType
{
	GOOGLE,
	APPLE,
}


public enum TimeLimitType
{
	//제한없음
	NONE,
	//계정당
	USER,
	//일일
	DAILY,
	//주간
	WEEKLY,
	//월간 
	MONTHLY,

}


[System.Serializable]
public class ShopItemData
{
	/// 아이템 카테고리
	/// </summary>
	public RewardCategory category;
	/// <summary>
	/// 아이템 Tid
	/// </summary>
	public long tid;
	/// <summary>
	/// 수량 최소, 최대
	/// </summary>
	public string count;
}


[System.Serializable]
public class ShopTimeData
{
	public string displayStartTime;
	public string displayEndTime;

	//어떤 조건이든 일일 시간 체크
	public long dailyStartSecond;
	public long dailyEndSecond;
	////////////////

	public TimeLimitType limitType;
	public int buyLimitCount;
}

[System.Serializable]
public class ShopOpenData
{
	public OpenCondition openCondition;
	public float openDuration;
}



[System.Serializable]
public class ShopData : BaseData
{
	public ShopType shopType;

	public float saleRatio;
	public ShopOpenData openData;
	public ShopTimeData timeData;
	public Cost cost;
	public List<PlatformProductID> productIDs;
	public List<Reward> itemList;
	public bool isHide;
}

[System.Serializable]
public class PlatformProductID
{
	public StoreType platform;
	public string id;

}


[System.Serializable]
public class ShopDataSheet : DataSheetBase<ShopData>
{
	public List<ShopData> GetDatas(ShopType type)
	{
		return infos.FindAll(x => x.shopType == type);
	}

}
