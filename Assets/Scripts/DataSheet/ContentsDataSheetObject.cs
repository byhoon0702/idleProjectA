//========AUTO GENERATED CODE======//
using UnityEngine;
using System;

[Serializable]
public class ContentsDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public ContentsDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}

		var firstData = dataSheet.infos[0];
		string path = $"Assets/Resources/RuntimeDatas/Contents";

		if (UnityEditor.AssetDatabase.IsValidFolder(path) == false)
		{
			UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "Contents");
		}
		RenameAsset<ContentItemObject>(path, "Content");

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"Content_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (ContentItemObject)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(ContentItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<ContentItemObject>();
				UnityEditor.AssetDatabase.CreateAsset(scriptable, assetPath);
			}
			scriptable.SetBasicData(data);
			UnityEditor.EditorUtility.SetDirty(scriptable);
			UnityEditor.AssetDatabase.SaveAssetIfDirty(scriptable);

		}
		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();



#endif
	}
}
