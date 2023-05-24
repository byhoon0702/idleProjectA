using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(NormalUnitCostume))]
public class UnitCostumeInspector : Editor
{
	NormalUnitCostume costume;
	private void OnEnable()
	{
		costume = target as NormalUnitCostume;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Init"))
		{
			costume.Init();
		}
		if (GUILayout.Button("Change"))
		{
			//costume.ChangeLibrary();

			costume.ChangeBody();
			costume.ChangeHead();
			costume.ChangeWeapon();

			costume.SyncAnimation();
		}
	}
}
