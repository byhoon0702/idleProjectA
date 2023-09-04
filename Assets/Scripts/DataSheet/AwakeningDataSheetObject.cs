//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class AwakeningDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public AwakeningDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/Awakening";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "Awakening");
		}
		RenameAsset<AwakeningItemObject>(path, "Awakening");

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"Awakening_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (AwakeningItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AwakeningItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<AwakeningItemObject>();
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
