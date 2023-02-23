using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public partial class DataTableEditor
{
	private Vector2 tidListScrollPos;
	private string tidListFilter;


	private void DrawTidList()
	{
		EditorGUILayout.Space(3);
		if (GUILayout.Button("Load Json List"))
		{
			LoadFileList("Json");
		}
		if (GUILayout.Button("Load CSV List"))
		{
			LoadFileList("Csv");
		}

		GUILayout.Space(10);
		tidListFilter = EditorGUILayout.TextField("Filter", tidListFilter);

		if (filePaths != null)
		{
			tidListScrollPos = EditorGUILayout.BeginScrollView(tidListScrollPos);
			foreach (var datalist in datalistforDataListPage)
			{
				Type infosType = datalist.Value.data.GetType();
				FieldInfo infosFieldinfo = infosType.GetField("infos", BindingFlags.Public | BindingFlags.Instance);

				var infosValue = (IList)infosFieldinfo.GetValue(datalist.Value.data);
				bool showdTitle = false;
				foreach (var v in infosValue)
				{
					Type baseDataType = v.GetType();
					FieldInfo tidFieldInfo = baseDataType.GetField("tid", BindingFlags.Public | BindingFlags.Instance);
					FieldInfo descFieldInfo = baseDataType.GetField("description", BindingFlags.Public | BindingFlags.Instance);
					var tidValue = tidFieldInfo.GetValue(v);
					var descValue = descFieldInfo.GetValue(v);

					string tidValueToString = tidValue.ToString();

					if (tidValueToString.Length >= 10)
					{
						tidValueToString = tidValueToString.Insert(3, " ");
						tidValueToString = tidValueToString.Insert(6, " ");
					}

					string text = $"{tidValueToString} : {descValue}";

					if (tidListFilter.HasStringValue())
					{
						if (text.ToLower().Contains(tidListFilter.ToLower()) == false)
						{
							continue;
						}
					}

					if (showdTitle == false)
					{
						showdTitle = true;
						GUILayout.Label($"{infosType.ToString()}", "PreToolbar");
					}

					GUILayout.Label(text);
				}
			}
			EditorGUILayout.EndScrollView();
		}

	}
}
