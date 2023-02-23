using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Unit), true)]
public class UnitInfoEditor : Editor
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
			if (unit.ControlSide == ControlSide.PLAYER)
			{
				attackerType = AttackerType.Enemy;
			}
			else
			{
				attackerType = AttackerType.Player;
			}
			HitInfo hitInfo = new HitInfo(attackerType, unit.MaxHp * 10);
			unit.Hit(hitInfo);
		}
		if (GUILayout.Button("Extension"))
		{
			UnitInfoEditorExtension.ShowEditor(unit);
		}

		try
		{
			EditorGUILayout.Space();
			UnitInfo info = null;
			if (unit is PlayerUnit)
			{
				info = (unit as PlayerUnit).info;
			}
			else if (unit is EnemyUnit)
			{
				info = (unit as EnemyUnit).info;
			}
			if (info != null)
			{
				UnitEditorUtility.ShowUnitInfo(info, unit.Hp, unit.MaxHp);
				EditorGUILayout.Space();
			}
			UnitEditorUtility.ShowConditionList(unit.conditionModule);
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}
}
