//========AUTO GENERATED CODE======//
using UnityEngine;
using Unity.VisualScripting;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class EventStageDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public EventStageDataSheet dataSheet;

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

			string rootpath = "Assets/Resources/RuntimeDatas/EventMaps";
			string path = $"{rootpath}/{typename}s";

			if (AssetDatabase.IsValidFolder(rootpath) == false)
			{
				AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "EventMaps");
			}
			if (AssetDatabase.IsValidFolder(path) == false)
			{
				AssetDatabase.CreateFolder(rootpath, $"{typename}s");
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
