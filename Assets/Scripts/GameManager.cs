using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface FiniteStateMachine
{
    void OnEnter();
    void OnUpdate(float time);
    void OnExit();

}

public enum GameState
{
    None,
    LOADING,
    BGLOADING,
    PLAYERSPAWN,
    BATTLESTART,
    BATTLE,
    BATTLEEND,
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

    public LoadingState loadingState;
    public BGLoadState bgloadState;
    public SpawnState spawnState;
    public BattleStartState battleStartState;
    public BattleState battleState;
    public BattleEndState battleEndState;

    public FiniteStateMachine currentState;

    private void Awake()
	{
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
        
	}
    public void ChangeState(GameState state)
    {
        currentState?.OnExit();
        switch (state)
        {
            case GameState.LOADING:
                currentState = loadingState;
                break;
            case GameState.BGLOADING:
                currentState = bgloadState;
                break;
            case GameState.PLAYERSPAWN:
                currentState = spawnState;
                break;
            case GameState.BATTLESTART:
                currentState = battleStartState;
                break;
            case GameState.BATTLE:
                currentState = battleState;
                break;
            case GameState.BATTLEEND:
                currentState = battleEndState;
                break;
        }
        currentState?.OnEnter();
    }

	// Update is called once per frame
	void Update()
	{
        currentState?.OnUpdate(Time.deltaTime);

    }
}
