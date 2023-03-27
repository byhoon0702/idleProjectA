using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UICoreInfoPopup : MonoBehaviour, IUIClosable
{
	[Header("코어등급")]
	[SerializeField] private TextMeshProUGUI coreText;


	[Header("캐릭강화")]
	[SerializeField] private TextMeshProUGUI userAttackPowerText;
	[SerializeField] private TextMeshProUGUI userHPText;


	[Header("하이퍼모드 강화")]
	[SerializeField] private TextMeshProUGUI hyperAttackPowerText;
	[SerializeField] private TextMeshProUGUI hyperAttackSpeedText;
	[SerializeField] private TextMeshProUGUI hyperMoveSpeedText;
	[SerializeField] private TextMeshProUGUI hyperCriticalAttackPowerText;


	[Header("개조조건")]
	[SerializeField] private Image[] abilityIconList;
	[SerializeField] private TextMeshProUGUI[] abilityLevelTextList;


	[Header("Button")]
	[SerializeField] private Button upgradeButton;
	[SerializeField] private Button closeButton;

	private UIManagementCore owner;


	private void Awake()
	{
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
	}

	public void OnUpdate(UIManagementCore _owner)
	{
		owner = _owner;

		var sheet = DataManager.Get<UserGradeDataSheet>();
		Grade nextGrade = sheet.NextGrade(UserInfo.info.UserGrade.grade);

		UserGradeData currentInfo = UserInfo.info.UserGrade;
		UserGradeData nextInfo = sheet.Get(nextGrade);


		// 코어등급
		coreText.text = $"{currentInfo.grade} -> {nextInfo.grade}";


		// 캐릭강화
		userAttackPowerText.text = $"{currentInfo.attackPowerRatio} -> {nextInfo.attackPowerRatio}";
		userHPText.text = $"{currentInfo.hpUpRatio} -> {nextInfo.hpUpRatio}";

		// 하이퍼모드 강화
		hyperAttackPowerText.text = $"{currentInfo.hyperAttackPower} -> {nextInfo.hyperAttackPower}";
		hyperAttackSpeedText.text = $"{currentInfo.hyperAttackSpeed} -> {nextInfo.hyperAttackSpeed}";
		hyperMoveSpeedText.text = $"{currentInfo.hyperMoveSpeed} -> {nextInfo.hyperMoveSpeed}";
		hyperCriticalAttackPowerText.text = $"{currentInfo.hyperCriticalAttackPower} -> {nextInfo.hyperCriticalAttackPower}";

		// 개조조건
		var coreItems = Inventory.it.FindItemsByType(ItemType.Core);
		for(int i=0 ; i<coreItems.Count ; i++)
		{
			abilityIconList[i].sprite = Resources.Load<Sprite>($"Icon/{coreItems[i].Icon}");
			abilityLevelTextList[i].text = $"Lv. {coreItems[i].Level}/{currentInfo.coreAbilityMaxLevel}";
		}

		upgradeButton.interactable = Upgradable();
	}

	public bool Upgradable()
	{
		var itemDataList = DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Core);

		foreach(var itemData in itemDataList)
		{
			ItemCore coreItem = Inventory.it.FindItemByTid(itemData.tid) as ItemCore;
			if(coreItem == null)
			{
				return false;
			}

			if(coreItem.Level < UserInfo.info.UserGrade.coreAbilityMaxLevel)
			{
				return false;
			}
		}

		return true;
	}

	public void OnUpgradeButtonClick()
	{
		if(Upgradable() == false)
		{
			return;
		}

		UserInfo.info.UserGradeUp();
		UserInfo.SaveUserData();
		ToastUI.it.Create("진급 성공!");
		owner.OnUpdate(true);
		Close();
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
