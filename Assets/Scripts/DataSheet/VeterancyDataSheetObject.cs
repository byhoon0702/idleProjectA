//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class VeterancyDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public VeterancyDataSheet dataSheet;


	public override void Call(string fileName)
	{
#if UNITY_EDITOR


		if (dataSheet.infos.Count == 0)
		{
			return;
		}

		string path = $"Assets/Resources/RuntimeDatas/Veterancy";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "Veterancy");
		}

		RenameAsset<VeterancyObject>(path, "Veterancy");

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"Veterancy_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (VeterancyObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(VeterancyObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<VeterancyObject>();
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
