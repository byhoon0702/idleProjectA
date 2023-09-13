using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public enum IntroState_e
{
	None,
	INTRO,
	DATALOADING,
	LOGIN,
	ENTERGAME
}

public class Intro : MonoBehaviour
{
	public static Intro it { get; private set; }

	public VideoPlayer videoPlayer;


	public UIPopupLogin uiPopupLogin;
	public UIPopupAgreement uiPopupAgreement;



	public IntroState introState { get; private set; }
	public DataLoadingState dataLoadingState { get; private set; }
	public LoginState loginState { get; private set; }
	public EnterGameState enterGameState { get; private set; }

	public IntroState_e currentState { get; private set; }
	public IntroFSM currentFSM;

	public LanguageContainer language;
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI progressText;
	[SerializeField] private Slider progress;
	[SerializeField] private TextMeshProUGUI touchToStartText;
	[SerializeField] private GameObject debugObject;
	[SerializeField] private TextMeshProUGUI debugText;


	private int touchToStartAlphaToggle = -1;
	private void Awake()
	{
		it = this;

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClickStartGame);

		debugObject.SetActive(UnityEnv.HouseMode);
		SetActiveTabToStart(false);
	}

	private void Start()
	{
		introState = new IntroState();
		dataLoadingState = new DataLoadingState();
		loginState = new LoginState();
		enterGameState = new EnterGameState();

		ChangeState(IntroState_e.INTRO);
		GameSetting.Instance.LoadSettings();

		SoundManager.Instance.Init();
		SoundManager.Instance.PlayBgm(Resources.Load<AudioClip>("BgmSound/bgm_intro_00"));
	}
	public void ShowTermsAgreementPopup()
	{
		uiPopupAgreement.Open(() => { ChangeState(IntroState_e.LOGIN); });
	}
	public void OnClickVideo()
	{
		ChangeState(IntroState_e.DATALOADING);
	}

	public void ChangeState(IntroState_e state)
	{
		if (currentState == state)
		{
			return;
		}

		currentFSM?.OnExit();
		switch (state)
		{
			case IntroState_e.INTRO:
				currentFSM = introState;
				break;

			case IntroState_e.DATALOADING:
				currentFSM = dataLoadingState;
				break;

			case IntroState_e.LOGIN:
				currentFSM = loginState;
				break;

			case IntroState_e.ENTERGAME:
				currentFSM = enterGameState;
				break;
		}
		currentFSM?.OnEnter();
		currentState = state;
	}



	private void Update()
	{
		currentFSM?.OnUpdate(Time.deltaTime);
		//if (next != null)
		//{
		//	currentFSM = next;
		//}
		if (UnityEnv.HouseMode)
		{
			string text = "";

			text += $"State: {currentState}\n";
			//text += $"uid: {UserInfo.UserName}({UserInfo.UID})";

			debugText.text = text;
		}

		float alpha = touchToStartText.alpha;
		if (touchToStartAlphaToggle == -1)
		{
			alpha -= Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			touchToStartText.alpha = alpha;

			if (alpha <= 0)
			{
				touchToStartAlphaToggle = 1;
			}
		}
		else
		{
			alpha += Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			touchToStartText.alpha = alpha;

			if (alpha >= 1)
			{
				touchToStartAlphaToggle = -1;
			}
		}
	}

	private void OnClickStartGame()
	{
		//if (PlatformManager.Instance.IsSignedIn() == false)
		//{
		//	return;
		//}
		//if (currentState == IntroState_e.LOGIN)
		//{
		//	ChangeState(IntroState_e.ENTERGAME);
		//}
	}

	public void SetProgressBar(float _ratio, string _text)
	{
		progress.value = _ratio;
		progressText.text = _text;
	}


	public void SetActiveProgressBar(bool _isActive)
	{
		progress.gameObject.SetActive(_isActive);
	}

	public void SetActiveTabToStart(bool _isActive)
	{
		touchToStartText.gameObject.SetActive(_isActive);
	}
}
