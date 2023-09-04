
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageContainer))]
public class StageContainerInspector : Editor
{
	private StageContainer container;
	//StageMapObjectDictionary infoList = new StageMapObjectDictionary();
	private void OnEnable()
	{
		container = target as StageContainer;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Load"))
		{
			UpdateStageMapList("Assets/Resources/RuntimeDatas/Maps");
		}
	}
	private void UpdateStageMapList(string path)
	{
		//StageMapObjectDictionary infoList = new StageMapObjectDictionary();

		//var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		//for (int i = 0; i < guids.Length; i++)
		//{
		//	string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

		//	StageMapObject stageMapObject = (StageMapObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(StageMapObject));
		//	if (infoList.ContainsKey(stageMapObject.stageType) == false)
		//	{
		//		infoList.Add(stageMapObject.stageType, new StageMapObjectList());
		//	}

		//	infoList[stageMapObject.stageType].list.Add(stageMapObject);
		//}

		//container.SetDataList(infoList);
		//EditorUtility.SetDirty(container);
	}
}
