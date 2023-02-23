using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCore : ItemBase
{
	public override int MaxLevel => UserInfo.UserGrade.coreAbilityMaxLevel;






	public override VResult Setup(InstantItem _instantItem)
	{
		VResult vResult = base.Setup(_instantItem);

		if (vResult.Fail())
		{
			return vResult;
		}

		return vResult.SetOk();
	}


	public bool Levelupable()
	{
		return IsMaxLv == false;
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})], Grade: {UserInfo.UserGrade.grade}, lv: {Level})";
	}
}
