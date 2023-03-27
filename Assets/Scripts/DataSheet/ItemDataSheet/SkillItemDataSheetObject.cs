//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class SkillItemDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public SkillItemDataSheet dataSheet;

	public override void Call()
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];
		string typename = firstData.name;

		string path = $"Assets/Resources/RuntimeDatas/Skills";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"Skills");
		}
		else
		{
			AssetDatabase.DeleteAsset(path);
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", $"Skills");
		}

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"{data.tid}_{data.name}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = ScriptableObject.CreateInstance<SkillItemObject>();
			scriptable.SetBasicData(data.tid, data.name, data.description, data.itemGrade, data.starLv);
			scriptable.SetEquipAbilities(data.EquipAbilityInfos().ToArray());
			scriptable.SetOwnedAbilities(data.OwnAbilityInfos().ToArray());

			AssetDatabase.CreateAsset(scriptable, assetPath);
			AssetDatabase.SaveAssets();
		}
#endif
	}
}
