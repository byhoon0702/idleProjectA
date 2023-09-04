using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCallbacks
{
	public delegate void OnCurrencyChanged(CurrencyType type);
	public delegate void ItemChanged(List<long> _changedItems);
	public delegate void LevelupChanged(int _beforeLv, int _afterLv);
	public delegate void TotalCombatChanged(IdleNumber _beforeCombat, IdleNumber _afterCombat);

	public static event OnCurrencyChanged onCurrencyChanged;
	public static void CallCurrencyChanged(CurrencyType type)
	{
		onCurrencyChanged?.Invoke(type);
	}
	/// <summary>
	/// 아이템이 변화했을때 호출됨
	/// </summary>
	public static event ItemChanged onItemChanged;
	public static void CallItemChanged(List<long> _changedItems)
	{
		onItemChanged?.Invoke(_changedItems);
	}


	/// <summary>
	/// 유저 레벨 변경
	/// </summary>
	public static event LevelupChanged onLevelupChanged;
	public static void CallLevelupChanged(int _beforeLv, int _afterLv)
	{
		VLog.Log($"[레벨 변경] {_beforeLv} -> {_afterLv}");
		onLevelupChanged?.Invoke(_beforeLv, _afterLv);
	}


	/// <summary>
	/// 전투력 변경
	/// </summary>
	public static event TotalCombatChanged onTotalCombatChanged;
	public static void CallTotalCombatChanged(IdleNumber _beforeCombat, IdleNumber _afterCombat)
	{
		VLog.Log($"[전투력 변경] {_beforeCombat.ToString()} -> {_afterCombat.ToString()}");
		onTotalCombatChanged?.Invoke(_beforeCombat, _afterCombat);
	}
}
