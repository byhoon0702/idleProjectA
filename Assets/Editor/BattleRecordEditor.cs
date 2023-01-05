using UnityEditor;
using UnityEngine;
using System;


public class BattleRecordEditor : EditorWindow
{
	Vector2 scrollPos;


	private GUIStyle labelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;

			return style;
		}
	}

	private GUIStyle charNameStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("PreToolbar");
			style.richText = true;

			return style;
		}
	}

	private GUIStyle buttonStyle
	{
		get
		{
			GUIStyle buttonStyle = new GUIStyle("Button");
			buttonStyle.richText = true;

			return buttonStyle;
		}
	}


	[MenuItem("Custom Menu/Battle Record")]
	private static void InitEditor()
	{
		BattleRecordEditor window = ScriptableObject.CreateInstance<BattleRecordEditor>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 300);
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			GUILayout.Label("플레이중이 아님");
			return;
		}


		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		foreach (var record in GameManager.it.battleRecord.records)
		{
			ShowCharInfo(record);
			GUILayout.Space(10);
		}
		GUILayout.EndScrollView();
	}

	private void Update()
	{
		Repaint();
	}

	private void ShowCharInfo(RecordData _record)
	{
		Character character = CharacterManager.it.GetCharacter(_record.charID, true);
		string charName;
		if (character != null)
		{
			charName = $"[{character.info.charNameAndCharId} - State: {character.currentState}]";
		}
		else
		{
			charName = $"[<color=red>Invalid</color>({_record.charID})]";
		}

		if (Selection.activeGameObject == character.gameObject)
		{
			charName = $"<color=yellow>{charName}</color>";
		}

		GUILayout.BeginHorizontal();
		if (GUILayout.Button($"Select", GUILayout.MaxWidth(60), GUILayout.MinWidth(60)))
		{
			Selection.activeGameObject = character.gameObject;
		}
		GUILayout.Label(charName, charNameStyle, GUILayout.MinWidth(400), GUILayout.MaxWidth(400));

		if (character.target != null)
		{
			if (GUILayout.Button($"Target: {character.target.info.charNameAndCharId}({character.target.charID}", buttonStyle))
			{
				Selection.activeGameObject = character.target.gameObject;
			}
		}
		else
		{
			if (GUILayout.Button($"Target: Null", buttonStyle))
			{
			}
		}
		GUILayout.EndHorizontal();


		GUILayout.Label(_record.ToStringEditor());
	}
}
