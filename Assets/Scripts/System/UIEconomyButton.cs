using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEconomyButton : Selectable, IDragHandler, IEndDragHandler
{
	[SerializeField] private TextMeshProUGUI _textmeshLabel;
	[SerializeField] private Image _imageEconomy;
	[SerializeField] private TextMeshProUGUI _textmeshEconomy;

	[SerializeField] private ButtonRepeatEvent _onButtonDown;
	[SerializeField] private UnityAction _onClick;

	public bool onlyOnce;
	private const float longTouchBeginTime = 0.3f;
	private const float touchIntervalTime = 0.05f;
	float touchTime = 0;
	bool isHeldButton;
	bool isPressed;
	bool failed = false;
	public void SetEvent(ButtonRepeatEvent onButtonDown, UnityAction onClick = null)
	{
		_onButtonDown = onButtonDown;
		_onClick = onClick;
	}
	public void SetLabel(string label)
	{
		_textmeshLabel.text = label;
	}
	public void SetButton(CurrencyType type, Sprite icon, string value, bool isAvailable = true)
	{
		_imageEconomy.enabled = icon != null;
		switch (type)
		{
			case CurrencyType.NONE:
				_textmeshEconomy.text = "구매불가";
				break;
			case CurrencyType.FREE:
				_textmeshEconomy.text = "무료";
				break;
			case CurrencyType.CASH:
				_textmeshEconomy.text = value;
				break;
			case CurrencyType.ADS:
				_imageEconomy.sprite = icon;
				_textmeshEconomy.text = value;
				break;
			case CurrencyType.END:
				break;
			default:
				_imageEconomy.sprite = icon;
				_textmeshEconomy.text = value;
				break;
		}

		if (isAvailable)
		{
			_textmeshEconomy.color = Color.white;
		}
		else
		{
			_textmeshEconomy.color = Color.red;
		}
	}

	public void SetButton(Sprite sprite, string value, bool isAvailable = true)
	{
		_imageEconomy.enabled = sprite != null;
		_imageEconomy.sprite = sprite;
		_textmeshEconomy.text = value;

		if (isAvailable)
		{
			_textmeshEconomy.color = Color.white;
		}
		else
		{
			_textmeshEconomy.color = Color.red;
		}
	}

	public void SetInteractable(bool interactable)
	{
		this.interactable = interactable;
		if (interactable)
		{
			_textmeshEconomy.color = _textmeshLabel.color = Color.white;
		}
		else
		{
			_textmeshEconomy.color = _textmeshLabel.color = Color.gray;
		}
	}

	Vector2 mousePosition;
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (this.interactable == false)
		{
			return;
		}
		base.OnPointerDown(eventData);
		mousePosition = eventData.position;
		if (onlyOnce || failed)
		{
			return;
		}
		isPressed = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		isPressed = false;
		isHeldButton = false;
	}
	bool isOk = false;
	private void Update()
	{
		if (isPressed)
		{
			touchTime += Time.deltaTime;
			if (touchTime > longTouchBeginTime)
			{
				isHeldButton = true;
			}
			if (isHeldButton && touchTime > touchIntervalTime)
			{
				if (_onButtonDown != null)
				{
					try
					{
						isOk = _onButtonDown.Invoke();
					}
					catch (System.Exception ex)
					{
						isOk = false;
					}
					//Debug.Log($"Button State : {isOk}");
					if (isOk == false)
					{
						isHeldButton = false;
						isPressed = false;
						failed = true;
					}
				}
				touchTime = 0;
			}
		}
	}
	bool isDrag = false;
	public void OnDrag(PointerEventData eventData)
	{
		isDrag = true;
		touchTime = 0;
		isHeldButton = false;
		isPressed = false;
		failed = false;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		isDrag = false;
	}
	public override void OnPointerUp(PointerEventData eventData)
	{
		if (isDrag)
		{
			return;
		}

		base.OnPointerUp(eventData);

		if (isHeldButton == false)
		{
			if (_onButtonDown != null)
			{
				_onButtonDown.Invoke();
			}
			else if (_onClick != null)
			{
				_onClick.Invoke();
			}
		}

		touchTime = 0;
		isHeldButton = false;
		isPressed = false;
		failed = false;
	}


	public void UpdateValue(string value)
	{
		_textmeshEconomy.text = value;
	}


}
