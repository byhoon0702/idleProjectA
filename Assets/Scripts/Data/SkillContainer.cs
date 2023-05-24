using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using JetBrains.Annotations;

public abstract class SkillCooldown
{
	public Unit caster;
	public SkillSlot slot;
	public SkillCooldown(Unit _caster, SkillSlot _parent)
	{
		caster = _caster;
		slot = _parent;
	}
	public abstract void Begin();
	public abstract bool Cooldown();
	public abstract float Progress();
	public abstract bool IsFinish();
	public abstract void Reset();
}

public class SkillCoolTime : SkillCooldown
{
	public float coolTime;

	public SkillCoolTime(Unit caster, SkillSlot slot) : base(caster, slot) { }

	public override void Begin()
	{
		coolTime = slot.item.CooldownValue;

	}
	public override bool Cooldown()
	{
		coolTime -= Time.deltaTime;
		return coolTime <= 0;
	}
	public override float Progress()
	{
		return coolTime / slot.item.CooldownValue;
	}
	public override bool IsFinish()
	{
		return coolTime <= 0;
	}
	public override void Reset()
	{
		coolTime = 0;
	}
}
public class SkillCoolAttackCount : SkillCooldown
{
	public int attackCount { get { return caster.attackCount - beginCount; } }
	public int cooldownCount;
	private int beginCount;
	public SkillCoolAttackCount(Unit caster, SkillSlot slot) : base(caster, slot) { }
	public override void Begin()
	{
		beginCount = caster.attackCount;
		cooldownCount = Mathf.FloorToInt(slot.item.CooldownValue);
	}
	public override bool Cooldown()
	{
		return attackCount >= cooldownCount;

	}
	public override float Progress()
	{
		return (cooldownCount - attackCount) / (float)cooldownCount;
	}
	public override bool IsFinish()
	{
		return attackCount >= cooldownCount;
	}
	public override void Reset()
	{
		cooldownCount = Mathf.FloorToInt(slot.item.CooldownValue);
	}
}

public class SkillCoolHitCount : SkillCooldown
{
	public int hitCount
	{
		get
		{
			return caster.hitCount - beginCount;
		}
	}
	public int cooldownCount;
	private int beginCount;
	public SkillCoolHitCount(Unit caster, SkillSlot slot) : base(caster, slot) { }
	public override void Begin()
	{
		beginCount = caster.hitCount;
		cooldownCount = Mathf.FloorToInt(slot.item.CooldownValue);
	}
	public override bool Cooldown()
	{
		return hitCount >= cooldownCount;
	}
	public override float Progress()
	{
		return (cooldownCount - hitCount) / (float)cooldownCount;
	}
	public override bool IsFinish()
	{
		return hitCount >= cooldownCount;
	}
	public override void Reset()
	{
		cooldownCount = 0;
	}
}

public class SkillCoolKillCount : SkillCooldown
{

	private int cooldownCount;
	private int beginCount;
	public int killCount
	{
		get
		{
			return caster.killCount - beginCount;
		}
	}
	public SkillCoolKillCount(Unit caster, SkillSlot slot) : base(caster, slot) { }
	public override void Begin()
	{
		beginCount = caster.killCount;
		cooldownCount = Mathf.FloorToInt(slot.item.CooldownValue);
	}

	public override bool Cooldown()
	{
		return killCount >= cooldownCount;
	}
	public override float Progress()
	{
		return (cooldownCount - killCount) / (float)cooldownCount;
	}
	public override bool IsFinish()
	{
		return killCount >= cooldownCount;
	}
	public override void Reset()
	{
		cooldownCount = 0;
	}
}


[System.Serializable]
public class SkillSlot : DataSlot
{
	public int index;

	public Unit owner;
	public RuntimeData.SkillInfo item;
	public SkillCooldown cooldown;

	private float globalCooldownProgress;
	public float GlobalCooldownProgress => globalCooldownProgress;
	[SerializeField] private long tid;
	public long itemTid => tid;

	public Sprite icon
	{
		get
		{
			if (item == null || item.icon == null)
			{
				return null;
			}
			return item.icon;
		}
	}
	public void Use()
	{
		cooldown?.Begin();
	}
	public void SetGlobalCoolDown()
	{
		globalCooldownProgress = 1f;
	}

	public bool IsUsable()
	{
		if (item == null || item.itemObject == null)
		{
			return false;
		}

		return true;
	}
	public void Unequip()
	{
		if (item != null)
		{
			item.isEquipped = false;
		}
		item = null;
	}
	public bool Equip(Unit _owner, long _tid)
	{
		if (_tid == 0)
		{
			return false;
		}
		owner = _owner;
		var info = GameManager.UserDB.skillContainer.skillList.Find(x => x.tid == _tid);

		item = info;
		tid = _tid;
		Equip();

		return true;
	}

	private void Equip()
	{
		switch (item.CooldownType)
		{
			case SkillCooldownType.KILLCOUNT:
				cooldown = new SkillCoolKillCount(owner, this);
				break;
			case SkillCooldownType.HITCOUNT:
				cooldown = new SkillCoolHitCount(owner, this);
				break;
			case SkillCooldownType.ATTACKCOUNT:
				cooldown = new SkillCoolAttackCount(owner, this);
				break;
			default:
				cooldown = new SkillCoolTime(owner, this);
				break;

		}

		if (item.isEquipped == false)
		{
			Use();
		}
		item.isEquipped = true;
		owner?.skillModule.AddSkill(this);
	}

	public bool Equip(RuntimeData.SkillInfo info)
	{
		if (item != null && item.tid == info.tid)
		{
			return false;
		}

		item = info;

		Equip();

		return true;
	}

	public void Cooldown()
	{
		cooldown?.Cooldown();
	}

	public void GlobalCooldown(float deltaTime)
	{
		globalCooldownProgress -= deltaTime;
	}

	public bool IsReady()
	{
		bool iscooldown = true;
		if (cooldown != null)
		{
			iscooldown = cooldown.IsFinish();
		}
		return IsUsable() && iscooldown && globalCooldownProgress <= 0;
	}

	public void ResetCoolDown(Unit caster)
	{
		owner = caster;
		item.isEquipped = true;
		switch (item.CooldownType)
		{
			case SkillCooldownType.KILLCOUNT:
				cooldown = new SkillCoolKillCount(owner, this);
				break;
			case SkillCooldownType.HITCOUNT:
				cooldown = new SkillCoolHitCount(owner, this);
				break;
			case SkillCooldownType.ATTACKCOUNT:
				cooldown = new SkillCoolAttackCount(owner, this);
				break;
			default:
				cooldown = new SkillCoolTime(owner, this);
				break;

		}

		cooldown?.Reset();
		globalCooldownProgress = 0;
	}

	public void Trigger(Unit caster)
	{
		item.itemObject.Trigger(caster, item, caster.HitInfo);
		Use();
	}
}

[CreateAssetMenu(fileName = "SkillContainer", menuName = "ScriptableObject/Container/SkillContainer", order = 1)]
public class SkillContainer : BaseContainer
{

	public bool isAutoSkill;
	public bool isAutoHyper;
	public List<RuntimeData.SkillInfo> skillList;
	public SkillSlot this[int index]
	{
		get
		{
			return skillSlot[index];
		}
	}

	public SkillSlot[] skillSlot;


	public void SetList(List<RuntimeData.SkillInfo> _skillList)
	{
		skillList = _skillList;
	}
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		SkillContainer temp = ScriptableObject.CreateInstance<SkillContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		for (int i = 0; i < skillList.Count; i++)
		{
			if (i < temp.skillList.Count)
			{
				skillList[i].Load(temp.skillList[i]);
			}
		}

		for (int i = 0; i < skillSlot.Length; i++)
		{
			if (i < temp.skillSlot.Length)
			{
				skillSlot[i].Equip(null, temp.skillSlot[i].itemTid);
			}

		}

		temp = null;
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		int last = 6;
		skillSlot = new SkillSlot[last];

		LoadScriptableObject();
		isAutoSkill = false;
		isAutoHyper = false;

		SetItemListRawData(ref skillList, DataManager.Get<SkillDataSheet>().GetInfosClone());

		//임시로 데이터 설정
		for (int i = 0; i < last; i++)
		{
			skillSlot[i] = new SkillSlot();
			skillSlot[i].index = i;
		}
	}

	public bool Equip(Unit owner, long tid)
	{
		for (short i = 0; i < skillSlot.Length; i++)
		{
			if (skillSlot[i].item == null)
			{
				skillSlot[i].Equip(owner, tid);
				return true;
			}
		}
		return false;
	}

	public void UnEquip(long tid)
	{
		for (short i = 0; i < skillSlot.Length; i++)
		{
			if (skillSlot[i].item == null)
			{

				continue;
			}
			if (skillSlot[i].itemTid == tid)
			{
				skillSlot[i].Unequip();
				break;
			}
		}
	}


	public void GlobalCooldown()
	{
		for (int i = 0; i < skillSlot.Length; i++)
		{
			skillSlot[i].SetGlobalCoolDown();
		}

	}

	public RuntimeData.SkillInfo Get(long tid)
	{
		for (int i = 0; i < skillList.Count; i++)
		{
			if (skillList[i].tid == tid)
			{
				return skillList[i];
			}
		}
		return null;
	}



	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var skillList = Resources.LoadAll<NewSkill>($"RuntimeDatas/Skills/Items");
		AddDictionary(scriptableDictionary, skillList);
	}
}
