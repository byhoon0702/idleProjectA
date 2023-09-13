using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{

	public class UnitInfo
	{
		public Unit owner;
		public virtual T GetData<T>() where T : UnitData
		{
			return null;
		}

		public UnitStats stats;

		public IdleNumber hp;

		protected IdleNumber _maxHp;
		public IdleNumber maxHp
		{
			get { return _maxHp; }
			set { _maxHp = value; }
		}


		public IdleNumber prevMaxHp;

		public IdleNumber attackPower;


		public virtual bool HyperAvailable { get; protected set; }

		protected float moveSpeedBase = 2f;
		protected float attackSpeedBase = 1f;


		public virtual IdleNumber AttackPower()
		{
			return new IdleNumber();
		}

		/// <summary>
		/// 공격속도
		/// </summary>
		public virtual float AttackSpeed()
		{
			return 1;
		}

		public virtual IdleNumber HPRecovery()
		{
			return new IdleNumber();
		}

		/// <summary>
		/// 크리티컬 발동여부
		/// </summary>
		public virtual CriticalType IsCritical()
		{
			return CriticalType.Normal;
		}


		public virtual float CriticalChanceRatio()
		{
			return 1;
		}

		public virtual float CriticalX2ChanceRatio()
		{
			return 1;
		}

		/// <summary>
		/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
		/// </summary>
		public virtual float CriticalDamageMultifly()
		{
			return 1;
		}


		/// <summary>
		/// 크리티컬X2 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
		/// </summary>
		public virtual float CriticalX2DamageMultifly()
		{
			return 1;
		}

		/// <summary>
		/// 이동속도
		/// </summary>
		/// <returns></returns>
		public virtual float MoveSpeed()
		{
			return 1;
		}

		public virtual void SetProjectile(long tid)
		{

			//ProjectileData data = DataManager.Get<ProjectileDataSheet>().GetData(tid);

			//projectileData = data.Clone();

		}
	}


	public class PlayerUnitInfo : UnitInfo
	{
		public UnitData rawData { get; private set; }
		public override T GetData<T>()
		{
			return rawData as T;
		}

		public long defaultSkillTid
		{
			get
			{
				return GetData<UnitData>().skillTid;
			}
		}

		public int skillLevel = 5;
		public override bool HyperAvailable
		{
			get => PlatformManager.UserDB.awakeningContainer.HyperActivate;
			protected set => base.HyperAvailable = value;
		}

		public PlayerUnitInfo(Unit _owner, UnitData _data)
		{
			owner = _owner;
			rawData = _data.Clone<UnitData>();
			stats = PlatformManager.UserDB.UserStats;
			maxHp = stats.GetValue(StatsType.Hp);
			prevMaxHp = maxHp;
			hp = maxHp;
		}

		public override IdleNumber AttackPower()
		{
			IdleNumber basePower = stats.GetValue(StatsType.Atk);
			IdleNumber final = stats.GetValue(StatsType.Final_Damage_Buff);

			IdleNumber total = basePower;
			if (final > 0)
			{
				total = basePower * (1 + (final / 100f));
			}

			return total;
		}

		/// <summary>
		/// 틱당 체력회복량
		/// </summary>
		public override IdleNumber HPRecovery()
		{
			IdleNumber itemBuffRatio = stats.GetValue(StatsType.Hp_Recovery);

			return itemBuffRatio;
		}

		/// <summary>
		/// 공격속도
		/// </summary>
		public override float AttackSpeed()
		{

			float basicSpeed = attackSpeedBase * (1 + (stats.GetValue(StatsType.Atk_Speed) / 100f));

			float total = basicSpeed;

			total = Mathf.Clamp(total, GameManager.Config.ATTACK_SPEED_MIN, GameManager.Config.ATTACK_SPEED_MAX);
			return total;
		}

		public void UpdateMaxHP(PlayerUnit player)
		{
			maxHp = stats.GetValue(StatsType.Hp);
			if (prevMaxHp < maxHp)
			{
				player.Hp += maxHp - prevMaxHp;

				prevMaxHp = maxHp;
			}
			else
			{
				player.Hp = maxHp;
				prevMaxHp = maxHp;
			}
		}

		public float attackTime
		{
			get
			{
				return 1;
			}
		}

		public override float CriticalChanceRatio()
		{

			float total = stats.GetValue(StatsType.Crits_Chance).GetValueFloat();


			if (total > GameManager.Config.CRITICAL_CHANCE_MAX_RATIO)
			{
				total = GameManager.Config.CRITICAL_CHANCE_MAX_RATIO;
			}

			return total;
		}

		/// <summary>
		/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
		/// </summary>
		public override float CriticalDamageMultifly()
		{
			float total = 1 + stats.GetValue(StatsType.Crits_Damage).GetValueFloat();

			return total;
		}

		/// <summary>
		/// 이동속도
		/// </summary>
		/// <returns></returns>
		public override float MoveSpeed()
		{
			float basicSpeed = moveSpeedBase * (1 + (stats.GetValue(StatsType.Move_Speed) / 100f));
			float total = basicSpeed;

			return total;
		}
	}

	public class EnemyUnitInfo : UnitInfo
	{
		public EnemyUnitData rawData { get; private set; }
		public override T GetData<T>()
		{
			return rawData as T;
		}
		public long defaultAttakSkill { get; private set; }
		public int skillLevel = 5;
		public int unitLevel = 1;

		public float knockbackResist { get; private set; }
		public int maxPhase { get; private set; }
		public int currentPhase { get; private set; }
		public float searchRange
		{
			get
			{
				if (owner.isBoss)
				{
					return GameManager.Config.BOSS_TARGET_RANGE_CLOSE;
				}
				else
				{
					return GameManager.Config.ENEMY_TARGET_RANGE_CLOSE;
				}
			}
		}
		public float attackTime
		{
			get
			{
				return 1;
			}
		}
		public EnemyObject enemyObject { get; private set; }

		public EnemyPhaseInfo currentPhaseInfo { get; private set; }

		public EnemyUnitInfo(Unit _owner, EnemyUnitData _data, RuntimeData.StageInfo _stageInfo, int phase = 0)
		{
			owner = _owner;
			rawData = _data.Clone<EnemyUnitData>();
			defaultAttakSkill = rawData.skillTid;
			if (defaultAttakSkill == 0)
			{
				defaultAttakSkill = 1701500001;
			}
			stats = new UnitStats();
			stats.Generate();
			CalculateBaseAttackPowerAndHp(_stageInfo);

			currentPhase = 0;
			maxPhase = phase;
			enemyObject = PlatformManager.UserDB.unitContainer.GetScriptableObject<EnemyObject>(rawData.tid);
		}

		public override IdleNumber AttackPower()
		{
			IdleNumber total = attackPower;

			return total;
		}

		/// <summary>
		/// 공격속도
		/// </summary>
		public override float AttackSpeed()
		{

			float basicSpeed = attackSpeedBase;

			float total = Mathf.Clamp(basicSpeed, GameManager.Config.ATTACK_SPEED_MIN, GameManager.Config.ATTACK_SPEED_MAX);

			return total;
		}

		public override float CriticalChanceRatio()
		{

			float total = 1;


			if (total > GameManager.Config.CRITICAL_CHANCE_MAX_RATIO)
			{
				total = GameManager.Config.CRITICAL_CHANCE_MAX_RATIO;
			}

			return total;
		}

		/// <summary>
		/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
		/// </summary>
		public override float CriticalDamageMultifly()
		{
			float total = 1;

			return total;
		}

		/// <summary>
		/// 이동속도
		/// </summary>
		/// <returns></returns>
		public override float MoveSpeed()
		{
			float basicSpeed = moveSpeedBase;
			return basicSpeed;
		}

		public void CalculateBaseAttackPowerAndHp(RuntimeData.StageInfo _stageInfo)
		{
			stats.UpdataModifier(StatsType.Hp, new StatsModifier(_stageInfo.UnitHP(owner.isBoss), StatModeType.Replace));
			stats.UpdataModifier(StatsType.Atk, new StatsModifier(_stageInfo.UnitAttackPower(owner.isBoss), StatModeType.Replace));

			List<AbilityInfo> infos = new List<AbilityInfo>();
			for (int i = 0; i < rawData.phaseBuff.Count; i++)
			{
				infos.Add(new AbilityInfo(rawData.phaseBuff[i]));
			}

			for (int i = 0; i < infos.Count; i++)
			{
				var abil = infos[i];
				switch (abil.type)
				{
					case StatsType.Hp:
						{
							stats.UpdataModifier(StatsType.Hp, new StatsModifier(abil.GetValue(currentPhase), StatModeType.Multi));
							//maxHp *= abil.GetValue(currentPhase) / 100f;
						}
						break;
					case StatsType.Atk:
						{
							stats.UpdataModifier(StatsType.Atk, new StatsModifier(abil.GetValue(currentPhase), StatModeType.Multi));
							//attackPower *= abil.GetValue(currentPhase) / 100f;
						}
						break;
					case StatsType.Knockback_Resist:
						{
							knockbackResist += abil.GetValue(currentPhase);
						}
						break;
				}
			}
			maxHp = stats.GetValue(StatsType.Hp);
			attackPower = stats.GetValue(StatsType.Atk);
			hp = maxHp;
		}

		public bool CanChangePhase()
		{
			if (maxPhase == 0)
			{
				return false;

			}

			if (currentPhase == maxPhase)
			{
				return false;
			}

			return true;
		}
		public void ChangePhase()
		{
			currentPhase++;
			currentPhaseInfo = enemyObject.phaseInfoList[currentPhase - 1];

			//knockbackResist = //data.phaseBuff[currentPhase].type;
		}
	}

}


