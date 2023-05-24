
using UnityEngine;
using System.Collections.Generic;


public class EnemyLevelData
{
	public int level;
	public string attack;
}

public class EnemyData : BaseData
{
	public string name;
	public List<EnemyLevelData> leveldata;
}

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
		unitAnimation.AddAttackEvent(Attack);
		rigidbody2D = GetComponent<Rigidbody2D>();

		GameUIManager.it.ShowUnitGauge(this);

		if (isBoss)
		{
			StageManager.it.bossSpawn = true;
			UIController.it.UiStageInfo.SetBossHpGauge(1f);
		}
		skillModule.Init(this, 1701500001);

	}


	public override void HeadingToTarget()
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
		}

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
		//transform.Translate(headingDirection * MoveSpeed * delta);
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
			GameUIManager.it.ShowFloatingText(correctionDamage, HeadPosition, reverse, _hitInfo.criticalType);
			ShakeUnit();

			currentMode?.OnHit(_hitInfo);
			hitCount++;

			var go = Instantiate(hitEffect).GetComponent<UVAnimation>();
			go.transform.position = position;
			go.transform.localScale = Vector3.one;
			go.transform.rotation = unitAnimation.transform.rotation;
			go.Play(null);

		}
		Hp -= correctionDamage;
		if (isBoss)
		{

			UIController.it.UiStageInfo.SetBossHpGauge(Mathf.Clamp01((float)(Hp / MaxHp)));
		}

		GameManager.it.battleRecord.RecordAttackPower(_hitInfo);

		//UIController.it.UiStageInfo.RefreshDPSCount();

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

	public void Attack()
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

	public override void KnockBack(float power, Vector3 dir, int hitCount, bool isLastHit = true)
	{
		float knockbackPower = Mathf.Max(0, power);

		KnockbackMove move = new KnockbackMove();
		move.Set(transform, NeutralizeType.KNOCKBACK, knockbackPower, dir, 1);

		var existMove = neutralizeMoves.Find(x => x.type.Equals(NeutralizeType.KNOCKBACK));

		if (existMove == null)
		{
			neutralizeMoves.Add(move);
		}
		else
		{
			existMove.AddPower(knockbackPower);
		}

		OnCrowdControl();
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
			}

			UIController.it.UiStageInfo.RefreshKillCount();
			ChangeState(StateType.DEATH);

			GameManager.UserDB.userInfoContainer.GainUserExp(Random.Range(10, 24));
			GameManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD).Earn((IdleNumber)Random.Range(100, 430));
		}
	}

	public override void Death()
	{
		DisposeModel();

		GameObject go = Instantiate(deathEffect);
		go.transform.position = transform.position;

		int count = Random.Range(1, 4);
		for (int i = 0; i < count; i++)
		{
			GameObject killReward = (GameObject)Instantiate(Resources.Load("Item/FieldItem"));
			killReward.GetComponent<FieldItem>().Appear(i, transform.position, UnitManager.it.Player.transform);
		}

		Destroy(gameObject);
	}
}
