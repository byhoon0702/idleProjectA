using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenGridClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
	[SerializeField] private UIGrandpaAndChickenTree owner;

	private bool isPointerDown = false;

	public void OnPointerDown(PointerEventData eventData)
	{
		isPointerDown = true;

		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);

		owner.OnPointerDown(localPoint);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointerDown = false;

		owner.OnPointerCancel();
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		if (isPointerDown == true)
		{
			Vector2 localPoint;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);

			owner.OnPointerMove(localPoint);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown = false;

		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);

		owner.OnPointerUp(localPoint);
	}
}
