using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UserDB))]
public class UserDBInspector : Editor
{
	UserDB userDb;
	private void OnEnable()
	{
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}

