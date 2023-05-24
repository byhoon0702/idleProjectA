using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIStatHyperClass : UIItemHyperInfo
{

	UIPageHyper parent;
	private int index;
	public void OnUpdate(UIPageHyper _parent, int _level)
	{
		parent = _parent;


		textLevel.text = $"LV {_level}";
	}
}
