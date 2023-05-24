
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using static UnityEngine.UI.GridLayoutGroup;

public enum NeutralizeType
{
	_NONE,
	KNOCKBACK,
	AIRBORNE,

}


public class NeutralizeMove
{
	public NeutralizeType type;
	public float power;
	public Vector3 dir;
	public float duration;
	public float additionalRatio;

	protected float elapsedTime;
	protected Transform transform;


	protected bool isLastHit = false;
	public void Set(Transform _transform, NeutralizeType _type, float _power, Vector3 _dir, float _duration, bool _isLastHit = true)
	{
		type = _type;
		power = _power;
		dir = _dir;
		duration = _duration;

		transform = _transform;
		isLastHit = _isLastHit;
		elapsedTime = 0;
	}

	public void IsLastHit(bool _isLastHit = true)
	{
		isLastHit = _isLastHit;
	}
	public virtual void AddPower(float _power)
	{
		if (power > 0)
		{
			power += _power * additionalRatio;
		}
		else
		{
			power = _power;
		}
	}
	public virtual void OnUpdate(float delta)
	{

		float force = Mathf.Lerp(power, 0, elapsedTime);
		transform.Translate(dir * force * delta);
		elapsedTime += delta;
	}

	public bool IsEnd()
	{
		return power <= 0;
	}
}

public class KnockbackMove : NeutralizeMove
{
	public void InstantlyMove(float power)
	{
		transform.Translate(dir * power, Space.Self);
	}
	public override void OnUpdate(float delta)
	{
		if (isLastHit == false)
		{
			return;
		}
		float force = power - (3f * elapsedTime);
		if (force < 0)
		{
			power = 0;
			return;
		}
		transform.Translate(dir * force * delta);
		elapsedTime += delta;
	}
}

public class AirborneMove : NeutralizeMove
{
	bool reachedMaximum = false;
	bool checkMaximum = false;

	public override void AddPower(float _power)
	{
		if (power > 0)
		{

			power += _power * additionalRatio;
		}
		else
		{
			power = _power;
		}
	}

	public void InstantlyMove(float power)
	{
		transform.Translate(dir * power, Space.Self);
	}
	public override void OnUpdate(float delta)
	{
		if (isLastHit == false)
		{
			return;
		}
		if (reachedMaximum)
		{
			duration -= delta;
			if (duration <= 0)
			{
				reachedMaximum = false;
			}
			return;
		}

		float force = power - (9.8f * elapsedTime);

		if (force < 0 && checkMaximum == false)
		{
			if (reachedMaximum == false)
			{
				reachedMaximum = true;
				checkMaximum = true;
				Debug.Log("최고 높이");
			}
		}

		if (transform.localPosition.y < 0)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
			power = 0;
			return;
		}
		transform.Translate(dir * force * delta, Space.Self);
		elapsedTime += delta;
	}
}





public abstract class Unit : HittableUnit
{
	public float shakeAmount = 0.3f;
	public Vector2 colliderSize = new Vector2(1f, 1.5f);

	public bool isBoss = false;

	protected bool isRewardable = true;

	public UnitBehavior unitBehavior;

	public int attackCount;
	public int killCount;
	public int hitCount;

	public List<NeutralizeMove> neutralizeMoves = new List<NeutralizeMove>();
	public List<AdditionalDamageModule> additionalDamageModules = new List<AdditionalDamageModule>();

	public void InitState()
	{
		fsmModule = GetComponent<UnitFsmModule>();
		fsmModule.Init(this);

		currentState = StateType.IDLE;
	}

	public virtual void Init()
	{
		// 스킬초기화
		attackCount = 0;
		hitCount = 0;
		killCount = 0;
		InitState();

		if (boxCollider2D == null)
		{
			boxCollider2D = transform.GetComponent<Collider2D>();
			if (boxCollider2D == null)
			{
				boxCollider2D = gameObject.AddComponent<Collider2D>();
			}
		}

		//boxCollider.isTrigger = true;
	}

	public void InitMode(string path, UnitInfo info)
	{
		unitMode?.OnInit(this);
		unitMode?.OnSpawn(path, info.resource);

		unitHyperMode?.OnInit(this);
		//unitHyperMode?.OnSpawn(path, info.hyperResource);

		unitHyperMode?.OnModeExit();
		unitMode?.OnModeEnter(StateType.IDLE);
		currentMode = unitMode;
	}
	public void ShakeUnit()
	{
		unitAnimation.transform.localPosition = UnityEngine.Random.insideUnitCircle * 0.3f;
		unitAnimation.transform.DOLocalMove(Vector3.zero, 0.1f);
	}
	public virtual bool TriggerSkill(SkillSlot skillSlot)
	{

		return true;
	}


	public virtual void ChangeCostume()
	{ }

	public virtual void NormalAttack()
	{
		unitAnimation.SetParameter("attackIndex", UnityEngine.Random.Range(0, 3));
		unitAnimation.SetParameter("attackSpeed", AttackSpeed);
		currentMode?.OnAttackNormal();
	}

	public virtual void ChangeState(StateType stateType, bool force = false)
	{
		if (currentState == StateType.DEATH)
		{
			return;
		}

		if (stateType == StateType.DEATH && isRewardable)
		{
			isRewardable = false;
			//StageManager.it.CheckKillRewards(UnitType, transform);
		}
		currentState = stateType;
		VLog.AILog($"{NameAndId} StateChange {currentState} -> {stateType}");
		fsmModule?.ChangeState(stateType, force);
	}
	public virtual void ActiveHyperEffect()
	{ }

	public virtual void InactiveHyperEffect()
	{
	}
	public override void Hit(HitInfo _hitInfo)
	{
		if (Hp > 0)
		{
			//if (_hitInfo.ShakeCamera)
			//{
			//	SceneCamera.it.ShakeCamera();
			//}
			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower, CenterPosition, CenterPosition, _hitInfo.criticalType);
			ShakeUnit();
		}
		Hp -= _hitInfo.TotalAttackPower;

		GameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}
	}

	public override void Heal(HealInfo _healInfo)
	{
		//base.Heal(_healInfo);

		if (currentState != StateType.DEATH)
		{
			IdleNumber newHP = Hp + _healInfo.healRecovery;
			IdleNumber rawHP = MaxHp;
			if (rawHP < newHP)
			{
				newHP = rawHP;
			}

			IdleNumber addHP = newHP - Hp;
			Hp += addHP;
			GameManager.it.battleRecord.RecordHeal(_healInfo, addHP);
			GameUIManager.it.ShowFloatingText(_healInfo.healRecovery, CenterPosition, CenterPosition, CriticalType.Normal);
		}
	}


	public override bool IsAlive()
	{
		return Hp > 0;
	}

	public void Kill()
	{
		Hp = (IdleNumber)0;
	}


	// Update is called once per frame
	protected virtual void Update()
	{
		if (GameManager.GameStop)
		{
			return;
		}

		float delta = Time.deltaTime;


		//unitBehavior?.OnPreUpdate(this, delta);
		//unitBehavior?.OnUpdate(this, delta);
		//unitBehavior?.OnPostUpdate(this, delta);

		for (int i = 0; i < neutralizeMoves.Count; i++)
		{
			neutralizeMoves[i].OnUpdate(delta);
			if (neutralizeMoves[i].IsEnd())
			{
				neutralizeMoves.RemoveAt(i);
			}
		}

		for (int i = 0; i < additionalDamageModules.Count; i++)
		{
			additionalDamageModules[i].OnUpdate(delta);
			if (additionalDamageModules[i].isEnd())
			{
				additionalDamageModules[i].RemoveParticle();
				additionalDamageModules.RemoveAt(i);
			}
		}

		fsmModule?.OnUpdate(delta);
	}
	protected void FixedUpdate()
	{
		fsmModule?.OnFixedUpdate(Time.fixedDeltaTime);
	}

	protected virtual void LateUpdate()
	{
		CheckDeathState();
	}

	public virtual void CheckDeathState()
	{
		if (Hp <= 0)
		{
			ChangeState(StateType.DEATH);
		}
	}

	public virtual void KnockbackStart()
	{
		ChangeState(StateType.IDLE);
	}

	public virtual void StunStart()
	{
		// 애니메이션 시작
		ChangeState(StateType.IDLE);
	}

	public virtual void StunFinish()
	{
		// 애니메이션 종료
	}


	public virtual void OnCrowdControl()
	{

	}
}
