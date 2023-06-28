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
		toggle.onValueChanged.RemoveAllListeners();
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

				//DOTween.To(() => layoutElement.preferredWidth, x => layoutElement.preferredWidth = x, 250f, 0.3f);
				//layoutElement.preferredWidth = 230;
			}
		}
		else
		{
			selectObject.gameObject.SetActive(false);
			//DOTween.To(() => layoutElement.preferredWidth, x => layoutElement.preferredWidth = x, 210f, 0.3f);
			//layoutElement.preferredWidth = 175;
		}
	}

	void OnSelect()
	{

	}
}
