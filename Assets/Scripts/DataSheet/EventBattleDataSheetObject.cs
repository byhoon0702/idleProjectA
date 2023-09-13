//========AUTO GENERATED CODE======//
using UnityEngine;
using Unity.VisualScripting;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class EventBattleDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public EventBattleDataSheet dataSheet;
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

			string path = $"Assets/Resources/RuntimeDatas/EventDungeon/{typename}s";

			if (UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/RuntimeDatas/EventDungeon") == false)
			{
				UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "EventDungeon");
			}
			if (UnityEditor.AssetDatabase.IsValidFolder(path) == false)
			{
				UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/EventDungeon", $"{typename}s");
			}
			RenameAsset<DungeonItemObject>(path, typename);
			var data = dataSheet.infos[i];
			string name = $"{typename}_{data.tid}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (DungeonItemObject)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(DungeonItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<DungeonItemObject>();
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
