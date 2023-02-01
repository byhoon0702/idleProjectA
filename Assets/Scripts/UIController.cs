using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	private static UIController instance;
	public static UIController it => instance;

	[SerializeField] private TextMeshProUGUI textStageTitle;

	[SerializeField] private Button buttonPlayBoss;

	[Header("-------------------------")]
	[SerializeField] private UILeftMenu uiLeftMenu;
	[SerializeField] private UITopMoney uiTopMoney;

	private void Start()
	{
		buttonPlayBoss.onClick.RemoveAllListeners();
		buttonPlayBoss.onClick.AddListener(OnClickPlayBoss);
	}

	private void Awake()
	{
		instance = this;
	}

	public void Init()
	{
		uiLeftMenu.Init();
		uiTopMoney.Init();
	}

	private void OnClickPlayBoss()
	{
		SpawnManager.it.KillAllEnemy();
		SpawnManager.it.SpawnBoss();
		buttonPlayBoss.gameObject.SetActive(false);
	}

	// 현재 플레이중인 스테이지에 맞춰서 하단메뉴, 우측메뉴, dps 현황표 등 활성화 메뉴 결정.
	public void RefreshUI()
	{
		var stageInfo = StageManager.it.CurrentStageInfo;

		textStageTitle.text = $"{stageInfo.areaName}\nACT {stageInfo.act}-{stageInfo.stage}";

		switch (StageManager.it.CurrentStageType)
		{
			case StageType.NORMAL:
				{
					bool isBossAlive = SpawnManager.it.IsBossDead == false;
					bool isInfiniteSpawn = StageManager.it.isCurrentStageLimited == false;

					bool canChallengeToBoss = isBossAlive == false && isInfiniteSpawn == true;

					buttonPlayBoss.gameObject.SetActive(canChallengeToBoss);
				}
				break;
		}
	}

	public void ShowDefeatNavigator()
	{
		// 패배시 강화컨텐츠 네비게이션
	}
}
