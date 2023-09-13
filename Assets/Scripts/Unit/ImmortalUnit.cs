using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalUnit : EnemyUnit
{
	public override bool IsAlive()
	{
		return true;
	}

	public override void CheckDeathState()
	{
		//base.CheckDeathState();
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

			GameUIManager.it.ShowFloatingText(correctionDamage, HeadPosition, reverse, textType, _hitInfo.sprite);
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
		while (Hp <= correctionDamage)
		{
			correctionDamage = correctionDamage - Hp;
			var unitData = info.rawData;
			info.unitLevel = info.unitLevel + 1;
			info.CalculateBaseAttackPowerAndHp(StageManager.it.CurrentStage); //= new RuntimeData.EnemyUnitInfo(this, unitData as EnemyUnitData, StageManager.it.CurrentStage);
		}
		Hp -= correctionDamage;
		UIController.it.UiStageInfo.SetBossHpGauge(Mathf.Clamp01((float)(Hp / MaxHp)));

		if (Hp <= 0)
		{
			//PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.MONSTER_HUNT, info.rawData.tid, (IdleNumber)1);
		}


		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			SoundManager.Instance.PlayEffect(_hitInfo.hitSound);
		}
	}
}
