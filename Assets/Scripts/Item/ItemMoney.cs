using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoney : ItemBase
{
	// 리필 정보
	private ItemRefillData refillData;

	public TimeRefill refill = new TimeRefill();
	public TimeReset reset = new TimeReset();

	private int refillMax = 60;

	public bool Refillable
	{
		get
		{
			return refillData != null && refillData.refill_isUse;
		}
	}

	public bool Resetable
	{
		get
		{
			return refillData != null && refillData.reset_isUse;
		}
	}


	public int RefillMax
	{
		get
		{
			if(refillData != null)
			{
				refillMax = Mathf.Max(0, refillData.systemMaximumAmount);
			}

			return refillMax;
		}
	}

	public int SystemMax
	{
		get
		{
			if(refillData != null)
			{
				return refillData.systemMaximumAmount;
			}

			return int.MaxValue;
		}
	}

	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = new VResult();
		vResult = base.Setup(_instantItem);
		if(vResult.Fail())
		{
			return vResult;
		}

		SetupData();
		if (Refillable)
		{
			if (_instantItem.nextRefillTime.IsNullOrWhiteSpace())
			{
				DateTime nextRefillTime = TimeManager.it.server_utc.AddMinutes(refillData.refill_intervalMinutes);
				instantItem.nextRefillTime = TimeUtil.DateTimeToString(nextRefillTime);
			}

			refill.Initialize(_instantItem.nextRefillTime, refillData.refill_intervalMinutes, refillData.refill_amount, RefillMax);
		}

		if(Resetable)
		{
			if(_instantItem.nextResetTime.IsNullOrWhiteSpace())
			{
				if(_instantItem.nextResetTime.IsNullOrWhiteSpace())
				{
					_instantItem.nextResetTime = TimeUtil.DateTimeToString(TimeUtil.TodayResetUtcTime());
				}

				reset.Initialize(instantItem.nextResetTime);
			}
		}

		return vResult.SetOk();
	}

	private void SetupData()
	{
		if (data.itemRefillTid != 0)
		{
			refillData = DataManager.Get<ItemRefillDataSheet>().Get(data.itemRefillTid);
		}
	}

	public bool ProcessUpdate(out RefillResult _outUpdateResult)
	{
		_outUpdateResult = null;
		RefillResult refillResult = null;
		bool needUpdate = false;


		if(Refillable && refill.ProcessUpdate())
		{
			var refillPoint = refill.CalcRefillPoint(Count.GetValueToInt());
			refillResult = new RefillResult(Tid, refillPoint.point, refillPoint.nextDate);
			needUpdate = true;
		}

		// 동시 발생과 Reset 발생에 대해서 처리하고
		// Reset이 없으면 Refill을 결과로 처리한다
		if(Resetable && reset.ProcessUpdate())
		{
			var resetPoint = reset.CalcNextReset(Count.GetValueToInt(), refillData.reset_amount, refillData.reset_setHour, refillData.reset_setMinute);
			if(refillResult != null)
			{
				_outUpdateResult = new RefillResult(RefillType.Both, Tid, resetPoint.point, refillResult.nextRefill, resetPoint.nextDate);
			}
			else // only reset
			{
				_outUpdateResult = new RefillResult(RefillType.Reset, Tid, resetPoint.point, resetPoint.nextDate);
			}

			needUpdate = true;
		}
		else
		{
			_outUpdateResult = refillResult;
		}

		return needUpdate;
	}

	// 강제로 다음 리필시간을 현재 시간으로 부터 계산해준다.
	// 쿨타임 초기화 아이템 사용
	public void ForceResetNextRefill(out InstantItem _outResetResult)
	{
		_outResetResult = DeepClone();
		_outResetResult.nextRefillTime = TimeUtil.DateTimeToString(refill.RefillTimeFromNow());
	}


	// ====================================================================
	// 최대갯수 보다 많이 보유했다가, 최대 갯수보다 적게 보유하는 시점이 refill 갱신 시점이다.
	//	> 사용시점에 Refill 시간의 시작을 업데이트 해야 한다.
	// ====================================================================
	public bool PrepareNextRefillUpdate(int _consumeAmount, out string _outNextRefillTime)
	{
		_outNextRefillTime = string.Empty;
		if(Refillable)
		{
			int currentCount = Count.GetValueToInt();
			int refillMaxCount = refillMax;
			if(currentCount >= refillMaxCount)
			{
				if ((currentCount - _consumeAmount) < refillMaxCount)
				{
					_outNextRefillTime = TimeUtil.DateTimeToString(refill.RefillTimeFromNow());
					return true;
				}
			}
		}

		return false;
	}

	/// <summary>
	/// 다음 리필 시간 갱신
	/// </summary>
	public void SetNextRefill(string _nextRefillDate)
	{
		instantItem.nextRefillTime = _nextRefillDate;
		refill.CommitData(TimeUtil.StringToDateTime(_nextRefillDate));
	}

	public void SetNextReset(string _nextResetDate)
	{
		instantItem.nextResetTime = _nextResetDate;
		reset.CommitData(TimeUtil.StringToDateTime(_nextResetDate));
	}


	public override string ToString()
	{
		return string.Format($"[{ItemName}({Tid})] count: {Count.ToString()}, refill:{Refillable}, reset:{Resetable}, nextRefill:{instantItem.nextRefillTime}, nextReset:{instantItem.nextResetTime}");
	}
}
