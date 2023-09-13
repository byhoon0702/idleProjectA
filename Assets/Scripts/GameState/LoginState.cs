using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class LoginState : RootState
{
	public override void Init()
	{

	}
	public override IntroFSM RunNextState(float time)
	{
		return this;
	}

	public override async Task<IntroFSM> OnEnter()
	{
		elapsedTime = 0;

		PlatformManager.Instance.ShowLoadingRotate(true);
		Intro.it.SetActiveTabToStart(true);
		AuthenticationService.Instance.SignedIn += OnSignedIn;
		AuthenticationService.Instance.SignInFailed += OnSignInFailed;
		AuthenticationService.Instance.SignedOut += OnSignedOut;
		AuthenticationService.Instance.Expired += OnExpired;

		//토큰이 존재하면 기록이 있다고 판단하여 진행
		if (AuthenticationService.Instance.SessionTokenExists == false)
		{
			PlatformManager.Instance.ShowLoadingRotate(false);
			OnClickLogin();
			return this;
		}
		else
		{

#if UNITY_EDITOR

			OnClickLoginGuest();
#else
			OnClickLoginGoogle();
#endif

		}
		return this;
	}

	private async void OnSignedIn()
	{
		if (this == null)
		{
			return;
		}
		PlatformManager.Instance.ShowLoadingRotate(true);
		bool result = await PlatformManager.UserDB.LoadLoginData();
		if (this == null)
		{
			return;
		}

		await PurchaseManager.Instance.LoadHistory();
		if (this == null)
		{
			return;
		}
		PlatformManager.Instance.ShowLoadingRotate(false);

		await RemoteConfigManager.Instance.FetchConfigs();

		if (RemoteConfigManager.Instance.NeedUpdate())
		{
			return;
		}

		System.Action onClose = () =>
		{
			if (PlatformManager.UserDB.userInfoContainer.userInfo.UserName.IsNullOrEmpty() == false)
			{
				Intro.it.ChangeState(IntroState_e.ENTERGAME);
			}
			else
			{
				Intro.it.uiPopupLogin.Open(() => Intro.it.ChangeState(IntroState_e.ENTERGAME));
			}
		};

		if (RemoteConfigManager.Instance.IsNoticeExist(onClose))
		{
			return;
		}

		onClose.Invoke();
	}

	private void OnExpired()
	{
		PlatformManager.Instance.ShowLoadingRotate(false);
		PopAlert.Create("오류", "토큰이 만료되었습니다.\n재시도 하시겠습니까?", OnClickLogin);
	}

	private void OnSignedOut()
	{
		PlatformManager.Instance.ShowLoadingRotate(false);
		PlatformManager.UserDB.LogOut();
	}

	private void OnSignInFailed(RequestFailedException obj)
	{
		PlatformManager.Instance.ShowLoadingRotate(false);
		PopAlert.Create("오류", "로그인 실패\n재시도 하시겠습니까?", OnClickLogin);
		//Intro.it.uiPopupLogin.OnUpdate();

	}

	public override void OnExit()
	{
		AuthenticationService.Instance.SignedIn -= OnSignedIn;
		AuthenticationService.Instance.SignInFailed -= OnSignInFailed;
		AuthenticationService.Instance.SignedOut -= OnSignedOut;
		AuthenticationService.Instance.Expired -= OnExpired;
	}

	public void OnClickLogin()
	{
#if UNITY_EDITOR
		PlatformManager.Instance.ShowLoadingRotate(false);
		OnClickLoginGuest();

#else
		OnClickLoginGoogle();
#endif

	}
	public void OnClickLoginGoogle()
	{
		PlatformManager.Instance.LogOut();
		PlatformManager.Instance.OnClickGoogleLogin();
		//플랫폼 인증 아이디 사용할것 
	}

	public void OnClickLoginGuest()
	{
		PlatformManager.Instance.OnClickAnonymous();
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
	}
}
