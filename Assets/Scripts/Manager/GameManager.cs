
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

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


	[SerializeField] private LanguageContainer languageContainer;
	public static LanguageContainer Language
	{
		get
		{
			return it.languageContainer;
		}
	}
	[SerializeField] private UserDB userDB;
	public static UserDB UserDB
	{
		get
		{
			return it.userDB;
		}
	}
	public BattleRecord battleRecord;

	public static ConfigMeta Config
	{
		get
		{
			return it.userDB.configMeta;
		}
	}

	public static bool GameStop = false;
	public static float GlobalTimeScale = 1f;

	private void Awake()
	{
		instance = this;
	}


	// Start is called before the first frame update
	void Start()
	{
		GameStop = false;
		DataManager.LoadAllJson();
		GameSetting.it.LoadSettings();

		if (UserDB.userInfoContainer.userInfo == null)
		{
			UserDB.userInfoContainer.userInfo = new UserInfo();
			//UserDB.userInfoContainer.LoadFromFile();
		}

		VSoundManager soundManager = new GameObject("Sound Manager").AddComponent<VSoundManager>();
		soundManager.transform.SetParent(transform);

		battleRecord = new BattleRecord();

		Application.targetFrameRate = 60;

		userDB.Init();

		//currentFSM = loadingState.OnEnter();
	}

	private void OnDestroy()
	{
		userDB.Clear();
	}

	public void EndStage()
	{
		//currentFSM = battleEndState.OnEnter();
	}
	public void ChangeState(GameState state)
	{
		if (currentState == state)
		{
			VLog.LogWarning($"같은 State전환. {state}");
			return;
		}


		//currentFSM?.OnExit();
		//switch (state)
		//{
		//	case GameState.LOADING:
		//		currentFSM = loadingState;
		//		break;
		//	case GameState.BGLOADING:
		//		currentFSM = bgloadState;
		//		break;
		//	case GameState.PLAYERSPAWN:
		//		currentFSM = spawnState;
		//		break;
		//	case GameState.BATTLESTART:
		//		currentFSM = battleStartState;
		//		break;
		//	case GameState.BATTLE:
		//		currentFSM = battleState;
		//		break;
		//	case GameState.BATTLEEND:
		//		currentFSM = battleEndState;
		//		break;
		//}
		//currentFSM?.OnEnter();
		//currentState = state;
	}

	float time = 0f;
	private void FixedUpdate()
	{
		time += Time.fixedUnscaledDeltaTime;

		if (time > 1f)
		{
			UserDB.Save();
			time = 0;
		}

	}



}
