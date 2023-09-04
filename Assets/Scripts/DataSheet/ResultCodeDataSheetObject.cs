//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;

[Serializable]
public class ResultCodeDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public ResultCodeDataSheet dataSheet;

	public override void Call(string fileName)
	{
		GenerateResultCode(dataSheet.infos);
	}
	public static void GenerateResultCode(List<ResultCodeData> _infos)
	{
#if UNITY_EDITOR
		string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, $"Scripts/System/VResultCode.cs");
		List<Int64> tidList = new List<Int64>();
		List<string> keyList = new List<string>();

		using (StreamWriter writer = new StreamWriter(path, false))
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"//========AUTO GENERATED CODE======//");
			sb.AppendLine($"public enum VResultCode ");
			sb.AppendLine("{");
			for (Int32 i = 0; i < _infos.Count; i++)
			{
				if (tidList.Contains(_infos[i].tid))
				{
					Debug.LogError($"TID 중복오류 tid: {_infos[i].tid}, {_infos[i].key}: {_infos[i].description}");
					continue;
				}
				tidList.Add(_infos[i].tid);

				if (keyList.Contains(_infos[i].key))
				{
					Debug.LogError($"Key 중복오류 tid: {_infos[i].tid}, {_infos[i].key}: {_infos[i].description}");
					continue;
				}
				keyList.Add(_infos[i].key);

				sb.AppendLine("\t/// <summary>");
				sb.AppendLine($"\t/// {_infos[i].description}");
				sb.AppendLine("\t/// </summary>");
				sb.AppendLine($"\t{_infos[i].key} = {_infos[i].tid},\n");
			}
			sb.AppendLine("}");
			writer.Write(sb.ToString());
		}

		UnityEditor.AssetDatabase.Refresh();
#endif
	}
}
