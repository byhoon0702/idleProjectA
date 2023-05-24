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
	private bool tidListFilterAndOperator;
	private string tidListHighlight;
	private string tidListSheet;
	private bool tidListSheetOnly;
	private bool tidListShowDetail;
	private GUIStyle tidListLabelStyle
	{
		get 
		{
			var style = new GUIStyle("Label");
			style.richText = true;

			return style;
		}
	}


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
		tidListShowDetail = EditorGUILayout.Toggle("Show Detail", tidListShowDetail);
		GUILayout.BeginHorizontal();
		tidListFilter = EditorGUILayout.TextField("Filter", tidListFilter);
		tidListFilterAndOperator = GUILayout.Toggle(tidListFilterAndOperator, "AND", GUILayout.MaxWidth(100));
		GUILayout.EndHorizontal();

		tidListHighlight = EditorGUILayout.TextField("Highlight", tidListHighlight);
		GUILayout.BeginHorizontal();
		tidListSheet = EditorGUILayout.TextField("Sheet", tidListSheet);
		tidListSheetOnly = GUILayout.Toggle(tidListSheetOnly, "ONLY", GUILayout.MaxWidth(100));
		GUILayout.EndHorizontal();

		if (filePaths != null)
		{
			tidListScrollPos = EditorGUILayout.BeginScrollView(tidListScrollPos);
			foreach (var datalist in datalistforDataListPage)
			{
				Type infosType = datalist.Value.data.GetType();
				FieldInfo infosFieldinfo = infosType.GetField("infos", BindingFlags.Public | BindingFlags.Instance);

				var infosValue = (IList)infosFieldinfo.GetValue(datalist.Value.data);
				bool showdTitle = false;
				foreach (var info in infosValue)
				{
					Type baseDataType = info.GetType();
					FieldInfo tidFieldInfo = baseDataType.GetField("tid", BindingFlags.Public | BindingFlags.Instance);
					FieldInfo descFieldInfo = baseDataType.GetField("description", BindingFlags.Public | BindingFlags.Instance);
					var tidValue = tidFieldInfo.GetValue(info);
					var descValue = descFieldInfo.GetValue(info);
					string tidValueToString = tidValue.ToString();

					if (tidValueToString.Length >= 10)
					{
						tidValueToString = tidValueToString.Insert(3, " ");
						tidValueToString = tidValueToString.Insert(6, " ");
					}




					string text = $"{tidValueToString} : {descValue}";

					if (tidListShowDetail)
					{
						foreach(var field in baseDataType.GetFields())
						{
							if(field.Name == "tid" || field.Name == "description")
							{
								continue;
							}

							text += $" : {field.Name}({field.GetValue(info)})";
						}
					}

					if (tidListFilter.HasStringValue())
					{
						string[] splitText = tidListFilter.Split(',');
						bool found = false;

						if (tidListFilterAndOperator)
						{
							foreach (var v in splitText)
							{
								if (v.HasStringValue() == false)
								{
									continue;
								}
								if (text.ToLower().Contains(v.ToLower()) == false)
								{
									found = true;
									break;
								}
							}
						}
						else
						{
							found = true;
							foreach (var v in splitText)
							{
								if (v.HasStringValue() == false)
								{
									continue;
								}
								if (text.ToLower().Contains(v.ToLower()))
								{
									found = false;
									break;
								}
							}
						}
						if (found)
						{
							continue;
						}
					}
					if(tidListHighlight.HasStringValue())
					{
						string[] splitText = tidListHighlight.Split(',');
						foreach (var v in splitText)
						{
							if (v.HasStringValue() == false)
							{
								continue;
							}
							text = text.Replace(v, $"<color=yellow>{v}</color>");
						}
					}
					if(tidListSheet.HasStringValue())
					{
						string[] splitText = tidListSheet.Split(',');
						bool found = false;

						if (tidListSheetOnly)
						{
							foreach (var v in splitText)
							{
								if (v.HasStringValue() == false)
								{
									continue;
								}
								if (infosType.ToString().ToLower().Contains(v.ToLower()) == false)
								{
									found = true;
									break;
								}
							}
						}
						else
						{
							foreach (var v in splitText)
							{
								if (v.HasStringValue() == false)
								{
									continue;
								}
								if (infosType.ToString().ToLower().Contains(v.ToLower()))
								{
									found = true;
									break;
								}
							}
						}

						if (found)
						{
							continue;
						}
					}

					if (showdTitle == false)
					{
						showdTitle = true;
						GUILayout.Label($"{infosType.ToString()}", "PreToolbar");
					}

					GUILayout.BeginHorizontal();
					if(GUILayout.Button("TID COPY", GUILayout.MaxWidth(70)))
					{
						GUIUtility.systemCopyBuffer = tidValue.ToString();
					}
					if (GUILayout.Button("FULL COPY", GUILayout.MaxWidth(80)))
					{
						GUIUtility.systemCopyBuffer = text;
					}
					GUILayout.Label(text, tidListLabelStyle);
					GUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndScrollView();
		}

	}
}
