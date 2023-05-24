using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIItemJuvenescenceElement : MonoBehaviour
{
	[SerializeField] private Image imageIcon;
	[SerializeField] private Toggle[] points;
	[SerializeField] private Button buttonUpgrade;
	[SerializeField] private UIItemJuvenescenceStats[] stats;

	private UIPageJuvenescence parent;
	private RuntimeData.JuvenescenceElementInfo info;

	private void Awake()
	{
		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClick);
	}

	public void OnUpdate(UIPageJuvenescence _parent, RuntimeData.JuvenescenceElementInfo _info)
	{
		parent = _parent;
		info = _info;
		for (int i = 0; i < points.Length; i++)
		{
			points[i].isOn = i < info.Point;
		}

		for (int i = 0; i < stats.Length; i++)
		{
			stats[i].OnUpdate(info.StatsInfo[i]);
		}
	}

	public void OnClick()
	{
		//info.LevelUp();
		parent.LevelUp(info);
	}
}
