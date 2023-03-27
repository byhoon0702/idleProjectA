using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EffectInfoCell : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI effectLabel;
	[SerializeField] private TextMeshProUGUI effectValue;

	public void OnUpdate(string label, string value)
	{
		effectLabel.text = label;
		effectValue.text = value;
	}
}
