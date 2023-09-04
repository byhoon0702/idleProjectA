
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.CloudCode;

public interface FSM
{
	FSM OnEnter();
	void OnUpdate(float time);
	void OnExit();

	FSM RunNextState(float time);
}

public enum GameState
{
	None = 0,
	LOADING = 10,
	BGLOADING = 20,
	PLAYERSPAWN = 30,
	BATTLESTART = 100,
	BATTLE,
	REWARD,
	BATTLEEND,
	BOSSSPAWN = 200,
	BOSSSTART,
	BOSSBATTLE,
	BOSSEND,
}


/// 전투 흐름
/// 로딩 화면
/// 배경 로딩
/// 캐릭터 스폰
/// 전투 시작
/// 전투
/// 전투 종료


public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager it
	{
		get
		{
			return instance;
		}
	}
	public float gameSpeed = 1;
	public bool fixedScroll;

	public bool firstEnter = true;

	///public FSM currentFSM;
	public GameState currentState;

	[SerializeField] private UserInfoContainer userInfoContainer;
	public static UserInfoContainer UserInfoContainer
	{
		get { return it.userInfoContainer; }
	}


	public BattleRecord battleRecord;

	[SerializeField] private SystemSleepMode sleepMode;
	public SystemSleepMode SleepMode => sleepMode;

	public static ConfigMeta Config
	{
		get
		{
			return PlatformManager.ConfigMeta;
		}
	}
	private float touchTime;
	public bool isSleepMode;

	public static bool GameStop = false;
	public static float GlobalTimeScale = 1f;

	public event Action<List<RuntimeData.RewardInfo>> OnSleepModeAcquiredItem;
	public event Action<IdleNumber> OnSleepModeAcquiredGold;
	public event Action<IdleNumber> OnSleepModeAcquiredExp;

	public event Action AddRewardEvent;

	long currTicks;
	long prevPlayTicks;
	private void Awake()
	{
		instance = this;
	}

	public void SleepModeAcquiredItem(List<RuntimeData.RewardInfo> info)
	{
		OnSleepModeAcquiredItem?.Invoke(info);
	}
	public void SleepModeAcquiredGold(IdleNumber info)
	{
		OnSleepModeAcquiredGold?.Invoke(info);
	}
	public void SleepModeAcquiredExp(IdleNumber info)
	{
		OnSleepModeAcquiredExp?.Invoke(info);
	}

	bool containerInit = false;
	// Start is called before the first frame update
	async void Start()
	{
		GameStop = false;
		//DataManager.LoadAllJson();

		battleRecord = new BattleRecord();

		Application.targetFrameRate = 60;

		PlatformManager.UserDB.Init();

		StageManager.it.Init();
		touchTime = Time.realtimeSinceStartup;
		try
		{
			//await RemoteConfigManager.Instance.FetchConfigs();
			//currentFSM = loadingState.OnEnter();

			await CallTimeStampEndPoint();

			var playerInfo = await PlatformManager.Instance.GetPlayerInfo();
			GameSetting.Instance.AutoPowerSaveChanged += OnAutoPowerSaveChanged;

			double totalMinutes = GetOfflineRewardTime();
			if (totalMinutes >= 1)
			{
				StageManager.it.OfflineRewards((int)totalMinutes);
			}
			containerInit = true;
		}

		catch (Exception ex)
		{

		}
	}
	void OnAutoPowerSaveChanged(bool isOn)
	{
		touchTime = Time.realtimeSinceStartup;
	}

	private void OnDestroy()
	{
		GameSetting.Instance.AutoPowerSaveChanged -= OnAutoPowerSaveChanged;

	}
	//Unity Cloud Code 를 이용하여 서버시간을 가져옴, 데이터 로드 이후 가장 먼저 실행되어야함
	public async Task CallTimeStampEndPoint()
	{
		try
		{
			long timeStamp = await CloudCodeService.Instance.CallEndpointAsync<long>("TimeStamp", new Dictionary<string, object>());
			if (this == null) return;
			DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			Debug.Log(timeStamp / 1000);

			var utcerverTime = dt.AddSeconds(timeStamp / 1000);

			TimeManager.Instance.SetServerTime(utcerverTime);

			SetLoginTime();
		}
		catch (CloudCodeException e)
		{
			Debug.LogException(e);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	public void SetLoginTime()
	{
		DateTime resetTime = TimeManager.Instance.UtcResetTime;

		if (DateTime.TryParse(PlatformManager.UserDB.userInfoContainer.LastLoginTime, out DateTime result))
		{
			TimeSpan ts = resetTime - result;
			//최근 접속 시간과 하루 이상 차이가 나면 초기화
			if (ts.TotalMinutes >= 0)
			{
				PlatformManager.UserDB.ResetDataByDateTime();
			}

			TimeManager.Instance.LastLoginTimeForOfflineReward = result.ToString();
		}
		else
		{
			Debug.LogWarning("Could not Parse 'String to Datetime'. Presume New User");
			PlatformManager.UserDB.ResetDataByDateTime();
			TimeManager.Instance.LastLoginTimeForOfflineReward = TimeManager.Instance.UtcNow.ToString();
		}
		var time = TimeManager.Instance.UtcNow;
		PlatformManager.UserDB.userInfoContainer.LastLoginTime = time.ToString();

	}

	private void EnterSleepMode(bool inputAnyKey)
	{
		if (inputAnyKey == false)
		{
			if (isSleepMode == false)
			{

				if (Time.realtimeSinceStartup - touchTime > 50)
				{

					sleepMode.Open();

				}
			}
		}
		else
		{
			touchTime = Time.realtimeSinceStartup;
		}
	}
	private void OnInputMobile()
	{
		EnterSleepMode(Input.touchCount > 0);

	}
	private void OnInputPC()
	{
		EnterSleepMode(Input.anyKey);
	}
	private void CheckSleepMode()
	{
		if (StageManager.it.CurrentStage.StageType != StageType.Normal)
		{
			touchTime = Time.realtimeSinceStartup;
			return;
		}
#if UNITY_EDITOR
		OnInputPC();
#else
		OnInputMobile();
#endif
	}

	private void Update()
	{
		if (GameSetting.Instance.AutoPowerSave)
		{
			CheckSleepMode();
		}

		if (containerInit)
		{
			PlatformManager.UserDB.buffContainer.OnUpdateBuff();

			currTicks = TimeManager.Instance.Now.Ticks;
			PlatformManager.UserDB.userInfoContainer.userInfo.PlayTicks += (currTicks - prevPlayTicks);
			PlatformManager.UserDB.userInfoContainer.userInfo.DailyPlayTicks += (currTicks - prevPlayTicks);
			prevPlayTicks = currTicks;
		}

	}

	float time = 0f;
	private void FixedUpdate()
	{
		time += Time.fixedUnscaledDeltaTime;

		if (time > 1f)
		{
			PlatformManager.UserDB.Save();
			time = 0;
		}
	}

	public void CallAddRewardEvent()
	{
		AddRewardEvent?.Invoke();
	}

	public double GetOfflineRewardTime()
	{
		DateTime lastTime = DateTime.Now;
		DateTime.TryParse(TimeManager.Instance.LastLoginTimeForOfflineReward, out lastTime);
		TimeSpan ts = TimeManager.Instance.UtcNow - lastTime;
		return ts.TotalMinutes;
	}

	DateTime applicationTime;
	private async void OnApplicationFocus(bool focus)
	{
		if (focus)
		{

			PlatformManager.Instance.ShowLoadingRotate(true);
			await CallTimeStampEndPoint();
			PlatformManager.Instance.ShowLoadingRotate(false);
			TimeSpan ts = TimeManager.Instance.Now - applicationTime;
			if (ts.TotalMinutes >= 1)
			{

				Debug.Log(ts.TotalMinutes);
				StageManager.it.OfflineRewards((int)ts.TotalMinutes);
			}
		}
		else
		{
			applicationTime = new DateTime(TimeManager.Instance.Now.Ticks);
		}
	}
}
