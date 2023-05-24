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

	private bool isGoldBoxRewardable => goldBoxCount < GameManager.Config.DAILY_GOLD_BOX_LIMIT;
	private bool isDiaBoxRewardable => diaBoxCount < GameManager.Config.DAILY_DIA_BOX_LIMIT;

	private bool isInitialized = false;

	public void Init()
	{
		isInitialized = true;

		goldBoxCount = 0;
		diaBoxCount = 0;

		SetGoldBoxFrequency();
		SetDiaBoxFrequency();
	}

	public void Switch(bool isOn)
	{
		ResetFrequency();
		gameObject.SetActive(isOn);
	}

	public void SetGoldBoxFrequency()
	{
		//테스트
		goldBoxFrequency = 10;// UnityEngine.Random.Range(VGameManager.Config.MINIMUM_GOLD_AD_FREQUENCY, VGameManager.Config.MAXIMUM_GOLD_AD_FREQUENCY);
	}

	public void SetDiaBoxFrequency()
	{//테스트
		diaBoxFrequency = 15;// UnityEngine.Random.Range(VGameManager.Config.MINIMUM_DIA_AD_FREQUENCY, VGameManager.Config.MAXIMUM_DIA_AD_FREQUENCY);
	}

	public void ResetFrequency()
	{
		isShow = false;
		if (goldBoxFrequency <= 0)
		{
			goldBoxFrequency = UnityEngine.Random.Range(GameManager.Config.MINIMUM_GOLD_AD_FREQUENCY, GameManager.Config.MAXIMUM_GOLD_AD_FREQUENCY);
		}
		if (diaBoxFrequency <= 0)
		{
			diaBoxFrequency = UnityEngine.Random.Range(GameManager.Config.MINIMUM_DIA_AD_FREQUENCY, GameManager.Config.MAXIMUM_DIA_AD_FREQUENCY);
		}
	}

	public void OpenAdReward(int _rewardTid, IdleNumber _count)
	{
		rewardPage.ShowPage(_rewardTid, _count);
	}

	bool isShow = false;

	private void Update()
	{
		if (isInitialized == false)
		{
			return;
		}
		if (isShow)
		{
			return;
		}

		goldBoxFrequency -= Time.deltaTime;
		diaBoxFrequency -= Time.deltaTime;

		if (goldBoxFrequency <= 0)
		{
			if (goldChest.IsActivated == false && isGoldBoxRewardable)
			{
				isShow = true;
				goldChest.Show();
			}
		}

		if (diaBoxFrequency <= 0)
		{
			if (diaChest.IsActivated == false && isDiaBoxRewardable)
			{
				isShow = true;
				diaChest.Show();
			}
		}
	}
}
