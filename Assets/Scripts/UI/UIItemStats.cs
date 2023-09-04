using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemStats : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textLabel;
	[SerializeField] private TextMeshProUGUI textValue;

	public void OnUpdate(RuntimeData.AbilityInfo info)
	{
		string typeName = info.type.ToUIString();

		string value = $"+{info.Value.ToString()}{info.tailChar}";
		OnUpdate(typeName, value);
	}
	public void OnUpdate(string label, string value)
	{
		textLabel.text = label;
		textValue.text = value;
	}
}
