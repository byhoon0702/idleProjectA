using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAwakeningRuneLevelUpPopup : UIBase
{
	[SerializeField] private Button bg;
	[SerializeField] Image _iconRune;
	[SerializeField] UIEconomyButton _buttonLevelUp;
	public UIEconomyButton ButtonLevelUp => _buttonLevelUp;
	[SerializeField] Button _buttonMaxLevel;
	[SerializeField] TextMeshProUGUI textLevel;
	[SerializeField] TextMeshProUGUI textName;
	[SerializeField] TextMeshProUGUI textAbility;


	private UIManagementAwakening _parent;
	private RuntimeData.AwakeningLevelInfo _levelInfo;
	private RuntimeData.AwakeningInfo _info;

	private void Awake()
	{
		_buttonLevelUp.SetButtonEvent(OnClickLevelUP);

	}
	protected override void OnEnable()
	{
		base.OnEnable();
		bg.SetButtonEvent(Close);
	}

	private bool OnClickLevelUP()
	{
		var item = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.AWAKENING_STONE);
		if (item.Pay(_levelInfo.GetCost()) == false)
		{
			ToastUI.Instance.Enqueue("각성석이 부족합니다.");
			return false;
		}

		PlatformManager.UserDB.awakeningContainer.RuneLevelUp(_levelInfo);
		_parent.OnUpdate();
		Refresh();
		return true;
	}

	public void Show(UIManagementAwakening parent, RuntimeData.AwakeningInfo info, RuntimeData.AwakeningLevelInfo levelInfo, Sprite icon)
	{
		gameObject.SetActive(true);
		bg.gameObject.SetActive(true);
		OnUpdate(parent, info, levelInfo, icon);
	}

	private void OnUpdate(UIManagementAwakening parent, RuntimeData.AwakeningInfo info, RuntimeData.AwakeningLevelInfo levelInfo, Sprite icon)
	{
		_parent = parent;
		_info = info;
		_levelInfo = levelInfo;
		_iconRune.sprite = icon;

		Refresh();
	}

	public void Refresh()
	{
		bool isMaxLevel = _levelInfo.IsMax();

		if (!isMaxLevel)
		{
			textLevel.text = $"{_levelInfo.Level}/{_levelInfo.MaxLevel}";
		}
		else
		{
			textLevel.text = "MaxLevel";
		}
		var item = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.AWAKENING_STONE);

		_buttonLevelUp.SetButton(_info.CostIcon, $"{item.Value.ToString()}/{_levelInfo.GetCost().ToString()}", item.Value >= _levelInfo.GetCost());

		textName.text = PlatformManager.Language[_levelInfo.RawData.Name];
		IdleNumber currentValue = _levelInfo.Ability.Value;
		IdleNumber nextValue = _levelInfo.GetNextInfo();

		_buttonLevelUp.gameObject.SetActive(!isMaxLevel);
		_buttonMaxLevel.gameObject.SetActive(isMaxLevel);
		if (isMaxLevel)
		{
			textAbility.text = $"{_levelInfo.Ability.type.ToUIString()}\n{currentValue.ToString()}%";
		}
		else
		{
			textAbility.text = $"{_levelInfo.Ability.type.ToUIString()}\n{currentValue.ToString()}% -> {nextValue.ToString()}%";
		}

	}

	protected override void OnClose()
	{
		base.OnClose();
		bg.gameObject.SetActive(false);
	}
}
