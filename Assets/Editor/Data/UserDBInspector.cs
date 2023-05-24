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
		userDb = target as UserDB;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Save"))
		{
			userDb.Save();

		}
		if (GUILayout.Button("Load"))
		{
			userDb.Load();

		}
	}
}

