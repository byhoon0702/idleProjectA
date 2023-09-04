using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Testss))]
public class TestssInspector : Editor
{
	Testss testss;
	private void OnEnable()
	{
		testss = target as Testss;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Test"))
		{
			testss.OnTest();
		}
	}
}
