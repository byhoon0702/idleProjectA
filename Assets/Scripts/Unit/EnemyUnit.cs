
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuntimeData;



public class EnemyUnit : Unit
{
	public GameObject deathEffect;
	public EnemyUnitInfo info;
	public override ControlSide ControlSide => ControlSide.ENEMY;
	public override UnitType UnitType => info.rawData.type;
	public override float SearchRange => info.searchRange;
	public override float AttackTime => info.attackTime;
	public override IdleNumber AttackPower => info.AttackPower();
	public override HitInfo HitInfo
	{
		get
		{
			return new HitInfo(gameObject.layer, AttackPower);
		}
	}
	public override float AttackSpeed => info.AttackSpeed();

	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }
	public override IdleNumber MaxHp => info.maxHp;

	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.left;

	public void Spawn(RuntimeData.StageMonsterInfo _spawnInfo)
	{
		info = new EnemyUnitInfo(this, _spawnInfo.data, StageManager.it.CurrentStage, _spawnInfo.phase);

		Init();

		InitMode(info.enemyObject.prefab);

		headingDirection = Vector3.right;

		rigidbody2D = GetComponent<Rigidbody2D>();

		GameUIManager.it.ShowUnitGauge(this);

		if (isBoss)
		{
			StageManager.it.bossSpawn = true;
			UIController.it.UiStageInfo.SetBossHpGauge(1f);
		}
		Vector3 position = transform.position;
		position.z = 0;
		transform.position = position;
		skillModule.Init(this, info.defaultAttakSkill);

		//GameSetting.Instance.FxChanged += OnFxChange;
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

	protected override void OnHit(HitInfo hitInfo)
	{

	}

	protected override IEnumerator OnHitRoutine(HitInfo hitInfo)
	{
		yield return null;
	}
	public override void Hit(HitInfo _hitInfo, RuntimeData.SkillInfo _skillInfo)
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

		PlayHitSound();
		if (isBoss)
		{
			IdleNumber value = PlatformManager.UserDB.GetValue(StatsType.Boss_Damage_Buff);

			if (value != 0)
			{
				correctionDamage *= 1 + (value / 100f);
			}
		}
		else
		{
			IdleNumber value = PlatformManager.UserDB.GetValue(StatsType.Mob_Damage_Buff);
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

		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			SoundManager.Instance.PlayEffect(_hitInfo.hitSound);
		}

		StageManager.it.SetOfflineKill();
		if (isBoss)
		{
			UIController.it.UiStageInfo.SetBossHpGauge(Mathf.Clamp01((float)(Hp / MaxHp)));
			if (Hp <= 0)
			{

				if (StageManager.it.usePhase)
				{
					if (info.CanChangePhase())
					{
						ChangePhase();
						return;
					}
				}

				PlatformManager.UserDB.userInfoContainer.dailyKillCount++;
				PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.MONSTER_HUNT, info.rawData.tid, (IdleNumber)1);
			}
		}
		else
		{
			if (Hp <= 0)
			{
				PlatformManager.UserDB.userInfoContainer.dailyKillCount++;
				PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.MONSTER_HUNT, info.rawData.tid, (IdleNumber)1);
			}
		}
	}

	public List<SkillSlot> skillSlot = new List<SkillSlot>();


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
		skillModule.DefaultAttack();
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

		if (info == null)
		{
			return;
		}
		float differ = power - info.knockbackResist;
		if (differ <= 0)
		{
			return;
		}
		if (target != null && rigidbody2D != null && transform != null)
		{
			rigidbody2D.AddForce((transform.position - target.position).normalized * differ, ForceMode2D.Impulse);
			ChangeState(StateType.KNOCKBACK);
		}
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

				KillReward();
			}

			UIController.it.UiStageInfo.RefreshKillCount();
			ChangeState(StateType.DEATH);
			//GameSetting.Instance.FxChanged -= OnFxChange;
		}
	}
	void KillReward()
	{
		if (StageManager.it.CurrentStage.Rule is StageInfinity)
		{
			return;
		}

		IdleNumber expbuff = PlatformManager.UserDB.UserStats.GetValue(StatsType.Buff_Gain_Gold);
		IdleNumber exp = StageManager.it.CurrentStage.GetMonsterExp();
		if (expbuff > 0)
		{
			exp = StageManager.it.CurrentStage.GetMonsterExp() * (1 + (expbuff / 100f));
		}

		PlatformManager.UserDB.userInfoContainer.GainUserExp(exp);

		if (exp > 0)
		{
			GameObjectPoolManager.it.fieldItemPool.Get(out FieldItem fieldItem);
			fieldItem.Appear(2, transform.position, UnitManager.it.Player.transform);

			StageManager.it.AddAcquiredItem(1, new AddItemInfo(0, exp, RewardCategory.EXP));
			GameManager.it.SleepModeAcquiredExp(exp);
		}

		IdleNumber goldbuff = PlatformManager.UserDB.UserStats.GetValue(StatsType.Buff_Gain_Gold);
		IdleNumber gold = StageManager.it.CurrentStage.GetMonsterGold();
		if (goldbuff > 0)
		{
			gold = StageManager.it.CurrentStage.GetMonsterGold() * (1 + (goldbuff / 100f));
		}

		var goldItem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD);
		goldItem.Earn(gold);

		if (gold > 0)
		{
			GameObjectPoolManager.it.fieldItemPool.Get(out FieldItem fieldItem);
			fieldItem.Appear(0, transform.position, UnitManager.it.Player.transform);
			StageManager.it.AddAcquiredItem(goldItem.Tid, new AddItemInfo(goldItem.Tid, gold, RewardCategory.Currency));
			GameManager.it.SleepModeAcquiredGold(gold);
		}

		List<RewardInfo> rewards = StageManager.it.CurrentStage.GetMonsterRewardList();
		if (rewards != null)
		{
			for (int i = 0; i < rewards.Count; i++)
			{
				var reward = rewards[i];
				reward.UpdateCount(StageManager.it.CurrentStage.StageNumber);
				var list = new List<RewardInfo>();
				if (reward.Category == RewardCategory.RewardBox)
				{
					list.AddRange(PlatformManager.UserDB.OpenRewardBox(reward));
				}
				else
				{
					list.Add(reward);
				}

				PlatformManager.UserDB.AddRewards(list, false);
				StageManager.it.AddAcquiredItem(list);
				GameManager.it.SleepModeAcquiredItem(list);
				GameObjectPoolManager.it.fieldItemPool.Get(out FieldItem fieldItem);
				fieldItem.Appear(1, transform.position, UnitManager.it.Player.transform);
			}
		}

		PlatformManager.UserDB.buffContainer.GainExp();

	}


	public override void Death()
	{
		DisposeModel();

		GameObject go = Instantiate(deathEffect);
		go.transform.position = transform.position;

		Destroy(gameObject);
	}
	public override bool TriggerSkill(SkillSlot skillSlot)
	{
		if (target == null)
		{
			FindTarget(0, true);
		}

		IdleNumber totalAttackPower = AttackPower;
		IdleNumber skillvalue = skillSlot.item.skillAbility.Value;


		totalAttackPower = totalAttackPower * (1 + (skillvalue / 100f));

		HitInfo info = new HitInfo(gameObject.layer, totalAttackPower);


		skillModule.ActivateSkill(skillSlot, info);
		//if (skillSlot.item.Instant)
		//{
		//	skillModule.ActivateSkill(skillSlot, info);
		//}
		//else
		//{
		//	skillModule.RegisterUsingSkill(skillSlot, info);
		//}

		if (skillSlot.item.IsSkillState)
		{
			ChangeState(StateType.SKILL, true);
		}

		skillSlot.Use();
		//DialogueManager.it.CreateSkillBubble(skillSlot.item.Name, this);

		//unitAnimation.PlayAnimation(skillSlot.item.rawData.animation);
		return true;
	}
	public void ChangePhase()
	{
		for (int i = 0; i < skillSlot.Count; i++)
		{
			if (skillSlot[i] == null)
			{
				continue;
			}
			skillModule.RemoveSkill(skillSlot[i]);
		}
		skillSlot.Clear();
		info.ChangePhase();

		info.CalculateBaseAttackPowerAndHp(StageManager.it.CurrentStage);

		var phaseInfo = info.currentPhaseInfo;
		InitMode(phaseInfo.Prefab);

		RuntimeData.SkillInfo skillinfo = new SkillInfo(phaseInfo.PhaseChangeSkill.Tid);

		skillinfo.itemObject.Trigger(this, skillinfo);

		for (int i = 0; i < phaseInfo.SkillList.Count; i++)
		{
			RuntimeData.SkillInfo _skill = new SkillInfo(phaseInfo.SkillList[i].Tid);
			SkillSlot slot = new SkillSlot();
			slot.Equip(this, _skill);
			skillSlot.Add(slot);
		}
	}
	public override void Debuff(DebuffInfo _debuffInfo)
	{
		base.Debuff(_debuffInfo);

	}

	public override void Buff(BuffInfo buffInfo)
	{
		base.Buff(buffInfo);
	}

	public override void AddDebuff(AppliedBuff debuffInfo)
	{
		if (info == null)
		{
			return;
		}
		if (info.stats == null)
		{
			return;
		}
		if (debuffInfo == null)
		{
			return;
		}
		info.stats.UpdataModifier(debuffInfo.ability.type, new StatsModifier(debuffInfo.ability.Value, StatModeType.SkillDebuff, debuffInfo));
	}

	public override void AddBuff(AppliedBuff buffinfo)
	{
		info.stats.UpdataModifier(buffinfo.ability.type, new StatsModifier(buffinfo.ability.Value, StatModeType.Buff, buffinfo));
	}
	public override void RemoveBuff(AppliedBuff key)
	{
		info.stats.RemoveAllModifiers(key);
	}

	public override void ActiveHyperEffect()
	{

	}

	public override void InactiveHyperEffect()
	{

	}
}
