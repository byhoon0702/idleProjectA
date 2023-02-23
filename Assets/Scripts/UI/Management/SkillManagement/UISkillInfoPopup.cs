using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UISkillInfoPopup : MonoBehaviour, IUIClosable
{
	[Header("Title")]
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI skillNameText;
	[SerializeField] private TextMeshProUGUI skillLevelText;
	[SerializeField] private TextMeshProUGUI skillGradeText;

	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;

	[Header("Desc")]
	[SerializeField] private TextMeshProUGUI descText;
	[SerializeField] private TextMeshProUGUI cooltimeText;
	[SerializeField] private TextMeshProUGUI toOwnText;

	[Header("Button")]
	[SerializeField] private Button changeButton;
	[SerializeField] private TextMeshProUGUI changeButtonText;
	[SerializeField] private Button upgradeButton;
	[SerializeField] private Button closeButton;

	private UIManagementSkill owner;
	public UISkillData UIData { get; private set; }


	private void Awake()
	{
		changeButton.onClick.RemoveAllListeners();
		changeButton.onClick.AddListener(OnChangeButtonClick);
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
	}

	public void OnUpdate(UIManagementSkill _owner, UISkillData _uiData)
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
		skillNameText.text = UIData.SkillName;
		skillGradeText.text = UIData.SkillGradeText;
	}

	private void UpdateLevelInfo()
	{
		skillLevelText.text = UIData.SkillLevelText;
		expSlider.value = UIData.ExpRatio;
		textExp.text = UIData.ExpText;
	}

	private void UpdateDesc()
	{
		descText.text = UIData.SkillDesc;
		cooltimeText.text = UIData.SkillCooltimeText;
		toOwnText.text = UIData.ToOwnAbilityText;
	}

	private void UpdateButton()
	{
		bool equipped = UserInfo.IsEquipSkill(UIData.ItemTid);
		if (equipped)
		{
			changeButtonText.text = "해제";
		}
		else
		{
			changeButtonText.text = "장착";
		}

		ItemSkill item = UIData.GetItem();
		bool hasItem = item != null && item.Count > 0;
		bool upgradable = UIData.Upgradable();

		changeButton.interactable = hasItem;
		upgradeButton.interactable = hasItem && upgradable;
	}

	public void OnChangeButtonClick()
	{
		bool equipped = UserInfo.IsEquipSkill(UIData.ItemTid);

		if(equipped)
		{
			owner.UnEquipSkill(UIData.ItemTid);
		}
		else
		{
			owner.EquipSkill(UIData.ItemTid);
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

		var uiSkill = FindObjectOfType<UIManagementSkill>();
		if(uiSkill != null)
		{
			uiSkill.OnUpdate(true);
		}
	}
}
