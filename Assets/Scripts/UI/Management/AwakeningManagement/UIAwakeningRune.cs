using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UIAwakeningRune : MonoBehaviour
{
	[SerializeField] private Image _iconRune;
	[SerializeField] private Image _iconRuneOn;
	[SerializeField] private Button _buttonRune;
	[SerializeField] private TextMeshProUGUI _textProgress;

	private RuntimeData.AwakeningLevelInfo _levelInfo;
	private RuntimeData.AwakeningInfo _info;

	private UIManagementAwakening _parent;

	private Action<RuntimeData.AwakeningLevelInfo, Sprite> _onClickAction;

	private void Awake()
	{
		_buttonRune.SetButtonEvent(OnClickButton);
	}

	public void SetUI(UIManagementAwakening parent, RuntimeData.AwakeningInfo info, RuntimeData.AwakeningLevelInfo levelInfo, Action<RuntimeData.AwakeningLevelInfo, Sprite> onClickAction)
	{
		_parent = parent;
		_info = info;
		_levelInfo = levelInfo;

		_onClickAction = onClickAction;
		_textProgress.text = $"{_levelInfo.Level}/{_levelInfo.MaxLevel}";

		if (_levelInfo.Level >= _levelInfo.MaxLevel)
		{
			_iconRuneOn.gameObject.SetActive(true);
		}
		else
		{
			_iconRuneOn.gameObject.SetActive(false);
		}
	}

	public void OnClickButton()
	{
		_onClickAction?.Invoke(_levelInfo, _iconRune.sprite);
	}
}
