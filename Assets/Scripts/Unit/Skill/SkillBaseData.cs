using System;
using UnityEngine;

public class SkillBaseData : ScriptableObject
{
	/// <summary>
	/// tid
	/// </summary>
	[SerializeField] public Int64 tid;

	/// <summary>
	/// 스킬 프리셋
	/// </summary>
	[SerializeField] public string skillPreset;

	/// <summary>
	/// 타게팅 종류
	/// </summary>
	[Tooltip("타게팅 종류")]
	[SerializeField] public TargetingType targetingType = TargetingType.Default;

	public string icon => tid.ToString();
}

public abstract class SkillBase
{
	/// <summary>
	/// 해당 스킬을 사용하는 캐릭터
	/// </summary>
	public Unit owner;

	/// <summary>
	/// 스킬명(UI표시용)
	/// </summary>
	public virtual string name => sheetData.skillName;

	/// <summary>
	/// 스킬 TID
	/// </summary>
	public Int64 tid => skillData.tid;

	/// <summary>
	/// 스킬 프리셋 이름
	/// </summary>
	public string preset => skillData.skillPreset;

	/// <summary>
	/// 스킬 아이콘
	/// </summary>
	public string skillIcon => skillData.icon;


	/// <summary>
	/// 스킬사용시간. Action()을 호출하면 this.skillUseRemainTime에 이 값이 들어간다.
	/// </summary>
	public abstract float skillUseTime { get; }

	/// <summary>
	/// 기본 쿨타임
	/// </summary>
	public virtual float cooltime => sheetData.cooltime;
	/// <summary>
	/// 쿨타임 계산이 진행중이다.
	/// </summary>
	public bool coolDowning;
	/// <summary>
	/// 쿨타임 남은시간
	/// </summary>
	public float remainCooltime;
	/// <summary>
	/// 이 스킬을 사용하는데 필요한 시간.
	/// 이 값이 0보다 크면 스킬 사용중이다.
	/// </summary>
	public float skillUseRemainTime { get; set; }

	/// <summary>
	/// 쿨타임 감소 속도
	/// </summary>
	public virtual float cooltimeTimeScale => 1;

	/// <summary>
	/// 타겟이 있어야만 스킬을 사용할 수 있다
	/// </summary>
	public virtual bool needAttackState => true;

	/// <summary>
	/// UI에 표시되는 스킬게이지
	/// </summary>
	public float uiSkillGauge => Mathf.Clamp01(1 - (remainCooltime / cooltime));


	public long skillEffectTid => sheetData.skillEffectTid;
	public Color fontColor;
	private SkillBaseData skillData;
	private SkillData sheetData;



	public SkillBase(SkillBaseData _skillData)
	{
		skillData = _skillData;

		if (skillData is DefaultAttackData)
		{
			sheetData = DataManager.Get<SkillDataSheet>().Get("default");
		}
		else
		{
			sheetData = DataManager.Get<SkillDataSheet>().Get(skillData.tid);
		}


		if (sheetData == null)
		{
			VLog.LogError($"스킬정보가 데이터시트에 없음. tid: {skillData.tid}");
		}
	}

	/// <summary>
	/// SkillModule에서만 초기화시 딱 한번 호출
	/// </summary>
	public void SetUnit(Unit _unit)
	{
		owner = _unit;

		CalculateSkillLevelData(1);

		SetCooltime();
		coolDowning = true;
		skillUseRemainTime = 0;
	}

	/// <summary>
	/// 스킬 레벨이 셋팅될때 호출. 레벨별 대미지 수치등을 캐싱할때 사용
	/// </summary>
	public abstract void CalculateSkillLevelData(Int32 _skillLevel);

	/// <summary>
	/// 스킬 사용 준비가 되었다.
	/// </summary>
	public virtual void Ready()
	{
		coolDowning = false;
	}

	/// <summary>
	/// 스킬을 사용한다.
	/// </summary>
	public virtual void Action()
	{
		SetCooltime();
		coolDowning = true;

		//// 기본공격은 로그 표시안함
		//if (!(this is DefaultAttack))
		//    VLog.SkillLog("Use Skill. " + this.skillKey);
	}

	/// <summary>
	/// 스킬을 사용할 수 있는지
	/// </summary>
	/// <returns></returns>
	public virtual bool Usable()
	{
		// 쿨타임
		bool check_cooltime = remainCooltime <= 0;

		// 공격중일때만 필요한경우 
		bool check_target = needAttackState ? owner.currentState == StateType.ATTACK : true;

		// 스턴 상태이상
		bool stun = owner.conditionModule.HasCondition(UnitCondition.Stun) == false;


		return check_cooltime && check_target && stun; // <- 헷갈릴 수 있으니 무조건 AND연산으로 처리할수 있게 해주세요
	}

	/// <summary>
	/// 쿨타임을 적용한다
	/// </summary>
	public void SetCooltime()
	{
		remainCooltime = cooltime;
		skillUseRemainTime = skillUseTime;
	}

	public void UpdateCoolTime(float _dt)
	{
		float totalTime = _dt * cooltimeTimeScale;

		if (skillUseRemainTime > 0)
		{
			// 스킬이 지속되는동안엔 쿨타임 감소가 되지 않는다.
			skillUseRemainTime -= totalTime;
		}
		else
		{
			// 스킬 사용시간이 끝나면 쿨타임을 감소시킨다
			remainCooltime -= totalTime;
			if (remainCooltime <= 0)
			{
				Ready();
			}
		}
	}
}
