using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct FieldSettings
{
	public string fieldName;
	public float width;

}
[Serializable]
public class BaseDataSheetObject : ScriptableObject
{
	public string tooltip;

	public int itemPerPage = 5;
	public List<FieldSettings> fieldSettings = new List<FieldSettings>();

	public virtual void Call(string fileName)
	{

	}

#if UNITY_EDITOR
	public void RenameAsset<T>(string folder_path, string header) where T : ItemObject
	{
		var list = UnityEditor.AssetDatabase.FindAssets("t:scriptableObject", new string[] { folder_path });

		for (int i = 0; i < list.Length; i++)
		{

			string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(list[i]);

			T temp = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
			string name = $"{header}_{temp.Tid}";
			if (temp.name.Equals(name) == false)
			{
				UnityEditor.AssetDatabase.RenameAsset(assetPath, name);
			}
		}
		UnityEditor.AssetDatabase.Refresh();
	}

	public void RenameSkillAsset<T>(string folder_path, string header) where T : SkillCore
	{
		var list = UnityEditor.AssetDatabase.FindAssets("t:scriptableObject", new string[] { folder_path });

		for (int i = 0; i < list.Length; i++)
		{

			string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(list[i]);

			T temp = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
			string name = $"Skill_{temp.Tid}_{temp.Description}";
			if (temp.name.Equals(name) == false)
			{
				UnityEditor.AssetDatabase.RenameAsset(assetPath, name);
			}
		}
		UnityEditor.AssetDatabase.Refresh();
	}

	public virtual void MakeScriptableObject<T1, T2>(List<T1> dataList, string path, string header) where T1 : BaseData where T2 : ItemObject
	{
		for (int i = 0; i < dataList.Count; i++)
		{
			T1 data = dataList[i];
			string name = $"{header}_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (T2)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T2));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<T2>();
				UnityEditor.AssetDatabase.CreateAsset(scriptable, assetPath);
			}
			scriptable.SetBasicData(data);
			UnityEditor.EditorUtility.SetDirty(scriptable);
			UnityEditor.AssetDatabase.SaveAssetIfDirty(scriptable);
		}
	}


#endif
}
