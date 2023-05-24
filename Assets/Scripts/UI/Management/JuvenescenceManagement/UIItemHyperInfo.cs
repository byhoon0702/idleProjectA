using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIItemHyperInfo : Selectable
{
	[SerializeField] protected Image icon;
	[SerializeField] protected TextMeshProUGUI textLevel;

	private float pressStartTime;

	private bool showToolTip;
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		pressStartTime = Time.unscaledTime;
	}
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);

		pressStartTime += Time.unscaledTime;
		if (pressStartTime > 1)
		{
			ShowTooltip();
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		showToolTip = false;
	}

	public void ShowTooltip()
	{
		if (showToolTip)
		{
			return;
		}
		showToolTip = true;
	}
}
