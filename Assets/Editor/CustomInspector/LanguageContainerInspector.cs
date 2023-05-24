using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageContainer))]
public class LanguageContainerInspector : Editor
{
	LanguageContainer container;
	private void OnEnable()
	{
		container = (LanguageContainer)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			container.ReadFile();
			EditorUtility.SetDirty(container);
			AssetDatabase.SaveAssetIfDirty(container);
			AssetDatabase.Refresh();
		}
	}
}
