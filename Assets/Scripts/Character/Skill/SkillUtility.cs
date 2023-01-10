using System;
using System.Collections.Generic;
using UnityEngine;



public static class SkillUtility
{
	private static List<Character> _allockCharacter = new List<Character>(100);

	/// <summary>
	/// 타겟에게 대미지를 준다.
	/// </summary>
	public static void SimpleAttack(Character _attacker, Character _target, IdleNumber _attackPower, string _attackName, Color _color, bool _checkCritical = true)
	{
		bool isCritical = _attacker.info.IsCritical();
		float criticalChanceMul = 1;
		if (_checkCritical && isCritical)
		{
			criticalChanceMul = _attacker.info.CriticalDamageMultifly();
		}

		_target.Hit(_attacker, _attackPower, _attackName, _color, criticalChanceMul);
	}

	
	/// <summary>
	/// 포지션 위치에서 범위안에 있는 유닛들을 찾아줌(메모리할당X)
	/// </summary>
	public static List<Character> GetCharacterRangeNonAlloc(Vector2 _position, float _radius, List<Character> _searchList)
	{
		_allockCharacter.Clear();

		for (Int32 i = 0; i < _searchList.Count; i++)
		{
			Vector2 targetPosition = _searchList[i].transform.position;

			var direction = (_position - targetPosition).normalized;
			var distance = Vector3.Distance(targetPosition, _position);
			if (distance < _radius)
			{
				_allockCharacter.Add(_searchList[i]);
			}
		}

		return _allockCharacter;
	}

	public static bool Cumulative(float probability)
	{
		return UnityEngine.Random.Range(0, 1f) <= probability;
	}
}
