using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnManager))]
public class SpawnManagerEditor : Editor
{
	SpawnManager spawnManager;

	private void OnEnable()
	{
		spawnManager = target as SpawnManager;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("플레이어 킬"))
		{
			spawnManager.KillPlayer();
		}
	}
}
