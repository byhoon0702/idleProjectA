using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticMethods
{
	public static void CreateListCell(this Transform t, int listCount, GameObject prefab, Action action = null)
	{
		int makeCount = listCount - t.childCount;
		if (makeCount > 0)
		{
			for (int i = 0; i < makeCount; i++)
			{
				GameObject.Instantiate(prefab, t);
			}
		}
		action?.Invoke();
	}

}
