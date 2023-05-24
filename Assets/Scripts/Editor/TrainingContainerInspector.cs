using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrainingContainer))]
public class TrainingContainerInspector : Editor
{
	TrainingContainer container;
	private void OnEnable()
	{
		container = (TrainingContainer)target;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			UpdateTrainingList(container.trainingInfos, "Assets/Resources/RuntimeDatas/TrainingItems");
		}

		serializedObject.ApplyModifiedProperties();
	}
	private void UpdateTrainingList(List<RuntimeData.TrainingInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);
			RuntimeData.TrainingInfo info = new RuntimeData.TrainingInfo();

			//TrainingItemObject skillitemObject = (TrainingItemObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(TrainingItemObject));
			//info.level = 0;
			//info.count = 0;
			//info.tid = skillitemObject.Tid;


			//infoList.Add(info);
		}

		//infoList.Sort((x, y) => { return x.tid.CompareTo(y.tid); });
		EditorUtility.SetDirty(target);
		AssetDatabase.SaveAssetIfDirty(target);
	}
}
