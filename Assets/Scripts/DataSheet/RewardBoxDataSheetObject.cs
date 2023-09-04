//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class RewardBoxDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public RewardBoxDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}



		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/RewardBoxs/";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "RewardBoxs");
		}

		RenameAsset<RewardBoxItemObject>(path, "RewardBox");
		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"RewardBox_{data.tid}_{data.description}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (RewardBoxItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(RewardBoxItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<RewardBoxItemObject>();
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
