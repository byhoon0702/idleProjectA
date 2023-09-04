//========AUTO GENERATED CODE======//
using UnityEngine;
using System;

[Serializable]
public class PetDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public PetDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/Pets";

		if (UnityEditor.AssetDatabase.IsValidFolder(path) == false)
		{
			UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"Pets");
		}

		RenameAsset<PetItemObject>(path, "Pet");

		MakeScriptableObject<PetData, PetItemObject>(dataSheet.infos, path, "Pet");

		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();
#endif

	}
}
