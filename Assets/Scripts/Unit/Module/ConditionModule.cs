using System;
using System.Collections.Generic;
using UnityEngine;



public sealed class ConditionModule
{
	/// <summary>
	/// 가지고 있는 컨디션들
	/// </summary>
	public List<ConditionBase> conditions = new List<ConditionBase>();

	private Unit unit;
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




	public ConditionModule(Unit _unit)
	{
		unit = _unit;
		ability = new ConditionAbility();
	}

	/// <summary>
	/// 컨디션 추가
	/// </summary>
	/// <param name="_condition"></param>
	public void AddCondition(ConditionBase _condition)
	{
		// 해당 컨디션을 적용할 수 있는지
		if(Applicable(_condition) == false)
		{
			return;
		}

		// 컨디션을 적용할 유닛 등록
		_condition.unit = unit;

		// 컨디션 최종추가
		this.conditions.Add(_condition);
		ability.Calculate(this);

		// 이펙트 추가
		string conditionTypeKey = _condition.conditionType.ToString();
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
		_condition.Start();

		// 컨디션 등록 델리게이트
		this.onAddCondition?.Invoke(_condition);
		VLog.ConditionLog($"Add Condition. Unit: {unit.NameAndId}, Condition: {_condition.GetType()}");
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
				VLog.ConditionLog($"Remove Condition. Unit: {unit.NameAndId}, Condition: {conditions[i].conditionType}");

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
				VLog.ConditionLog($"Remove Condition. Unit: {unit.NameAndId}, Condition: {conditions[i].GetType()}");

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

	/// <summary>
	/// 컨디션 적용이 가능한지 여부
	/// </summary>
	private bool Applicable(ConditionBase _condition)
	{
		// 캐릭터별 저항 체크
		if(unit.ConditionApplicable(_condition) == false)
		{
			return false;
		}

		// 다른 컨디션들에 따라 지금 등록되는 컨디션을 최종적용한다.
		for (int i = 0 ; i < this.conditions.Count ; i++)
		{
			// 컨디션 종류가 같다면
			if (this.conditions[i].conditionType == _condition.conditionType)
			{
				if (_condition.conditionType == UnitCondition.Knockback)
				{
					// 넉백이 적용중일땐 더 들어오지 않음
					return false;
				}
			}
		}

		return true;
	}
}
