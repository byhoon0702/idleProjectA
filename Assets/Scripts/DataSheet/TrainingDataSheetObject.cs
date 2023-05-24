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

	public override void Call()
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];


		string path = $"Assets/Resources/RuntimeDatas/TrainingItems";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"TrainingItems");
		}


		var infos = dataSheet.GetInfosClone();
		for (int i = 0; i < infos.Count; i++)
		{
			var data = infos[i];
			string name = $"{data.tid}_{data.name}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (TrainingItemObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(TrainingItemObject));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<TrainingItemObject>();
				AssetDatabase.CreateAsset(scriptable, assetPath);
			}
			//scriptable.SetEquipAbilities(data.equipValues.ToArray());
			//scriptable.SetOwnedAbilities(data.ownValues.ToArray());
			scriptable.SetData(data);
			scriptable.FindIconResource();
			EditorUtility.SetDirty(scriptable);
		}
		AssetDatabase.SaveAssets();
#endif
	}
}
