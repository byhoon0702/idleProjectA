using UnityEditor;
using UnityEngine;


public class BattleRecordEditor : EditorWindow
{
	Vector2 scrollPos;

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

		GUIStyle labelStyle = new GUIStyle("Label");
		labelStyle.richText = true;

		GUIStyle charNameStyle = new GUIStyle("PreToolbar");
		charNameStyle.richText = true;

		GUIStyle buttonStyle = new GUIStyle("Button");
		buttonStyle.richText = true;
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		foreach (var record in GameManager.it.battleRecord.records)
		{
			Character character = CharacterManager.it.GetCharacter(record.charID, true);
			string charName;
			if (character != null)
			{
				charName = $"[{character.info.data.name}({record.charID}) - State: {character.currentState}]";
			}
			else
			{
				charName = $"[<color=red>Invalid</color>({record.charID})]";
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
			GUILayout.Label(charName, charNameStyle);
			GUILayout.EndHorizontal();

			if (character.target != null)
			{
				if (GUILayout.Button($"Target: {character.target.info.data.name}({character.target.charID}", buttonStyle, GUILayout.MaxWidth(200)))
				{
					Selection.activeGameObject = character.target.gameObject;
				}
			}
			else
			{
				if (GUILayout.Button($"Target: Null", buttonStyle, GUILayout.MaxWidth(200)))
				{
				}
			}

			GUILayout.Label(record.ToStringEditor());

			GUILayout.Space(10);
		}
		GUILayout.EndScrollView();
	}

	private void Update()
	{
		Repaint();
	}
}
