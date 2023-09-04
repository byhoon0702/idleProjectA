using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAwakeningUpgradePopup : UIBase
{
	[SerializeField] private Button bg;
	[SerializeField] Button _buttonPrev;
	[SerializeField] Button _buttonNext;
	[SerializeField] Button _buttonUpgrade;

	[SerializeField] UIItemStats[] _itemStats;
	[SerializeField] TextMeshProUGUI _textMaxLevel;
	[SerializeField] TextMeshProUGUI _textCostumeName;
	[SerializeField] TextMeshProUGUI _textAwakeningLevel;
	[SerializeField] TextMeshProUGUI _textDescription1;
	[SerializeField] TextMeshProUGUI _textDescription2;

	[SerializeField] RawImage _imageCharacter;

	private UIManagementAwakening _parent;
	private RuntimeData.AwakeningInfo _info;
	GameObject character;
	private void Awake()
	{

		_buttonNext.SetButtonEvent(OnClickNext);
		_buttonPrev.SetButtonEvent(OnClickPrev);
		_buttonUpgrade.SetButtonEvent(OnClickUpgrade);
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		bg.SetButtonEvent(Close);
	}

	private void OnClickNext()
	{
		if (currentIndex >= maxIndex)
		{
			return;
		}
		currentIndex++;
		_info = PlatformManager.UserDB.awakeningContainer.InfoList[currentIndex];
		OnUpdateInfo();
	}
	private void OnClickPrev()
	{
		if (currentIndex <= 0)
		{
			return;
		}
		currentIndex--;
		_info = PlatformManager.UserDB.awakeningContainer.InfoList[currentIndex];
		OnUpdateInfo();

	}

	private void OnClickUpgrade()
	{
		if (PlatformManager.UserDB.awakeningContainer.Awaken(_info))
		{
			_parent.OnUpdate();
		}
	}

	int currentIndex = 0;
	int maxIndex = 0;
	public void OnUpdate(UIManagementAwakening parent, RuntimeData.AwakeningInfo info, bool isUpgradable = false)
	{
		_parent = parent;
		_info = info;

		currentIndex = PlatformManager.UserDB.awakeningContainer.InfoList.FindIndex(x => x.Tid == _info.Tid);
		maxIndex = PlatformManager.UserDB.awakeningContainer.InfoList.Count - 1;
		gameObject.SetActive(true);
		bg.gameObject.SetActive(true);
		OnUpdateInfo();

		_buttonUpgrade.gameObject.SetActive(isUpgradable);
	}

	public void OnUpdateInfo()
	{
		if (character != null)
		{
			Destroy(character);
		}
		character = GameUIManager.it.CreateUnitForUI(_info.Costume.CostumeObject);

		var costumeInfo = PlatformManager.UserDB.costumeContainer.FindCostumeItem(_info.RawData.costumeTid);
		_textAwakeningLevel.text = PlatformManager.Language[_info.RawData.name];
		_textCostumeName.text = PlatformManager.Language[costumeInfo.ItemName];

		int maxLevel = _info.RawData.awakeningLevels[0].MaxLevel;
		_textMaxLevel.text = $"LV.{maxLevel}";

		_textDescription1.text = string.Format(PlatformManager.Language["str_ui_awakening_description"], maxLevel);
		_textDescription2.text = PlatformManager.Language[_info.RawData.uiDescription];

		for (int i = 0; i < _itemStats.Length; i++)
		{
			if (i < _info.AbilityInfos.Count)
			{
				_itemStats[i].OnUpdate(_info.AbilityInfos[i]);
				_itemStats[i].gameObject.SetActive(true);
			}
			else
			{
				_itemStats[i].gameObject.SetActive(false);
			}
		}
	}

	protected override void OnClose()
	{
		base.OnClose();
		bg.gameObject.SetActive(false);
		Destroy(character);
	}
}
