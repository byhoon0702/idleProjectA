using System;
using System.Collections.Generic;
using UnityEngine;



public class ConditionAbility
{
	/// <summary>
	/// 공격력 증가 비율
	/// </summary>
	public float attackPowerUpRatio;
	/// <summary>
	/// 공격력 감소 비율
	/// </summary>
	public float attackPowerDownRatio;

	/// <summary>
	/// 공격속도 증가 비율
	/// </summary>
	public float attackSpeedUpRatio;
	/// <summary>
	/// 공격속도 감소 비율
	/// </summary>
	public float attackSpeedDownRatio;

	/// <summary>
	/// 크리티컬 증가 비율
	/// </summary>
	public float criticalChanceUpRatio;
	/// <summary>
	/// 크리티컬 감소 비율
	/// </summary>
	public float criticalChanceDownRatio;

	/// <summary>
	/// 피해증가 비율
	/// </summary>
	public float damageUpRatio;
	/// <summary>
	/// 피해감소 비율
	/// </summary>
	public float damageDownRatio;

	/// <summary>
	/// 이동속도 증가 비율
	/// </summary>
	public float moveSpeedUpRatio;
	/// <summary>
	/// 이동속도 감소 비율
	/// </summary>
	public float moveSpeedDownRatio;




	/// <summary>
	/// 컨디션 정보가 변화할때마다 호출해서 데이터를 최신화 시킬수 있어야 함
	/// </summary>
	public void Calculate(ConditionModule _module)
	{
		attackPowerUpRatio = CalculateAttackPowerUpRatio(_module);
		attackPowerDownRatio = CalculateAttackPowerDownRatio(_module);

		attackSpeedUpRatio = CalcutateAttackSpeedUpRatio(_module);
		attackSpeedDownRatio = CalculateAttackSpeedDownRatio(_module);

		criticalChanceUpRatio = CalculateCriticalChanceUpRatio(_module);
		criticalChanceDownRatio = CalculateCriticalChanceDownRatio(_module);

		damageUpRatio = CalculateDamageUpRatio(_module);
		damageDownRatio = CalculateDamageDownRatio(_module);

		moveSpeedUpRatio = CalculateMoveSpeedUpRatio(_module);
		moveSpeedDownRatio = CalculateMoveSpeedDownRatio(_module);
	}

	private float CalculateAttackPowerUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackPowerUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackPowerUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateAttackPowerDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackPowerDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackPowerDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalcutateAttackSpeedUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackSpeedUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackSpeedUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateAttackSpeedDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.AttackSpeedDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as AttackSpeedDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateDamageUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.DamageUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as DamageUpCondition).ratio;
		}

		return outTotal;
	}


	private float CalculateDamageDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.DamageDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as DamageDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateCriticalChanceUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.CriticalChanceUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as CriticalChanceUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateCriticalChanceDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.CriticalChanceDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as CriticalChanceDownCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateMoveSpeedUpRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.MoveSpeedUp);

		foreach (var condition in conditions)
		{
			outTotal += (condition as MoveSpeedUpCondition).ratio;
		}

		return outTotal;
	}

	private float CalculateMoveSpeedDownRatio(ConditionModule _module)
	{
		float outTotal = 0;
		var conditions = _module.GetConditions(UnitCondition.MoveSpeedDown);

		foreach (var condition in conditions)
		{
			outTotal += (condition as MoveSpeedDownCondition).ratio;
		}

		return outTotal;
	}
}

public sealed class ConditionModule
{
	/// <summary>
	/// 가지고 있는 컨디션들
	/// </summary>
	public List<ConditionBase> conditions = new List<ConditionBase>();

	private Character character;
	private Dictionary<string, ParticleSystem> effects = new Dictionary<string, ParticleSystem>();

	public ConditionAbility ability { get; private set; }

	/// <summary>
	/// 컨디션이 추가될때 호출된다.
	/// </summary>
	public event Action<ConditionBase> onAddCondition;

	/// <summary>
	/// 컨디션이 제거될 떄 호출된다.
	/// </summary>
	public event Action<ConditionBase> onRemoveCondition;




	public ConditionModule(Character _character)
	{
		character = _character;
		ability = new ConditionAbility();
	}

	/// <summary>
	/// 컨디션 추가
	/// </summary>
	/// <param name="condition"></param>
	public void AddCondition(ConditionBase condition)
	{
		// 다른 컨디션들에 따라 지금 등록되는 컨디션을 최종적용한다.
		for (int i = 0; i < this.conditions.Count; i++)
		{
			// 컨디션 종류가 같다면
			if (this.conditions[i].conditionType == condition.conditionType)
			{
				// FIXME: 중복처리는 일단 보류. 일단 다 적용되도 문제는 없는거 같다.
			}
		}

		// 컨디션을 적용할 유닛 등록
		condition.character = character;

		// 컨디션 최종추가
		this.conditions.Add(condition);
		ability.Calculate(this);

		// 이펙트 추가
		string conditionTypeKey = condition.conditionType.ToString();
		// 관련 이펙트가 생성되지 않았고 && 이펙트가 지정되에 있을 때 // 이펙트처리 by myoung1 2022/01/02
		//if (!this.effects.ContainsKey(conditionTypeKey) && !string.IsNullOrEmpty(condition.effectPath))
		//{
		//	ParticleSystem ps = ObjectPoolManager.inst.Get<ParticleSystem>(condition.effectPath);
		//	ps.transform.SetParent(this.character.transform, false);
		//	ps.transform.position = condition.effectPosition;
		//	ps.transform.localRotation = Quaternion.identity;
		//	ps.transform.localScale = Vector3.one * this.character.radius * 2;
		//	ps.Play();
		//
		//	this.effects.Add(conditionTypeKey, ps);
		//}

		// 컨디션 초기화가 끝낫음을 알린다.
		condition.Start();

		// 컨디션 등록 델리게이트
		this.onAddCondition?.Invoke(condition);
		VLog.ConditionLog($"Add Condition. Unit: {character.info.charNameAndCharId}, Condition: {condition.GetType()}");
	}

	/// <summary>
	/// 컨디션 강제 종료
	/// </summary>
	public void RemoveCondition(UnitCondition condition)
	{
		for (int i = 0; i < this.conditions.Count; i++)
		{
			if (this.conditions[i].conditionType == condition)
			{
				VLog.ConditionLog($"Remove Condition. Unit: {character.info.charNameAndCharId}, Condition: {conditions[i].conditionType}");

				// 제거되는 컨디션
				UnitCondition removeCondition = this.conditions[i].conditionType;

				// 컨디션을 제거한다.
				this.onRemoveCondition?.Invoke(this.conditions[i]);
				this.conditions[i].Finish();
				this.conditions.RemoveAt(i);
				ability.Calculate(this);

				TryRemoveEffect(removeCondition);
				break;
			}
		}
	}

	/// <summary>
	/// 컨디션을 가지고 있는지
	/// </summary>
	public bool HasCondition(UnitCondition condition)
	{
		for (int i = 0; i < this.conditions.Count; i++)
		{
			if (this.conditions[i].conditionType == condition)
				return true;
		}

		return false;
	}

	public List<ConditionBase> GetConditions(UnitCondition _condition)
	{
		List<ConditionBase> outConditions = new List<ConditionBase>();

		foreach(var condition in conditions)
		{
			if(condition.conditionType == _condition)
			{
				outConditions.Add(condition);
			}
		}

		return outConditions;
	}

	/// <summary>
	/// 컨디션 업데이트
	/// </summary>
	/// <param name="dt"></param>
	public void Update(float dt)
	{
		/////////////////////////////////////////////////////////
		// 컨디션 업데이트
		for (int i = 0; i < this.conditions.Count; i++)
		{
			this.conditions[i].Update(dt);

			// 컨디션을 종료할 수 있으면
			if (this.conditions[i].IsEnd())
			{
				VLog.ConditionLog($"Remove Condition. Unit: {character.info.charNameAndCharId}, Condition: {conditions[i].GetType()}");

				// 제거되는 컨디션
				UnitCondition removeCondition = this.conditions[i].conditionType;

				// 컨디션을 종료한다.
				this.onRemoveCondition?.Invoke(this.conditions[i]);
				this.conditions[i].Finish();
				this.conditions.RemoveAt(i--);
				ability.Calculate(this);

				TryRemoveEffect(removeCondition);
			}
		}
	}

	/// <summary>
	/// 모든 컨디션들을 제거함
	/// 컨디션 모듈을 제거하기 전에도 반드시 호출해줘야 함.
	/// </summary>
	public void Collect()
	{
		// 컨디션 종료 강제호출
		for (int i = 0; i < this.conditions.Count; i++)
		{
			this.onRemoveCondition?.Invoke(this.conditions[i]);
			this.conditions[i].Finish();
		}

		//// 이펙트 전부 종료
		//foreach (var e in this.effects.Values) // check by myoung1 2023/01/02
		//{
		//	ObjectPoolManager.inst.Return(e.gameObject);
		//}

		this.conditions.Clear();
		this.effects.Clear();
		ability = new ConditionAbility();
	}

	private bool TryRemoveEffect(UnitCondition condition)
	{
		//string effectKey = condition.ToString(); // check by myoung1. 2023/01/02
		//
		//// 같은 종류의 컨디션이 없고 && 이펙트가 존재한다면
		//if (!HasCondition(condition) && this.effects.ContainsKey(effectKey))
		//{
		//	// 기존 이펙트 제거
		//	ObjectPoolManager.inst.Return(this.effects[effectKey].gameObject);
		//	this.effects.Remove(effectKey);
		//	return true;
		//}

		return false;
	}
}
