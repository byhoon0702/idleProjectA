using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class CurrencyInfo : ItemInfo
	{
		[JsonProperty][SerializeField] private IdleNumber value;
		[JsonIgnore] public override IdleNumber Value => value;
		[JsonIgnore] public IdleNumber max { get; private set; }
		[JsonIgnore] public CurrencyType type { get; private set; }
		[JsonIgnore] public CurrencyData rawData { get; private set; }
		[JsonIgnore] public CurrencyItemObject itemObject { get; private set; }
		public CurrencyInfo()
		{

		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as CurrencyData;
			tid = rawData.tid;
			value = (IdleNumber)0;
			max = (IdleNumber)rawData.maxValue;
			type = rawData.type;
		}


		public bool Pay(IdleNumber cost)
		{
			if (value - cost < 0)
			{
				return false;
			}

			value -= cost;
			return true;
		}

		public bool Earn(IdleNumber money)
		{
			if (value + money > max)
			{
				value = max;
				return true;
			}

			value += money;
			return true;
		}


	}
}


