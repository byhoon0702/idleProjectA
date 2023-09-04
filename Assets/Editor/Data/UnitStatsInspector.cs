using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(UnitStats))]
public class UnitStatsInspector : Editor
{
	UnitStats unitStats;
	private void OnEnable()
	{
		//unitStats = target as UnitStats;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Generate"))
		{
			//unitStats.Generate();
			//EditorUtility.SetDirty(unitStats);

			//AssetDatabase.SaveAssetIfDirty(unitStats);
			//AssetDatabase.Refresh();
		}
	}
}
