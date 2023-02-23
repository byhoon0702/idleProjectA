using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdRewardChest : MonoBehaviour
{
	[SerializeField] private UIAdRewardPage rewardPage;

	[SerializeField] private UIAdRewardChestItem goldChest;
	[SerializeField] private UIAdRewardChestItem diaChest;

	[SerializeField] private float goldBoxFrequency;
	[SerializeField] private float diaBoxFrequency;

	// dltmdduq1118 이후 서버 연동 작업
	private int goldBoxCount = 0;
	private int diaBoxCount = 0;

	private bool isGoldBoxRewardable => goldBoxCount < ConfigMeta.it.DAILY_GOLD_BOX_LIMIT;
	private bool isDiaBoxRewardable => diaBoxCount < ConfigMeta.it.DAILY_DIA_BOX_LIMIT;

	private bool isInitialized = false;

	public void Init()
	{
		isInitialized = true;

		goldBoxCount = 0;
		diaBoxCount = 0;

		SetGoldBoxFrequency();
		SetDiaBoxFrequency();
	}

	public void SetGoldBoxFrequency()
	{
		goldBoxFrequency = UnityEngine.Random.Range(ConfigMeta.it.MINIMUM_GOLD_AD_FREQUENCY, ConfigMeta.it.MAXIMUM_GOLD_AD_FREQUENCY);
	}

	public void SetDiaBoxFrequency()
	{
		diaBoxFrequency = UnityEngine.Random.Range(ConfigMeta.it.MINIMUM_DIA_AD_FREQUENCY, ConfigMeta.it.MAXIMUM_DIA_AD_FREQUENCY);
	}

	public void ResetFrequency()
	{
		if (goldBoxFrequency <= 0)
		{
			goldBoxFrequency = UnityEngine.Random.Range(ConfigMeta.it.MINIMUM_GOLD_AD_FREQUENCY, ConfigMeta.it.MAXIMUM_GOLD_AD_FREQUENCY);
		}
		if (diaBoxFrequency <= 0)
		{
			diaBoxFrequency = UnityEngine.Random.Range(ConfigMeta.it.MINIMUM_DIA_AD_FREQUENCY, ConfigMeta.it.MAXIMUM_DIA_AD_FREQUENCY);
		}
	}

	public void OpenAdReward(int _rewardTid, IdleNumber _count)
	{
		rewardPage.ShowPage(_rewardTid, _count);
	}

	private void Update()
	{
		if (isInitialized == false)
		{
			return;
		}

		goldBoxFrequency -= Time.deltaTime;
		diaBoxFrequency -= Time.deltaTime;

		if (goldBoxFrequency <= 0)
		{
			if (goldChest.IsActivated == false && isGoldBoxRewardable)
			{
				goldChest.Show();
			}
		}

		if (diaBoxFrequency <= 0)
		{
			if (diaChest.IsActivated == false && isDiaBoxRewardable)
			{
				diaChest.Show();
			}
		}
	}
}
