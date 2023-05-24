using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemHyperClass : UIItemHyperInfo
{


	private UIPageHyper parent;

	private RuntimeData.HyperClassInfo info;
	public void OnUpdate(UIPageHyper _parent, RuntimeData.HyperClassInfo _info)
	{
		parent = _parent;

		info = _info;
		textLevel.text = $"Lv {_info.Level}";
	}
}
