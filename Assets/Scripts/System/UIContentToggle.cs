using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIContentToggle : Toggle, IUIContent
{
	[SerializeField] private ContentType contentType;
	public ContentType ContentType { get { return contentType; } }

	[SerializeField] private GameObject redDot;
	public bool ignoreStatus;

	protected override void OnEnable()
	{
		base.OnEnable();
		AddEvent();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		RemoveEvent();
	}


	public void AddEvent()
	{
		ContentsContainer.AddEvent(ChangeState);
		ContentsContainer.AddReddotEvent(ContentReddot);
	}

	public bool IsAvailable()
	{
		return PlatformManager.UserDB.contentsContainer.TryEnter(contentType);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		if (IsAvailable() == false && ignoreStatus == false)
		{
			return;
		}

		base.OnPointerClick(eventData);
	}
	public void RemoveEvent()
	{
		ContentsContainer.RemoveEvent(ChangeState);
		ContentsContainer.RemoveReddotEvent(ContentReddot);
	}

	public void ChangeState(List<ContentsInfo> infolist)
	{
		for (int i = 0; i < infolist.Count; i++)
		{
			var info = infolist[i];
			if (contentType.HasFlag(info.type))
			{
				if (info.ShowReddot == true)
				{
					UpdateRedDot(info.ShowReddot);
					return;
				}
			}
		}
		UpdateRedDot(false);
	}

	public void ContentReddot(ContentType type)
	{
		if (contentType == type)
		{
			UpdateRedDot(true);
		}
	}

	public void UpdateRedDot(bool showReddot)
	{
		if (redDot == null)
		{
			return;
		}
		redDot.SetActive(showReddot);
	}
}
