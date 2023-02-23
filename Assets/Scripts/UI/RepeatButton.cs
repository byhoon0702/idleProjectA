using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RepeatButton : Selectable
{
	private const float LONG_CLICK_START_TIME = 1;


	public Action repeatCallback;

	private bool isPressed;
	private bool callRepeat;
	private float pressStartTime;

	public void SetInteractable(bool _value)
	{
		interactable = _value;
		if (interactable)
		{
			DoStateTransition(SelectionState.Normal, false);
		}
		else
		{
			DoStateTransition(SelectionState.Disabled, false);
		}
	}



	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		isPressed = true;
		callRepeat = false;
		pressStartTime = Time.unscaledTime;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		isPressed = false;
		if (callRepeat == false)
		{
			repeatCallback?.Invoke();
		}
	}

	private void Update()
	{
		if (isPressed && (pressStartTime + LONG_CLICK_START_TIME < Time.unscaledTime))
		{
			callRepeat = true;
			repeatCallback?.Invoke();
		}
	}
}
