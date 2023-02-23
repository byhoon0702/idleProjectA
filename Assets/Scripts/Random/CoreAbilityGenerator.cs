using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;





public class CoreAbilityGenerator
{
	private Random gradeSeed;
	private Random abilitySeed;
	private Random valueSeed;



	public CoreAbilityGenerator()
	{
		gradeSeed = new Random();
		abilitySeed = new Random();
		valueSeed = new Random();
	}

	/// <summary>
	/// 진급 능력 데이터 랜덤 생성
	/// </summary>
	public AbilityInfo GenerateAbility()
	{
		Grade grade = RandomGrade();
		Stats resultAbility = RandomAbility();

		GetAbilityValueRange(grade, resultAbility, out var minValue, out var maxValue);
		IdleNumber resultValue = RandomAbilityValue(minValue, maxValue);

		return new AbilityInfo(resultAbility, resultValue);
	}

	/// <summary>
	/// 랜덤 등급
	/// </summary>
	public Grade RandomGrade()
	{
		var probabilitySheet = DataManager.Get<CoreAbilityProbabilityDataSheet>();

		double value = gradeSeed.NextDouble();
		double accum = 0;
		for (int i = probabilitySheet.infos.Count - 1 ; i >= 0 ; i--)
		{
			accum += probabilitySheet.infos[i].probability;
			if (accum >= value)
			{
				return probabilitySheet.infos[i].grade;
			}
		}

		return Grade.D;
	}

	/// <summary>
	/// 랜덤 어빌리티
	/// </summary>
	public Stats RandomAbility()
	{
		var abilitySheet = DataManager.Get<CoreAbilityDataSheet>();
		var abilityTypes = abilitySheet.GetAbilityTypes();

		Stats result = abilityTypes[abilitySeed.Next(0, abilityTypes.Count)];

		return result;
	}

	/// <summary>
	/// 범위 내의 랜덤한 어빌리티 값
	/// </summary>
	public IdleNumber RandomAbilityValue(IdleNumber _minValue, IdleNumber _maxValue)
	{
		double value = valueSeed.NextDouble();

		double min = _minValue.GetValue();
		double max = _maxValue.GetValue();

		double result = ((max - min) * value) + min;

		IdleNumber resultValue = new IdleNumber((long)(result * 100) * 0.01d);
		return resultValue;
	}

	public void GetAbilityValueRange(Grade _grade, Stats _ability, out IdleNumber _minValue, out IdleNumber _maxValue)
	{
		var abilityInfo = DataManager.Get<CoreAbilityDataSheet>().GetByAbilityType(_ability);
		var probabilitySheet = DataManager.Get<CoreAbilityProbabilityDataSheet>();

		double rangeMin;
		double rangeMax = probabilitySheet.Get(_grade).rangeMax;

		if (_grade == Grade.D)
		{
			rangeMin = 0;
		}
		else
		{
			rangeMin = probabilitySheet.Get((Grade)((int)_grade - 1)).rangeMax;
		}

		_minValue = new IdleNumber(abilityInfo.min + (abilityInfo.max - abilityInfo.min) * rangeMin);
		_maxValue = new IdleNumber(abilityInfo.min + (abilityInfo.max - abilityInfo.min) * rangeMax);
	}

}
