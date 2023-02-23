using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnit : Unit
{
	public override float SearchRange
	{

		get
		{
			return 0;
		}
	}

	public override IdleNumber Hp
	{
		set
		{
			Hp = value;
		}
		get
		{
			return (IdleNumber)100;
		}
	}

	public override IdleNumber MaxHp
	{
		get
		{
			return (IdleNumber)100;
		}
	}

	public override ControlSide ControlSide
	{
		get
		{
			return ControlSide.NO_CONTROL;
		}
	}

	public override IdleNumber AttackPower
	{
		get
		{
			return (IdleNumber)0;
		}
	}

	public override float AttackSpeedMul
	{
		get
		{
			return 0;
		}
	}

	protected override string ModelResourceName => ""; 



	public override void Spawn(UnitData data, int _level = 1)
	{
		var model = UnitModelPoolManager.it.Get(data.resource);
		model.transform.SetParent(transform);
		model.transform.localPosition = Vector3.zero;
		model.transform.localScale = Vector3.one;

		base.model = model.gameObject;
		unitAnimation = base.model.GetComponent<UnitAnimation>();
		if (unitAnimation == null)
		{
			unitAnimation = base.model.AddComponent<UnitAnimation>();
		}
	}
	protected override void InitSkill(SkillModule _skillModule)
	{

	}

	protected override void OnChangeState(StateType stateType)
	{
	}


	protected override void Update()
	{

	}
	protected override void LateUpdate()
	{

	}
	public override void Hit(HitInfo _hitInfo)
	{
		if (_hitInfo == null)
		{
			return;
		}
		GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower.ToString(), _hitInfo.fontColor, unitAnimation.CenterPivot.position, _hitInfo.criticalType, isPlayer: false);
	}

	public override SkillEffectData GetSkillEffectData()
	{
		return null;
	}
}
