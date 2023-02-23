using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCallbacks
{
	public delegate void ItemChanged(List<long> _changedItems);
	public delegate void LevelupChanged(int _beforeLv, int _afterLv);
	public delegate void TotalCombatChanged(IdleNumber _beforeCombat, IdleNumber _afterCombat);



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
		onLevelupChanged?.Invoke(_beforeLv, _afterLv);
	}


	/// <summary>
	/// 전투력 변경
	/// </summary>
	public static event TotalCombatChanged onTotalCombatChanged;
	public static void CallTotalCombatChanged(IdleNumber _beforeCombat, IdleNumber _afterCombat)
	{
		onTotalCombatChanged?.Invoke(_beforeCombat, _afterCombat);
	}
}
