using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;




public class GachaEditor : EditorWindow
{
	public int gachaLevel = 1;
	public GachaType gachaType;
	public Int32 simulateCount = 100;
	private Vector2 scrollPos;

	public string simulateResult;
	public string gradeResult;
	public string itemResult;

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





	[MenuItem("Custom Menu/Random/Gacha")]
	public static void ShowEditor()
	{
		GachaEditor window = GetWindow<GachaEditor>();
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

		GUILayout.BeginHorizontal();
		gachaType = (GachaType)EditorGUILayout.EnumPopup(gachaType);
		gachaLevel = EditorGUILayout.IntField("Lv", gachaLevel);
		simulateCount = EditorGUILayout.IntField("Count", simulateCount);
		GUILayout.EndHorizontal();

		ShowSummonTestMenu();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		GUILayout.Label("시뮬레이트 정보", "PreToolbar");
		GUILayout.Label(simulateResult, labelStyle);
		GUILayout.Label("등급결과", "PreToolbar");
		GUILayout.Label(gradeResult, labelStyle);
		GUILayout.Label("아이템결과", "PreToolbar");
		GUILayout.Label(itemResult, labelStyle);
		EditorGUILayout.EndScrollView();
	}

	public void ShowSummonTestMenu()
	{
		if (GUILayout.Button("Summom Test"))
		{
			List<GachaResult> items = new List<GachaResult>();


			UIGachaData gachaData = new UIGachaData();
			var vResult = gachaData.Setup(DataManager.Get<GachaDataSheet>().GetByType(gachaType));
			if (vResult.Fail())
			{
				Debug.LogError(vResult.ToString());
				return;
			}

			for (int i = 0 ; i < simulateCount ; i++)
			{
				GachaResult item = null;

				switch (gachaType)
				{
					case GachaType.Equip:
						item = GachaGenerator.GenerateEquip(gachaLevel, gachaData.Probabilities);
						break;
					case GachaType.Skill:
						item = GachaGenerator.GenerateSkill(gachaLevel, gachaData.Probabilities);
						break;
					case GachaType.Pet:
						item = GachaGenerator.GeneratePet(gachaLevel, gachaData.Probabilities);
						break;
				}

				bool found = false;
				foreach (var v in items)
				{
					if (v.itemTid == item.itemTid)
					{
						v.itemCount += 1;
						found = true;
						break;
					}
				}

				if (found == false)
				{
					items.Add(item);
				}
			}
			simulateResult = $"레벨: {gachaLevel} / 타입: {gachaType} / 횟수: {simulateCount}";

			itemResult = "";
			Dictionary<Grade, Int32> grades = new Dictionary<Grade, int>();
			items.Sort((a, b) =>
			{
				return a.itemTid.CompareTo(b.itemTid);
			});
			foreach (var v in items)
			{
				var data = DataManager.Get<ItemDataSheet>().Get(v.itemTid);
				itemResult += $"[{data.itemType}][{data.itemGrade} - {v.itemCount.GetValue() / (double)simulateCount}] {data.name}({v.itemTid}) count: {v.itemCount.ToString()}\n";


				if (grades.ContainsKey(data.itemGrade) == false)
				{
					grades.Add(data.itemGrade, 0);
				}

				grades[data.itemGrade] += v.itemCount.GetValueToInt();
			}


			gradeResult = "";
			foreach (var v in grades)
			{
				gradeResult += $"[{v.Key} - {((double)v.Value / simulateCount).ToString("F6")}] count : {v.Value}\n";
			}
		}
	}
}
