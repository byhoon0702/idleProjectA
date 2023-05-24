using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Advancement Container", menuName = "ScriptableObject/Container/Advancement Container", order = 1)]
public class AdvancementContainer : BaseContainer
{

	[SerializeField] private RuntimeData.AdvancementInfo info;
	public RuntimeData.AdvancementInfo Info => info;
	public override void FromJson(string json)
	{
		AdvancementContainer temp = CreateInstance<AdvancementContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		info.Load(temp.info);
	}

	public override void Load(UserDB _parent)
	{
		var list = DataManager.Get<UnitDataSheet>().GetInfosClone();
		info = new RuntimeData.AdvancementInfo();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].type == UnitType.Player)
			{
				info.SetRawData(list[i]);
				break;
			}
		}
	}

	public void RemoveModifiers(UnitStats userStats, Unit owner)
	{
		if (info.advancementInfo == null)
		{
			return;
		}
		for (int i = 0; i < info.advancementInfo.stats.Count; i++)
		{
			var stat = info.advancementInfo.stats[i];
			userStats.RemoveModifier(stat.type, this);
		}
	}

	public void AddModifiers(UnitStats userStats, Unit owner)
	{
		if (info.advancementInfo == null)
		{
			return;
		}
		for (int i = 0; i < info.advancementInfo.stats.Count; i++)
		{
			var stat = info.advancementInfo.stats[i];
			GameManager.UserDB.AddModifiers(false, stat.type, new StatsModifier((IdleNumber)stat.value, StatModeType.FlatAdd, this));
		}
	}
	public void LevelUp(Unit owner, UnitAdvancementInfo _info)
	{
		info.LevelUp(owner, _info);
		info.ChangeCostume(owner, _info);
		(owner as PlayerUnit).UpdateMaxHp();
		owner.Heal(new HealInfo(AttackerType.Player, owner.MaxHp));
	}

	public void ChangeCostume(Unit owner, UnitAdvancementInfo _info)
	{
		info.ChangeCostume(owner, _info);

	}
	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
}
