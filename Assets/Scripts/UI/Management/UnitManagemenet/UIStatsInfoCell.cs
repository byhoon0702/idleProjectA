using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStatsInfoCell : MonoBehaviour
{
	public TextMeshProUGUI textmeshLabel;
	public TextMeshProUGUI textmeshValue;

	public void OnUpdate(string label, string value)
	{
		textmeshLabel.text = label;
		textmeshValue.text = value;
	}

}
