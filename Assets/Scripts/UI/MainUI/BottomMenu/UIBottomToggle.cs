using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIBottomToggle : UIBehaviour
{
	[SerializeField] private LayoutElement layoutElement;
	[SerializeField] private Toggle toggle;
	[SerializeField] private Image bg;

	[SerializeField] private RectTransform selectObject;
	[SerializeField] private RectTransform iconMenu;
	[SerializeField] private RectTransform iconClose;
	private RectTransform rectTransform => transform as RectTransform;

	protected override void Awake()
	{
		toggle.onValueChanged.AddListener(OnValueChange);
	}
	public void OnValueChange(bool isOn)
	{
		//bg.enabled = !isOn;
		iconMenu.gameObject.SetActive(!isOn);
		iconClose.gameObject.SetActive(isOn);
		if (isOn)
		{
			if (!selectObject.gameObject.activeInHierarchy)
			{
				selectObject.gameObject.SetActive(true);
			}
		}
		else
		{
			selectObject.gameObject.SetActive(false);
		}
	}

	void OnSelect()
	{

	}
}
