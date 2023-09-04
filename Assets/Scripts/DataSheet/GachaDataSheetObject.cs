//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
[Serializable]
public class GachaDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public GachaDataSheet dataSheet;

	public override void Call(string fileName)
	{
#if UNITY_EDITOR
		if (dataSheet.infos.Count == 0)
		{
			return;
		}

		GachaData firstData = dataSheet.infos[0];
		string path = $"Assets/Resources/RuntimeDatas/Gachas";

		if (UnityEditor.AssetDatabase.IsValidFolder(path) == false)
		{
			UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "Gachas");
		}
		RenameAsset<EquipItemObject>(path, "Gachas");

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];

			string name = $"Gacha_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (GachaObject)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(GachaObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<GachaObject>();
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
