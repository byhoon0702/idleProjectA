using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data Table Editor Settings", order = 2)]
[System.Serializable]
public class DataTableEditorSettings : ScriptableObject
{
	[SerializeField] public string linkFieldName;
	[Range(15, 100)]
	[SerializeField] public float elementHeight = EditorGUIUtility.singleLineHeight;
	[SerializeField] public Vector2 scrollViewSize = new Vector2(800, 600);
	[SerializeField] public Vector2 cellSize;
	[SerializeField] public Vector2 elementSize;

	[SerializeField] public float rowSpace = 5;
	[SerializeField] public float columeSpace = 5;


}
