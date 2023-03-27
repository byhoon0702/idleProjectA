using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUserExp : ItemBase
{
	public override int Level => UserDataCalculator.GetLevelInfo(Exp).level;
	public LevelExpInfo levelInfo => UserDataCalculator.GetLevelInfo(Exp);

	public override void AddExp(long _exp)
	{
		int beforeLv = Level;
		base.AddExp(_exp);
		int afterLv = Level;

		if (beforeLv != afterLv)
		{
			EventCallbacks.CallLevelupChanged(beforeLv, afterLv);
			UserInfo.CalculateTotalCombatPower();
		}
	}
}
