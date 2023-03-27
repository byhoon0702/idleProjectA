using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StageInfoViewer : EditorWindow
{
	public WaveType waveType;
	public Vector2 scrollPos;
	private string tidFilter;
	private int areaIndex = -1;

	private GUIStyle LabelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;
			style.wordWrap = true;

			return style;
		}
	}

	[MenuItem("Custom Menu/Stage Viewer")]
	public static void ShowEditor()
	{
		StageInfoViewer window = EditorWindow.GetWindow<StageInfoViewer>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}


	private void OnGUI()
	{
		tidFilter = EditorGUILayout.TextField("Filter", tidFilter);
		if (StageManager.it == null)
		{
			GUILayout.Label("No initialized StageManager");
			return;
		}

		if(StageManager.it.metaGameStage == null)
		{
			GUILayout.Label("No initialized StageManager(metaGameStage)");
			return;
		}

		waveType = (WaveType)EditorGUILayout.EnumPopup("Type", waveType);
		areaIndex = EditorGUILayout.IntField("Area(-1은 전체)", areaIndex);
		if(StageManager.it.metaGameStage.stages.ContainsKey(waveType) == false)
		{
			GUILayout.Label("Stage count is zero");
			return;
		}


		scrollPos = GUILayout.BeginScrollView(scrollPos);
		foreach (var stage in StageManager.it.metaGameStage.stages[waveType])
		{
			if(areaIndex != -1 && stage.AreaIndex != areaIndex)
			{
				continue;
			}

			if (tidFilter.HasStringValue())
			{
				string[] splitText = tidFilter.Split(',');
				bool found = true;

				foreach (var v in splitText)
				{
					if (v.HasStringValue() == false)
					{
						continue;
					}
					if (stage.stageInfo.tid.ToString().Contains(v.ToString()))
					{
						found = false;
						break;
					}
				}

				if (found)
				{
					continue;
				}
			}

			GUILayout.Label(stage.ToString(), "PreToolbar");
			GUILayout.Label("--- 적 등장정보 ---");
			GUILayout.Label(stage.ToStringSpawnEnemyInfos(), LabelStyle);
			GUILayout.Label("--- 보상정보 ---");
			GUILayout.Label(stage.ToStringRewards(), LabelStyle);
			GUILayout.Space(10);
		}
		GUILayout.EndScrollView();
	}
}
