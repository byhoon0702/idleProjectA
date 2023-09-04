
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContentButton : Button, IUIContent
{
	[SerializeField] private ContentType contentType;
	public ContentType ContentType { get { return contentType; } }
	[SerializeField] private GameObject redDot;
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
