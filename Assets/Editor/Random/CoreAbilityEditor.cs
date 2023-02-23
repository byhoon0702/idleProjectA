using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;




public class CoreAbilityEditor : EditorWindow
{
	public Int32 simulateCount = 100;
	public Grade grade;
	public Stats abilityTyle;
	private Vector2 scrollPos;


	[MenuItem("Custom Menu/Random/Core Ability")]
	public static void ShowEditor()
	{
		CoreAbilityEditor window = GetWindow<CoreAbilityEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			return;
		}

		if (VGameManager.it.currentState <= GameState.LOADING)
		{
			return;
		}
		var gradeList = DataManager.Get<UserGradeDataSheet>().GetGradeList();
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
				Grade g = UserInfo.coreAbil.abilityGenerator.RandomGrade();
				result[g]++;
			}

			string text = "";

			foreach (Grade v in gradeList)
			{
				text += $"{v}: {result[v]}({((double)result[v] / simulateCount).ToString("F3")}), ";
			}

			Debug.Log($"등급 결과. ({simulateCount}회) \n{text}");
		}


		var abilitySheet = DataManager.Get<CoreAbilityDataSheet>();

		if (GUILayout.Button("Ability Test"))
		{
			Dictionary<Stats, int> result = new Dictionary<Stats, int>();

			foreach (Stats v in abilitySheet.GetAbilityTypes())
			{
				result.Add(v, 0);
			}

			for (int i = 0 ; i < simulateCount ; i++)
			{
				Stats g = UserInfo.coreAbil.abilityGenerator.RandomAbility();
				result[g]++;
			}

			string text = "";

			foreach (Stats v in abilitySheet.GetAbilityTypes())
			{
				text += $"{v}: {result[v]}({((double)result[v] / simulateCount).ToString("F6")}), ";
			}

			Debug.Log($"어빌리티 결과.({simulateCount}회) \n{text}");
		}


		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		abilityTyle = (Stats)EditorGUILayout.EnumPopup(abilityTyle);
		grade = (Grade)EditorGUILayout.EnumPopup(grade);
		GUILayout.EndHorizontal();
		if (GUILayout.Button("랜덤 수치 계산"))
		{
			Dictionary<string, Int32> list = new Dictionary<string, Int32>();

			for (Int32 i = 0 ; i < simulateCount ; i++)
			{
				UserInfo.coreAbil.abilityGenerator.GetAbilityValueRange(grade, abilityTyle, out var min, out var max);
				IdleNumber result = UserInfo.coreAbil.abilityGenerator.RandomAbilityValue(min, max);

				string key = result.GetValue().ToString("F4");
				if (list.ContainsKey(key) == false)
				{
					list.Add(key, 0);
				}

				list[key]++;
			}


			var keyList = list.Keys.ToArray<string>().ToList();
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

		foreach (Stats v in abilitySheet.GetAbilityTypes())
		{
			foreach (Grade grade in gradeList)
			{
				UserInfo.coreAbil.abilityGenerator.GetAbilityValueRange(grade, v, out var min, out var max);

				GUILayout.Label($"[{grade}] abil: {v}, min: {min.ToString()}, max: {max.ToString()}");
			}
		}
		EditorGUILayout.EndScrollView();

	}
}
