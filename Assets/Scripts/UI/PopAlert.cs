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

	public void Create(string _resultCodeKey, Action _okCallback = null, Action _cancelCallback = null)
	{
		ResultCodeData resultCodeData = DataManager.it.Get<ResultCodeDataSheet>().Get(_resultCodeKey);
		Create(resultCodeData, _okCallback, _cancelCallback);
	}

	private void Create(ResultCodeData _resultCodeData, Action _okCallback = null, Action _cancelCallback = null)
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
		current.Init(_resultCodeData.Clone(), _okCallback, _cancelCallback);
	}

	/// <summary>
	/// 일반 알림팝업(단순 표시용)
	/// </summary>
	public void CreateInfo(string _contents)
	{
		ResultCodeData resultCodeData = DataManager.it.Get<ResultCodeDataSheet>().Get("INFO");
		resultCodeData = resultCodeData.Clone();
		resultCodeData.content = _contents;

		Create(resultCodeData);
	}

	/// <summary>
	/// 오류 처리용 기본팝업(단순 표시용)
	/// </summary>
	public void CreateErrorInfo(string _contents)
	{
		ResultCodeData resultCodeData = DataManager.it.Get<ResultCodeDataSheet>().Get("ERROR_INFO");
		resultCodeData = resultCodeData.Clone();
		resultCodeData.content = _contents;

		Create(resultCodeData);
	}

	/// <summary>
	/// 오류 처리용 기본팝업(인트로 강제 이동)
	/// </summary>
	public void CreateErrorIntro()
	{
		ResultCodeData resultCodeData = DataManager.it.Get<ResultCodeDataSheet>().Get("ERROR_INTRO");
		Create(resultCodeData, PopupCallback.GoToIntro);
	}

	/// <summary>
	/// 오류 처리용 기본팝업(앱 종료)
	/// </summary>
	public void CreateErrorQuit()
	{
		ResultCodeData resultCodeData = DataManager.it.Get<ResultCodeDataSheet>().Get("ERROR_APP_QUIT");
		Create(resultCodeData, PopupCallback.ExitApplication);
	}
}

public enum PopAlertType
{
	DEFAULT,
	ERROR,
}
