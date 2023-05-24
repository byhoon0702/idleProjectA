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

	public override void Call()
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}


		var firstData = dataSheet.infos[0];
		string typename = firstData.name;

		string path = $"Assets/Resources/RuntimeDatas/Skills/Items";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas/Skills", $"Items");
		}

		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			string name = $"{data.tid}_{data.name}";
			string assetPath = $"{path}/{name}.asset";

			var scriptable = (NewSkill)AssetDatabase.LoadAssetAtPath(assetPath, typeof(NewSkill));
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<NewSkill>();
				AssetDatabase.CreateAsset(scriptable, assetPath);
			}
			scriptable.SetEditorData(data);
			//scriptable.SetUseAbility(data.useValue);

			EditorUtility.SetDirty(scriptable);
			AssetDatabase.SaveAssetIfDirty(scriptable);
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
#endif
	}
}
