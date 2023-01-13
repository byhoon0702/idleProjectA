using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PopupCallback
{
	public static void ExitApplication()
	{
		Application.Quit();
	}

	public static void GoToIntro()
	{
		VLog.LogWarning("구현안됨. GotoIntro");
	}
}
