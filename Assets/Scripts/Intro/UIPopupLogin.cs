using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using TMPro;
using System.Text.RegularExpressions;

public class UIPopupLogin : MonoBehaviour
{
	[SerializeField] GameObject objDevLogin;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textDescription;
	[SerializeField] private TMP_InputField inputField;

	[SerializeField] private Button buttonLoginDev;
	[SerializeField] private Button buttonLoginGoogle;
	[SerializeField] private Button buttonLoginGuest;

	System.Action _onClose;

	bool isEditor
	{
		get
		{
#if UNITY_EDITOR
			return true;
#else
			return false;
#endif
		}
	}

	private void Awake()
	{
		//buttonLoginDev.onClick.RemoveAllListeners();
		//buttonLoginDev.onClick.AddListener(OnClickDev);
		//buttonLoginGoogle.onClick.RemoveAllListeners();
		//buttonLoginGoogle.onClick.AddListener(OnClickLoginGoogle);
		//buttonLoginGuest.onClick.RemoveAllListeners();
		//buttonLoginGuest.onClick.AddListener(OnClickLoginGuest);
	}
	public void Open(System.Action onClose = null)
	{
		_onClose = onClose;
		gameObject.SetActive(true);
		textDescription.text = "특수 문자 제외 12자 까지 가능합니다.";
		//objDevLogin.SetActive(isEditor);
		//buttonLoginGoogle.gameObject.SetActive(!isEditor);
		//buttonLoginGuest.gameObject.SetActive(false);
	}

	public void OnClickOk()
	{
		string nickName = inputField.text;

		if (nickName.NickNameValidate(out string message) == false)
		{
			textDescription.text = message;
			return;
		}

		PlatformManager.Firebase.Log("fb_nick_completed");
		PlatformManager.UserDB.userInfoContainer.NameChange(nickName);
		AuthenticationService.Instance.UpdatePlayerNameAsync(nickName);
		PlatformManager.UserDB.Save();
		gameObject.SetActive(false);
		_onClose?.Invoke();

	}

	//public void OnClickLoginGoogle()
	//{
	//	PlatformManager.Instance.OnClickGoogleLogin(() =>
	//	{
	//		string nickName = GooglePlayGames.PlayGamesPlatform.Instance.localUser.userName;

	//	});
	//	//플랫폼 인증 아이디 사용할것 

	//	gameObject.SetActive(false);
	//}

	//public void OnClickLoginGuest()
	//{
	//	PlatformManager.Instance.OnClickAnonymous(() =>
	//	{
	//	});

	//	gameObject.SetActive(false);
	//}

	//public void OnClickDev()
	//{
	//	PlatformManager.Instance.OnClickAnonymous(() =>
	//	{
	//	});
	//	gameObject.SetActive(false);
	//}
}
