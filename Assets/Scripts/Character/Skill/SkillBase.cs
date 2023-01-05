using UnityEngine;



public class SkillBaseData
{
	public float cooltime;

	public SkillBaseData(float _cooltime)
	{
		cooltime = _cooltime;
	}
}

public abstract class SkillBase
{
	/// <summary>
	/// 해당 스킬을 사용하는 캐릭터
	/// </summary>
	public Character owner;


	/// <summary>
	/// 스킬 아이콘
	/// </summary>
	public abstract string iconPath { get; }


	/// <summary>
	/// 스킬사용시간. Action()을 호출하면 this.skillUseRemainTime에 이 값이 들어간다.
	/// </summary>
	public abstract float skillUseTime { get; }

	/// <summary>
	/// 기본 쿨타임
	/// </summary>
	public float cooltime { get; private set; }
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


	public Color fontColor;

	public SkillBase(SkillBaseData _sklllData)
	{
		cooltime = _sklllData.cooltime;
	}

	/// <summary>
	/// SkillModule에서만 초기화시 딱 한번 호출
	/// </summary>
	public void SetCharacter(Character _character)
	{
		owner = _character;

		SetCooltime();
		coolDowning = true;
	}

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

		// 스턴 상태
		bool check_no_stun = owner.conditionModule.HasCondition(UnitCondition.Stun) == false;

		// 공격중일때만 필요한경우 
		bool check_target = needAttackState ? owner.currentState == StateType.ATTACK : true;



		return check_cooltime && check_no_stun && check_target; // <- 헷갈릴 수 있으니 무조건 AND연산으로 처리할수 있게 해주세요
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
