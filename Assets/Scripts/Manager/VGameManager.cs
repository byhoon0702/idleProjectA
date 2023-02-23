
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

public class VGameManager : MonoBehaviour
{
	private static VGameManager instance;
	public static VGameManager it => instance;
	public float gameSpeed = 1;
	public bool fixedScroll;
	public MapController mapController;

	public LoadingState loadingState;
	public BGLoadState bgloadState;
	public SpawnState spawnState;
	public BattleStartState battleStartState;
	public BattleState battleState;
	public BattleEndState battleEndState;

	public FiniteStateMachine currentFSM;
	public GameState currentState;


	public BattleRecord battleRecord;

	private void Awake()
	{
		instance = this;
	}


	// Start is called before the first frame update
	void Start()
	{
		GameSetting.it.LoadSettings();

		VSoundManager soundManager = new GameObject("Sound Manager").AddComponent<VSoundManager>();
		soundManager.transform.SetParent(transform);

		loadingState = new LoadingState();
		bgloadState = new BGLoadState();
		spawnState = new SpawnState();
		battleStartState = new BattleStartState();
		battleState = new BattleState();
		battleEndState = new BattleEndState();
		battleRecord = new BattleRecord();

		Application.targetFrameRate = 60;

		ChangeState(GameState.LOADING);
	}
	public void ChangeState(GameState state)
	{
		currentFSM?.OnExit();
		switch (state)
		{
			case GameState.LOADING:
				currentFSM = loadingState;
				break;
			case GameState.BGLOADING:
				currentFSM = bgloadState;
				break;
			case GameState.PLAYERSPAWN:
				currentFSM = spawnState;
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
}
