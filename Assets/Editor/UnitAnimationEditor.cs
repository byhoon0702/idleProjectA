using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitAnimation))]
public class UnitAnimationEditor : Editor
{
	UnitAnimation unitAnimation;

	private void OnEnable()
	{
		unitAnimation = target as UnitAnimation;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("모델 세팅"))
		{
			unitAnimation.SetModel();
			EditorUtility.SetDirty(unitAnimation);
		}
	}
}
