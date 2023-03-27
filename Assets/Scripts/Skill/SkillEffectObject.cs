
using UnityEngine;

using UnityEngine.Pool;

public class SkillEffectObject : MonoBehaviour
{

	[SerializeField] private SkillEffectData mData;
	public SkillEffectData data
	{
		get
		{
			return mData;
		}
	}

	[HideInInspector]
	public int index;

	public Vector3 pos
	{ get; private set; }
	public Vector3 targetPos
	{ get; private set; }

	public HittableUnit targetUnit
	{ get; private set; }

	public AffectedInfo hitinfo
	{
		private set;
		get;
	}
	public UnitBase caster
	{
		private set;
		get;
	}

	public float duration
	{
		get;
		private set;
	}

	//스킬 정보
	[HideInInspector] public SkillInfoObject skillInfoObject;

	public BaseSkillAction mainSkill;

	private bool isFire;
	private bool isDependent;

	private float elapsedTime;

	private SkillBase skillBase;
	private SkillData skillData;
	private bool isCancel;

	public void SetData(SkillBase _skillBase)
	{
		SetData(_skillBase.skillEffectData, _skillBase.skillInfoObject, _skillBase.sheetData);
	}
	public void SetData(SkillEffectData _data, SkillInfoObject _skillInfoObject = null, SkillData _skillData = null)
	{
		if (_data == null)
		{
			return;
		}
		mData = _data;
		skillData = _skillData;
		skillInfoObject = _skillInfoObject;

		if (skillInfoObject == null || (skillInfoObject.name != mData.skillInfoObject))
		{
			skillInfoObject = (SkillInfoObject)Resources.Load($"{PathHelper.skillAbilitySOPath}/{mData.skillInfoObject}");
		}
		isDependent = mData.isDependent;
		duration = mData.duration > 0 ? mData.duration : 1;
	}

	public void Cancel()
	{
		isCancel = true;
		mainSkill?.Cancel();
	}

	public void Reset()
	{
		isFire = false;
		elapsedTime = 0;
		isCancel = false;
		index = 0;
	}

	public bool isTargetAlive()
	{

		if (targetUnit == null)
		{
			return false;
		}

		return targetUnit.IsAlive();
	}

	public void UpdateFromOutSide(float time)
	{
		if (isDependent == false)
		{
			return;
		}

		if (isCancel)
		{
			mainSkill?.OnUpdate(this, time);
		}

		if (isFire)
		{
			mainSkill?.OnUpdate(this, time);
		}

	}

	private void Update()
	{
		if (isCancel)
		{
			Cancel();
			return;
		}
		if (isDependent)
		{
			return;
		}

		if (isFire)
		{
			if (elapsedTime > duration)
			{
				Reset();
				Release();
				return;
			}

			mainSkill?.OnUpdate(this, elapsedTime);

			elapsedTime += Time.deltaTime;
		}
	}

	private void OnSkillStart(Vector3 pos, Vector3 targetPos/*, StatInfo skillValue, int skillLevel = 1*/)
	{
		Reset();
		isFire = true;
		this.pos = pos;
		this.targetPos = targetPos;

		targetUnit = null;
		transform.position = pos;

		if (caster != null)
		{
			targetUnit = caster.target;

			transform.position = caster.position;
		}

		if (skillInfoObject == null)
		{
			skillInfoObject = (SkillInfoObject)Resources.Load($"{PathHelper.skillAbilitySOPath}/{mData.skillInfoObject}");
		}

		if (skillInfoObject.mainSkillInfo != null)
		{
			mainSkill = CreateSkillAction(skillInfoObject.mainSkillInfo.skillType);
			mainSkill.OnSet(this, skillInfoObject.mainSkillInfo);

		}
		//if (skillInfoObject.subSkillInfo != null)
		//{
		//	subSkill = CreateSkillAction(skillInfoObject.subSkillInfo.skillType);

		//	subSkill?.OnSet(this, skillInfoObject.subSkillInfo);
		//}
	}

	public BaseSkillAction CreateSkillAction(SkillType skillType)
	{
		BaseSkillAction skillAction = null;
		switch (skillType)
		{
			case SkillType.ATTACK:

				skillAction = new AttackSkillAction();
				break;
			case SkillType.HEAL:
				skillAction = new HealSkillAction();
				break;
			case SkillType.BUFF:
				skillAction = new BuffSkillAction();
				break;
			case SkillType.DEBUFF:
				skillAction = new DebuffSkillAction();
				break;

		}
		return skillAction;
	}



	public void SetAffectedInfo(AffectedInfo _affectedInfo)
	{
		hitinfo = _affectedInfo;
	}

	public void OnSkillStart(UnitBase _attacker, Vector3 pos, Vector3 targetPos)
	{
		if (isFire)
		{
			return;
		}
		gameObject.SetActive(true);
		caster = _attacker;
		targetUnit = caster.target;

		OnSkillStart(pos, targetPos);
	}

	//public void OnSkillStart(SkillBase _skillBase, Vector3 _pos, Vector3 _targetPos)
	//{
	//	if (isFire)
	//	{
	//		return;
	//	}
	//	gameObject.SetActive(true);
	//	skillBase = _skillBase;
	//	caster = skillBase.owner;
	//	targetUnit = caster.target;

	//	OnSkillStart(_pos, _targetPos);
	//}


	public void OnSkillStart(UnitBase _attacker, UnitBase _target, IdleNumber power)
	{
		if (isFire)
		{
			return;
		}

		caster = _attacker;
		targetPos = _target.CenterPosition;
		//OnSkillStart(caster.CenterPosition, targetPos, power);
	}


	public void OnSkillStart(UnitBase _attacker, Vector3 targetPos, IdleNumber power)
	{
		if (isFire)
		{
			return;
		}

		caster = _attacker;

		OnSkillStart(caster.CenterPosition, targetPos);
	}

#if UNITY_EDITOR
	public void OnSkillStartEditor(UnitBase _attacker, Vector3 pos, Vector3 targetPos)
	{
		if (isFire)
		{
			return;
		}

		isDependent = false;
		caster = _attacker;

		//OnSkillStart(pos, targetPos, caster != null ? caster.AttackPower : new IdleNumber(1000));
	}
#endif
	public void Release()
	{
		Reset();
		gameObject.SetActive(false);
	}
}
