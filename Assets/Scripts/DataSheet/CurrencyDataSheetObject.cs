//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class CurrencyDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public CurrencyDataSheet dataSheet;

	public override void Call(string fileName)
	{
#if UNITY_EDITOR
		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/CurrencyItems";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "CurrencyItems");
		}

		RenameAsset<CurrencyItemObject>(path, "Currency");
		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"Currency_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (CurrencyItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CurrencyItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<CurrencyItemObject>();
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
