using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[System.Serializable]
public class StageCreateInfo
{
	public long stageMapTid;
	public int stageLevelMin;
	public int stageLevelMax;
	public StageDifficulty difficulty;
	public int enemyCount;
	public string levelWeightOperator;
	public string bossWeightOperator;

	public List<RewardOperator> rewards = new List<RewardOperator>();
}

[Serializable]
public class RewardOperator
{
	public long itemTid;
	public string op;
	public float probability;

	/// <summary>
	/// 일반스테이지 장비 보상을 랜덤하게 계산하는 용도
	/// </summary>
	public bool randomDrop;
}



[System.Serializable]
public class StageCreateSave
{
	public long prefixID;
	public long waveTID;
	public List<StageCreateInfo> stageLevelData = new List<StageCreateInfo>();
}


public class SheetTypeNameInfo
{
	public string typeName;
}






public class StageInfoCreator : EditorWindow
{
	private const int RANDOM_DROP_ITEM_COUNT = 2;
	private const int RANDOM_SEED = 930681;
	public static string CREATE_INFO_FILE_PATH
	{
		get
		{
			return Application.dataPath.Replace("/Assets", "") + "/StageCreateData";
		}
	}
	public static string SHEET_FILE_PATH => $"{Application.dataPath}/AssetFolder/Resources/Data/Json/";

	private GUIStyle LabelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;

			return style;
		}
	}
	private GUIStyle ButtonStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Button");
			style.richText = true;

			return style;
		}
	}



	public Vector2 scrollPos;
	public bool editStageInfo = false;
	public bool editItemInfo = false;

	public Dictionary<WaveType, StageCreateSave> createInfo = new Dictionary<WaveType, StageCreateSave>();
	private bool isDirty;

	private List<StageMapDataSheet> _mapSheets;
	private List<ItemDataSheet> _itemSheets;
	private List<EquipItemDataSheet> _equipSheets;


	private List<StageMapDataSheet> MapSheets
	{ 
		get
		{
			if(_mapSheets == null)
			{
				LoadDataSheet();
			}

			return _mapSheets;
		}
	}
	private List<ItemDataSheet> ItemSheets
	{
		get
		{
			if (_itemSheets == null)
			{
				LoadDataSheet();
			}

			return _itemSheets;
		}
	}
	private List<EquipItemDataSheet> EquipSheets
	{
		get
		{
			if (_equipSheets == null)
			{
				LoadDataSheet();
			}

			return _equipSheets;
		}
	}



	[MenuItem("Custom Menu/DataEditor/Stage Creator")]
	public static void ShowEditor()
	{
		StageInfoCreator window = GetWindow<StageInfoCreator>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}

	private void OnEnable()
	{
		LoadCreateInfo();
	}

	private void OnGUI()
	{
		// 상단 버튼 메뉴
		ShowTopButtonMenu();
		GUILayout.Space(15);

		// 보기 옵션
		ShowOptionMenu();
		GUILayout.Space(15);

		// 리스트 출력
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		foreach (var v in createInfo)
		{
			ShowCreateInfoList(v.Key.ToString(), v.Value);
			GUILayout.Space(5);
		}
		EditorGUILayout.EndScrollView();
	}

	private void ShowTopButtonMenu()
	{
		if (GUILayout.Button($"생성데이터 경로: {CREATE_INFO_FILE_PATH}"))
		{
			Application.OpenURL(CREATE_INFO_FILE_PATH);
		}
		if (GUILayout.Button($"데이터시트 경로: {SHEET_FILE_PATH}"))
		{
			Application.OpenURL(SHEET_FILE_PATH);
		}
		GUILayout.BeginHorizontal();

		Color originColor = GUI.color;
		if (isDirty)
		{
			GUI.color = Color.red;
		}
		if (GUILayout.Button("생성정보 저장", ButtonStyle, GUILayout.MinHeight(40)))
		{
			SaveCreateInfo();
			isDirty = false;
		}
		GUI.color = originColor;

		if (GUILayout.Button("생성정보 불러오기", ButtonStyle, GUILayout.MinHeight(40)))
		{
			LoadCreateInfo();
			GUI.FocusControl("");
			isDirty = false;
		}
		GUILayout.EndHorizontal();

		if (GUILayout.Button("스테이지 데이터 시트 생성", ButtonStyle, GUILayout.MinHeight(40)))
		{
			foreach (var v in createInfo)
			{
				CreateStageDataSheet(v.Value, v.Key);
			}
		}
	}

	private void ShowOptionMenu()
	{
		editStageInfo = GUILayout.Toggle(editStageInfo, "스테이지 정보 수정");
		EditorGUI.BeginDisabledGroup(editStageInfo);
		editItemInfo = GUILayout.Toggle(editItemInfo, "아이템 드랍율 수정");
		EditorGUI.EndDisabledGroup();

		if (editStageInfo)
		{
			// 보여지는게 너무 많으면 헷갈리니까 걍 꺼버림
			editItemInfo = false;
		}
	}

	/// <summary>
	/// 스테이지 생성정보 저장
	/// </summary>
	private void SaveCreateInfo()
	{
		foreach (WaveType waveType in Enum.GetValues(typeof(WaveType)))
		{
			if (waveType == WaveType._None)
			{
				continue;
			}

			if (createInfo.ContainsKey(waveType))
			{
				TrySave(createInfo[waveType], waveType);
			}
			else
			{
				TrySave(new StageCreateSave(), waveType);
			}
		}
	}

	/// <summary>
	/// 스테이지 생성정보 불러오기
	/// </summary>

	private void LoadCreateInfo()
	{
		createInfo.Clear();
		_mapSheets = null;
		_itemSheets = null;
		_equipSheets = null;

		foreach (WaveType waveType in Enum.GetValues(typeof(WaveType)))
		{
			if (waveType == WaveType._None)
			{
				continue;
			}
			try
			{
				StageCreateSave createData = new StageCreateSave();
				string json = System.IO.File.ReadAllText($"{CREATE_INFO_FILE_PATH}/CreateSave_{waveType}.json");
				JsonUtility.FromJsonOverwrite(json, createData);

				createInfo.Add(waveType, createData);
			}
			catch (Exception e)
			{
				Debug.LogError($"StageCreateSave 로드 실패. 파일명: CreateSave_{waveType}\n{e}");
			}
		}
	}

	private void ShowCreateInfoList(string _name, StageCreateSave _data)
	{
		GUI.changed = false;

		GUILayout.Label($"{_name}(prefix: {_data.prefixID} / wave: {_data.waveTID})", "PreToolbar");
		if (editStageInfo)
		{
			_data.prefixID = EditorGUILayout.LongField("TID Prefix", _data.prefixID);
			_data.waveTID = EditorGUILayout.LongField("Wave TID", _data.waveTID);
		}

		for(int i=0 ; i< _data.stageLevelData.Count ; i++)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("  Lv", GUILayout.Width(30));
			_data.stageLevelData[i].stageLevelMin = EditorGUILayout.IntField(_data.stageLevelData[i].stageLevelMin, GUILayout.Width(40));
			GUILayout.Label(" ~ ", GUILayout.Width(15));
			_data.stageLevelData[i].stageLevelMax = EditorGUILayout.IntField(_data.stageLevelData[i].stageLevelMax, GUILayout.Width(40));

			GUILayout.Label("  난이도", GUILayout.Width(50));
			_data.stageLevelData[i].difficulty = (StageDifficulty)EditorGUILayout.EnumPopup(_data.stageLevelData[i].difficulty, GUILayout.Width(70));
			GUILayout.Label("  등급가중치", GUILayout.Width(70));
			_data.stageLevelData[i].levelWeightOperator = EditorGUILayout.TextField(_data.stageLevelData[i].levelWeightOperator, GUILayout.Width(40));
			GUILayout.Label("  보스가중치", GUILayout.Width(70));
			_data.stageLevelData[i].bossWeightOperator = EditorGUILayout.TextField(_data.stageLevelData[i].bossWeightOperator, GUILayout.Width(40));
			GUILayout.Label("  등장수", GUILayout.Width(50));
			_data.stageLevelData[i].enemyCount = EditorGUILayout.IntField(_data.stageLevelData[i].enemyCount, GUILayout.Width(30));


			var mapData = GetMapData(_data.stageLevelData[i].stageMapTid);
			if (mapData != null)
			{
				GUILayout.Label($"  맵({mapData.description})", GUILayout.Width(150));
			}
			else
			{
				GUILayout.Label($"  맵(<color=red>Unknown</color>)", LabelStyle, GUILayout.Width(150));
			}
			_data.stageLevelData[i].stageMapTid = EditorGUILayout.LongField(_data.stageLevelData[i].stageMapTid);
			EditorGUILayout.EndHorizontal();


			if (editItemInfo)
			{
				// 아이템 정보표시(수정가능)
				for (int j = 0 ; j < _data.stageLevelData[i].rewards.Count ; j++)
				{
					var item = _data.stageLevelData[i].rewards[j];

					var itemData = GetItemData(item.itemTid);
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("     ", GUILayout.MaxWidth(135), GUILayout.MinWidth(135));
					if (itemData != null)
					{
						GUILayout.Label($"  {itemData.description}", LabelStyle, GUILayout.Width(150));
					}
					else
					{
						GUILayout.Label("  TID(<color=red>Unknown</color>)", LabelStyle, GUILayout.Width(150));
					}
					item.itemTid = EditorGUILayout.LongField(item.itemTid, GUILayout.Width(80));
					GUILayout.Label("  확률", GUILayout.Width(50));
					item.probability = EditorGUILayout.FloatField(item.probability, GUILayout.Width(40));
					GUILayout.Label("  계산식", GUILayout.Width(60));
					item.op = EditorGUILayout.TextField(item.op, GUILayout.Width(300));
					GUILayout.Label("  랜덤하게 드랍", GUILayout.Width(120));
					item.randomDrop = EditorGUILayout.Toggle(item.randomDrop, GUILayout.Width(40));
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("     ", GUILayout.Width(135));
				if (GUILayout.Button("ADD", GUILayout.Width(135)))
				{
					_data.stageLevelData[i].rewards.Add(new RewardOperator());
				}
				if (GUILayout.Button("SUB", GUILayout.Width(135)))
				{
					_data.stageLevelData[i].rewards.RemoveAt(_data.stageLevelData[i].rewards.Count - 1);
				}
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				// 아이템 정보표시(요약)
				for (int j = 0 ; j < _data.stageLevelData[i].rewards.Count ; j++)
				{
					if (editStageInfo)
					{
						continue;
					}

					var item = _data.stageLevelData[i].rewards[j];

					var itemData = GetItemData(item.itemTid);
					EditorGUILayout.BeginHorizontal();
					if (itemData != null)
					{
						ShowOperatorInfo($"{itemData.name}({itemData.tid})", item.op, item.probability, item.randomDrop);
					}
					else
					{
						ShowOperatorInfo($"<color=red>Unknown</color>({item.itemTid})", item.op, item.probability, item.randomDrop);
					}
					EditorGUILayout.EndHorizontal();
				}
			}
			GUILayout.Space(10);
		}


		if (editStageInfo)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("ADD"))
			{
				_data.stageLevelData.Add(new StageCreateInfo());
			}
			if (GUILayout.Button("SUB"))
			{
				_data.stageLevelData.RemoveAt(_data.stageLevelData.Count - 1);
			}
			GUILayout.EndHorizontal();
		}


		if (GUI.changed)
		{
			isDirty = true;
		}
	}

	/// <summary>
	/// 그냥 텍스트 편한게 찍는 용도
	/// </summary>
	private void ShowOperatorInfo(string _title, string _operator, float _ratio, bool _randomDrop)
	{
		if(_randomDrop)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("     ", GUILayout.MaxWidth(135), GUILayout.MinWidth(135));
			GUILayout.Label($"  <color=yellow>{_title}</color>: {_operator}, <color=magenta>확률</color>: {_ratio} <color=magenta>[랜덤하게 드랍. 최대 {RANDOM_DROP_ITEM_COUNT}개]</color>", LabelStyle);
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("     ", GUILayout.MaxWidth(135), GUILayout.MinWidth(135));
			GUILayout.Label($"  <color=yellow>{_title}</color>: {_operator}, <color=magenta>확률</color>: {_ratio}", LabelStyle);
			EditorGUILayout.EndHorizontal();
		}
	}

	private void CreateStageDataSheet(StageCreateSave _data, WaveType _waveType)
	{
		var sheet = new StageInfoDataSheet();
		int stageMaxCount = _data.stageLevelData[_data.stageLevelData.Count - 1].stageLevelMax;

		sheet.typeName = typeof(StageInfoDataSheet).ToString();
		sheet.prefixID = _data.prefixID;

		System.Random r = new System.Random(RANDOM_SEED);

		for (int i=0 ; i<stageMaxCount ; i++)
		{
			int stageLv = i + 1;
			StageInfoData stageInfo = new StageInfoData();
			StageCreateInfo levelData = GetLevelData(_data, stageLv);
			if(levelData == null)
			{
				EditorUtility.DisplayDialog("Error", $"저장실패. 스테이지 레벨정보 찾지 못함. {stageLv}", "확인");
				return;
			}

			stageInfo.tid = _data.prefixID + stageLv;
			stageInfo.subTitle = $"{stageLv}";
			stageInfo.stageLevel = stageLv;
			stageInfo.difficulty = levelData.difficulty;
			stageInfo.stageWaveTid = _data.waveTID;
			stageInfo.stageMapTid = levelData.stageMapTid;
			stageInfo.enemyCount = levelData.enemyCount;

			// 등급 가중치
			if (levelData.levelWeightOperator.HasStringValue())
			{
				string replace = FourArithmeticCalculator.ReplaceReservedWord(levelData.levelWeightOperator, 0, stageLv);
				float weight = FourArithmeticCalculator.CalculateFourArithmetics(replace);

				stageInfo.levelWeight = weight;
			}
			else
			{
				stageInfo.levelWeight = 1;
			}

			// 보스가중치
			if (levelData.bossWeightOperator.HasStringValue())
			{
				string replace = FourArithmeticCalculator.ReplaceReservedWord(levelData.bossWeightOperator, 0, stageLv);
				float weight = FourArithmeticCalculator.CalculateFourArithmetics(replace);

				stageInfo.bossWeight = weight;
			}
			else
			{
				stageInfo.bossWeight = 2;
			}

			// 보상데이터 변환
			stageInfo.killRewards = MakeKillRewards(levelData, stageLv, r);

			sheet.infos.Add(stageInfo);
		}

		TrySave(sheet, _waveType);
	}

	private List<MetaRewardInfo> MakeKillRewards(StageCreateInfo _levelData, int _stageLv, System.Random _random)
	{
		List<MetaRewardInfo> rewards = new List<MetaRewardInfo>();
		List<MetaRewardInfo> randomReward = new List<MetaRewardInfo>();

		foreach (var v in _levelData.rewards)
		{
			if (v.itemTid != 0)
			{
				string replace = FourArithmeticCalculator.ReplaceReservedWord(v.op, 0, _stageLv);
				string count = FourArithmeticCalculator.CalculateFourArithmetics(replace).ToString();

				var reward = new MetaRewardInfo() { tid = v.itemTid, count = count, dropRatio = v.probability };
				
				if(v.randomDrop)
				{
					randomReward.Add(reward);
				}
				else
				{
					rewards.Add(reward);
				}
			}
		}


		for (int i=0 ; i<RANDOM_DROP_ITEM_COUNT ; i++)
		{
			if(randomReward.Count == 0)
			{
				break;
			}

			int idx = _random.Next(0, randomReward.Count);
			rewards.Add(randomReward[idx]);
			randomReward.RemoveAt(idx);
		}

		return rewards;
	}

	public StageCreateInfo GetLevelData(StageCreateSave _data, int _stageLevel)
	{
		for(int i=0 ; i< _data.stageLevelData.Count ; i++)
		{
			if(_data.stageLevelData[i].stageLevelMin <= _stageLevel && _stageLevel <= _data.stageLevelData[i].stageLevelMax)
			{
				return _data.stageLevelData[i];
			}
		}

		return null;
	}

	private ItemData GetItemData(long _itemTid)
	{
		foreach (var v in EquipSheets)
		{
			var data = v.Get(_itemTid);
			if (data != null)
			{
				return data;
			}
		}
		foreach (var v in ItemSheets)
		{
			var data = v.Get(_itemTid);
			if (data != null)
			{
				return data;
			}
		}

		return null;
	}

	private StageMapData GetMapData(long _itemTid)
	{
		foreach(var v in MapSheets)
		{
			var data = v.Get(_itemTid);
			if(data != null)
			{
				return data;
			}
		}

		return null;
	}

	private void LoadDataSheet()
	{
		_mapSheets = new List<StageMapDataSheet>();
		_itemSheets = new List<ItemDataSheet>();
		_equipSheets = new List<EquipItemDataSheet>();

		var paths = System.IO.Directory.GetFiles(SHEET_FILE_PATH);
		foreach (var path in paths)
		{
			string json = System.IO.File.ReadAllText(path);
			SheetTypeNameInfo sheetInfo = null;

			try
			{
				sheetInfo = JsonUtility.FromJson<SheetTypeNameInfo>(json);
			}
			catch
			{
				continue;
			}

			if (sheetInfo.typeName == "StageMapDataSheet")
			{
				var sheet = new StageMapDataSheet();
				JsonUtility.FromJsonOverwrite(json, sheet);

				_mapSheets.Add(sheet);
			}
			else if(sheetInfo.typeName == "EquipItemDataSheet")
			{
				var sheet = new EquipItemDataSheet();
				JsonUtility.FromJsonOverwrite(json, sheet);

				_equipSheets.Add(sheet);
			}
			else if(sheetInfo.typeName == "ItemDataSheet")
			{
				var sheet = new ItemDataSheet();
				JsonUtility.FromJsonOverwrite(json, sheet);

				_itemSheets.Add(sheet);
			}
		}
	}

	public void TrySave(StageInfoDataSheet sheet, WaveType _waveType)
	{
		try
		{
			string json = JsonUtility.ToJson(sheet, true);

			var fullPath = $"{SHEET_FILE_PATH}/StageInfoDataSheet_{_waveType}.json";

			System.IO.File.WriteAllText(fullPath, json);
			AssetDatabase.Refresh();
			Debug.Log($"Save Completed. path: {fullPath}");
		}
		catch(System.Exception e)
		{
			Debug.LogError($"Json 파일 생성실패\n{e}");
		}
	}
	public void TrySave(StageCreateSave createSave, WaveType _waveType)
	{
		try
		{
			string json = JsonUtility.ToJson(createSave, true);

			var fullPath = $"{CREATE_INFO_FILE_PATH}/CreateSave_{_waveType}.json";

			System.IO.File.WriteAllText(fullPath, json);
			AssetDatabase.Refresh();
			Debug.Log($"Save Completed. path: {fullPath}");
		}
		catch (System.Exception e)
		{
			Debug.LogError($"Json 파일 생성실패\n{e}");
		}
	}
}
