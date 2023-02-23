using UnityEditor;
using UnityEngine;
using System;


public class BattleRecordEditor : EditorWindow
{
	public Vector2 scrollPos;
	public bool filterPlayerSide = false;
	public bool filterEnemySide = false;
	public bool filterDeath = false;


	private GUIStyle labelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;
			style.wordWrap = true;

			return style;
		}
	}

	[MenuItem("Custom Menu/Battle Record")]
	private static void InitEditor()
	{
		BattleRecordEditor window = GetWindow<BattleRecordEditor>();
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

		if(VGameManager.it.battleRecord == null)
		{
			GUILayout.Label("Record is nulll");
			return;
		}

		GUILayout.Label("플레이어", "PreToolbar");
		GUILayout.Label($"{VGameManager.it.battleRecord.playerDPS}");

		GUILayout.Label("펫", "PreToolbar");
		GUILayout.Label($"{VGameManager.it.battleRecord.petDPS}");

		GUILayout.Label("적", "PreToolbar");
		GUILayout.Label($"{VGameManager.it.battleRecord.enemyDPS}");

		GUILayout.Label("Unknown", "PreToolbar");
		GUILayout.Label($"{VGameManager.it.battleRecord.unknownDPS}");
	}

	private void Update()
	{
		Repaint();
	}
}
