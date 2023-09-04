using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManagementAwakening : UIBase
{

	[Header("각성석")]
	[SerializeField] private UIAwakeningRune[] runes;
	public UIAwakeningRune[] Runes => runes;
	[SerializeField] private GameObject objStone;
	[SerializeField] private Image imageCurrency;
	[SerializeField] private TextMeshProUGUI textCurrency;
	[SerializeField] private TextMeshProUGUI textAwakeningLevel;

	[Header("각성 버튼")]
	[SerializeField] private GameObject objAwakenIconBgOff;
	[SerializeField] private GameObject objAwakenIconBgOn;

	[SerializeField] private Animator animator;
	[SerializeField] private GameObject objAwakenBgAfter;
	[SerializeField] private GameObject objAwakenBgAfterRingBig;
	[SerializeField] private GameObject objAwakenBgAfterRingSmall;


	[SerializeField] private GameObject objDefaultAwaken;
	[SerializeField] private Image imageAwaken;

	[SerializeField] private UIAwakeningResult uiAwakeningResult;
	[SerializeField] private UIAwakeningRuneLevelUpPopup levelUPPopup;
	public UIAwakeningRuneLevelUpPopup PopupLevelUP => levelUPPopup;
	[SerializeField] private UIAwakeningUpgradePopup upgradePopup;
	[SerializeField] private UIAwakeningUpgradePopup infoPopup;

	[SerializeField] private Button _buttonUpgrade;
	public Button ButtonUpgrade => _buttonUpgrade;
	[SerializeField] private Button _buttonInfo;

	private RuntimeData.AwakeningInfo info = null;

	private void Awake()
	{
		_buttonUpgrade.SetButtonEvent(OnClickUpgrade);
		_buttonInfo.SetButtonEvent(OnClickInfo);
	}

	private bool _canAwaken;
	public void OnUpdate()
	{
		info = PlatformManager.UserDB.awakeningContainer.SelectedInfo;

		if (info == null)
		{
			Debug.LogError("No Awakening INfo");
		}

		textAwakeningLevel.text = PlatformManager.Language[info.RawData.name];
		for (int i = 0; i < runes.Length; i++)
		{
			runes[i].SetUI(this, info, PlatformManager.UserDB.awakeningContainer.RuneInfoList[i], OnClickLevelUp);
		}

		var currency = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.AWAKENING_STONE);
		textCurrency.text = $"{currency.Value.ToString()}";

		OnUpdateCenterRune();
	}

	public void OnUpdateCenterRune()
	{
		_canAwaken = PlatformManager.UserDB.awakeningContainer.CanBeAwaken(info, out string msg);
		objAwakenBgAfter.SetActive(_canAwaken);
		objAwakenBgAfterRingBig.SetActive(_canAwaken);
		objAwakenBgAfterRingSmall.SetActive(_canAwaken);

		animator.enabled = _canAwaken;

		if (info.ItemObject.Icons != null && info.ItemObject.Icons.Length > 1)
		{
			imageAwaken.enabled = true;
			if (_canAwaken)
			{
				imageAwaken.sprite = info.ItemObject.Icons[1];
			}
			else
			{

				imageAwaken.sprite = info.ItemObject.Icons[0];
			}
			objDefaultAwaken.SetActive(false);
		}
		else
		{
			objDefaultAwaken.SetActive(true);
			imageAwaken.enabled = false;
		}
	}

	public void OnClickLevelUp(RuntimeData.AwakeningLevelInfo levelInfo, Sprite icon)
	{
		if (levelUPPopup.Activate())
		{
			levelUPPopup.Show(this, info, levelInfo, icon);
		}

	}

	public void OnClickUpgrade()
	{

		if (PlatformManager.UserDB.awakeningContainer.Awaken(info))
		{
			if (uiAwakeningResult.Activate())
			{
				uiAwakeningResult.SetData(PlatformManager.UserDB.awakeningContainer.SelectedInfo);
			}
			OnUpdate();
		}
	}

	public void OnClickInfo()
	{
		if (infoPopup.Activate())
		{
			infoPopup.OnUpdate(this, info);
		}

	}
}
