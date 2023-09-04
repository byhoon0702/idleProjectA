using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIContent
{
	ContentType ContentType { get; }
	void AddEvent();
	void RemoveEvent();

	bool IsAvailable();

	void UpdateRedDot(bool showReddot);
}



public class UIBase : MonoBehaviour, IUIClosable
{
	[SerializeField] private ContentType contentType;
	public ContentType ContentType => contentType;

	protected Action onCloseAction;
	public virtual bool Activate()
	{
		return Activate(null);
	}
	public void ActivateWithoutReturn()
	{
		Activate(null);
	}

	public virtual bool Activate(Action onCloseAction)
	{

		bool isOpen = PlatformManager.UserDB.contentsContainer.IsOpen(contentType, true);

		if (isOpen == false)
		{
			Close();
			return false;
		}

		if (Closable())
		{
			GameUIManager.it.uIClosables.Push(this);
		}

		this.onCloseAction = onCloseAction;
		OnActivate();
		gameObject.SetActive(true);
		var content = PlatformManager.UserDB.contentsContainer.Get(contentType);
		if (content != null)
		{
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.CONTENTS_ENTER, content.Tid, (IdleNumber)1);
		}

		return true;
	}

	protected virtual void OnEnable()
	{
		AddCloseListener();
	}
	protected virtual void OnDisable()
	{
		RemoveCloseListener();
	}

	protected virtual void OnActivate()
	{

	}

	public void AddCloseListener()
	{
		if (GameUIManager.it != null)
		{
			GameUIManager.it.onClose += Close;
		}
	}

	public void RemoveCloseListener()
	{
		if (GameUIManager.it != null)
		{
			GameUIManager.it.onClose -= Close;
		}
	}

	public virtual bool Closable()
	{
		return true;
	}

	public void Close()
	{
		OnClose();

	}
	protected virtual void OnClose()
	{
		gameObject.SetActive(false);
		onCloseAction?.Invoke();
	}
}
