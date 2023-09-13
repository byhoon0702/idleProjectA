using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAlert : MonoBehaviour
{
	private static PopAlert Instance;
	public PopAlertDefault current;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}
		DontDestroyOnLoad(gameObject);
	}
	bool isNoticeShow;
	public static void CreateNotice(string title, string desc, string okString, Action okCallback = null, Action cancelCallback = null)
	{
		if (Instance.isNoticeShow)
		{
			return;
		}
		Instance.isNoticeShow = true;
		if (Instance.current == null)
		{
			Instance.current = Instantiate(Resources.Load<PopAlertDefault>("PopAlertDefault"), Instance.transform);
		}
		Instance.current.gameObject.SetActive(true);
		Instance.current.Init(title, desc, okString, "", _onOkCallback: () => { Instance.isNoticeShow = false; okCallback?.Invoke(); }, _onCancelCallback: cancelCallback);
	}

	public static void Create(string title, string desc, Action okCallback = null, Action cancelCallback = null)
	{
		if (Instance.isNoticeShow)
		{
			return;
		}
		if (Instance.current == null)
		{
			Instance.current = Instantiate(Resources.Load<PopAlertDefault>("PopAlertDefault"), Instance.transform);
		}
		Instance.current.gameObject.SetActive(true);
		Instance.current.Init(title, desc, _onOkCallback: okCallback, _onCancelCallback: cancelCallback);
	}
	public static void Create(string title, string desc, string okString, string cancelString, Action okCallback = null, Action cancelCallback = null)
	{
		if (Instance.isNoticeShow)
		{
			return;
		}
		if (Instance.current == null)
		{
			Instance.current = Instantiate(Resources.Load<PopAlertDefault>("PopAlertDefault"), Instance.transform);
		}
		Instance.current.gameObject.SetActive(true);
		Instance.current.Init(title, desc, okString, cancelString, _onOkCallback: okCallback, _onCancelCallback: cancelCallback);
	}


	public static void Create(VResult _resultCode, Action _okCallback = null, Action _cancelCallback = null)
	{
		//if (current != null)
		//{
		//	try
		//	{
		//		current.Close();
		//	}
		//	catch (Exception e)
		//	{
		//		VLog.LogError("콜백함수 처리오류\n{e}");
		//	}
		//	current = null;
		//}

		//current = Instantiate(Resources.Load<PopAlertDefault>("PopAlertDefault"), Instance.transform);
		//current.Init(_resultCode.Clone(), _okCallback, _cancelCallback);
	}

	public static void CreateException(Exception _e, string _msg)
	{
	}
}

public enum PopAlertType
{
	DEFAULT,
	ERROR,
}
