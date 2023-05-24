using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemJuvenescenceStats : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTile;
	[SerializeField] private Toggle[] points;

	public void OnUpdate(RuntimeData.JuvenescenceElementStat stat)
	{
		textTile.text = stat.stats.type.ToUIString();

		for (int i = 0; i < points.Length; i++)
		{
			points[i].isOn = i < stat.Point;
		}
	}
}
