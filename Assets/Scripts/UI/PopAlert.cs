using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAlert : MonoBehaviour
{
	public static PopAlert it { get; private set; }
	[NonSerialized] public PopAlertDefault current;


	private void Awake()
	{
		it = this;
	}

	public void Create(VResult _resultCode, Action _okCallback = null, Action _cancelCallback = null)
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

		current = Instantiate(Resources.Load<PopAlertDefault>("PopAlertDefault"), transform);
		current.Init(_resultCode.Clone(), _okCallback, _cancelCallback);
	}
}

public enum PopAlertType
{
	DEFAULT,
	ERROR,
}
