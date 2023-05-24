﻿//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
using System.Data;
using Unity.VisualScripting;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class EquipItemDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public EquipItemDataSheet dataSheet;

	/// <summary>
	/// 아이템 스크립터블 오브젝트 생성
	/// </summary>
	public override void Call()
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];
		string typename = firstData.equipType.ToString().ToLower().FirstCharacterToUpper();

		string path = $"Assets/Resources/RuntimeDatas/EquipItems/{typename}s";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/EquipItems", $"{typename}s");
		}


		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"{data.tid}_{data.name}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (EquipItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(EquipItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<EquipItemObject>();
				AssetDatabase.CreateAsset(scriptable, assetPath);
			}
			scriptable.SetBasicData(data.tid, data.name, data.description, data.equipType, data.itemGrade, data.starLevel);

			scriptable.FindIconResource();
			scriptable.FindEquipObejct();
			EditorUtility.SetDirty(scriptable);
			AssetDatabase.SaveAssetIfDirty(scriptable);

		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
#endif

	}
}
