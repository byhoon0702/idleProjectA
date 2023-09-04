//========AUTO GENERATED CODE======//
using UnityEngine;
using System;

[Serializable]
public class AdsRewardChestDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public AdsRewardChestDataSheet dataSheet;
	public override void Call(string fileName)
	{
#if UNITY_EDITOR
		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/AdsRewardChests";

		if (UnityEditor.AssetDatabase.IsValidFolder(path) == false)
		{
			UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "AdsRewardChests");
		}
		RenameAsset<AdsRewardChestItemObject>(path, "AdsRewardChest");

		MakeScriptableObject<AdsRewardChestData, AdsRewardChestItemObject>(dataSheet.infos, path, "AdsRewardChest");

		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();

#endif
	}
}
