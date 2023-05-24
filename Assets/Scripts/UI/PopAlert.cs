using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAlert : MonoBehaviour
{
	private static PopAlert it;
	private static PopAlertDefault current;


	private void Awake()
	{
		it = this;
	}

	public static void Create(VResult _resultCode, Action _okCallback = null, Action _cancelCallback = null)
	{
		if (current != null)
		{
			try
			{
				current.Close();
			}
			catch(Exception e)
			{
				VLog.LogError("콜백함수 처리오류\n{e}");
			}
			current = null;
		}

		current = Instantiate(Resources.Load<PopAlertDefault>("PopAlertDefault"), it.transform);
		current.Init(_resultCode.Clone(), _okCallback, _cancelCallback);
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
