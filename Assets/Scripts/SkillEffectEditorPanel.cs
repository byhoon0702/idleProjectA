using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
public class SkillEffectEditorPanel : BaseEditorPanel
{

	[HideInInspector]
	public SkillObject createdPrefab;

	public SkillInfoObject currentSkillInfoObject;
	private SkillEffectDataSheet skillEffectDataSheet;
	private SkillEffectData currentData;

	private List<long> tidList;

	private List<string> hitobjectPathList;

	private List<string> damageBehaviorList;
	private List<string> damageBehaviorNameList;

	public Dictionary<int, string[]> layerStates = new Dictionary<int, string[]>();
	public int[] selectedAniIndex;


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

		skillEffectDataSheet = DataManager.Get<SkillEffectDataSheet>();
		var list = skillEffectDataSheet.infos;
		tidList = new List<long>();
		hitobjectPathList = new List<string>();
		damageBehaviorList = new List<string>();
		damageBehaviorNameList = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			dataList.Add($"{list[i].tid} : {list[i].name}");
			tidList.Add(list[i].tid);
		}


		string path = PathHelper.projectilePath.Resources().AssetFolder().Assets();
		string[] guids = UnityEditor.AssetDatabase.FindAssets("t:prefab", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = Path.GetFileNameWithoutExtension(assetpath);

			projectileObjectList.Add(filename);
			projectilePathList.Add(assetpath);
		}
		path = PathHelper.hyperCasualFXPath.Resources().AssetFolder().Assets();
		string[] hitguids = UnityEditor.AssetDatabase.FindAssets("t:prefab", new string[] { path });
		for (int i = 0; i < hitguids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(hitguids[i]);
			string filename = Path.GetFileNameWithoutExtension(assetpath);
			hitEffectList.Add(filename);
			hitobjectPathList.Add(assetpath);
		}

		path = PathHelper.applyDamageBehaviorPath.Resources().AssetFolder().Assets();
		string[] damageBehaviors = UnityEditor.AssetDatabase.FindAssets("t:AppliedDamageBehavior", new string[] { path });
		for (int i = 0; i < damageBehaviors.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(damageBehaviors[i]);
			string filename = Path.GetFileNameWithoutExtension(assetpath);
			damageBehaviorNameList.Add(filename);
			damageBehaviorList.Add(assetpath);
		}

		path = PathHelper.projectileBehaviorPath.Resources().AssetFolder().Assets();
		string[] projectileMovementBehaviors = UnityEditor.AssetDatabase.FindAssets("t:SkillActionBehavior", new string[] { path });
		for (int i = 0; i < projectileMovementBehaviors.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(projectileMovementBehaviors[i]);
			string filename = Path.GetFileNameWithoutExtension(assetpath);
			damageBehaviorNameList.Add(filename);
			damageBehaviorList.Add(assetpath);
		}


		OnClickSkillEffectEditorWindow();
	}

	public void OnClickSkillEffectEditorWindow()
	{
#if UNITY_EDITOR
		SkillEffectEditorWindow.CreateWindow(this);
#endif
	}


	public void CreateNewData(SkillEffectData _data)
	{
		currentData = _data;
	}

	public SkillEffectData GetDataByIndex(int index)
	{
		if (skillEffectDataSheet.infos.Count > index)
		{
			return skillEffectDataSheet.infos[index];
		}
		return null;
	}
	public void Save()
	{
		if (currentData == null)
		{
			return;
		}
		var data = skillEffectDataSheet.GetData(currentData.tid);
		if (data == null)
		{
			skillEffectDataSheet.AddData(currentData);
		}
		else
		{
			skillEffectDataSheet.SetData(currentData.tid, currentData);
		}

		string path = $"{Application.dataPath}/AssetFolder/Resources/Data/Json/SkillEffectDataSheet.json";
		JsonConverter.FromData(skillEffectDataSheet, path);
	}

	public void OverrideAnimation(string name)
	{
		UnitEditor.it.OverrideAnimation(name);
	}

	public void PlayAnimation(int layerIndex, string aniStateName = "attack")
	{

		UnitEditor.it.PlayerAnimation(layerIndex, aniStateName);
	}

	public void SetUnitAnimationState(Dictionary<int, List<string>> layers)
	{
		selectedAniIndex = new int[layers.Keys.Count];
		layerStates.Clear();
		foreach (var layer in layers)
		{
			if (layerStates.ContainsKey(layer.Key) == false)
			{
				layerStates.Add(layer.Key, layer.Value.ToArray());
			}
			else
			{
				layerStates[layer.Key] = layer.Value.ToArray();
			}
		}
	}

	public void CalculateSkillValue()
	{
		if (currentSkillInfoObject == null)
		{
			return;
		}

		var dic = UnitEditor.it.viewPlayer.info.rawStatus;

		var maininfo = currentSkillInfoObject.mainSkillInfo;
		//var subinfo = currentSkillInfoObject.subSkillInfo;

		StatInfo minfo = new StatInfo(new StatusData() { type = Ability.Attackpower });
		minfo.value = new IdleNumber(10);


		//IdleNumber mainnumber = FourArithmeticCalculator.CalculateFourArithmetics(maininfo.ConvertToMath(dic, 2, minfo));
		//IdleNumber subnumber = FourArithmeticCalculator.CalculateFourArithmetics(subinfo.ConvertToMath(dic, 2)); ;

		//Debug.Log(mainnumber.ToString());
		//Debug.Log(subnumber.ToString());

	}
}
#endif
