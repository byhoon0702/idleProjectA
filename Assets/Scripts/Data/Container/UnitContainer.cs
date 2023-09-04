using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainer : BaseContainer
{
	public override void Dispose()
	{

	}

	public override void FromJson(string json)
	{

	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var enemies = Resources.LoadAll<EnemyObject>("RuntimeDatas/Unit/Enemies");
		AddDictionary(scriptableDictionary, enemies);
	}

	public override string Save()
	{
		return "";
	}

	public override void UpdateData()
	{

	}
	public override void DailyResetData()
	{

	}
}
