using UnityEngine;
using System;
using System.Data;
using Unity.VisualScripting;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class ShopDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public ShopDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR
		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];

			string typename = data.shopType.ToString().ToLower().FirstCharacterToUpper();
			string path = $"Assets/Resources/RuntimeDatas/ShopItems/{typename}s";

			if (AssetDatabase.IsValidFolder(path) == false)
			{
				AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/ShopItems", $"{typename}s");
			}
			RenameAsset<ShopItemObject>(path, typename);
			string name = $"{typename}_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (ShopItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ShopItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<ShopItemObject>();
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
