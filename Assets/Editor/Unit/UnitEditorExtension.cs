using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitEditorExtension : EditorWindow
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
		UnitEditorExtension window = ScriptableObject.CreateInstance<UnitEditorExtension>();
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
			if(unitBase is Unit)
			{
				unitInfo = (unitBase as Unit).info;
				hp = (unitBase as Unit).info.hp;
				maxHp = (unitBase as Unit).info.rawHp;
			}
			else if(unitBase is Companion)
			{
				unitInfo = (unitBase as Companion).info;
			}
			if (unitInfo != null)
			{
				UnitEditorUtility.ShowCharacterInfo(unitInfo, hp, maxHp);
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
