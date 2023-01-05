using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	private static UIController instance;
	public static UIController it => instance;

	[SerializeField] Button buttonPlayBoss;

	private void Start()
	{
		buttonPlayBoss.onClick.RemoveAllListeners();
		buttonPlayBoss.onClick.AddListener(OnClickPlayBoss);
	}

	private void Awake()
	{
		instance = this;
	}

	private void OnClickPlayBoss()
	{
		StageManager.it.PlayBossStage();
	}

	// 현재 플레이중인 스테이지에 맞춰서 하단메뉴, 우측메뉴, dps 현황표 등 활성화 메뉴 결정.
	public void RefreshUI()
	{
		switch (StageManager.it.CurrentStageType)
		{
			case StageManager.StageType.NORMAL1:
				break;
			case StageManager.StageType.NORMAL2:
				break;
			case StageManager.StageType.BOSS:
				break;
		}
	}

	public void ShowDefeatNavigator()
	{

	}
}
