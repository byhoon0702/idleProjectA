//========AUTO GENERATED CODE======//
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class YouthDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public YouthDataSheet dataSheet;

	public override void Call()
	{
#if UNITY_EDITOR

		if (dataSheet.infos.Count == 0)
		{
			return;
		}

		string path = $"Assets/Resources/RuntimeDatas/Youth";

		if (AssetDatabase.IsValidFolder(path) == false)
		{
			AssetDatabase.CreateFolder("Assets/Resources/RuntimeDatas", "Youth");
		}


		for (int i = 0; i < dataSheet.infos.Count; i++)
		{
			var data = dataSheet.infos[i];
			for (int ii = 0; ii < data.buffList.Length; ii++)
			{
				var buff = data.buffList[ii];
				string name = $"{data.tid}_{ii + 1}_{buff.name}";
				string assetPath = $"{path}/{name}.asset";

				var scriptable = (YouthBuffObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(YouthBuffObject));
				if (scriptable == null)
				{
					scriptable = ScriptableObject.CreateInstance<YouthBuffObject>();
					AssetDatabase.CreateAsset(scriptable, assetPath);
				}
				scriptable.SetData(buff);

				EditorUtility.SetDirty(scriptable);
				AssetDatabase.SaveAssetIfDirty(scriptable);
			}
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
#endif
	}
}
