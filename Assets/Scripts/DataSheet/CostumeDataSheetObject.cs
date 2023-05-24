//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
using System.Data;
using Unity.VisualScripting;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CostumeDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public CostumeDataSheet dataSheet;

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
		string typename = firstData.costumeType.ToString().ToLower().FirstCharacterToUpper();

		string path = $"Assets/Resources/RuntimeDatas/Costumes/{typename}s";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/Costumes", $"{typename}s");
		}
		//else
		//{
		//	AssetDatabase.DeleteAsset(path);
		//	AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/Costumes", $"{typename}s");
		//}

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"{data.tid}_{data.name}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (CostumeItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CostumeItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<CostumeItemObject>();
				AssetDatabase.CreateAsset(scriptable, assetPath);
			}

			scriptable.SetBasicData(data.tid, data.name, data.description, data.costumeType, data.itemGrade, data.starLevel);
			scriptable.FindIconResource();
			scriptable.FindObject();

			EditorUtility.SetDirty(scriptable);
		}
		AssetDatabase.SaveAssets();
#endif

	}
}
