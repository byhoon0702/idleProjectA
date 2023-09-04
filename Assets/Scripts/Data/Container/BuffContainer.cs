using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RuntimeData
{
	[System.Serializable]
	public class BuffInfo : ModifyInfo
	{
		public int Level;
	}

	[System.Serializable]
	public class AdBuffInfo : ItemInfo
	{
		[SerializeField] private bool _isActive;
		public bool IsActive
		{
			get
			{
				return (_isActive && (EndTime - TimeManager.Instance.Now).TotalSeconds > 0)
					|| PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid).unlock;
			}
		}

		[SerializeField] private string _exp;

		public IdleNumber Exp
		{
			get { return (IdleNumber)_exp; }
			set { _exp = value.ToString(); }
		}

		[SerializeField] private string _endTime;

		public System.DateTime EndTime
		{
			get; private set;
		}




		public AbilityInfo Ability { get; private set; }
		public AdBuffData RawData { get; private set; }

		private PersistentItemInfo adFreeItem;
		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);

			AdBuffInfo temp = info as AdBuffInfo;

			_endTime = temp._endTime;
			_exp = temp._exp;


		}

		public void WatchAd()
		{
			if (IsActive)
			{
				return;
			}

			MobileAdsManager.Instance.ShowAds(() => { Activate(); });
		}

		public bool LevelUp()
		{
			if (Exp < NeedExp())
			{
				ToastUI.Instance.Enqueue("경험치가 부족합니다.");
				return false;
			}

			Exp -= NeedExp();
			_level++;

			if (IsActive)
			{
				AddBuffToUserStat();
			}
			return true;
		}

		public void Activate()
		{
			System.DateTime endtime = TimeManager.Instance.Now.AddMinutes(RawData.duration);
			_endTime = endtime.ToString();
			EndTime = endtime;
			_isActive = true;
			AddBuffToUserStat();

			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.ACTIVATE_BUFF, 0, (IdleNumber)1);
		}

		public void OnUpdate()
		{
			if (adFreeItem.unlock)
			{
				return;
			}

			if (_isActive)
			{
				System.TimeSpan ts = EndTime - TimeManager.Instance.Now;

				if (ts.TotalSeconds <= 0)
				{
					PlatformManager.UserDB.RemoveAllModifiers(this);
					_isActive = false;
				}
			}
		}

		private void AddBuffToUserStat()
		{
			IdleNumber value = (IdleNumber)RawData.stats.perLevel;
			Ability.RemoveAllModifiersFromSource(this);
			if (IsActive)
			{
				Ability.AddModifiers(new StatsModifier(value * Level, StatModeType.Add, this));
			}

			PlatformManager.UserDB.AddModifiers(Ability.type, new StatsModifier(Ability.Value, StatModeType.AdsBuff));
		}

		public override void UpdateData()
		{
			base.UpdateData();

			System.DateTime endtime;
			if (System.DateTime.TryParse(_endTime, out endtime))
			{
				EndTime = endtime;
			}
			_isActive = (EndTime - TimeManager.Instance.Now).TotalSeconds > 0;

			AddBuffToUserStat();

			adFreeItem = PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid);
		}

		public override void SetRawData<T>(T data)
		{
			RawData = data as AdBuffData;
			tid = RawData.tid;

			Ability = new AbilityInfo(RawData.stats);
			_exp = "0";
		}

		public IdleNumber NeedExp()
		{
			IdleNumber needExp = (IdleNumber)(RawData.BaseExp * (1 + ((RawData.ExpPerLevel * Level) / 100f)));
			return needExp;
		}
	}
}


public class EventUpdateAdBuffArg
{
	public int Index;
	public RuntimeData.AdBuffInfo BuffInfo;
	public EventUpdateAdBuffArg(int index, RuntimeData.AdBuffInfo info)
	{
		Index = index;
		BuffInfo = info;
	}
}


public class BuffContainer : BaseContainer
{
	public List<RuntimeData.AdBuffInfo> adBuffList = new List<RuntimeData.AdBuffInfo>();


	public delegate void EventUpdateAdBuff(RuntimeData.AdBuffInfo info);
	public event EventUpdateAdBuff UpdateAdBuff;
	public override void Dispose()
	{

	}
	public override void FromJson(string json)
	{
		BuffContainer temp = CreateInstance<BuffContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref adBuffList, temp.adBuffList);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		SetListRawData(ref adBuffList, DataManager.Get<AdBuffDataSheet>().GetInfosClone());

	}
	public override void DailyResetData()
	{

	}
	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < adBuffList.Count; i++)
		{
			var adbuff = adBuffList[i];
			adbuff.UpdateData();

		}
	}


	public void OnUpdateBuff()
	{
		for (int i = 0; i < adBuffList.Count; i++)
		{
			var buff = adBuffList[i];
			buff.OnUpdate();
			UpdateAdBuff?.Invoke(buff);
		}
	}


	public event System.Action GainAdExp;
	public void GainExp()
	{
		for (int i = 0; i < adBuffList.Count; i++)
		{
			var adBuff = adBuffList[i];
			if (adBuff.IsActive)
			{
				adBuff.Exp += (IdleNumber)1;
			}
		}
		GainAdExp?.Invoke();
	}
}
