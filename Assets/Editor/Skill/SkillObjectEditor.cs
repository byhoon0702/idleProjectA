using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
//[CustomEditor(typeof(SkillObject))]
public class SkillObjectEditor : Editor
{
	SkillObject skillObject;
	private void OnEnable()
	{
		skillObject = target as SkillObject;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Set"))
		{

		}
	}
}
