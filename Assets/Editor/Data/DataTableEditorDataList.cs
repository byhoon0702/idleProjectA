using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Text;

using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;

public struct DataListInfo
{
	public string name;
	public string path;
	public object data;

	public bool IsNameMatch(string _name)
	{
		return name == _name;
	}
}

public partial class DataTableEditor
{
	private Vector2 listScrollPos;
	private Vector2 searchResultScrollPos;
	private long searchTid;
	private List<string> filePaths;

	private Dictionary<string, DataListInfo> datalistforDataListPage = new Dictionary<string, DataListInfo>();
	private Dictionary<string, DataListInfo> tidIncludedList = new Dictionary<string, DataListInfo>();
	public void LoadFileList(string path)
	{
		scriptableObject = null;

		LoadAllDataPath(path);
		//LoadAllData(path);
	}

	private void DrawDataList()
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

		EditorGUILayout.Space(3);
		EditorGUILayout.HelpBox("Json 또는 Csv 파일을 불러오세요", MessageType.Warning);

		EditorGUILayout.BeginHorizontal();

		listScrollPos = EditorGUILayout.BeginScrollView(listScrollPos, GUILayout.MaxWidth(400));

		if (filePaths != null)
		{
			DrawDataListElement(datalistforDataListPage);
		}

		EditorGUILayout.EndScrollView();
		if (datalistforDataListPage != null && datalistforDataListPage.Count > 0)
		{
			EditorGUILayout.Space(3, false);
			EditorGUILayout.BeginVertical("window", GUILayout.MaxWidth(400));
			searchTid = EditorGUILayout.LongField("찾을 TID", searchTid);
			if (GUILayout.Button("검색"))
			{
				SearchTid();
			}

			EditorGUILayout.Space(1);
			if (tidIncludedList != null && tidIncludedList.Count > 0)
			{
				EditorGUILayout.LabelField("검색 결과");
				searchResultScrollPos = EditorGUILayout.BeginScrollView(searchResultScrollPos);
				DrawDataListElement(tidIncludedList);
				EditorGUILayout.EndScrollView();
			}

			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.EndHorizontal();
	}

	private void SearchTid()
	{
		foreach (var data in datalistforDataListPage)
		{
			var datalist = data.Value;
			System.Type type = datalist.data.GetType();
			FieldInfo info = type.GetField("infos");
			IList list = (IList)info.GetValue(datalist.data);

			System.Type dataType = list[0].GetType();
			for (int i = 0; i < list.Count; i++)
			{
				long tid = (long)dataType.GetField("tid").GetValue(list[i]);

				if (tid == searchTid)
				{
					if (tidIncludedList.ContainsKey(data.Key))
					{
						tidIncludedList[data.Key] = new DataListInfo() { name = data.Key, path = datalist.path, data = datalist.data };
					}
					else
					{
						tidIncludedList.Add(data.Key, new DataListInfo() { name = data.Key, path = datalist.path, data = datalist.data });
					}
					break;
				}
			}
		}
	}

	private void DrawDataListElement(Dictionary<string, DataListInfo> elementDic)
	{

		var list = elementDic.Values.ToList();

		list.Sort((x, y) =>
		{
			System.Type xt = x.data.GetType();
			System.Type yt = y.data.GetType();

			FieldInfo fieldinfoX = xt.GetField("prefixID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo fieldinfoY = yt.GetField("prefixID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			long prefixX = 0;
			long prefixY = 0;
			if (fieldinfoX != null)
			{
				prefixX = (long)fieldinfoX.GetValue(x.data);
			}
			if (fieldinfoY != null)
			{
				prefixY = (long)fieldinfoY.GetValue(y.data);
			}

			return prefixX.CompareTo(prefixY);
		});

		foreach (var datalist in list)
		{
			System.Type t = datalist.data.GetType();

			FieldInfo fieldinfo = t.GetField("prefixID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			long prefixID = 0;
			if (fieldinfo != null)
			{
				prefixID = (long)fieldinfo.GetValue(datalist.data);
				if (prefixID < minTidPrefix)
				{
					prefixID = minTidPrefix;
					fieldinfo.SetValue(datalist.data, prefixID);
				}
			}

			GUIContent guiContent = new GUIContent();
			guiContent.text = datalist.name;
			GUILayout.BeginVertical(datalist.name, "window");

			GUILayout.BeginHorizontal();
			EditorGUI.indentLevel++;
			EditorGUI.indentLevel++;

			GUILayout.Label($"프리픽스ID : {prefixID}");
			if (GUILayout.Button("불러오기", GUILayout.Width(130)))
			{
				string path = datalist.path;
				currentJsonFilePath = path;
				if (path.Contains(".csv"))
				{
					FromCSV(false);
				}
				else
				{
					FromJson(false);
				}
				pageIndex = 1;
			}

			EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
	}
}
