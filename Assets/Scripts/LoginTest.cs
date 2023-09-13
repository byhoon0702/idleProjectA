using UnityEngine;
using GooglePlayGames;

using GooglePlayGames.BasicApi;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;

using System.Text;
using System.Threading.Tasks;
using TMPro;

public class LoginTest : MonoBehaviour
{
	public TextMeshProUGUI info;
	public TextMeshProUGUI result;

	private string playerId;
	private string userName;
	private string created;


	private void Awake()
	{
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
	}

	async void Start()
	{
		InitializationOptions options = new InitializationOptions();
#if !UNITY_EDITOR
		options.SetEnvironmentName("editor");
#else
		options.SetEnvironmentName("production");
#endif
		await UnityServices.InitializeAsync(options);

		SetupEvents();
	}


	private void UpdateUI()
	{
		info.text = $"{Application.cloudProjectId}.{AuthenticationService.Instance.Profile}\n{playerId}\n{userName}\n{created}";
		string sessionToken = PlayerPrefs.GetString($"{Application.cloudProjectId}.{AuthenticationService.Instance.Profile}.unity.services.authentication.session_token");
		Debug.Log(sessionToken);
	}

	private void SetupEvents()
	{
		AuthenticationService.Instance.SignedIn += OnSignedIn;
		AuthenticationService.Instance.SignInFailed += OnSignInFailed;
		AuthenticationService.Instance.SignedOut += OnSignedOut;
		AuthenticationService.Instance.Expired += OnExpired;
	}

	public void OnClickClearSession()
	{
		AuthenticationService.Instance.ClearSessionToken();
	}
	private async Task SignInGoogleAsync(string authCode)
	{
		try
		{
			await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
		}

		catch (AuthenticationException ex)
		{
			result.text = ex.Message;
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			result.text = ex.Message;
			Debug.LogException(ex);
		}
	}

	private async Task SignInAnonymouslyAsync(System.Action onComplete = null)
	{
		try
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();


		}
		catch (AuthenticationException ex)
		{
			result.text = ex.Message;
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			result.text = ex.Message;
			Debug.LogException(ex);
		}
	}

	public void OnClickSignOut()
	{
		AuthenticationService.Instance.SignOut();
	}
	public void OnClickLogin()
	{
#if UNITY_EDITOR
		OnClickAnonymous();
#else
		OnClickGoogleLogin();
#endif
	}

	public async void OnClickGetPlayerInfo()
	{
		await GetPlayerInfoAsync();
	}
	async Task GetPlayerInfoAsync()
	{
		var m_PlayerInfo = await AuthenticationService.Instance.GetPlayerInfoAsync();
		var m_ExternalIds = GetExternalIds(m_PlayerInfo);
		created = m_PlayerInfo.CreatedAt.ToString();
		playerId = m_PlayerInfo.Id;
		userName = m_PlayerInfo.Username;
		Debug.Log(m_ExternalIds);
		UpdateUI();
	}

	string GetExternalIds(PlayerInfo playerInfo)
	{
		var sb = new StringBuilder();
		if (playerInfo.Identities != null)
		{
			foreach (var id in playerInfo.Identities)
				sb.Append(id.TypeId + " ");

			return sb.ToString();
		}

		return "None";
	}

	public void OnClickGoogleLogin(System.Action onComplete = null)
	{
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
	}
	public async void OnClickAnonymous(System.Action onComplete = null)
	{
		await SignInAnonymouslyAsync(onComplete);

	}

	private void OnSignedIn()
	{
		playerId = AuthenticationService.Instance.PlayerId;

		//Shows how to get a playerID
		Debug.Log($"PlayedID: {AuthenticationService.Instance.PlayerId}");

		//Shows how to get an access token
		Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
		UpdateUI();
	}

	private void OnExpired()
	{
		result.text = "Expired";
		UpdateUI();
	}

	private void OnSignedOut()
	{
		playerId = "";
		userName = "";
		created = "";
		UpdateUI();
	}

	private void OnSignInFailed(RequestFailedException obj)
	{
		result.text = obj.Message;
		UpdateUI();
	}

}
