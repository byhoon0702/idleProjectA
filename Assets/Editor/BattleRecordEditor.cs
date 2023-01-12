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

		GUILayout.BeginHorizontal();
		filterPlayerSide = GUILayout.Toggle(filterPlayerSide, "플레이어 표시 제외");
		filterEnemySide = GUILayout.Toggle(filterEnemySide, "적 표시 제외");
		filterDeath = GUILayout.Toggle(filterDeath, "죽은애들 제외");
		GUILayout.EndHorizontal();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		foreach (var record in VGameManager.it.battleRecord.records)
		{
			Character character = CharacterManager.it.GetCharacter(record.charID, true);
			if (filterPlayerSide)
			{
				if(character != null && character.info.controlSide == ControlSide.PLAYER)
				{
					continue;
				}
			}
			if(filterEnemySide)
			{
				if(character != null && character.info.controlSide == ControlSide.ENEMY)
				{
					continue;
				}
			}
			if (filterDeath)
			{
				if (character != null && character.currentState == StateType.DEATH)
				{
					continue;
				}
			}

			ShowCharInfo(character, record);
			GUILayout.Space(10);
		}
		GUILayout.EndScrollView();
	}

	private void Update()
	{
		Repaint();
	}

	private void ShowCharInfo(Character _character, RecordData _record)
	{
		string charName;
		string stateText;
		string hpText = "";
		string descText = "";
		if (_character != null)
		{
			charName = $"[{_character.info.charNameAndCharId}] ";
			stateText = $"State: {_character.currentState}";
			if (_character.info.data.rankType == RankType.BOSS || _character.info.data.rankType == RankType.MID_BOSS || _character.info.data.rankType == RankType.FINISH_GEM)
			{
				descText = $"- <color=magenta>{_character.info.data.rankType}</color>";
			}
			if (_character.info.data.hp.GetValue() <= 0)
			{
				hpText = $"(HP: {0} / {_character.rawData.hp.ToString()})";
			}
			else
			{
				hpText = $"(HP: {_character.info.data.hp.ToString()} / {_character.rawData.hp.ToString()})";
			}
		}
		else
		{
			charName = $"[<color=red>Invalid</color>({_record.charID})]";
			stateText = $"";
		}

		if (_character != null && Selection.activeGameObject == _character.gameObject)
		{
			charName = $"<color=yellow>{charName}</color>";
		}
		if(_character != null && _character.currentState == StateType.DEATH)
		{
			stateText = $"<color=red>{stateText}</color>";
		}

		GUILayout.BeginHorizontal();
		if (GUILayout.Button($"Select", GUILayout.MaxWidth(60), GUILayout.MinWidth(60)))
		{
			Selection.activeGameObject = _character.gameObject;
		}
		GUILayout.Label($"{charName} - {stateText} {hpText} {descText}", charNameStyle);

		if (_character != null && _character.target != null)
		{
			if (GUILayout.Button($"Target: {_character.target.info.charNameAndCharId}", buttonStyle, GUILayout.MinWidth(400), GUILayout.MaxWidth(400)))
			{
				Selection.activeGameObject = _character.target.gameObject;
			}
		}
		else
		{
			if (GUILayout.Button($"Target: Null", buttonStyle, GUILayout.MinWidth(400), GUILayout.MaxWidth(400)))
			{
			}
		}
		GUILayout.EndHorizontal();

		GUILayout.Label(_record.ToStringEditor());
	}
}
