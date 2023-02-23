using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitInfoEditorExtension : EditorWindow
{
	private UnitBase unitBase;


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

	public static void ShowEditor(UnitBase _target)
	{
		UnitInfoEditorExtension window = ScriptableObject.CreateInstance<UnitInfoEditorExtension>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();

		window.unitBase = _target;
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			GUILayout.Label("플레이중이 아님");
			return;
		}

		unitBase = (UnitBase)EditorGUILayout.ObjectField(unitBase, typeof(UnitBase), true);
		EditorGUILayout.Space();


		try
		{
			// 컨디션 정보표시
			if (unitBase is Unit)
			{
				UnitEditorUtility.ShowConditionTest(unitBase, (unitBase as Unit).conditionModule);
				EditorGUILayout.Space();
			}

			// 유닛 정보 표시
			UnitInfo unitInfo = null;
			IdleNumber hp = new IdleNumber();
			IdleNumber maxHp = new IdleNumber();
			if(unitBase is PlayerUnit)
			{
				unitInfo = (unitBase as PlayerUnit).info;
				hp = (unitBase as Unit).Hp;
				maxHp = (unitBase as Unit).MaxHp;
			}
			else if (unitBase is EnemyUnit)
			{
				unitInfo = (unitBase as EnemyUnit).info;
				hp = (unitBase as Unit).Hp;
				maxHp = (unitBase as Unit).MaxHp;
			}
			else if(unitBase is Pet)
			{
				unitInfo = (unitBase as Pet).info;
			}
			if (unitInfo != null)
			{
				UnitEditorUtility.ShowUnitInfo(unitInfo, hp, maxHp);
				EditorGUILayout.Space();
			}

			// 하이퍼 모드 표시
			UnitEditorUtility.ShowHyperInfo(unitBase, UnitGlobal.it.hyperModule);
			EditorGUILayout.Space();


			// 상태적용 테스트
			if (unitBase is Unit)
			{
				UnitEditorUtility.ShowConditionList((unitBase as Unit).conditionModule);
				EditorGUILayout.Space();
			}
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}

	private void Update()
	{
		Repaint();
	}
}
