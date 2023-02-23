using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

	public IntroState introState;
	public DataLoadingState dataLoadingState;
	public LoginState loginState;
	public EnterGameState enterGameState;

	public IntroState_e currentState { get; private set; }
	public FiniteStateMachine currentFSM;

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
	}

	public void ChangeState(IntroState_e state)
	{
		if(currentState == state)
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
		if(UnityEnv.HouseMode)
		{
			string text = "";

			text += $"State: {currentState}\n";
			text += $"uid: {UserInfo.UserName}({UserInfo.UID})";

			debugText.text = text;
		}

		float alpha = touchToStartText.alpha;
		if (touchToStartAlphaToggle == -1)
		{
			alpha -= Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			touchToStartText.alpha = alpha;

			if(alpha <= 0)
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
		if (currentState == IntroState_e.LOGIN)
		{
			ChangeState(IntroState_e.ENTERGAME);
		}
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
