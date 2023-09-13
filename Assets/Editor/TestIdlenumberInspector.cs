using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestIdlenumber))]
public class TestIdlenumberInspector : Editor
{
	TestIdlenumber testss;
	private void OnEnable()
	{
		testss = target as TestIdlenumber;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Test"))
		{
			testss.OnCalculate();
		}
	}
}
