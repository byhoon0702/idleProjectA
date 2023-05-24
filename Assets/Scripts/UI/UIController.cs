using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
	private static UIController instance;
	public static UIController it
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<UIController>();

			}
			return instance;
		}
	}

	[SerializeField] private UIStageInfo uiStageInfo;

	[Header("Bottoms")]
	[SerializeField] private UIManagement management;
	[SerializeField] private UIManagementEquip equipment;
	[SerializeField] private UIManagementSkill skill;
	[SerializeField] private UIManagementJuvenescence juvenescence;
	//[SerializeField] private UIManagementPet pet;
	[SerializeField] private UIDungeonList dungeonList;


	[Header("-------------------------")]
	[SerializeField] private UITraining training;
	[SerializeField] private UITopMoney topMoney;
	[SerializeField] private HyperSkillUi hyperSkill;
	[SerializeField] private SkillGlobalUi skillGlobal;
	[SerializeField] private UICoinEffectPool coinEffectPool;
	[SerializeField] private UIAdRewardChest adRewardChest;
	[SerializeField] private UIRewardLog uiRewardLog;

	[SerializeField] private UIBottomMenu bottomMenu;


	public UIBottomMenu BottomMenu => bottomMenu;
	public UIManagement Management => management;
	public UIManagementEquip Equipment => equipment;
	//public UIManagementSkill Skill => skill;
	//	public UIManagementPet Pet => pet;
	public UIDungeonList DungeonList => dungeonList;


	public UIStageInfo UiStageInfo => uiStageInfo;
	public HyperSkillUi HyperSkill => hyperSkill;
	public SkillGlobalUi SkillGlobal => skillGlobal;
	public UIDungeonList UIDungeonList => dungeonList;

	private bool isCoinEffectActivated = true;



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
	public void ShowAdRewardChest(bool isShow)
	{

		adRewardChest.Switch(isShow);

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

		var coinEffect = coinEffectPool.Get("", "default");
		coinEffect.Run(pos);
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

	public void ToggleEquipment(EquipTabType type = EquipTabType.WEAPON, long tid = 0)
	{
		InactiveAllMainUI();
		if (equipment.gameObject.activeInHierarchy)
		{
			return;
		}

		equipment.gameObject.SetActive(true);
		equipment.OnUpdate(type, tid);
	}

	public void ToggleJuvenescence()
	{
		InactiveAllMainUI();
		if (juvenescence.gameObject.activeInHierarchy)
		{
			return;
		}

		juvenescence.gameObject.SetActive(true);
		juvenescence.SetPage(JuvenescencePage.Juvenescence);
	}

	public void ToggleSkill()
	{
		InactiveAllMainUI();
		if (skill.gameObject.activeInHierarchy)
		{
			return;
		}

		skill.gameObject.SetActive(true);
		skill.OnUpdate(0);
	}
	public void TogglePet()
	{
		//InactiveAllMainUI();
		//if (pet.gameObject.activeInHierarchy)
		//{
		//	return;
		//}

		//pet.gameObject.SetActive(true);
		//pet.OnUpdate(false);
	}

	public void ToggleShop()
	{
		//InactiveAllMainUI();
		//if (dungeonList.gameObject.activeInHierarchy)
		//{
		//	return;
		//}

		//dungeonList.gameObject.SetActive(true);
		//dungeonList.OnUpdate();
	}
	public void ShowDungeonList()
	{
		InactiveAllMainUI();
		if (dungeonList.gameObject.activeInHierarchy)
		{
			return;
		}

		dungeonList.gameObject.SetActive(true);
		dungeonList.OnUpdate();
	}

	public void ToggleMemory()
	{

	}

	public void ToggleGacha()
	{

	}

	public void InactiveAllMainUI()
	{
		management.gameObject.SetActive(false);
		equipment.gameObject.SetActive(false);

		skill.gameObject.SetActive(false);
		juvenescence.gameObject.SetActive(false);
		dungeonList.gameObject.SetActive(false);
	}

	public void InactivateAllBottomToggle()
	{
		bottomMenu.InactivateAllToggle();
	}

	public void ShowMap()
	{

	}

	public void ShowBuffPage()
	{

	}
}
