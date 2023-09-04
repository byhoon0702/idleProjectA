using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RuntimeData
{
	[System.Serializable]
	public class AdvancementInfo : BaseInfo
	{
		[SerializeField] private bool _gotAdvancement;
		public bool GotAdvancement => _gotAdvancement;
		public List<AbilityInfo> AbilityList { get; private set; }
		public AdvancementData rawData { get; private set; }

		public CostumeItemObject Costume { get; private set; }
		public CurrencyItemObject Currency { get; private set; }

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);



			AdvancementInfo temp = info as AdvancementInfo;
			_gotAdvancement = temp._gotAdvancement;
		}

		public override void SetRawData<T>(T data) where T : class
		{
			rawData = data as AdvancementData;
			tid = rawData.tid;
			AbilityList = new List<AbilityInfo>();

			for (int i = 0; i < rawData.stats.Count; i++)
			{
				AbilityList.Add(new AbilityInfo(rawData.stats[i]));
			}
			_gotAdvancement = false;
		}

		public override void UpdateData()
		{
			Costume = PlatformManager.UserDB.costumeContainer.GetScriptableObject<CostumeItemObject>(rawData.costumeTid);
			Currency = PlatformManager.UserDB.inventory.GetScriptableObject<CurrencyItemObject>(rawData.currencyTid);
		}

		public void Advancement()
		{
			_gotAdvancement = true;

			PlatformManager.UserDB.inventory.FindCurrency(Currency.currencyType).Pay((IdleNumber)rawData.currencyValue);
		}
	}
}
