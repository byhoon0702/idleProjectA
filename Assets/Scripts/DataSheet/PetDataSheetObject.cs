//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class PetDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public PetDataSheet dataSheet;
	public override void Call()
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/Pets";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"Pets");
		}

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"{data.tid}_{data.name}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (PetItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(PetItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<PetItemObject>();
				AssetDatabase.CreateAsset(scriptable, assetPath);
			}

			scriptable.SetBasicData(data.tid, data.name, data.description, data.itemGrade, data.starlevel);


			EditorUtility.SetDirty(scriptable);
			AssetDatabase.SaveAssetIfDirty(scriptable);
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
#endif

	}
}
