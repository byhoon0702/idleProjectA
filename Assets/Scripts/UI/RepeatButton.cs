using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public delegate bool ButtonRepeatEvent();

public class RepeatButton : Selectable
{
	private const float LONG_CLICK_START_TIME = 0.5f;

	public ButtonRepeatEvent repeatCallback;
	public Action<bool> onbuttonUp;

	private bool isPressed;
	private bool callRepeat;
	private float pressStartTime;
	bool failed = false;
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
		if (failed)
		{
			return;
		}
		base.OnPointerDown(eventData);
		isPressed = true;
		callRepeat = false;
		pressStartTime = Time.unscaledTime;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		isPressed = false;
		failed = false;
		onbuttonUp?.Invoke(callRepeat);
	}
	protected override void OnDisable()
	{
		isPressed = false;
		callRepeat = false;
	}

	private void Update()
	{
		if (isPressed && (pressStartTime + LONG_CLICK_START_TIME < Time.unscaledTime))
		{
			callRepeat = true;
			bool isOk = (bool)repeatCallback?.Invoke();
			if (isOk == false)
			{
				failed = true;
				isPressed = false;
			}
		}
	}
}
