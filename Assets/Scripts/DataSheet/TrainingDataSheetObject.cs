using UnityEngine;
using System;
using System.Data;
using Unity.VisualScripting;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class TrainingDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public TrainingDataSheet dataSheet;

	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];

		string path = $"Assets/Resources/RuntimeDatas/Trainings";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"Trainings");
		}
		RenameAsset<TrainingItemObject>(path, "Training");

		MakeScriptableObject<TrainingData, TrainingItemObject>(dataSheet.infos, path, "Training");

		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();
#endif
	}
}
