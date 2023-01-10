using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
	private Character character;


	private void OnEnable()
	{
		character = target as Character;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying == false)
		{
			return;
		}

		EditorGUILayout.Space();
		if(GUILayout.Button("Death"))
		{
			character.Hit(character, character.rawData.hp * 10, "Editor", Color.white, 1);
		}
		if(GUILayout.Button("Extension"))
		{
			CharacterEditorExtension.ShowEditor(character);
		}

		EditorGUILayout.Space();
		CharacterEditorExtension.ShowCharInfo(character);
		EditorGUILayout.Space();
		CharacterEditorExtension.ShowRecordData(character);
		EditorGUILayout.Space();
		CharacterEditorExtension.ShowConditionList(character);
	}
}
