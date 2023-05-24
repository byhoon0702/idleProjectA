using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VeterancyContainer))]
public class VeterancyContainerInspector : Editor
{
	VeterancyContainer container;
	private void OnEnable()
	{
		container = (VeterancyContainer)target;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			UpdateTrainingList(container.veterancyInfos, "Assets/Resources/RuntimeDatas/Veterancy");
		}

		serializedObject.ApplyModifiedProperties();
	}
	private void UpdateTrainingList(List<RuntimeData.VeterancyInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);



			VeterancyObject costumeitemObject = (VeterancyObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(VeterancyObject));
			RuntimeData.VeterancyInfo info = new RuntimeData.VeterancyInfo(costumeitemObject);
			///info.level = 1;


			infoList.Add(info);
		}

		EditorUtility.SetDirty(container);
	}
}
