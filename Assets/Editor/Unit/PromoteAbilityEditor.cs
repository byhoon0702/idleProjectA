using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;




public class PromoteAbilityEditor : EditorWindow
{
	public Int32 simulateCount = 100;
	public Grade grade;
	public AbilityType abilityTyle;
	private Vector2 scrollPos;


	[MenuItem("Custom Menu/Promote Ability Test")]
	public static void ShowEditor()
	{
		PromoteAbilityEditor window = GetWindow<PromoteAbilityEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			return;
		}

		if(VGameManager.it.currentState <= GameState.LOADING)
		{
			return;
		}
		var gradeList = DataManager.it.Get<HyperModeDataSheet>().GetGradeList();
		simulateCount = EditorGUILayout.IntField("Count", simulateCount);
		if (GUILayout.Button("Grade Test"))
		{
			Dictionary<Grade, Int32> result = new Dictionary<Grade, int>();
			foreach (Grade v in gradeList)
			{
				result.Add(v, 0);
			}

			for (Int32 i = 0 ; i < simulateCount ; i++)
			{
				Grade g = UserInfo.proAbil.RandomGrade();
				result[g]++;
			}

			string text = "";

			foreach(Grade v in gradeList)
			{
				text += $"{v}: {result[v]}({((float)result[v] / simulateCount).ToString("F3")}), ";
			}

			Debug.Log($"등급 결과. ({simulateCount}회) \n{text}");
		}


		var abilitySheet = DataManager.it.Get<UserPromoteAbilityDataSheet>();

		if (GUILayout.Button("Ability Test"))
		{
			Dictionary<AbilityType, Int32> result = new Dictionary<AbilityType, int>();

			foreach (AbilityType v in abilitySheet.GetAbilityTypes())
			{
				result.Add(v, 0);
			}

			for (Int32 i = 0 ; i < simulateCount ; i++)
			{
				AbilityType g = UserInfo.proAbil.RandomAbility();
				result[g]++;
			}

			string text = "";

			foreach (AbilityType v in abilitySheet.GetAbilityTypes())
			{
				text += $"{v}: {result[v]}({((float)result[v] / simulateCount).ToString("F3")}), ";
			}

			Debug.Log($"어빌리티 결과.({simulateCount}회) \n{text}");
		}


		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		abilityTyle = (AbilityType)EditorGUILayout.EnumPopup(abilityTyle);
		grade = (Grade)EditorGUILayout.EnumPopup(grade);
		GUILayout.EndHorizontal();
		if (GUILayout.Button("랜덤 수치 계산"))
		{
			Dictionary<double, Int32> list = new Dictionary<double, Int32>();

			for (Int32 i = 0 ; i < simulateCount ; i++)
			{
				UserInfo.proAbil.GetAbilityValueRange(grade, abilityTyle, out var min, out var max);
				IdleNumber result = UserInfo.proAbil.RandomAbilityValue(min, max);

				if (list.ContainsKey(result.GetValue()) == false)
				{
					list.Add(result.GetValue(), 0);
				}

				list[result.GetValue()]++;
			}


			var keyList = list.Keys.ToArray<double>().ToList();
			keyList.Sort();

			string text = "";
			for (Int32 i = 0 ; i < keyList.Count ; i++)
			{
				var value = list[keyList[i]];

				text += $"[abil: {keyList[i]}, cnt: {value}], ";
			}

			Debug.Log($"수치 시물레이션({simulateCount}회)\n{text}");
		}


		GUILayout.Space(10);
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		foreach (AbilityType v in abilitySheet.GetAbilityTypes())
		{
			foreach (Grade grade in gradeList)
			{
				UserInfo.proAbil.GetAbilityValueRange(grade, v, out var min, out var max);

				GUILayout.Label($"[{grade}] abil: {v}, min: {min.ToString()}, max: {max.ToString()}");
			}
		}
		EditorGUILayout.EndScrollView();

	}
}
