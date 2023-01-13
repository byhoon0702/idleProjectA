using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class FourArithmeticTester : EditorWindow
{
	/// <summary>
	/// 스킬레벨 표시
	/// </summary>
	public const Int32 skillLvCount = 50;
	public List<string> calculateResult = new List<string>();


	public string fourArithmeticValue;
	public float defaultValue;

	public Vector2 scrollPos;


	private GUIStyle labelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;

			return style;
		}
	}



	[MenuItem("Custom Menu/사칙연산 테스트")]
	public static void ShowEditor()
	{
		FourArithmeticTester window = EditorWindow.GetWindow<FourArithmeticTester>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	public static void ShowEditor(string _fourArithmeticValue)
	{
		FourArithmeticTester window = EditorWindow.GetWindow<FourArithmeticTester>();
		window.titleContent = new GUIContent(window.ToString());
		window.fourArithmeticValue = _fourArithmeticValue;
		window.Show();
	}

	private void OnGUI()
	{
		GUILayout.Label("데이터", "PreToolbar");
		GUILayout.Label("+, -, *, /, (, ) 사용가능");
		fourArithmeticValue = EditorGUILayout.TextField("사칙연산(띄어쓰기로 구분)", fourArithmeticValue);
		defaultValue = EditorGUILayout.FloatField($"데이터 기본값({FourArithmeticCalculator.DEFAULT_VALUE})", defaultValue);
		EditorGUILayout.LabelField($"스킬레벨({FourArithmeticCalculator.SKILL_LEVEL})");

		if (GUILayout.Button("Calculate"))
		{
			Calculate();
		}


		GUILayout.Label("연산결과", "PreToolbar");
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		foreach(var result in calculateResult)
		{
			GUILayout.Label(result, labelStyle);
		}
		EditorGUILayout.EndScrollView();
	}

	private void Calculate()
	{
		calculateResult.Clear();
		try
		{
			for (Int32 i = 1 ; i <= skillLvCount ; i++)
			{
				if (string.IsNullOrEmpty(fourArithmeticValue) == false)
				{
					string reservedWord = FourArithmeticCalculator.ReplaceReservedWord(fourArithmeticValue, defaultValue, i);
					double result = FourArithmeticCalculator.CalculateFourArithmetic(reservedWord);

					calculateResult.Add($"[LV {i}] <color=yellow>{result}</color> / 수식: {reservedWord}");
				}
				else
				{
					double result = FourArithmeticCalculator.Calculate(fourArithmeticValue, defaultValue, i);
					calculateResult.Add($"[LV {i}] <color=yellow>{result}</color>");
				}
			}
		}
		catch (Exception e)
		{
			calculateResult.Add($"계산 오류..\n{e.ToString()}");
			return;
		}
	}
}
