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
#if IS_EDITOR
			return $"{Application.dataPath}/LocalSave";

#else
			return $"{Application.persistentDataPath}/LocalSave";
#endif
		}
	}
	public readonly string path
	{
		get
		{
#if IS_EDITOR
			return $"{directory}/SaveData.txt";

#else
			return $"{directory}/SaveData.dat";
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
			new SaveData(userDb.youthContainer.GetType(), userDb.youthContainer.Save()),
			new SaveData(userDb.advancementContainer.GetType(), userDb.advancementContainer.Save()),
			new SaveData(userDb.juvenescenceContainer.GetType(), userDb.juvenescenceContainer.Save())
		};
	}

	public void Save()
	{

		//if (Directory.Exists($"{Application.dataPath}/Save") == false)
		//{
		//	Directory.CreateDirectory($"{Application.dataPath}/Save");
		//}

		//Debug.Log(path);
		FileInfo fileInfo = new FileInfo(path);
		//Debug.Log(fileInfo.Directory.FullName);
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

	}

	public void Load(UserDB userDB)
	{
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


					//JsonUtility.FromJsonOverwrite((string)savedata.json, fields[i].GetValue(userDB));
					break;
				}
			}
		}

		//userDB.
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


public abstract class BaseContainer : ScriptableObject
{

	protected ScriptableDictionary scriptableDictionary;
	protected UserDB parent;
	public abstract void Load(UserDB _parent);

	public abstract void LoadScriptableObject();

	public T GetScriptableObject<T>(long tid) where T : ScriptableObject
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

	protected virtual void SetItemListRawData<T1, T2>(ref List<T1> infolist, List<T2> datas) where T1 : ItemInfo, new() where T2 : BaseData
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

	protected virtual void SetStatListRawData<T1, T2>(ref List<T1> infolist, List<T2> datas) where T1 : RuntimeData.StatInfo, new() where T2 : BaseData
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

}

[CreateAssetMenu]
public class UserDB : ScriptableObject
{
	[SerializeField] private UnitStats userStats;
	[SerializeField] private UnitStats hyperStats;

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
	public YouthContainer youthContainer;
	public JuvenescenceContainer juvenescenceContainer;
	#endregion

	public UserDBSave saveData = new UserDBSave();
	public LoginInfo loginInfo { get; private set; }


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
		userInfoContainer.SetUserInfo(nickname, uuid, "", 1, 0);

		PlayerPrefs.SetString("Login", json);
		PlayerPrefs.Save();
	}
	public bool LoadLoginData()
	{
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


	public void Init()
	{
		UserStats = Instantiate(userStats);
		HyperStats = CreateInstance<UnitStats>();

		inventory.Load(this);
		equipContainer.Load(this);
		skillContainer.Load(this);
		costumeContainer.Load(this);
		petContainer.Load(this);
		training.Load(this);
		veterancyContainer.Load(this);
		stageContainer.Load(this);
		youthContainer.Load(this);
		advancementContainer.Load(this);
		juvenescenceContainer = CreateInstance<JuvenescenceContainer>();
		juvenescenceContainer.Load(this);
		Load();

		InitStats();
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





	public void InitStats()
	{
		for (int i = 0; i < training.trainingInfos.Count; i++)
		{
			training.trainingInfos[i].RemoveModifier(this);
			training.trainingInfos[i].AddModifier(this);
		}

		for (int i = 0; i < veterancyContainer.veterancyInfos.Count; i++)
		{
			veterancyContainer.veterancyInfos[i].RemoveModifier(this);
			veterancyContainer.veterancyInfos[i].AddModifier(this);
		}

		equipContainer.RemoveModifiers(this);
		equipContainer.AddModifiers(this);

		for (int i = 0; i < costumeContainer.equipSlot.Length; i++)
		{
			if (costumeContainer.equipSlot[i] != null)
			{
				costumeContainer.equipSlot[i].RemoveEquipModifier(this);
				costumeContainer.equipSlot[i].AddEquipModifier(this);
			}
		}

		for (int i = 0; i < youthContainer.info.Count; i++)
		{
			youthContainer.info[i].RemoveModifier(this);
			youthContainer.info[i].AddModifier(this);
		}

		UpdateUserStats();
	}

	public void UpdateUserStats()
	{
		//foreach (var info in abilityinfos.Values)
		//{
		//	info.UpdateValue();
		//}

		UserStats.UpdateAll();

		//for (int i = 0; i < abilityinfos.Count; i++)
		//{
		//	abilityinfos[i].UpdateValue();
		//}
	}
}

