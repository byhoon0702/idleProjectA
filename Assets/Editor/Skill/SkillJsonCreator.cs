using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;




/// <summary>
/// 게임 내 인스턴스 참조 금지
/// </summary>
public class SkillJsonCreator : EditorWindow
{
	public List<Int64> usingTids = new List<Int64>();
	public List<string> presetNames = new List<string>();


	/// <summary>
	/// {n} 기본값 + 레벨별 수치변화
	/// {g} 기본값 + 등급별 수치변화
	/// </summary>
	private Dictionary<string, string> presetDesc = new Dictionary<string, string>()
	{ 
		{ "Mirfiana_sk1Data", "체력이 가장 많은 적에게 공격력 1142%만큼 피해를 입히고 치명타 적중시 2초 동안 기절 상태로 만든다." },
		{ "SkillThrowData", "돌던지기" },
	};



	public Int64 selectedTid = 0;
	public string selectedPresetName = "";

	private Vector2 scrollPos;




	public static void ShowEditor()
	{
		SkillJsonCreator window = GetWindow<SkillJsonCreator>();
		window.titleContent = new GUIContent(window.ToString());
		window.RefreshAssetInfo();

		if (window.usingTids.Count > 0)
		{
			window.selectedTid = window.usingTids[window.usingTids.Count - 1] + 1;
		}
		window.Show();
	}

	private void OnGUI()
	{
		if(Application.isPlaying)
		{
			Close();
			return;
		}

		if(GUILayout.Button("Create Json"))
		{
			TrySave();
		}

		GUILayout.Space(10);

		selectedTid = EditorGUILayout.LongField("TID", selectedTid);
		selectedPresetName = EditorGUILayout.TextField("Preset", selectedPresetName);


		GUILayout.Space(10);
		GUILayout.Label("Preset List", "PreToolbar");
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		foreach (var presetName in presetNames)
		{
			if (presetName.ToLower().Contains(selectedPresetName.ToLower()) == false)
			{
				continue;
			}

			GUILayout.BeginHorizontal();
			if(GUILayout.Button(presetName, GUILayout.MaxWidth(150), GUILayout.MinWidth(150)))
			{
				selectedPresetName = presetName; 
				EditorGUI.FocusTextInControl("");
			}

			if (presetDesc.ContainsKey(presetName))
			{
				GUILayout.Label(presetDesc[presetName]);
			}
			else
			{
				GUILayout.Label("설명없음");
			}

			GUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();
	}

	public void RefreshAssetInfo()
	{
		usingTids.Clear();
		presetNames.Clear();

		var jsonDirectoryInfo = new System.IO.DirectoryInfo(SkillMeta.jsonFilePath);
		foreach (System.IO.FileInfo File in jsonDirectoryInfo.GetFiles())
		{
			if(File.Extension.ToLower().CompareTo(".json") == 0)
			{
				if (Int64.TryParse(File.Name.Replace(".json", ""), out selectedTid))
				{
					usingTids.Add(selectedTid);
				}
			}
		}


		var presetDirectoryInfo = new System.IO.DirectoryInfo(SkillMeta.presetClassPath);
		foreach (System.IO.FileInfo File in presetDirectoryInfo.GetFiles())
		{
			if (File.Extension.ToLower().CompareTo(".cs") == 0)
			{
				presetNames.Add(File.Name.Substring(0, File.Name.Length - 3));
			}
		}
	}

	public void TrySave()
	{
		// TID 유효성 검사
		if(selectedTid == 0)
		{
			Debug.LogError("TID가 유효하지 않음(0임)");
			return;
		}
		if(usingTids.Contains(selectedTid))
		{
			Debug.LogError("TID가 유효하지 않음(파일이 이미 있음)");
			return;
		}

		// 프리셋 유효성 검사
		if(string.IsNullOrEmpty(selectedPresetName))
		{
			Debug.LogError("스킬 프리셋이 비어있음");
			return;
		}
		if (presetNames.Contains(selectedPresetName) == false)
		{
			Debug.LogError("스킬 프리셋 이름이 잘못됨");
			return;
		}

		string json;
		try
		{
			var classObject = ScriptableObject.CreateInstance<SkillBaseData>();
			classObject.tid = selectedTid;
			classObject.skillPreset = selectedPresetName;
			classObject.skillName = selectedPresetName;
			if (presetDesc.ContainsKey(selectedPresetName))
			{
				classObject.description = presetDesc[selectedPresetName];
			}

			json = JsonUtility.ToJson(classObject);
		}
		catch (Exception e)
		{
			Debug.LogError($"Json 파일 생성실패\n{e}");
			return;
		}


		System.IO.File.WriteAllText(SkillMeta.jsonFilePath + selectedTid + ".json", json);
		AssetDatabase.Refresh();
		RefreshAssetInfo();
	}
}
