
using UnityEngine;
using System.Collections.Generic;
using RuntimeData;



public class EnemyUnit : Unit
{
	public GameObject deathEffect;
	public EnemyUnitInfo info;
	public override ControlSide ControlSide => ControlSide.ENEMY;
	public override UnitType UnitType => info.data.type;
	public override float SearchRange => info.searchRange;
	public override float AttackTime => info.attackTime;
	public override IdleNumber AttackPower => info.AttackPower();
	public override HitInfo HitInfo
	{
		get
		{
			return new HitInfo(AttackPower);
		}
	}
	public override float AttackSpeed => info.AttackSpeed();

	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }
	public override IdleNumber MaxHp => info.maxHp;

	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.left;

	public void Spawn(UnitData _spawnInfo)
	{
		info = new EnemyUnitInfo(this, _spawnInfo, StageManager.it.CurrentStage);
		instaneStats = Instantiate(stats);

		Init();

		InitMode("B/Enemy", info);
		headingDirection = Vector3.right;

		rigidbody2D = GetComponent<Rigidbody2D>();

		GameUIManager.it.ShowUnitGauge(this);

		if (isBoss)
		{
			StageManager.it.bossSpawn = true;
			UIController.it.UiStageInfo.SetBossHpGauge(1f);
		}
		skillModule.Init(this, 1701500001);
	}

	public override Vector3 HeadingToTarget()
	{

		if (target != null)
		{
			Vector3 normal = (target.transform.position - transform.position).normalized;
			headingDirection = normal;
			if (normal.x < 0)
			{
				if (currentDir != 1)
				{
					currentDir = 1;
					Vector3 scale = unitAnimation.transform.localScale;
					scale.x = Mathf.Abs(scale.x);
					unitAnimation.transform.localScale = scale;
				}
			}
			else if (normal.x > 0)
			{
				if (currentDir != -1)
				{
					currentDir = -1;
					Vector3 scale = unitAnimation.transform.localScale;
					scale.x = Mathf.Abs(scale.x) * currentDir;
					unitAnimation.transform.localScale = scale;
				}

			}
			return normal;
		}
		return Vector3.right;
	}
	public override void OnMove(float delta)
	{

		if (UnitManager.it.Player == null)
		{
			return;
		}
		target = UnitManager.it.Player;
		HeadingToTarget();
		rigidbody2D.MovePosition(transform.position + headingDirection * MoveSpeed * delta);
	}


	public override void Hit(HitInfo _hitInfo)
	{
		if (GameManager.GameStop)
		{
			return;
		}
		if (_hitInfo.TotalAttackPower == 0)
		{
			return;
		}
		if (Hp <= 0)
		{
			return;
		}

		IdleNumber correctionDamage = _hitInfo.TotalAttackPower;


		if (isBoss)
		{
			IdleNumber value = GameManager.UserDB.GetValue(StatsType.Boss_Damage_Buff);

			if (value != 0)
			{
				correctionDamage *= 1 + (value / 100f);
			}
		}
		else
		{
			IdleNumber value = GameManager.UserDB.GetValue(StatsType.Mob_Damage_Buff);
			if (value != 0)
			{
				correctionDamage *= 1 + (value / 100f);
			}
		}
		if (Hp > 0)
		{
			Vector3 reverse = headingDirection;
			reverse.x = HeadPosition.x + (0.7f * currentDir);
			reverse.y = HeadPosition.y + 0.6f;
			reverse.z = 0;


			TextType textType = TextType.ENEMY_HIT;

			if (_hitInfo.criticalType == CriticalType.CriticalX2)
			{
				textType = TextType.CRITICAL_X2;
			}
			else if (_hitInfo.criticalType == CriticalType.Critical)

			{
				textType = TextType.CRITICAL;
			}

			GameUIManager.it.ShowFloatingText(correctionDamage, HeadPosition, reverse, textType);
			ShakeUnit();
			StageManager.it.cumulativeDamage += correctionDamage;
			currentMode?.OnHit(_hitInfo);
			hitCount++;

			GameObject otherHit = UnitManager.it.Player.hitEffectObject;
			GameObject instancedHitEffect = null;
			if (otherHit != null)
			{
				instancedHitEffect = Instantiate(hitEffect);
			}
			else
			{
				instancedHitEffect = Instantiate(hitEffect);
				instancedHitEffect.GetComponent<UVAnimation>().Play(null);
			}


			instancedHitEffect.transform.position = position;
			instancedHitEffect.transform.localScale = Vector3.one;
			instancedHitEffect.transform.rotation = unitAnimation.transform.rotation;
		}
		Hp -= correctionDamage;
		if (Hp <= 0)
		{
			GameManager.UserDB.questContainer.ProgressAdd(QuestGoalType.MONSTER_HUNT, info.rawData.tid, (IdleNumber)1);
		}
		if (isBoss)
		{

			UIController.it.UiStageInfo.SetBossHpGauge(Mathf.Clamp01((float)(Hp / MaxHp)));
		}

		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}
	}

	public override void FindTarget(float _time, bool _ignoreSearchDelay)
	{
		base.FindTarget(_time, _ignoreSearchDelay);
		if (searchInterval > GameManager.Config.TARGET_SEARCH_DELAY || _ignoreSearchDelay)
		{
			searchInterval = 0;

			// 새로운 타겟을 찾음
			Unit newTarget = UnitManager.it.Player;
			if (TargetAddable(newTarget))
			{
				SetTarget(newTarget);
				HeadingToTarget();
			}
		}
	}

	public override void Attack()
	{
		if (target == null)
		{
			return;
		}
		HeadingToTarget();


		HitInfo hitinfo = new HitInfo(AttackPower);
		skillModule.DefaultAttack(hitinfo);
		attackCount++;
	}


	public override void OnCrowdControl()
	{
		ChangeState(StateType.NEUTRALIZE);
	}

	//public float knockPower;
	public override void KnockBack(float power, Vector3 dir, int hitCount, bool isLastHit = true)
	{
		if (Hp <= 0)
		{
			return;
		}

		rigidbody2D.AddForce(dir * power, ForceMode2D.Impulse);
		ChangeState(StateType.KNOCKBACK);
	}

	public override void AirBorne(float power, int hitCount, bool isLastHit = true)
	{
		float airbornePower = Mathf.Max(0, power);
		AirborneMove move = new AirborneMove();
		move.Set(transform, NeutralizeType.AIRBORNE, airbornePower, Vector3.up, 1);
		var existMove = neutralizeMoves.Find(x => x.type.Equals(NeutralizeType.AIRBORNE));

		if (existMove == null)
		{
			neutralizeMoves.Add(move);
		}
		else
		{
			existMove.AddPower(airbornePower);
		}

		OnCrowdControl();

	}

	public override void AdditionalDamage(AdditionalDamageInfo info, HitInfo hitinfo)
	{
		AdditionalDamageModule module = new AdditionalDamageModule();
		module.Set(this, info, hitinfo);
		additionalDamageModules.Add(module);
	}
	public override void CheckDeathState()
	{
		if (currentState is StateType.KNOCKBACK)
		{
			return;
		}
		if (Hp <= 0)
		{


			if (isBoss)
			{
				GameManager.it.battleRecord.bossKillCount++;
				StageManager.it.bossKillCount++;
				UnitManager.it.Boss = null;
			}
			else
			{
				GameManager.it.battleRecord.killCount++;
				StageManager.it.currentKillCount++;

				IdleNumber exp = StageManager.it.CurrentStage.GetMonsterExp();
				GameManager.UserDB.userInfoContainer.GainUserExp(exp);

				if (exp > 0)
				{
					UIController.it.ShowItemLog(StageManager.it.CurrentStage.MonsterExp, exp);
					GameObjectPoolManager.it.fieldItemPool.Get(out FieldItem fieldItem);
					fieldItem.Appear(2, transform.position, UnitManager.it.Player.transform);
				}
				IdleNumber gold = StageManager.it.CurrentStage.GetMonsterGold();
				GameManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD).Earn(gold);
				if (gold > 0)
				{
					UIController.it.ShowItemLog(StageManager.it.CurrentStage.MonsterGold, gold);
					GameObjectPoolManager.it.fieldItemPool.Get(out FieldItem fieldItem);
					fieldItem.Appear(0, transform.position, UnitManager.it.Player.transform);
				}

				RewardInfo reward = StageManager.it.CurrentStage.GetMonsterReward();
				if (reward != null)
				{
					reward.UpdateCount(StageManager.it.CurrentStage.StageNumber);
					GameManager.UserDB.OpenRewardBox(reward, false);
					UIController.it.ShowItemLog(reward, (IdleNumber)1);
					GameObjectPoolManager.it.fieldItemPool.Get(out FieldItem fieldItem);
					fieldItem.Appear(1, transform.position, UnitManager.it.Player.transform);
				}
			}

			UIController.it.UiStageInfo.RefreshKillCount();
			ChangeState(StateType.DEATH);
		}
	}

	public override void Death()
	{
		DisposeModel();

		GameObject go = Instantiate(deathEffect);
		go.transform.position = transform.position;

		Destroy(gameObject);
	}

	public override void ActiveHyperEffect()
	{

	}

	public override void InactiveHyperEffect()
	{

	}
}
