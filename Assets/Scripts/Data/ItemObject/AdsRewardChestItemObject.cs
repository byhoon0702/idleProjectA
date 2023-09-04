using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdsRewardChestItemObject : ItemObject
{

	public GameObject prefab;
	public AdsRewardChestData rawData { get; private set; }

	[SerializeField] private int _viewCount;
	public int ViewCount => _viewCount;

	public float appearTime { get; private set; }

	private float _time;
	private bool isShow;
	private System.Action _action;
	private RuntimeData.RewardInfo _reward;
	public void LoadData()
	{
		rawData = DataManager.Get<AdsRewardChestDataSheet>().Get(tid);
		_viewCount = rawData.dailyViewCount;
		appearTime = 0;
		isShow = false;
	}

	public void SetEvent(System.Action action)
	{
		_action = action;
	}


	public void Select()
	{
		appearTime = Random.Range(rawData.appearMinTime, rawData.appearMaxTime);
		_time = 0;
		isShow = false;
	}
	public bool IsWatchAll()
	{
		return _viewCount <= 0;
	}


	public RuntimeData.RewardInfo MakeRewardInfo()
	{
		var item = PlatformManager.UserDB.inventory.FindCurrency(rawData.reward.type);


		if (rawData.reward.isFixed)
		{
			_reward = new RuntimeData.RewardInfo(item.Tid, RewardCategory.Currency, Grade.D, (IdleNumber)rawData.reward.value);
		}
		else
		{
			///시간당 계산식이 들어가야함
			_reward = new RuntimeData.RewardInfo(item.Tid, RewardCategory.Currency, Grade.D, 100);
		}
		return _reward;
	}

	public void GetReward()
	{
		if (_reward == null)
		{
			MakeRewardInfo();
		}
		List<RuntimeData.RewardInfo> rewardList = new List<RuntimeData.RewardInfo>();
		rewardList.Add(_reward);
		PlatformManager.UserDB.AddRewards(rewardList, true);
	}

	public void Watch()
	{
		_viewCount--;
		//if(AdsFree)
		{
			GetReward();
			return;
		}
	}

	public void OnUpdate(float time)
	{
		if (isShow)
		{
			return;
		}
		_time += time;
		if (_time >= appearTime)
		{
			_action?.Invoke();
			isShow = true;
			_time = 0;
		}
	}

	public override void SetBasicData<T>(T data)
	{
		tid = data.tid;
		description = data.description;
	}
}
