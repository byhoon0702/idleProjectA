//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using Unity.VisualScripting;

#if UNITY_EDITOR
[Serializable]
public class StatusDataSheetObject : BaseDataSheetObject
{


	[SerializeField]
	public StatusDataSheet dataSheet;
	public override void Call(string fileName)
	{
		GenerateEnum();

	}

	private void GenerateEnum()
	{
		//string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, $"Scripts/Define/StatusDefine.cs");
		//using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
		//{
		//	using (StreamWriter writer = new StreamWriter(fs))
		//	{
		//		StringBuilder sb = new StringBuilder();
		//		StringBuilder subsb = new StringBuilder();
		//		sb.AppendLine($"//========AUTO GENERATED CODE======//");
		//		sb.AppendLine($"public enum Stats");
		//		sb.AppendLine("{");

		//		for (int i = 0; i < dataSheet.infos.Count; i++)
		//		{
		//			var info = dataSheet.infos[i];

		//			string[] split = info.name.Split('_');

		//			for (int ii = 0; ii < split.Length; ii++)
		//			{
		//				subsb.Append(split[ii].FirstCharacterToUpper());
		//			}
		//			sb.AppendLine("\t/// <summary>");
		//			sb.AppendLine($"\t/// {info.description}");
		//			sb.AppendLine("\t/// </summary>");
		//			sb.AppendLine($"\t{subsb} = {info.tid},");
		//			subsb.Clear();

		//		}

		//		sb.AppendLine("}");

		//		writer.Write(sb.ToString());
		//	}
		//}

		//AssetDatabase.Refresh();
	}
}
#endif
