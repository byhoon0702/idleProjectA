using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIProperty : MonoBehaviour
{
	[SerializeField] private Toggle[] presetToggles;
	[SerializeField] private UIPropertyItem[] uiPropertyItems;

	public void Init()
	{
		UpdateStatUpgrdadeUI();
		SetPresetToggles();
	}

	private void UpdateStatUpgrdadeUI()
	{
		for (int i = 0; i < uiPropertyItems.Length; i++)
		{
			var item = uiPropertyItems[i];
			item.SetUI();
		}
	}

	private void SetPresetToggles()
	{
		for (int i = 0; i < presetToggles.Length; i++)
		{
			var toggle = presetToggles[i];
			var index = i;

			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((_isOn) =>
			{
				var text = toggle.GetComponentInChildren<TextMeshProUGUI>();
				if (_isOn == true)
				{
					text.color = Color.yellow;
				}
				else
				{
					text.color = Color.black;
				}
				UserInfo.userData.selectedPropIndex = index;
				UpdateStatUpgrdadeUI();
			});
		}
	}
}
