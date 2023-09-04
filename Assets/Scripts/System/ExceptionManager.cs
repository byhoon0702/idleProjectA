using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionManager : MonoBehaviour
{

	public static ExceptionManager Instance;

	public bool ExceptionOccurred { get; private set; }
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
		//Application.logMessageReceived += HandleException;
		DontDestroyOnLoad(gameObject);
	}
	void HandleException(string logString, string stackTrace, LogType type)
	{
		switch (type)
		{
			case LogType.Error:
			case LogType.Exception:
				VResult result = new VResult();
				PopAlert.Create("오류", stackTrace);
				//ExceptionOccured = true;
				Debug.LogError($"{logString}\n - [StackTrace]:{stackTrace}");

				break;
		}
	}
}
