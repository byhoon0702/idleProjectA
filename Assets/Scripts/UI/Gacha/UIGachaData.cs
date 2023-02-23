using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIGachaData
{
	private static VResult _result = new VResult();
	public GachaData gachaData;
	public GachaEntryData gachaEntryData;


	public GachaType GachaType => gachaData.gachaType;
	public int GachaLevel
	{
		get
		{
			switch (GachaType)
			{
				case GachaType.Equip:
					return UserInfo.GachaEquipLv;
				case GachaType.Skill:
					return UserInfo.GachaSkillLv;
				case GachaType.Pet:
					return UserInfo.GachaPetLv;
			}

			return 1;
		}
	}
	public GachaEntryProbabilityInfo[] Probabilities => gachaEntryData.probabilities;

	/// <summary>
	/// 소환버튼 누를수 있는 횟수
	/// </summary>
	public int AdSummonMaxCount
	{
		get
		{
			foreach (var v in gachaData.summonInfos)
			{
				if (v.summonType == GachaButtonType.Ads)
				{
					return v.summonMaxCount;
				}
			}

			return 0;
		}
	}



	public VResult Setup(GachaData _gachaData)
	{
		gachaData = _gachaData;
		gachaEntryData = DataManager.Get<GachaEntryDataSheet>().Get(gachaData.summonInfos[0].gachaEntryTid);
		if(gachaEntryData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"GachaEntryDataSheet. gachaTID: {gachaData.tid}, entryTid: {gachaData.summonInfos[0].gachaEntryTid}");
		}

		return _result.SetOk();
	}

	/// <summary>
	/// 소환버튼 활성화가 가능한지(데이터 조립여부 확인. 소환가능여부와는 상관없음)
	/// </summary>
	/// <returns></returns>
	public bool IsActiveSummonButton(GachaButtonType _buttonType)
	{
		foreach(var v in gachaData.summonInfos)
		{
			if(v.summonType == _buttonType)
			{
				return true;
			}
		}

		return false;
	}

	public long ConsumeItemTid(GachaButtonType _buttonType)
	{
		foreach(var v in gachaData.summonInfos)
		{
			if(v.summonType == _buttonType)
			{
				return v.itemTidSummon;
			}
		}

		return 0;
	}

	public IdleNumber ConsumeItemCount(GachaButtonType _buttonType)
	{
		foreach (var v in gachaData.summonInfos)
		{
			if (v.summonType == _buttonType)
			{
				return new IdleNumber(v.cost);
			}
		}

		return new IdleNumber(0);
	}

	/// <summary>
	/// 소환되는 아이템 개수
	/// </summary>
	public int SummonItemCount(GachaButtonType _buttonType)
	{
		int count = 0;
		int addCount = 0;//광고용
		int maxCount = 0;
		foreach (var v in gachaData.summonInfos)
		{
			if (v.summonType == _buttonType)
			{
				count = v.defaultCount;
				addCount = v.addCount;
				maxCount = v.maxCount;
				break;
			}
		}

		if (_buttonType == GachaButtonType.Ads)
		{
			switch (GachaType)
			{
				case GachaType.Equip:
					count += (UserInfo.GachaEquipAdSummonCount * addCount);
					break;
				case GachaType.Skill:
					count += (UserInfo.GachaSkillAdSummonCount * addCount);
					break;
				case GachaType.Pet:
					count += (UserInfo.GachaPetAdSummonCount * addCount);
					break;
			}
		}

		return Mathf.Min(count, maxCount);
	}

	public double GetProbability(Grade _grade, int _level)
	{
		foreach(var prob in Probabilities)
		{
			if(prob.grade == _grade)
			{
				return prob.GetRatio(_level);
			}
		}

		return 0;
	}

	public void SummonGacha(GachaButtonType _buttonType, Action<List<GachaResult>> onResult)
	{
		if(TryConsume(_buttonType) == false)
		{
			return;
		}


		int summonCount = SummonItemCount(_buttonType);
		List<GachaResult> outResult = new List<GachaResult>();

		switch (GachaType)
		{
			case GachaType.Equip:
				for (int i = 0 ; i < summonCount ; i++)
				{
					outResult.Add(GachaGenerator.GenerateEquip(GachaLevel, Probabilities));
				}
				break;
			case GachaType.Skill:
				for (int i = 0 ; i < summonCount ; i++)
				{
					outResult.Add(GachaGenerator.GenerateSkill(GachaLevel, Probabilities));
				}
				break;
			case GachaType.Pet:
				for (int i = 0 ; i < summonCount ; i++)
				{
					outResult.Add(GachaGenerator.GeneratePet(GachaLevel, Probabilities));
				}
				break;
		}

		// 경험치 누적
		if (_buttonType == GachaButtonType.Ads)
		{
			switch (GachaType)
			{
				case GachaType.Equip:
					UserInfo.IncGachaEquipAdSummonCount();
					break;
				case GachaType.Skill:
					UserInfo.IncGachaSkillAdSummonCount();
					break;
				case GachaType.Pet:
					UserInfo.IncGachaPetAdSummonCount();
					break;
				default:
					break;
			}
		}
		else
		{
			switch (GachaType)
			{
				case GachaType.Equip:
					UserInfo.AddGachaEquipExp(summonCount);
					break;
				case GachaType.Skill:
					UserInfo.AddGachaSkillExp(summonCount);
					break;
				case GachaType.Pet:
					UserInfo.AddGachaPetExp(summonCount);
					break;
				default:
					break;
			}
		}
		Inventory.it.AddItems(outResult);
		UserInfo.SaveUserData();
		onResult?.Invoke(outResult);
	}

	public bool TryConsume(GachaButtonType _buttonType)
	{
		long consumeTid = ConsumeItemTid(_buttonType);
		IdleNumber consumeCount = ConsumeItemCount(_buttonType);

		if (Inventory.it.CheckMoney(consumeTid, consumeCount).Ok())
		{
			if (Inventory.it.ConsumeItem(consumeTid, consumeCount).Ok())
			{
				return true;
			}
		}

		return false;
	}
}
