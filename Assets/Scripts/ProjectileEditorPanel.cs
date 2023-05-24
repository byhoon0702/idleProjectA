using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR


public class ProjectileEditorPanel : BaseEditorPanel
{

	[HideInInspector]
	//public SkillObject createdPrefab;
	public TMP_Dropdown projectileDataListDropdown;
	public TMP_Dropdown projectileObjectListDropdown;
	public TMP_Dropdown hitEffectListDropdown;
	public TMP_Dropdown behaviorListDropdown;

	public TMP_InputField fireSpeedField;
	public TMP_InputField nameField;
	public TMP_InputField tidField;

	private ProjectileDataSheet projectileDataSheet;

	private float currentSpeed;
	//private SkillActionBehavior selectedBehavior;
	private GameObject selectedObjectEffect;
	private GameObject selectedHitEffect;

	private ProjectileData currentData;

	private List<long> tidList;
	private List<string> projectileObjectPathList;
	private List<string> projectileObjectNameList;
	private List<string> hitobjectPathList;
	private string currentName;



	public List<string> dataList = new List<string>();
	public List<string> projectileObjectList = new List<string>();
	public List<string> projectilePathList = new List<string>();
	public List<string> hitEffectList = new List<string>();
	public List<string> behaviorList = new List<string>();


	public long selectedTid;
	public int selectedDataIndex;
	public int selectedObjectIndex;
	public int selectedHitEffectIndex;
	public int selectedBehaviorIndex;

	public override void Init(EditorToolUI _editorToolUI)
	{
		base.Init(_editorToolUI);

		projectileDataSheet = DataManager.Get<ProjectileDataSheet>();
		var list = projectileDataSheet.infos;
		tidList = new List<long>();
		projectileObjectPathList = new List<string>();
		hitobjectPathList = new List<string>();
		projectileObjectNameList = new List<string>();


		for (int i = 0; i < list.Count; i++)
		{
			dataList.Add($"{list[i].tid} : {list[i].name}");
			tidList.Add(list[i].tid);
		}

		projectileDataListDropdown.AddOptions(dataList);


		string[] guids = UnityEditor.AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/AssetFolder/Resources/Projectile" });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = Path.GetFileNameWithoutExtension(assetpath);

			projectileObjectList.Add(filename);
			projectilePathList.Add(assetpath);
		}

		projectileObjectListDropdown.AddOptions(projectileObjectList);


		string[] hitguids = UnityEditor.AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/AssetFolder/Resources/HitEffect" });
		for (int i = 0; i < hitguids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(hitguids[i]);
			string filename = Path.GetFileNameWithoutExtension(assetpath);
			hitEffectList.Add(filename);
			hitobjectPathList.Add(assetpath);
		}

		hitEffectListDropdown.AddOptions(hitEffectList);

		//ProjectileEditorWindow.CreateWindow(this);
	}

	public void SetData(int index)
	{
		currentData = projectileDataSheet.infos[index].Clone();
	}


	public void SetSpeed(string input)
	{
		float.TryParse(input, out currentSpeed);
	}
	public void SetName(string name)
	{
		currentName = name;
	}

	public void CreateNewProjectileData(long tid, string description, string name, string resource, string hitEffect)
	{
		currentData = null;
		currentData = new ProjectileData(tid, description, name, resource, hitEffect);
	}

	public void Save()
	{
		if (currentData == null)
		{
			return;
		}
		var data = projectileDataSheet.GetData(currentData.tid);
		if (data == null)
		{
			projectileDataSheet.AddData(currentData);
		}
		else
		{
			projectileDataSheet.SetData(currentData.tid, currentData);
		}

		string path = $"{Application.dataPath}/AssetFolder/Resources/Json/ProjectileDataSheet.json";
		JsonConverter.FromData(projectileDataSheet, path);
	}

	public void PlayAttack()
	{
		//CharacterEditor.it.projectileName = projectileObjectNameList[projectileObjectListDropdown.value];
		//CharacterEditor.it.OnClickPlayAnimationAnyLayer("attack");
	}
}
#endif
