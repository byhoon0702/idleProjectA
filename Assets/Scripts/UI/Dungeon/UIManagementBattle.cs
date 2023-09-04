using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleTab
{
	DUNGEON,
	TOWER,
	GUARDIAN,
}


public class UIManagementBattle : UIBase
{
	[Space(10)]
	[SerializeField] private Button closeButton;
	[SerializeField] private UIContentToggle dungeonButton;
	public UIContentToggle DungeonButton => dungeonButton;

	[SerializeField] private UIContentToggle towerButton;
	public UIContentToggle TowerButton => towerButton;

	[SerializeField] private UIContentToggle guardianButton;
	public UIContentToggle GuardianButton => guardianButton;

	[SerializeField] private UIItemDungeonList itemPrefab;

	[Space(10)]
	[SerializeField] private UIPageBattleDungeon uiPageBattleDungeon;
	public UIPageBattleDungeon UiPageBattleDungeon => uiPageBattleDungeon;
	[SerializeField] private UIPageBattleGuardian uiPageBattleGuardian;
	public UIPageBattleGuardian UiPageBattleGuardian => uiPageBattleGuardian;
	[SerializeField] private UIPageBattleTower uiPageBattleTower;
	public UIPageBattleTower UiPageBattleTower => uiPageBattleTower;


	[SerializeField] private UIDungeonStagePopup uiDungeonStagePopup;
	public UIDungeonStagePopup UiDungeonStagePopup => uiDungeonStagePopup;
	[SerializeField] private UIDungeonPopup uiDungeonPopup;
	public UIDungeonPopup UiDungeonPopup => uiDungeonPopup;

	protected override void OnEnable()
	{
		AddCloseListener();
		uiDungeonStagePopup.Close();
		uiDungeonPopup.Close();
	}
	protected override void OnDisable()
	{
		RemoveCloseListener();
		uiDungeonStagePopup.Close();
		uiDungeonPopup.Close();
	}

	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		dungeonButton.onValueChanged.RemoveAllListeners();
		dungeonButton.onValueChanged.AddListener(isTrue =>
		{
			if (isTrue)
			{
				ChangeTab(BattleTab.DUNGEON);
			}

		});
		towerButton.onValueChanged.RemoveAllListeners();
		towerButton.onValueChanged.AddListener(isTrue =>
		{
			if (isTrue)
			{
				ChangeTab(BattleTab.TOWER);
			}
		});
		guardianButton.onValueChanged.RemoveAllListeners();
		guardianButton.onValueChanged.AddListener(isTrue =>
		{
			if (isTrue)
			{
				ChangeTab(BattleTab.GUARDIAN);
			}
		});
	}

	void ChangeTab(BattleTab type)
	{
		uiPageBattleDungeon.gameObject.SetActive(false);
		uiPageBattleTower.gameObject.SetActive(false);
		uiPageBattleGuardian.gameObject.SetActive(false);

		switch (type)
		{
			case BattleTab.DUNGEON:
				uiPageBattleDungeon.OnUpdate(this);
				uiPageBattleDungeon.gameObject.SetActive(true);
				break;
			case BattleTab.TOWER:
				uiPageBattleTower.OnUpdate(this);
				uiPageBattleTower.gameObject.SetActive(true);
				break;
			case BattleTab.GUARDIAN:
				uiPageBattleGuardian.OnUpdate(this);
				uiPageBattleGuardian.gameObject.SetActive(true);
				break;
		}
	}

	public void ShowDungeonPopup(BattleData data)
	{
		uiDungeonStagePopup.Close();
		uiDungeonPopup.Init(this, data);
	}
	public void ShowDungeonStagePopup(BattleData data)
	{
		uiDungeonStagePopup.Init(this, data);
		uiDungeonPopup.Close();
	}

	public void OnUpdate()
	{
		if (dungeonButton.isOn)
		{
			ChangeTab(BattleTab.DUNGEON);
		}
		else
		{
			dungeonButton.isOn = true;
		}
	}


	protected override void OnClose()
	{
		UIController.it.InactivateAllBottomToggle();

		base.OnClose();
	}
}


