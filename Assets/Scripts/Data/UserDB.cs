#if UNITY_EDITOR
#define IS_EDITOR
#endif


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;
using RuntimeData;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;


[System.Serializable]
public class SerializedContainerDictionary : SerializableDictionary<System.Type, BaseContainer>
{
}


[System.Serializable]
public class SaveData
{
	public Type type;
	public object json;

	private BaseContainer _container;
	public SaveData()
	{

	}
	public SaveData(Type _type, object _json)
	{
		type = _type;
		json = _json;
	}

	public SaveData Set<T>(T container) where T : BaseContainer
	{
		_container = container;
		type = _container.GetType();
		return this;
	}

	public void UpdateData()
	{
		json = _container.Save();
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

	public const string k_LocalSave = "LocalSave";
	public const string k_LoginSave = "Login";
	public void Set(UserDB userDb)
	{
		loginInfo = userDb.loginInfo;

		savedata = new List<SaveData>
		{
			new SaveData().Set(userDb.userInfoContainer),
			new SaveData().Set(userDb.training),
			new SaveData().Set(userDb.equipContainer),
			new SaveData().Set(userDb.skillContainer),
			new SaveData().Set(userDb.costumeContainer),
			new SaveData().Set(userDb.petContainer),
			new SaveData().Set(userDb.inventory),
			new SaveData().Set(userDb.veterancyContainer),
			new SaveData().Set(userDb.stageContainer),
			new SaveData().Set(userDb.advancementContainer),
			new SaveData().Set(userDb.questContainer),
			new SaveData().Set(userDb.gachaContainer),
			new SaveData().Set(userDb.relicContainer),
			new SaveData().Set(userDb.contentsContainer),
			new SaveData().Set(userDb.awakeningContainer),
			new SaveData().Set(userDb.collectionContainer),
			new SaveData().Set(userDb.buffContainer),
			new SaveData().Set(userDb.battlePassContainer),
			new SaveData().Set(userDb.shopContainer),
			new SaveData().Set(userDb.attendanceContainer),

		};
	}

	public void DeleteLocalSave()
	{
		loginInfo = null;
		savedata.Clear();
		PlayerPrefs.DeleteKey(k_LocalSave);
	}

	public void UpdateDatas()
	{
		for (int i = 0; i < savedata.Count; i++)
		{
			savedata[i].UpdateData();
		}
	}

	public string Save()
	{
		if (ExceptionManager.Instance.ExceptionOccurred)
		{
			return "";
		}

		JsonSerializerSettings settings = new JsonSerializerSettings();
		settings.Formatting = Formatting.None;
		var json = JsonConvert.SerializeObject(this, settings);
		PlayerPrefs.SetString(k_LocalSave, json);
		PlayerPrefs.Save();
		return json;
	}

	public void Load(UserDB userDB)
	{

		if (PlayerPrefs.HasKey(k_LocalSave) == false)
		{
			return;
		}
		string json = PlayerPrefs.GetString(k_LocalSave);

		LoadData(userDB, json);
	}

	public void LoadUserInfoOnly(UserDB userDB, string json)
	{
		UserDBSave data = JsonConvert.DeserializeObject<UserDBSave>(json);
		System.Type type = userDB.GetType();
		var fields = type.GetField("userInfoContainer");


		foreach (var savedata in data.savedata)
		{
			if (savedata.type == typeof(UserInfoContainer))
			{
				MethodInfo methodinfo = savedata.type.GetMethod("FromJson");
				if (methodinfo == null)
				{
					break;
				}
				try
				{
					methodinfo.Invoke(fields.GetValue(userDB), new object[] { (string)savedata.json });
				}
				catch (Exception e)
				{
					Debug.LogError(e);
					PlatformManager.UserDB._error_on_load = true;
				}
				break;
			}
		}
	}

	public void LoadData(UserDB userDB, string json)
	{
		Debug.Log($"Json Data: {json}");
		UserDBSave data = JsonConvert.DeserializeObject<UserDBSave>(json);
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
						PlatformManager.UserDB._error_on_load = true;
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
		//		if (Directory.Exists($"{directory}") == false)
		//		{

		//			return false;
		//		}
		//		isExist = File.Exists(path);
		//		if (isExist == false)
		//		{

		//			return false;
		//		}
		//		string json = "";
		//#if IS_EDITOR
		//		json = File.ReadAllText(path);

		//#else
		//		BinaryFormatter bf = new BinaryFormatter();
		//		FileStream file = File.Open(path, FileMode.Open);

		//		string json = (string)bf.Deserialize(file);
		//		file.Close();
		//#endif

		if (PlayerPrefs.HasKey(k_LocalSave) == false)
		{
			return false;
		}
		string json = PlayerPrefs.GetString(k_LocalSave);
		UserDBSave data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(json);

		if (data.Equals(default))
		{
			return false;
		}

		if (data.loginInfo == null || _loginInfo == null)
		{
			return false;
		}
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


public abstract class BaseContainer : ScriptableObject
{

	protected ScriptableDictionary scriptableDictionary;
	protected UserDB parent;
	public abstract void Load(UserDB _parent);

	/// <summary>
	/// 데이터 로드가 끝난후 데이터 갱신
	/// </summary>
	public abstract void UpdateData();

	public abstract void DailyResetData();

	public abstract void LoadScriptableObject();


	public void LoadListTidMatch<T>(ref List<T> origin, List<T> saved) where T : BaseInfo
	{
		if (saved == null)
		{
			return;
		}

		for (int i = 0; i < origin.Count; i++)
		{

			if (i < saved.Count)
			{
				if (saved[i] == null)
				{ continue; }
				long tid = origin[i].Tid;
				origin[i].Load(saved.Find(x => x.Tid == tid));
			}
		}
	}


	public void LoadListIndexMatch<T>(ref T[] origin, T[] saved) where T : BaseInfo
	{
		if (saved == null)
		{
			return;
		}

		for (int i = 0; i < origin.Length; i++)
		{
			if (i < saved.Length)
			{

				if (saved[i] == null)
				{ continue; }
				origin[i].Load(saved[i]);
			}
		}
	}

	public void LoadListIndexMatch<T>(ref List<T> origin, List<T> saved) where T : BaseInfo
	{
		if (saved == null)
		{
			return;
		}

		for (int i = 0; i < origin.Count; i++)
		{
			if (i < saved.Count)
			{

				if (saved[i] == null)
				{ continue; }
				origin[i].Load(saved[i]);
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
			long tid = long.Parse(split[1]);
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

	public abstract void Dispose();

}

public class UserDB
{
	public List<StatusData> statusDataList { get; private set; }
	public UnitStats UserStats { get; private set; }
	public Dictionary<StatsType, RuntimeData.AbilityInfo> HyperStats = new Dictionary<StatsType, RuntimeData.AbilityInfo>();

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
	public ShopContainer shopContainer;
	public CollectionContainer collectionContainer;
	public BuffContainer buffContainer;
	public UnitContainer unitContainer;
	public BattlePassContainer battlePassContainer;
	public AttendanceContainer attendanceContainer;
	#endregion

	public UserDBSave saveData = new UserDBSave();
	public LoginInfo loginInfo { get; private set; }


	public bool CanDataSave { get; private set; }
	public System.Random rewardChance { get; private set; } = new System.Random(21);
	public bool _error_on_load = false;

	public void OnUpdateContainer()
	{

	}
	public void SetLoginInfo(string uuid, string nickname, string platform)
	{
		if (loginInfo == null)
		{
			loginInfo = new LoginInfo();
		}

		if (uuid.IsNullOrEmpty())
		{
			return;
		}

		loginInfo.uuid = uuid;
		loginInfo.nickName = nickname;
		loginInfo.platform = platform;

		string json = JsonUtility.ToJson(loginInfo, true);

		userInfoContainer?.SetAccountInfo(nickname, uuid);
		userInfoContainer?.SetUserInfo("", 1, (IdleNumber)0, (IdleNumber)0);

		PlayerPrefs.SetString(UserDBSave.k_LoginSave, json);
		PlayerPrefs.Save();
	}

	public UserDB LoadLocalSave()
	{
		if (PlayerPrefs.HasKey(UserDBSave.k_LocalSave))
		{
			string json = PlayerPrefs.GetString(UserDBSave.k_LocalSave);
			UserDBSave save = new UserDBSave();
			UserDB local = new UserDB();
			local.InitializeContainer();
			save.LoadUserInfoOnly(local, json);
			return local;
		}
		return null;
	}

	public async Task<bool> LoadLoginData()
	{
		//나중에는 클라우드에서 유저 정보 얻어 오도록 변경 해야 함
		var cloudJson = await LoadDataFromCloud();
		UserDB cloudDb = null;
		if (cloudJson.IsNullOrEmpty() == false)
		{

			UserDBSave saveData = new UserDBSave();//Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(cloudJson);
			cloudDb = new UserDB();
			cloudDb.InitializeContainer();
			saveData.LoadUserInfoOnly(cloudDb, cloudJson);
		}

		var localData = LoadLocalSave();

		if (cloudDb != null)
		{
			if (localData == null)
			{
				PlayerPrefs.SetString(UserDBSave.k_LocalSave, cloudJson);
				return true;
			}
			if (cloudDb.userInfoContainer.userInfo.UUID == localData.userInfoContainer.userInfo.UUID)
			{
				if (cloudDb.userInfoContainer.userInfo.UserLevel > localData.userInfoContainer.userInfo.UserLevel)
				{
					PlayerPrefs.SetString(UserDBSave.k_LocalSave, cloudJson);
					return true;
				}
				string json = PlayerPrefs.GetString(UserDBSave.k_LocalSave);
				if (json.IsNullOrEmpty())
				{
					PlayerPrefs.SetString(UserDBSave.k_LocalSave, cloudJson);
				}
			}
			//PopAlert.Create("알림", $"Server : {cloudDb.userInfoContainer.userInfo.UUID}\nLocal : {localData.userInfoContainer.userInfo.UUID}");
			return true;
		}
		if (cloudJson.IsNullOrEmpty())
		{
			//PopAlert.Create("오류", "클라우드 데이터 없음");
			PlayerPrefs.DeleteKey(UserDBSave.k_LocalSave);
		}
		else
		{
			PlayerPrefs.SetString(UserDBSave.k_LocalSave, cloudJson);
		}
		return true;
	}

	public async Task<string> LoadDataFromCloud()
	{

		var result = await RemoteConfigManager.Instance.CloudData();

		return result;
	}

	public void LogOut()
	{
		loginInfo = null;
		Dispose();
		PlayerPrefs.DeleteKey(UserDBSave.k_LoginSave);
		saveData.DeleteLocalSave();
	}


	public IdleNumber GetValue(StatsType type)
	{
		return UserStats.GetValue(type);
	}
	public IdleNumber GetBaseValue(StatsType type)
	{
		return UserStats.GetBaseValue(type);
	}


	public void Dispose()
	{
		userInfoContainer?.Dispose();
		training?.Dispose();
		equipContainer?.Dispose();
		skillContainer?.Dispose();
		costumeContainer?.Dispose();
		petContainer?.Dispose();
		inventory?.Dispose();
		veterancyContainer?.Dispose();
		stageContainer?.Dispose();
		advancementContainer?.Dispose();
		questContainer?.Dispose();
		gachaContainer?.Dispose();
		relicContainer?.Dispose();
		awakeningContainer?.Dispose();
		contentsContainer?.Dispose();
		shopContainer?.Dispose();
		collectionContainer?.Dispose();
		buffContainer?.Dispose();
		battlePassContainer?.Dispose();
		attendanceContainer?.Dispose();

	}

	public void Clear()
	{


	}

	public string Save()
	{
		if (ExceptionManager.Instance.ExceptionOccurred)
		{
			return "";
		}
		if (CanDataSave == false)
		{
			return "";
		}

		saveData.UpdateDatas();
		return saveData.Save();
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

	public void InitializeContainer()
	{
		UserStats = new UnitStats();
		UserStats.Generate();
		LoadContainer(ref userInfoContainer);
	}

	public void Init()
	{
		CanDataSave = false;
		_error_on_load = false;
		try
		{
			LoadContainer(ref training);
			LoadContainer(ref veterancyContainer);
			LoadContainer(ref inventory);
			LoadContainer(ref equipContainer);
			LoadContainer(ref skillContainer);
			LoadContainer(ref petContainer);
			LoadContainer(ref stageContainer);
			LoadContainer(ref advancementContainer);
			LoadContainer(ref gachaContainer);
			LoadContainer(ref questContainer);
			LoadContainer(ref relicContainer);


			LoadContainer(ref awakeningContainer);
			LoadContainer(ref costumeContainer);
			LoadContainer(ref contentsContainer);

			LoadContainer(ref shopContainer);
			LoadContainer(ref collectionContainer);
			LoadContainer(ref buffContainer);
			LoadContainer(ref unitContainer);
			LoadContainer(ref battlePassContainer);
			LoadContainer(ref attendanceContainer);

			saveData.Set(this);
			Load();
			SetUnitData();
			InitDatas();

			VLog.Log("Data Load 성공");
			RemoteConfigManager.Instance.UpdateUserLevel(userInfoContainer.userInfo.UserLevel);
			RemoteConfigManager.Instance.UpdateStageLevel(stageContainer.LastPlayedNormalStage().StageNumber);

			CanDataSave = true;

		}
		catch (Exception ex)
		{
			VLog.LogError($"데이터 로딩중 오류발생!!!\n{ex.StackTrace}");
			_error_on_load = true;
			CanDataSave = false;
		}
	}

	public void LoadContainer<T>(ref T container) where T : BaseContainer
	{
		if (container == null)
		{
			container = ScriptableObject.CreateInstance<T>();
		}

		container.Load(this);
	}

	public void AddModifiers(StatsType type, StatsModifier modifier)
	{
		UserStats.AddModifier(type, modifier);
	}

	public void UpdateModifiers(StatsType type, StatsModifier modifier)
	{
		UserStats.UpdataModifier(type, modifier);
	}

	public void RemoveModifiers(StatsType type, object source)
	{
		UserStats.RemoveModifier(type, source);
	}

	public void RemoveAllModifiers(object source)
	{
		UserStats.RemoveAllModifiers(source);
	}

	public void RemoveAllModifiers(StatModeType type)
	{
		UserStats.RemoveAllModifiers(type);
	}

	private void OnUpdateData<T>(T container) where T : BaseContainer
	{
		//VLog.Log(container.GetType() + " UpdateData");
		container.UpdateData();
	}

	public void InitDatas()
	{
		Debug.Log("Init Data");
		OnUpdateData(stageContainer);
		OnUpdateData(contentsContainer);
		OnUpdateData(veterancyContainer);
		OnUpdateData(training);
		OnUpdateData(petContainer);
		OnUpdateData(advancementContainer);
		OnUpdateData(skillContainer);
		OnUpdateData(questContainer);
		OnUpdateData(inventory);
		OnUpdateData(gachaContainer);
		OnUpdateData(relicContainer);
		OnUpdateData(awakeningContainer);

		OnUpdateData(equipContainer);
		OnUpdateData(costumeContainer);

		OnUpdateData(collectionContainer);
		OnUpdateData(buffContainer);
		OnUpdateData(unitContainer);
		OnUpdateData(battlePassContainer);
		OnUpdateData(attendanceContainer);

		awakeningContainer.SetHyperActivate(null);

		UpdateUserStats();

	}
	private void OnResetData<T>(T container) where T : BaseContainer
	{
		container.DailyResetData();
	}

	/// <summary>
	/// 새벽 5시 이후로 초기화가 필요한 데이터들을 이곳에서 초기화 한다.
	/// </summary>
	public void ResetDataByDateTime()
	{
		VLog.Log("!!!!!!!==========초기화 성공==========!!!!!!!!");
		OnResetData(userInfoContainer);
		OnResetData(inventory);
		OnResetData(shopContainer);
		OnResetData(attendanceContainer);

	}

	public void UpdateUserStats()
	{
		UserStats.UpdateAll();
	}

	public void AddItem()
	{

	}

	public List<RuntimeData.RewardInfo> OpenRewardBox(RuntimeData.RewardInfo info)
	{
		if (info == null)
		{
			return null;
		}
		if (info.Category != RewardCategory.RewardBox)
		{
			return null;
		}

		var rewardBoxdata = DataManager.Get<RewardBoxDataSheet>().Get(info.Tid);
		List<RuntimeData.RewardInfo> rewardInfos = new List<RuntimeData.RewardInfo>();
		for (int i = 0; i < rewardBoxdata.rewards.Count; i++)
		{
			RuntimeData.RewardInfo reward = new RuntimeData.RewardInfo(rewardBoxdata.rewards[i]);
			reward.UpdateCount();
			rewardInfos.Add(reward);
		}

		var rewardList = RandomReward(rewardInfos, RandomLogic.RewardBox);
		//InternalAddStageRewards(rewardList, displayReward, showToast);

		return rewardList;
	}

	public void AddRewards(List<RuntimeData.RewardInfo> rewardList, bool displayReward, bool showToast = false)
	{
		InternalAddStageRewards(rewardList, displayReward, showToast);
	}


	private void InternalAddStageRewards(List<RuntimeData.RewardInfo> rewardList, bool displayReward, bool showToast = false)
	{
		List<AddItemInfo> infoList = new List<AddItemInfo>();
		for (int i = 0; i < rewardList.Count; i++)
		{
			var reward = rewardList[i];
			infoList.Add(new AddItemInfo(reward));

		}
		AddRewards(infoList, displayReward, showToast);

	}

	public void AddRewards(List<AddItemInfo> rewardList, bool displayReward, bool showToast = false)
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
				case RewardCategory.Persistent:
					{
						var info = inventory.GetPersistent(reward.tid);
						info.unlock = true;
						displayReward = false;
						showToast = false;
					}
					break;
			}
		}

		Save();
		DisplayReward(displayReward, showToast, rewardList);
		GameManager.it.CallAddRewardEvent();
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

			if ((reward.Chance == 100) || chance >= minChance && chance < minChance + maxChance)
			{
				getReward.Add(reward);
				break;
			}
			minChance += maxChance;

		}
		return getReward;

	}


	public void RemoveHyperAbility(object source)
	{
		foreach (var stats in HyperStats)
		{
			stats.Value.RemoveAllModifiersFromSource(source);
		}
	}

	public void AddHyperAbilityInfo(RuntimeData.AbilityInfo ability, object source)
	{

		if (HyperStats.ContainsKey(ability.type))
		{
			HyperStats[ability.type].AddModifiers(new StatsModifier(ability.Value, ability.modeType, source));
		}
		else
		{
			HyperStats.Add(ability.type, ability.Clone());
		}
	}
	public void AddHyperAbilityInfo(List<RuntimeData.AbilityInfo> abilities, object source)
	{
		for (int i = 0; i < abilities.Count; i++)
		{
			var ability = abilities[i];
			AddHyperAbilityInfo(ability, source);
		}
	}
}

