using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SliderHandle : MonoBehaviour, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

	public UnityEvent PointerUp;
	[Range(0, 1)]
	public float value;
	[SerializeField] private RectTransform handle;
	[SerializeField] private RectTransform parent;

	public void OnBeginDrag(PointerEventData eventData)
	{

	}

	public void OnDrag(PointerEventData eventData)
	{

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		PointerUp?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		PointerUp?.Invoke();
	}
}
