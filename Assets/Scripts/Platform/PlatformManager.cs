using System;
using GooglePlayGames;
using GooglePlayGames.OurUtils;
using GooglePlayGames.BasicApi;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;

using Unity.Services.CloudCode;
using System.Threading.Tasks;
using System.Collections.Generic;


public class PlatformManager : MonoBehaviour
{
	public static PlatformManager Instance;


	[SerializeField] private LanguageContainer languageContainer;
	public static LanguageContainer Language
	{
		get
		{
			return Instance.languageContainer;
		}
	}

	[SerializeField] private CommonData _commonData;
	public static CommonData CommonData => Instance._commonData;
	[SerializeField] private ConfigMeta _configMeta;
	public static ConfigMeta ConfigMeta => Instance._configMeta;

	public NotificationManager notificationManager;
	public LeaderBoard LeaderBoard;
	public GameObject objLoadingRotate;

	[SerializeField] private UserDB userDB;
	public bool SignInSuccess { get; private set; }
	public static UserDB UserDB
	{
		get
		{
			return Instance.userDB;
		}
	}
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


		DontDestroyOnLoad(this);

		//PlayGamesPlatform.DebugLogEnabled = true;

		PlayGamesPlatform.Activate();

		userDB = new UserDB();
		userDB.InitializeContainer();
	}

	public void InitializeGoogle()
	{

	}

	private void OnDestroy()
	{
		userDB?.Clear();
		userDB?.Dispose();
	}
	// Start is called before the first frame update
	private async void Start()
	{

		InitializationOptions options = new InitializationOptions();
#if UNITY_EDITOR
		options.SetEnvironmentName("editor");
#else
		options.SetEnvironmentName("production");
#endif
		await UnityServices.InitializeAsync(options);
		//AuthenticationService.Instance.ClearSessionToken();
		SetupEvents();

		notificationManager.Init();
	}

	public void ShowLoadingRotate(bool show)
	{
		objLoadingRotate.SetActive(show);
	}

	public async Task<PlayerInfo> GetPlayerInfo()
	{
		return await AuthenticationService.Instance.GetPlayerInfoAsync();
	}

	private void SetupEvents()
	{
		AuthenticationService.Instance.SignedIn += OnSignedIn;
		AuthenticationService.Instance.SignInFailed += OnSignInFailed;
		AuthenticationService.Instance.SignedOut += OnSignedOut;
		AuthenticationService.Instance.Expired += OnExpired;
	}

	private async void OnSignedIn()
	{
		ShowLoadingRotate(true);
		long timeStamp = await CloudCodeService.Instance.CallEndpointAsync<long>("TimeStamp", new Dictionary<string, object>());
		await AuthenticationService.Instance.GetPlayerInfoAsync();
		if (this == null) return;
		DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		Debug.Log(timeStamp / 1000);

		var utcerverTime = dt.AddSeconds(timeStamp / 1000);

		DateTime createdTime = (DateTime)AuthenticationService.Instance.PlayerInfo.CreatedAt;

		Debug.Log("Sign In Success");
		SignInSuccess = true;
		ShowLoadingRotate(false);
	}

	private void OnExpired()
	{
		SignInSuccess = false;
	}

	private void OnSignedOut()
	{

		SignInSuccess = false;
		userDB.LogOut();
	}

	private void OnSignInFailed(RequestFailedException obj)
	{
		SignInSuccess = false;
	}

	public void LogOut()
	{

		AuthenticationService.Instance.SignOut(true);

		//PlatformManager.UserDB.
		if (Intro.it == null)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Intro");
		}
	}

	public void OnClickGoogleLogin(System.Action onComplete = null)
	{
		ShowLoadingRotate(true);
		Debug.Log("구글 로그인");

		PlayGamesPlatform.Instance.ManuallyAuthenticate((success) =>
		{
			if (success == SignInStatus.Success)
			{
				PlayGamesPlatform.Instance.RequestServerSideAccess(true, async code =>
				{
					Debug.Log("Login Successful");
					await SignInGoogleAsync(code);
					onComplete?.Invoke();
				});
			}
			else
			{
				Debug.Log("Login Unseccessful");
			}
		});
		ShowLoadingRotate(false);
	}
	public async void OnClickAnonymous(System.Action onComplete = null)
	{
		ShowLoadingRotate(true);
		await SignInAnonymouslyAsync(onComplete);
		if (this == null) return;

		UserDB.SetLoginInfo(AuthenticationService.Instance.PlayerId, "Guest", "guest");
		ShowLoadingRotate(false);

	}

	public bool IsSignedIn()
	{
		return AuthenticationService.Instance.IsSignedIn;
	}

	private async Task SignInGoogleAsync(string authCode)
	{
		try
		{

			ShowLoadingRotate(true);
			await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);

			UserDB.SetLoginInfo(AuthenticationService.Instance.PlayerId, PlayGamesPlatform.Instance.localUser.userName, "google");

			Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
			//await RemoteConfigManager.Instance.FetchConfigs();
		}

		catch (AuthenticationException ex)
		{
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			Debug.LogException(ex);
		}
		ShowLoadingRotate(false);
	}

	private async Task SignInAnonymouslyAsync(System.Action onComplete = null)
	{
		try
		{
			ShowLoadingRotate(true);
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
			//await RemoteConfigManager.Instance.FetchConfigs();
			Debug.Log(Social.localUser.id);
			onComplete?.Invoke();

		}
		catch (AuthenticationException ex)
		{
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			Debug.LogException(ex);
		}
		ShowLoadingRotate(false);
	}

	private async Task LinkWithGooglePlayGamesAsync(string authCode)
	{
		try
		{
			await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
		}
		catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
		{
			Debug.LogError("이미 연결된 계정입니다.");
		}
		catch (AuthenticationException ex)
		{
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			Debug.LogException(ex);
		}
	}
}
