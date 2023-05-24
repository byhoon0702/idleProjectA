//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class DungeonStageDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public DungeonStageDataSheet dataSheet;

	public override void Call()
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

			var data = dataSheet.infos[i];
			string name = $"{data.tid}_{data.name}";
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
