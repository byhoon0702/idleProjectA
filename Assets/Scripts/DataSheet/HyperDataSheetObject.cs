//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class HyperDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public HyperDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}

		var firstData = dataSheet.infos[0];

		string path = $"Assets/Resources/RuntimeDatas/HyperClass";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "HyperClass");
		}
		RenameAsset<HyperClassObject>(path, "HyperClass");
		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"HyperClass_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (HyperClassObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(HyperClassObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<HyperClassObject>();
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
