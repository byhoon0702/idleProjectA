using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingDungeonBossUnit : Unit
{
	public EnemyUnitInfo info;

	// 플레이어 유닛의 
	public override float SearchRange => 0;


	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }

	public override IdleNumber MaxHp => info.maxHP;

	public override ControlSide ControlSide => ControlSide.ENEMY;

	public override IdleNumber AttackPower => info.AttackPower();

	public override float AttackSpeedMul => info.AttackSpeedMul();
	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.right;
	protected override string ModelResourceName => info.data.resource;


	public override void Spawn(UnitData _data, int _level = 1)
	{
		if (_data == null)
		{
			VLog.ScheduleLogError("No Unit Data");
		}

		info = new EnemyUnitInfo(this, _data);

		if (model == null)
		{
			LoadModel();
			gameObject.tag = "Enemy";
		}
		Init();
		GameUIManager.it.ShowUnitGauge(this);
	}

	public override bool ConditionApplicable(ConditionBase _condition)
	{
		bool result = info.ConditionApplicable(_condition);
		return result;
	}

	public override SkillEffectData GetSkillEffectData()
	{
		return info.normalSkillEffectData;
	}
}
