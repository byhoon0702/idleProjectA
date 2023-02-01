using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Unit), true)]
public class UnitEditor : Editor
{
	private Unit unit;


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
	private void OnEnable()
	{
		unit = target as Unit;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying == false)
		{
			return;
		}

		EditorGUILayout.Space();
		if (GUILayout.Button("Death"))
		{
			AttackerType attackerType;
			if(unit.controlSide == ControlSide.PLAYER)
			{
				attackerType = AttackerType.Enemy;
			}
			else
			{
				attackerType = AttackerType.Player;
			}
			HitInfo hitInfo = new HitInfo(attackerType, AttackType.MELEE, unit.info.rawHp * 10);
			unit.Hit(hitInfo);
		}
		if (GUILayout.Button("Extension"))
		{
			UnitEditorExtension.ShowEditor(unit);
		}

		try
		{
			EditorGUILayout.Space();
			UnitEditorUtility.ShowCharacterInfo(unit.info, unit.info.hp, unit.info.rawHp);
			EditorGUILayout.Space();
			UnitEditorUtility.ShowConditionList(unit.conditionModule);
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}
}
