//========AUTO GENERATED CODE======//
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class RelicItemDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public RelicItemDataSheet dataSheet;

	public override void Call(string fileName)
	{
#if UNITY_EDITOR
		if (dataSheet.infos.Count == 0)
		{
			return;
		}




		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/RelicItems/";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "RelicItems");
		}

		RenameAsset<RelicItemObject>(path, "RelicItem");
		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"RelicItem_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (RelicItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(RelicItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<RelicItemObject>();
				AssetDatabase.CreateAsset(scriptable, assetPath);
			}
			scriptable.SetBasicData(data);

			EditorUtility.SetDirty(scriptable);
			AssetDatabase.SaveAssetIfDirty(scriptable);

		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();



#endif
	}
}
