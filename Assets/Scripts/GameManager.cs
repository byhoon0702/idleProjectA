using System.Reflection;
using UnityEngine;
public interface FiniteStateMachine
{
	void OnEnter();
	void OnUpdate(float time);
	void OnExit();

}

public enum GameState
{
	None = 0,
	LOADING = 1,
	BGLOADING = 2,
	PLAYERSPAWN = 3,
	ANIMATIONSTATE = 4,
	FEVER = 5,
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
	public static GameManager it => instance;
	public float gameSpeed = 1;

	public MapController mapController;

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
	public SkillMeta skillDictionary;
	public ConfigMeta config;
	public BattleRecord battleRecord;

	private void Awake()
	{
		instance = this;
		LoadConfig();
		LoadSkillDictionary();
	}

	// Start is called before the first frame update
	void Start()
	{
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

	private void LoadConfig()
	{
		instance.config = ScriptableObject.CreateInstance<ConfigMeta>();
		TextAsset textAsset = Resources.Load<TextAsset>($"Json/{ConfigMeta.fileName.Replace(".json", "")}");

		if (textAsset == null)
		{
			VLog.LogError("Config 로드 실패");
			return;
		}

		JsonUtility.FromJsonOverwrite(textAsset.text, instance.config);
	}

	private void LoadSkillDictionary()
	{
		skillDictionary = ScriptableObject.CreateInstance<SkillMeta>();
		skillDictionary.CreateDictionary();

		TextAsset textAsset = Resources.Load<TextAsset>($"Json/{SkillMeta.fileName.Replace(".json", "")}");

		if (textAsset == null)
		{
			VLog.LogError("Skill 데이터 로드 실패");
			return;
		}

		JsonUtility.FromJsonOverwrite(textAsset.text, skillDictionary);
	}
}
