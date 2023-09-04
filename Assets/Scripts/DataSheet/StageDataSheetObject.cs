//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class StageDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public StageDataSheet dataSheet;


	public void StageNumberGenerate()
	{
		if (dataSheet.infos.Count == 0)
		{
			return;
		}
		int stageNumber = 1;
		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var info = dataSheet.infos[i];
			for (int ii = 0; ii < info.stageListData.Length; ii++)
			{
				info.stageListData[ii].stageNumber = stageNumber;
				stageNumber++;
			}
		}
	}

	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var firstData = dataSheet.infos[i];
			string typename = firstData.stageType.ToString().ToLower().FirstCharacterToUpper();

			string path = $"Assets/Resources/RuntimeDatas/Maps/{typename}s";

			if (AssetDatabase.IsValidFolder(path) == false)
			{
				AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/Maps", $"{typename}s");
			}

			RenameAsset<StageMapObject>(path, typename);

			var data = dataSheet.infos[i];
			string name = $"{typename}_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (StageMapObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(StageMapObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<StageMapObject>();
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
