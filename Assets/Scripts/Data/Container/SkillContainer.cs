

using System.Collections.Generic;
using UnityEngine;


public abstract class SkillCooldown
{
	public Unit caster;
	public SkillSlot slot;
	public float coolTime { get; protected set; }
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
	public SkillCoolTime(Unit caster, SkillSlot slot) : base(caster, slot) { }
	private float modifiedCoolTime;
	public override void Begin()
	{
		if (caster is PlayerUnit)
		{
			PlayerUnit player = caster as PlayerUnit;
			float reduce = player.info.stats.GetValue(StatsType.Skill_Cooltime).GetValueFloat();
			if (reduce > 0)
			{
				modifiedCoolTime = slot.item.CooldownValue - (float)((slot.item.CooldownValue * reduce) / 100f);
			}
			else
			{
				modifiedCoolTime = slot.item.CooldownValue;
			}

		}

		else
		{
			modifiedCoolTime = slot.item.CooldownValue;
		}


		coolTime = modifiedCoolTime;
	}
	public override bool Cooldown()
	{
		coolTime -= Time.deltaTime;
		return coolTime <= 0;
	}
	public override float Progress()
	{
		return coolTime / modifiedCoolTime;
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
			if (item == null || item.IconImage == null)
			{
				return null;
			}
			return item.IconImage;
		}
	}
	public void Use()
	{
		cooldown?.Begin();
	}
	public void SetGlobalCoolDown()
	{
		globalCooldownProgress = 3f;
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
		tid = 0;
		owner?.skillModule.RemoveSkill(this);
		item = null;
	}
	public bool Equip(Unit _owner, long _tid)
	{
		if (_tid == 0)
		{
			return false;
		}
		owner = _owner;
		var info = PlatformManager.UserDB.skillContainer.skillList.Find(x => x.Tid == _tid);
		if (info == null)
		{
			return false;
		}
		item = info;
		tid = _tid;
		Equip();

		return true;
	}
	public bool Equip(Unit _owner, RuntimeData.SkillInfo _info)
	{
		if (_info == null)
		{
			return false;
		}
		owner = _owner;

		item = _info;
		tid = _info.Tid;
		Equip();

		return true;
	}

	private void Equip()
	{
		if (item == null)
		{
			return;
		}
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
		PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EQUIP_SKILL, item.Tid, (IdleNumber)1);
		if (item.isEquipped == false)
		{
			Use();
		}
		item.isEquipped = true;
		owner?.skillModule.AddSkill(this);
	}

	public bool Equip(RuntimeData.SkillInfo info)
	{
		if (item != null && item.Tid == info.Tid)
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
		if (caster.IsAlive() == false)
		{
			return;
		}

		item.itemObject.Trigger(caster, item);
	}

	public override void AddModifiers(UserDB userDB)
	{

	}



	public override void RemoveModifiers(UserDB userDB)
	{

	}
}

[CreateAssetMenu(fileName = "SkillContainer", menuName = "ScriptableObject/Container/SkillContainer", order = 1)]
public class SkillContainer : BaseContainer
{

	public const int needCount = 5;
	public const float levelLimit = 10;

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

	public SkillSlot[] petSkillSlot { get; private set; }
	public SkillSlot[] skillSlot;
	public override void Dispose()
	{

	}
	public IdleNumber GetNeedCount(int level)
	{
		return (IdleNumber)(level * needCount * (1 + Mathf.FloorToInt(level / levelLimit)));
	}

	public void SetList(List<RuntimeData.SkillInfo> _skillList)
	{
		skillList = _skillList;
	}

	public override void DailyResetData()
	{

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

		LoadListTidMatch(ref skillList, temp.skillList);

		for (int i = 0; i < skillSlot.Length; i++)
		{
			if (i < temp.skillSlot.Length)
			{
				skillSlot[i].Equip(null, temp.skillSlot[i].itemTid);
			}

		}

		isAutoSkill = temp.isAutoSkill;
		isAutoHyper = temp.isAutoHyper;

	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		int last = 4;
		skillSlot = new SkillSlot[last];

		LoadScriptableObject();
		isAutoSkill = false;
		isAutoHyper = false;

		SetListRawData(ref skillList, DataManager.Get<SkillDataSheet>().GetInfosClone());

		//임시로 데이터 설정
		for (int i = 0; i < last; i++)
		{
			skillSlot[i] = new SkillSlot();
			skillSlot[i].index = i;
		}

		petSkillSlot = new SkillSlot[3];
		for (int i = 0; i < 3; i++)
		{
			petSkillSlot[i] = new SkillSlot();
			petSkillSlot[i].index = i;
		}

	}


	public bool EquipPetSkill(Unit owner, int index, long tid)
	{
		if (index < 0 || index >= petSkillSlot.Length)
		{
			return false;
		}
		return petSkillSlot[index].Equip(owner, tid);
	}
	public void UnEquipPetSkill(int index, long tid)
	{
		if (index < 0 || index >= petSkillSlot.Length)
		{
			return;
		}
		petSkillSlot[index].Unequip();
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


	public void PetGlobalCooldown()
	{

		for (int i = 0; i < petSkillSlot.Length; i++)
		{
			petSkillSlot[i].SetGlobalCoolDown();
		}
	}
	public void GlobalCooldown()
	{
		for (int i = 0; i < skillSlot.Length; i++)
		{
			skillSlot[i].SetGlobalCoolDown();
		}

	}

	public override void UpdateData()
	{
		for (int i = 0; i < skillList.Count; i++)
		{
			skillList[i].UpdateData();
		}
	}

	public void AddSkill(long tid, int count)
	{
		var item = FindSKill(tid);
		item.unlock = true;
		if (item.Level == 0)
		{
			item.SetLevel(1);

		}
		item.CalculateCount(count);
	}

	public RuntimeData.SkillInfo FindSKill(long tid)
	{
		for (int i = 0; i < skillList.Count; i++)
		{
			if (skillList[i].Tid == tid)
			{
				return skillList[i];
			}
		}
		return null;
	}

	public bool SkillEvolution(RuntimeData.SkillInfo info)
	{
		if (info.EvolutionLevel >= PlatformManager.CommonData.SkillEvolutionNeedsList.Count)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_max_evolution"]);
			return false;
		}

		var need = PlatformManager.CommonData.SkillEvolutionNeedsList[info.EvolutionLevel];
		var currency = parent.inventory.FindCurrency(need.type);

		if (currency.Check(need.count) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_need_skill_evolution_item"]);
			return false;
		}

		currency.Pay(need.count);
		info.Evoltion();

		return true;
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var skillList = Resources.LoadAll<SkillCore>($"RuntimeDatas/Skills/Player");
		AddDictionary(scriptableDictionary, skillList);
		skillList = Resources.LoadAll<SkillCore>($"RuntimeDatas/Skills/Pet");
		AddDictionary(scriptableDictionary, skillList);
		skillList = Resources.LoadAll<SkillCore>($"RuntimeDatas/Skills/Hyper");
		AddDictionary(scriptableDictionary, skillList);
		skillList = Resources.LoadAll<SkillCore>($"RuntimeDatas/Skills/Enemy");
		AddDictionary(scriptableDictionary, skillList);
	}
}
