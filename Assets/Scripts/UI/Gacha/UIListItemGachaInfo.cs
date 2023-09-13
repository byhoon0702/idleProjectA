using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIListItemGachaInfo : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI textLabel;
	[SerializeField] TextMeshProUGUI textValue;
	public void SetData(GachaChanceInfo info, int level)
	{
		textLabel.text = info.grade.ToString();
		textValue.text = $"{(info.chances[level] / 100f).ToString("00.00")}%";

		Color color = info.grade.GradeColor();
		textLabel.color = color;
		textValue.color = color;
	}
}
