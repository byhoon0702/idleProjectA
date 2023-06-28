#if UNITY_EDITOR
#define IS_EDITOR
#endif


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using RuntimeData;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public Type type;
	public object json;

	public SaveData(Type _type, object _json)
	{
		type = _type;
		json = _json;
	}
}

[System.Serializable]
public struct UserDBSave
{
	public readonly string directory
	{
		get
		{
#if SALES
			return "SalesSave";
#else
#if IS_EDITOR
			return $"{Application.dataPath}/../LocalSave";
#else
			return $"{Application.persistentDataPath}/LocalSave";
#endif
#endif
		}
	}
	public readonly string path
	{
		get
		{
#if SALES
			return $"{directory}/SalesSaveData";
#else
#if IS_EDITOR
			return $"{directory}/SaveData.txt";

#else
			return $"{directory}/SaveData.dat";
#endif
#endif
		}
	}

	[SerializeField] public LoginInfo loginInfo;
	[SerializeField] public List<SaveData> savedata;

	public void Set(UserDB userDb)
	{
		loginInfo = userDb.loginInfo;
		savedata = new List<SaveData>
		{
			new SaveData(userDb.userInfoContainer.GetType(), userDb.userInfoContainer.Save()),
			new SaveData(userDb.training.GetType(), userDb.training.Save()),
			new SaveData(userDb.equipContainer.GetType(), userDb.equipContainer.Save()),
			new SaveData(userDb.skillContainer.GetType(), userDb.skillContainer.Save()),
			new SaveData(userDb.costumeContainer.GetType(), userDb.costumeContainer.Save()),
			new SaveData(userDb.petContainer.GetType(), userDb.petContainer.Save()),
			new SaveData(userDb.inventory.GetType(), userDb.inventory.Save()),
			new SaveData(userDb.veterancyContainer.GetType(), userDb.veterancyContainer.Save()),
			new SaveData(userDb.stageContainer.GetType(), userDb.stageContainer.Save()),
			new SaveData(userDb.advancementContainer.GetType(), userDb.advancementContainer.Save()),
			new SaveData(userDb.questContainer.GetType(), userDb.questContainer.Save()),
			new SaveData(userDb.gachaContainer.GetType(), userDb.gachaContainer.Save()),
			new SaveData(userDb.relicContainer.GetType(), userDb.relicContainer.Save()),
			new SaveData(userDb.contentsContainer.GetType(), userDb.contentsContainer.Save())
		};
	}

	public void Save()
	{

#if SALES
		return;
#else
		FileInfo fileInfo = new FileInfo(path);
		fileInfo.Directory.Create();
#if IS_EDITOR

		var json = JsonConvert.SerializeObject(this, Formatting.Indented);
		File.WriteAllText(fileInfo.FullName, json);
#else
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(fileInfo.FullName, FileMode.OpenOrCreate);
		var json = JsonConvert.SerializeObject(this, Formatting.Indented);
		bf.Serialize(file, json);
		file.Close();
#endif
#endif
	}

	public void Load(UserDB userDB)
	{


#if SALES

		var json = Resources.Load(path) as TextAsset;
		UserDBSave data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(json.text);
#else
		if (Directory.Exists($"{directory}") == false)
		{
			return;
		}
		if (File.Exists(path) == false)
		{
			return;
		}
#if IS_EDITOR
		var json = File.ReadAllText(path);
		UserDBSave data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(json);
#else
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(path, FileMode.Open);

		string jsonbinary = (string)bf.Deserialize(file);
		file.Close();
		UserDBSave data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(jsonbinary);
#endif
#endif

		userDB.SetLoginInfo(data.loginInfo.uuid, data.loginInfo.nickName, data.loginInfo.platform);
		System.Type type = userDB.GetType();
		var fields = type.GetFields();
		for (int i = 0; i < fields.Length; i++)
		{

			foreach (var savedata in data.savedata)
			{
				if (savedata.type == fields[i].FieldType)
				{
					MethodInfo methodinfo = savedata.type.GetMethod("FromJson");
					if (methodinfo == null)
					{
						break;
					}
					try
					{
						methodinfo.Invoke(fields[i].GetValue(userDB), new object[] { (string)savedata.json });
					}
					catch (Exception e)
					{
						Debug.LogError(e);
					}
					break;
				}
			}
		}
	}

	public void DeleteFile()
	{
		File.Delete(path);
	}

	public bool CheckFile(LoginInfo _loginInfo)
	{
		bool isExist = false;
		if (Directory.Exists($"{directory}") == false)
		{

			return false;
		}
		isExist = File.Exists(path);
		if (isExist == false)
		{

			return false;
		}
#if IS_EDITOR
		var json = File.ReadAllText(path);
		UserDBSave data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(json);
#else
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(path, FileMode.Open);

		string jsonbinary = (string)bf.Deserialize(file);
		file.Close();
		UserDBSave data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(jsonbinary);
#endif

		Debug.Log($"닉네임 비교 : {data.loginInfo.nickName} == {_loginInfo.nickName}");
		if (data.loginInfo.nickName == _loginInfo.nickName)
		{
			return true;
		}

		return false;
	}
}


[System.Serializable]
public class ScriptableDictionary : SerializableDictionary<long, ScriptableObject>
{ }


public abstract class BaseContainer : ScriptableObject, IDisposable
{

	protected ScriptableDictionary scriptableDictionary;
	protected UserDB parent;
	public abstract void Load(UserDB _parent);

	/// <summary>
	/// 데이터 로드가 끝난후 데이터 갱신
	/// </summary>
	public abstract void UpdateData();

	public abstract void LoadScriptableObject();


	public void LoadList<T>(ref List<T> origin, List<T> saved) where T : BaseInfo
	{
		for (int i = 0; i < origin.Count; i++)
		{
			if (i < saved.Count)
			{
				long tid = origin[i].Tid;
				origin[i].Load(saved.Find(x => x.Tid == tid));
			}
		}
	}

	public virtual void GetScriptableObject<T>(long tid, out T outObject) where T : ScriptableObject
	{
		outObject = null;

		if (scriptableDictionary.ContainsKey(tid) == false)
		{
			return;
		}

		outObject = scriptableDictionary[tid] as T;
	}

	public virtual T GetScriptableObject<T>(long tid) where T : ScriptableObject
	{
		if (scriptableDictionary.ContainsKey(tid) == false)
		{
			return null;
		}

		return scriptableDictionary[tid] as T;
	}

	protected void AddDictionary<T>(ScriptableDictionary dict, T[] datas) where T : ScriptableObject
	{
		for (int i = 0; i < datas.Length; i++)
		{
			var data = datas[i];
			string[] split = data.name.Split('_');
			long tid = long.Parse(split[0]);
			dict.Add(tid, data);
		}
	}


	protected virtual void SetItemListRawData<T1, T2>(ref List<T1> infolist, T2 _data) where T1 : BaseInfo, new() where T2 : BaseData
	{
		if (infolist == null)
		{
			infolist = new List<T1>();
		}
		infolist.Clear();
		if (infolist.Count == 0)
		{
			infolist = new List<T1>();
			T1 data = new T1();
			data.SetRawData(_data);
			infolist.Add(data);
			return;
		}

		for (int i = 0; i < infolist.Count; i++)
		{
			infolist[i].SetRawData(_data);
		}

	}

	protected virtual void SetListRawData<T1, T2>(ref List<T1> infolist, List<T2> datas) where T1 : IDataInfo, new() where T2 : BaseData
	{
		if (infolist == null)
		{
			infolist = new List<T1>();
		}
		infolist.Clear();
		if (infolist.Count == 0)
		{
			infolist = new List<T1>();
			for (int i = 0; i < datas.Count; i++)
			{
				T1 data = new T1();
				data.SetRawData(datas[i]);
				infolist.Add(data);
			}
			return;
		}

		for (int i = 0; i < infolist.Count; i++)
		{
			infolist[i].SetRawData(datas[i]);
		}
	}
	public abstract string Save();

	public abstract void FromJson(string json);

	public void Dispose()
	{

	}
}

[CreateAssetMenu]
public class UserDB : ScriptableObject
{
	[SerializeField] private UnitStats userStats;
	[SerializeField] private UnitStats hyperStats;

	public List<StatusData> statusDataList { get; private set; }
	public UnitStats UserStats { get; private set; }

	public UnitStats HyperStats { get; private set; }

	public CommonData commonData;
	public ConfigMeta configMeta;
	/// <summary>
	/// 각종 정보 집합체
	/// </summary>
	#region Container
	public UserInfoContainer userInfoContainer;
	public TrainingContainer training;
	public EquipContainer equipContainer;
	public SkillContainer skillContainer;
	public CostumeContainer costumeContainer;
	public PetContainer petContainer;
	public InventoryContainer inventory;
	public VeterancyContainer veterancyContainer;
	public StageContainer stageContainer;
	public AdvancementContainer advancementContainer;
	public QuestContainer questContainer;
	public GachaContainer gachaContainer;
	public RelicContainer relicContainer;
	public AwakeningContainer awakeningContainer;
	public ContentsContainer contentsContainer;
	#endregion

	public UserDBSave saveData = new UserDBSave();
	public LoginInfo loginInfo { get; private set; }


	public System.Random rewardChance { get; private set; } = new System.Random(21);

	public void OnUpdateContainer()
	{

	}
	public void SetLoginInfo(string uuid, string nickname, string platform)
	{
		if (loginInfo == null)
		{
			loginInfo = new LoginInfo();

		}

		loginInfo.uuid = uuid;
		loginInfo.nickName = nickname;
		loginInfo.platform = platform;

		string json = JsonUtility.ToJson(loginInfo, true);
		userInfoContainer.SetUserInfo(nickname, uuid, "", 1, (IdleNumber)0, (IdleNumber)0);

		PlayerPrefs.SetString("Login", json);
		PlayerPrefs.Save();
	}
	public bool LoadLoginData()
	{
#if SALES
		return true;
#endif
		//나중에는 클라우드에서 유저 정보 얻어 오도록 변경 해야 함
		if (PlayerPrefs.HasKey("Login") == false)
		{
			Debug.Log("Login Prefs Not Exist");
			return false;
		}
		string json = PlayerPrefs.GetString("Login");

		loginInfo = (LoginInfo)JsonUtility.FromJson(json, typeof(LoginInfo));

		UserDBSave save = new UserDBSave();
		if (save.CheckFile(loginInfo) == false)
		{
			//로컬 파일이 없을때는 클라우드에서 내려 받음
			return LoadDataFromCloud();
		}

		return true;
	}

	public bool LoadDataFromCloud()
	{
		//클라우드에서 받은 정보를 로컬에 저장
		return false;
	}

	public void LogOut()
	{
		loginInfo = null;
		PlayerPrefs.DeleteKey("Login");
	}


	public IdleNumber GetValue(StatsType type)
	{
		return UserStats.GetValue(type);
	}
	public IdleNumber GetBaseValue(StatsType type)
	{
		return UserStats.GetBaseValue(type);
	}


	public void Clear()
	{


	}

	public void Save()
	{

		saveData.Set(this);
		saveData.Save();
#if UNITY_EDITOR
		if (Application.isPlaying == false)
		{
			UnityEditor.AssetDatabase.Refresh();
		}
#endif
	}

	public void Load()
	{
		saveData.Load(this);
	}

	public void SetUnitData()
	{
		statusDataList = new List<StatusData>();

		var list = DataManager.Get<StatusDataSheet>().GetInfosClone();
		for (int i = 0; i < list.Count; i++)
		{
			statusDataList.Add(list[i]);
		}
	}

	public void Init()
	{
		UserStats = Instantiate(userStats);

		HyperStats = CreateInstance<UnitStats>();
		userInfoContainer.Load(this);
		inventory.Load(this);
		equipContainer.Load(this);
		skillContainer.Load(this);
		costumeContainer.Load(this);
		petContainer.Load(this);
		training.Load(this);
		veterancyContainer.Load(this);
		stageContainer.Load(this);
		advancementContainer.Load(this);
		gachaContainer.Load(this);
		questContainer.Load(this);
		if (relicContainer == null)
		{
			relicContainer = CreateInstance<RelicContainer>();
		}
		relicContainer.Load(this);
		if (awakeningContainer == null)
		{
			awakeningContainer = CreateInstance<AwakeningContainer>();
		}
		awakeningContainer.Load(this);


		if (contentsContainer == null)
		{
			contentsContainer = CreateInstance<ContentsContainer>();
		}
		contentsContainer.Load(this);

		Load();
		SetUnitData();
		InitDatas();
	}

	public void AddModifiers(bool isHyper, StatsType type, StatsModifier modifier)
	{
		if (isHyper)
		{
			HyperStats.AddModifier(type, modifier);

		}
		else
		{
			UserStats.AddModifier(type, modifier);
		}
	}

	public void UpdateModifiers(bool isHyper, StatsType type, StatsModifier modifier)
	{
		if (isHyper)
		{
			HyperStats.UpdataModifier(type, modifier);
		}
		else
		{
			UserStats.UpdataModifier(type, modifier);
		}
	}

	public void RemoveModifiers(bool isHyper, StatsType type, object source)
	{
		if (isHyper)
		{
			HyperStats.RemoveModifier(type, source);
		}
		else
		{
			UserStats.RemoveModifier(type, source);
		}
	}

	public void InitDatas()
	{


		veterancyContainer.UpdateData();
		equipContainer.UpdateData();
		training.UpdateData();
		costumeContainer.UpdateData();
		petContainer.UpdateData();
		advancementContainer.UpdateData();

		UpdateUserStats();

		skillContainer.UpdateData();
		questContainer.UpdateData();

		inventory.UpdateData();

		gachaContainer.UpdateData();
		relicContainer.UpdateData();
		awakeningContainer.UpdateData();
		contentsContainer.UpdateData();
	}

	public void UpdateUserStats()
	{


		UserStats.UpdateAll();


	}

	public void AddItem()
	{

	}

	public void OpenRewardBox(RuntimeData.RewardInfo info, bool displayReward, bool showToast = false)
	{
		if (info == null)
		{
			return;
		}
		if (info.Category != RewardCategory.RewardBox)
		{
			return;
		}

		var rewardBoxdata = DataManager.Get<RewardBoxDataSheet>().Get(info.Tid);
		List<RuntimeData.RewardInfo> rewardInfos = new List<RuntimeData.RewardInfo>();
		for (int i = 0; i < rewardBoxdata.rewards.Count; i++)
		{
			RuntimeData.RewardInfo reward = new RuntimeData.RewardInfo(rewardBoxdata.rewards[i]);
			reward.UpdateCount();
			rewardInfos.Add(reward);
		}

		InternalAddStageRewards(RandomReward(rewardInfos, RandomLogic.RewardBox), displayReward, showToast);
	}


	public void AddRewards(List<RuntimeData.RewardInfo> rewardList, bool displayReward, bool showToast = false)
	{
		AddStageRewards(rewardList, displayReward, showToast);
	}

	public void AddStageRewards(List<RuntimeData.RewardInfo> rewardList, bool displayReward, bool showToast = false)
	{
		InternalAddStageRewards(rewardList, displayReward, showToast);
	}



	private void InternalAddStageRewards(List<RuntimeData.RewardInfo> rewardList, bool displayReward, bool showToast = false)
	{
		List<AddItemInfo> infoList = new List<AddItemInfo>();
		for (int i = 0; i < rewardList.Count; i++)
		{
			var reward = rewardList[i];
			infoList.Add(new AddItemInfo(reward.Tid, reward.fixedCount, reward.grade, reward.Category, reward.iconImage));

		}
		AddRewards(infoList, displayReward, showToast);

	}

	private void AddRewards(List<AddItemInfo> rewardList, bool displayReward, bool showToast = false)
	{
		for (int i = 0; i < rewardList.Count; i++)
		{
			var reward = rewardList[i];
			switch (reward.category)
			{
				case RewardCategory.Equip:
					{
						equipContainer.AddEquipItem(reward.tid, reward.value.GetValueToInt());
					}
					break;
				case RewardCategory.Pet:
					{
						petContainer.AddPetItem(reward.tid, reward.value.GetValueToInt());
					}
					break;
				case RewardCategory.Skill:
					{
						skillContainer.AddSkill(reward.tid, reward.value.GetValueToInt());
					}
					break;
				case RewardCategory.EXP:
					{
						userInfoContainer.GainUserExp(reward.value);
					}
					break;
				case RewardCategory.Costume:
					{
						costumeContainer.Buy(reward.tid);
					}
					break;
				case RewardCategory.Currency:
					{
						var data = DataManager.Get<CurrencyDataSheet>().Get(reward.tid);
						inventory.FindCurrency(data.type).Earn(reward.value);
					}
					break;
				case RewardCategory.RewardBox:
					{
						inventory.AddRandomBox(reward.tid, reward.value.GetValueToInt());
					}
					break;
				case RewardCategory.Relic:
					{
						relicContainer.AddItem(reward.tid, reward.value.GetValueToInt());
					}
					break;
			}
		}
		DisplayReward(displayReward, showToast, rewardList);
	}
	protected void DisplayReward(bool displayReward, bool showToast, List<AddItemInfo> rewardList)
	{
		if (displayReward)
		{
			if (showToast)
			{
				GameUIManager.it.uiController.ShowRewardToast(rewardList);
			}
			else
			{
				GameUIManager.it.uiController.ShowRewardPopup(rewardList);

			}
		}
	}

	public List<RuntimeData.RewardInfo> RandomReward(List<RuntimeData.RewardInfo> rewardList, System.Random r)
	{
		List<RuntimeData.RewardInfo> getReward = new List<RuntimeData.RewardInfo>();
		var chance = r.Next(0, RandomLogic.maxChance);

		int minChance = 0;
		for (int i = 0; i < rewardList.Count; i++)
		{
			var reward = rewardList[i];
			int maxChance = (int)(reward.Chance * 100);

			if ((reward.Chance == 100) || chance >= minChance && chance < maxChance)
			{
				getReward.Add(reward);
			}
			minChance += maxChance;

		}
		return getReward;

	}
}

