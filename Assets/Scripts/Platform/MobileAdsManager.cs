using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class MobileAdsManager : MonoBehaviour
{
	public static MobileAdsManager Instance;

	public GoogleAds googleAds;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}
		googleAds = new GoogleAds();
		DontDestroyOnLoad(gameObject);

	}
	// Start is called before the first frame update
	void Start()
	{
		googleAds.Init();
	}

	public void ShowAds(System.Action onClose)
	{
		googleAds.ShowRewardedAd(onClose);

	}



}

public class GoogleAds
{
	private RewardedAd _rewardedAd;

	private string _adUnitId = "ca-app-pub-4164546467580712/4915576560";
	private string _adUnitTestId = "ca-app-pub-3940256099942544/5224354917";

	public GoogleAds()
	{
		MobileAds.RaiseAdEventsOnUnityMainThread = true;
	}
	public void Init()
	{
		MobileAds.Initialize(initStatus =>
		{
			LoadRewardedAd();
		});

	}

	public void LoadRewardedAd()
	{
		if (_rewardedAd != null)
		{
			_rewardedAd.Destroy();
			_rewardedAd = null;
		}

		AdRequest adRequest = new AdRequest();

		RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
		{
			if (error != null || ad == null)
			{
				ToastUI.Instance.Enqueue($"준비된 광고가 없습니다\n{error}");
				return;
			}

			_rewardedAd = ad;
		});
	}

	public bool ShowRewardedAd(System.Action onclose)
	{

		if (_rewardedAd != null && _rewardedAd.CanShowAd())
		{
			RegisterReloadHandler(_rewardedAd);
			_rewardedAd.Show((GoogleMobileAds.Api.Reward reward) => { onclose?.Invoke(); });
			return true;
		}
		else
		{
			LoadRewardedAd();
			return false;
		}
	}

	private void RegisterReloadHandler(RewardedAd ad)
	{

		ad.OnAdFullScreenContentClosed += () =>
		{
			LoadRewardedAd();
		};
		ad.OnAdFullScreenContentFailed += (AdError error) =>
		{
			LoadRewardedAd();
		};
	}


}

