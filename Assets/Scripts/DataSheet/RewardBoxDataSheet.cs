using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class Reward
{
	/// <summary>
	/// 아이템 카테고리
	/// </summary>
	public RewardCategory category;
	/// <summary>
	/// 아이템 Tid
	/// </summary>
	public long tid;

	public string count;
}

[System.Serializable]
public class ChanceReward : Reward
{
	/// <summary>
	/// 확률
	/// </summary>
	public float chance;

	/// <summary>
	/// 수량 최소, 최대
	/// </summary>
	public string countMin;
	public string countMax;

	//스테이지별 카운트 증가량
	public string countPerStage;
}

[System.Serializable]
public class RewardBoxItem
{
	/// <summary>
	/// 아이템 카테고리
	/// </summary>
	public RewardCategory category;
	/// <summary>
	/// 아이템 Tid
	/// </summary>
	public long tid;
	/// <summary>
	/// 확률
	/// </summary>
	public float chance;

	/// <summary>
	/// 수량 최소, 최대
	/// </summary>
	public string countMin;
	public string countMax;
}


[System.Serializable]
public class RewardBoxData : ItemData
{
	public List<ChanceReward> rewards;
}

[System.Serializable]
public class RewardBoxDataSheet : DataSheetBase<RewardBoxData>
{

}
