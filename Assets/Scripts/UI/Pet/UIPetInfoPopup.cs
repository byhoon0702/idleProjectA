using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UIPetInfoPopup : MonoBehaviour, IUIClosable
{

	[Header("Title")]
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private TextMeshProUGUI gradeText;

	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;

	[Header("Desc")]
	[SerializeField] private TextMeshProUGUI descText;
	[SerializeField] private TextMeshProUGUI toOwnText;
	[SerializeField] private TextMeshProUGUI categoryText;

	[Header("Button")]
	[SerializeField] private Button changeButton;
	[SerializeField] private TextMeshProUGUI changeButtonText;
	[SerializeField] private Button upgradeButton;
	[SerializeField] private Button closeButton;

	private UIPet owner;
	public UIPetData UIData { get; private set; }


	private void Awake()
	{
		changeButton.onClick.RemoveAllListeners();
		changeButton.onClick.AddListener(OnChangeButtonClick);
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
	}

	public void OnUpdate(UIPet _owner, UIPetData _uiData)
	{
		owner = _owner;
		UIData = _uiData;


		UpdateTitle();
		UpdateLevelInfo();
		UpdateDesc();
		UpdateButton();
	}

	private void UpdateTitle()
	{
		icon.sprite = Resources.Load<Sprite>($"Icon/{UIData.Icon}");
		nameText.text = UIData.ItemName;
		gradeText.text = UIData.ItemGradeText;
	}

	private void UpdateLevelInfo()
	{
		levelText.text = UIData.LevelText;
		expSlider.value = UIData.ExpRatio;
		textExp.text = UIData.ExpText;
	}

	private void UpdateDesc()
	{
		descText.text = UIData.ItemDesc;
		categoryText.text = UIData.CategoryText;
		toOwnText.text = UIData.ToOwnText;
	}

	private void UpdateButton()
	{
		bool equipped = UserInfo.IsEquipPet(UIData.ItemTid);
		if (equipped)
		{
			changeButtonText.text = "해제";
		}
		else
		{
			changeButtonText.text = "장착";
		}

		ItemPet item = UIData.GetItem();
		bool hasItem = item != null && item.Count > 0;
		bool upgradable = UIData.Upgradable();

		changeButton.interactable = hasItem;
		upgradeButton.interactable = hasItem && upgradable;
	}

	public void OnChangeButtonClick()
	{
		bool equipped = UserInfo.IsEquipPet(UIData.ItemTid);

		if (equipped)
		{
			owner.UnEquipPet(UIData.ItemTid);
		}
		else
		{
			owner.EquipPet(UIData.ItemTid);
		}

		Close();
	}

	public void OnUpgradeButtonClick()
	{
		UIData.LevelupItem(() =>
		{
			UpdateLevelInfo();
			UpdateButton();
		});
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
