//========AUTO GENERATED CODE======//
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class SkillDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public SkillDataSheet dataSheet;

	public override void Call(string fileName)
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}



		string[] split = fileName.Split('_');

		string typename = split[1];

		string path = $"Assets/Resources/RuntimeDatas/Skills/{typename}";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/Skills", $"{typename}");
		}
		RenameSkillAsset<SkillCore>(path, typename);

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"Skill_{data.tid}_{data.description}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (SkillCore)AssetDatabase.LoadAssetAtPath(assetPath, typeof(SkillCore));
			if (scriptable == null)
			{
				name = $"Skill_{data.tid}_";
				assetPath = $"{path}/{name}.asset";
				scriptable = (SkillCore)AssetDatabase.LoadAssetAtPath(assetPath, typeof(SkillCore));
				if (scriptable == null)
				{
					name = $"Skill_{data.tid}";
					assetPath = $"{path}/{name}.asset";
					scriptable = (SkillCore)AssetDatabase.LoadAssetAtPath(assetPath, typeof(SkillCore));
					if (scriptable == null)
					{
						continue;
					}
				}
			}
			scriptable.SetBasicData(data);
			EditorUtility.SetDirty(scriptable);
			AssetDatabase.SaveAssetIfDirty(scriptable);
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
#endif
	}
}
