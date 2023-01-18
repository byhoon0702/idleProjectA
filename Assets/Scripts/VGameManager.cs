
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public interface FiniteStateMachine
{
	void OnEnter();
	void OnUpdate(float time);
	void OnExit();

}

public enum GameState
{
	None = 0,
	INTRO = 1,
	DATALOADING = 2,
	LOADING = 10,
	BGLOADING = 20,
	PLAYERSPAWN = 30,
	ANIMATIONSTATE = 40,
	FEVER = 50,
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

public class VGameManager : MonoBehaviour
{
	private static VGameManager instance;
	public static VGameManager it => instance;
	public float gameSpeed = 1;
	public bool fixedScroll;
	public MapController mapController;

	public IntroState introState;
	public DataLoadingState dataLoadingState;
	public LoadingState loadingState;
	public BGLoadState bgloadState;
	public SpawnState spawnState;
	public BattleStartState battleStartState;
	public BattleState battleState;
	public BattleEndState battleEndState;
	public AnimationState animationState;
	public FeverState feverState;

	public FiniteStateMachine currentFSM;
	public GameState currentState;
	public SkillMeta skillMeta;
	public ConfigMeta config;
	public BattleRecord battleRecord;

	private void Awake()
	{
		instance = this;
	}


	public void IdleNUll(IdleNumber aa)
	{

		if (aa <= 0)
		{
			Debug.Log("IdlenNumber Null");
		}

	}
	// Start is called before the first frame update
	void Start()
	{
		IdleNUll(null);
		introState = new IntroState();
		dataLoadingState = new DataLoadingState();
		animationState = new AnimationState();
		loadingState = new LoadingState();
		bgloadState = new BGLoadState();
		spawnState = new SpawnState();
		battleStartState = new BattleStartState();
		battleState = new BattleState();
		battleEndState = new BattleEndState();
		battleRecord = new BattleRecord();
		feverState = new FeverState();

		Application.targetFrameRate = 60;

		ChangeState(GameState.INTRO);
	}
	public void ChangeState(GameState state)
	{

		currentFSM?.OnExit();
		switch (state)
		{
			case GameState.INTRO:
				currentFSM = introState;
				break;
			case GameState.DATALOADING:
				currentFSM = dataLoadingState;
				break;
			case GameState.LOADING:
				currentFSM = loadingState;
				break;
			case GameState.BGLOADING:
				currentFSM = bgloadState;
				break;
			case GameState.PLAYERSPAWN:
				currentFSM = spawnState;
				break;
			case GameState.ANIMATIONSTATE:
				currentFSM = animationState;
				break;
			case GameState.FEVER:
				currentFSM = feverState;
				break;
			case GameState.BATTLESTART:
				currentFSM = battleStartState;
				break;
			case GameState.BATTLE:
				currentFSM = battleState;
				break;
			case GameState.BATTLEEND:
				currentFSM = battleEndState;
				break;
		}
		currentFSM?.OnEnter();
		currentState = state;
	}

	// Update is called once per frame
	void Update()
	{
		currentFSM?.OnUpdate(Time.deltaTime);
	}
	public void OnClickStartGame()
	{
		ChangeState(GameState.DATALOADING);
	}
}
