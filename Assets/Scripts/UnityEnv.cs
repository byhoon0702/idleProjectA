using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityEnv
{
	public static bool HouseMode
	{ 
		get
		{
#if HOUSE
			return true;
#else
			return false;
#endif

		}
	}

	public static bool IsApplicationQuit = false;
}
