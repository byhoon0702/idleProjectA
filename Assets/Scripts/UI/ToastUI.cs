using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastUI : MonoBehaviour
{
	public static ToastUI Instance { get; private set; }
	public float time = 0.3f;
	[SerializeField] private ItemToast[] uiItemToasts;
	private Queue<string> toastMessageQueue = new Queue<string>();

	private float progress = 0;

	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = null;
	}

	public void Enqueue(string text)
	{
		if (toastMessageQueue.Count > 4)
		{
			return;
		}
		toastMessageQueue.Enqueue(text);
	}
	public void EnqueueKey(string text)
	{
		Enqueue(PlatformManager.Language[text]);
	}


	void Update()
	{
		if (toastMessageQueue.Count == 0)
		{
			return;
		}

		progress += Time.deltaTime;
		if (progress >= time)
		{
			progress = 0;
			Create();
		}
	}

	private ItemToast GetToast()
	{
		for (int i = 0; i < uiItemToasts.Length; i++)
		{
			if (uiItemToasts[i].gameObject.activeSelf == false)
			{
				return uiItemToasts[i];
			}
		}
		return null;
	}

	private void Create()
	{
		var toast = GetToast();
		if (toast == null)
		{
			return;
		}
		toast.gameObject.SetActive(true);
		toast.transform.SetAsFirstSibling();
		toast.OnUpdate(toastMessageQueue.Dequeue());
	}
}
