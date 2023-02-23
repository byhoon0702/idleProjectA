using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIController : MonoBehaviour
{
	private static UIController instance;
	public static UIController it => instance;

	[SerializeField] private TextMeshProUGUI textStageTitle;

	[SerializeField] private Button buttonPlayBoss;

	[Header("Bottoms")]
	[SerializeField] private UIManagement management;
	[SerializeField] private UIPet pet;
	[SerializeField] private UIGacha gacha;

	[Header("-------------------------")]
	[SerializeField] private UITraining training;
	[SerializeField] private UITopMoney topMoney;
	[SerializeField] private HyperSkillUi hyperSkill;
	[SerializeField] private SkillGlobalUi skillGlobal;
	[SerializeField] private UICoinEffectPool coinEffectPool;
	[SerializeField] private UIAdRewardChest adRewardChest;
	[SerializeField] private UIRewardLog uiRewardLog;
	[SerializeField] private UIGachaRewardPopup gachaRewardPopup;

	public HyperSkillUi HyperSkill => hyperSkill;
	public SkillGlobalUi SkillGlobal => skillGlobal;

	private bool isCoinEffectActivated = true;



	private void Start()
	{
		buttonPlayBoss.onClick.RemoveAllListeners();
		buttonPlayBoss.onClick.AddListener(OnClickPlayBoss);
	}

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			var closableList = GetComponentsInChildren<IUIClosable>();
			if (closableList.Length > 0)
			{
				if (closableList[closableList.Length - 1].Closable())
				{
					closableList[closableList.Length - 1].Close();
				}
			}
		}
	}

	public void Init()
	{
		training.OnUpdate(false);

		topMoney.Init();
		adRewardChest.Init();
	}

	private void OnClickPlayBoss()
	{
		if(SpawnManagerV2.it != null)
		{
			SpawnManagerV2.it.KillAllEnemy();
			SpawnManagerV2.it.SpawnBoss();
		}
		else
		{
			SpawnManager.it.KillAllEnemy();
			SpawnManager.it.SpawnBoss();
		}
		buttonPlayBoss.gameObject.SetActive(false);

		//var stageInfo = DataManager.Get<StageInfoDataSheet>().Get(4);
		//StageManager.it.PlayStage(stageInfo);
	}

	// 현재 플레이중인 스테이지에 맞춰서 하단메뉴, 우측메뉴, dps 현황표 등 활성화 메뉴 결정.
	public void RefreshUI()
	{
		var stageInfo = StageManager.it.CurrentNormalStageInfo;

		textStageTitle.text = $"{stageInfo.areaName}\nACT {stageInfo.act}-{stageInfo.stage}";

		switch (StageManager.it.CurrentStageType)
		{
			case StageType.NORMAL:
				{
					bool isBossAlive;
					if(SpawnManagerV2.it != null)
					{
						isBossAlive = SpawnManagerV2.it.IsBossDead == false;
					}
					else
					{
						 isBossAlive = SpawnManager.it.IsBossDead == false;
					}
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

	public void ShowCoinEffect(Transform _fromObject)
	{
		if (isCoinEffectActivated == false)
		{
			return;
		}

		Vector2 pos = GameUIManager.it.ToUIPosition(_fromObject.position);

		var coinEffect = coinEffectPool.Get();
		coinEffect.Run(pos);
	}

	public void ShowGachaRewardPopup(UIGachaData _uiData, List<GachaResult> _newItems)
	{
		gachaRewardPopup.gameObject.SetActive(true);
		gachaRewardPopup.OnUpdate(_uiData, _newItems);
	}

	public void ShowItemLog(int _tid, IdleNumber _count)
	{
		uiRewardLog.ShowLog(_tid, _count);
	}

	public void SetCoinEffectActivate(bool _isActive)
	{
		isCoinEffectActivated = _isActive;
	}

	public void ToggleManagement()
	{
		InactiveAllMainUI();
		if (management.gameObject.activeInHierarchy)
		{
			return;
		}

		management.gameObject.SetActive(true);
		management.OnUpdate();
	}

	public void TogglePet()
	{
		InactiveAllMainUI();
		if (pet.gameObject.activeInHierarchy)
		{
			return;
		}

		pet.gameObject.SetActive(true);
		pet.OnUpdate(false);
	}

	public void ToggleGacha()
	{
		InactiveAllMainUI();
		if(gacha.gameObject.activeInHierarchy)
		{
			return;
		}

		gacha.gameObject.SetActive(true);
		gacha.OnUpdate();
	}

	private void InactiveAllMainUI()
	{
		management.gameObject.SetActive(false);
		pet.gameObject.SetActive(false);
		gacha.gameObject.SetActive(false);
	}
}
