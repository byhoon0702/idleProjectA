using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(YouthContainer))]
public class YouthContainerInspector : Editor
{
	YouthContainer container;
	private void OnEnable()
	{
		container = target as YouthContainer;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			UpdateYouthInfoList(container.info, "Assets/Resources/RuntimeDatas/Youth");
		}
	}
	private void UpdateYouthInfoList(List<RuntimeData.YouthInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			YouthBuffObject youthBuffItemObject = (YouthBuffObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(YouthBuffObject));
			RuntimeData.YouthInfo info = new RuntimeData.YouthInfo(youthBuffItemObject);
			infoList.Add(info);
		}


		EditorUtility.SetDirty(target);
	}
}
