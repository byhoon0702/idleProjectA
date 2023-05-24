using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupLogin : MonoBehaviour
{
	[SerializeField] GameObject objDevLogin;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TMP_InputField inputField;

	[SerializeField] private Button buttonLoginDev;
	[SerializeField] private Button buttonLoginGoogle;
	[SerializeField] private Button buttonLoginGuest;


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
		buttonLoginDev.onClick.RemoveAllListeners();
		buttonLoginDev.onClick.AddListener(OnClickDev);
		buttonLoginGoogle.onClick.RemoveAllListeners();
		buttonLoginGoogle.onClick.AddListener(OnClickLoginGoogle);
		buttonLoginGuest.onClick.RemoveAllListeners();
		buttonLoginGuest.onClick.AddListener(OnClickLoginGuest);
	}
	public void OnUpdate()
	{
		gameObject.SetActive(true);
		objDevLogin.SetActive(isEditor);
		buttonLoginGoogle.gameObject.SetActive(!isEditor);
		buttonLoginGuest.gameObject.SetActive(!isEditor);
	}

	public void OnClickLoginGoogle()
	{
		//플랫폼 인증 아이디 사용할것 
		Intro.it.userDB.SetLoginInfo(System.Guid.NewGuid().ToString(), "GoogleUesr", "google");
		gameObject.SetActive(false);
	}

	public void OnClickLoginGuest()
	{
		//플랫폼 인증 아이디 사용할것 
		Intro.it.userDB.SetLoginInfo(System.Guid.NewGuid().ToString(), "GuestUser", "guest");
		gameObject.SetActive(false);
	}

	public void OnClickDev()
	{
		Intro.it.userDB.SetLoginInfo(System.Guid.NewGuid().ToString(), inputField.text, "dev");
		gameObject.SetActive(false);
	}
}
