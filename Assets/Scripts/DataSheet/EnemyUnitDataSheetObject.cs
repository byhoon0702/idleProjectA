//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
[Serializable]
public class EnemyUnitDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public EnemyUnitDataSheet dataSheet;

	public override void Call(string fileName)
	{
#if UNITY_EDITOR
		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];

		string path = $"Assets/Resources/RuntimeDatas/Unit/Enemies";

		if (UnityEditor.AssetDatabase.IsValidFolder(path) == false)
		{
			UnityEditor.AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"Enemies");
		}
		RenameAsset<EnemyObject>(path, "Enemy");

		MakeScriptableObject<EnemyUnitData, EnemyObject>(dataSheet.infos, path, "Enemy");

		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();
#endif
	}
}
