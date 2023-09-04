using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIToggleBattlePass : MonoBehaviour
{
	[SerializeField] private Toggle toggle;
	[SerializeField] private TextMeshProUGUI textLabel;
	RuntimeData.BattlePassInfo _info;
	UIPopupBattlePass _parent;

	public void SetData(UIPopupBattlePass parent, RuntimeData.BattlePassInfo info)
	{
		_parent = parent;
		_info = info;

		textLabel.text = PlatformManager.Language[_info.rawData.name];
	}
	public void Toggle(bool isOn)
	{
		toggle.SetIsOnWithoutNotify(isOn);
		OnValueChanged(isOn);
	}

	public void OnValueChanged(bool isOn)
	{
		if (isOn)
		{
			_parent.SetData(_info);
		}
	}
}
