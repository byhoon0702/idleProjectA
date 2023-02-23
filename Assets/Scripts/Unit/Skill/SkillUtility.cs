using System;
using System.Collections.Generic;
using UnityEngine;



public static class SkillUtility
{
	/// <summary>
	/// 타겟에게 대미지를 준다.
	/// </summary>
	public static void SimpleAttack(UnitBase _attacker, UnitBase _target, IdleNumber _attackPower, Color _color, string _hitSound = "", bool _checkCritical = true)
	{
		float criticalChanceMul = 1;
		float criticalX2ChangMul = 1;

		if (_checkCritical)
		{
			CriticalType criticalType = _attacker.RandomCriticalType;

			criticalX2ChangMul = _attacker.CriticalX2DamageMultifly;
			criticalChanceMul = _attacker.CriticalDamageMultifly;
		}

		AttackerType attackerType = AttackerType.Unknown;
		if (_attacker is Pet)
		{
			attackerType = AttackerType.Pet;
		}
		else if (_attacker is PlayerUnit)
		{
			attackerType = AttackerType.Player;
		}
		else if (_attacker is EnemyUnit)
		{
			attackerType = AttackerType.Enemy;
		}
		else
		{
			attackerType = AttackerType.Unknown;
		}

		HitInfo hitInfo = new HitInfo(attackerType, _attackPower);
		hitInfo.fontColor = _color;
		hitInfo.criticalChanceMul = criticalChanceMul;
		hitInfo.CriticalX2ChanceMul = criticalX2ChangMul;
		hitInfo.hitSound = _hitSound;

		_target.Hit(hitInfo);
	}

	/// <summary>
	/// 캐릭터 기준으로 타겟 찾기
	/// </summary>
	public static List<Unit> GetTargetUnitNonAlloc(Unit _unit, TargetingType _targetingType)
	{
		return GetTargetUnitCalculator.Calculate(_unit, _targetingType);
	}

	/// <summary>
	/// 포지션 위치에서 범위안에 있는 유닛들을 찾아줌(메모리할당X)
	/// _checkFront : 내 위치 기준 전방에 있는 유닛들만
	/// </summary>
	public static List<Unit> GetUnitRangeNonAlloc(Vector2 _position, float _radius, List<Unit> _searchList, bool _checkFront)
	{
		return GetUnitRangeCalculator.Calculate(_position, _radius, _searchList, _checkFront);
	}

	public static bool Cumulative(float probability)
	{
		return UnityEngine.Random.Range(0, 1f) <= probability;
	}

	private static class GetTargetUnitCalculator
	{
		private static List<Unit> _allocTargets = new List<Unit>(100);
		private static readonly List<Unit> _allocEmpty = new List<Unit>(); // 빈 리스트 리턴용

		public static List<Unit> Calculate(Unit _unit, TargetingType _targetingType)
		{
			_allocTargets.Clear();
			List<Unit> searchList = UnitManager.it.GetEnemyUnit();

			// 타게팅 타입에 맞는 적 리스트 추출
			switch (_targetingType)
			{
				case TargetingType.Default:
					{
						if (_unit.target != null)
						{
							_allocTargets.Add(_unit.target);
						}
					}
					break;

				case TargetingType.Center:
					{
						var targetList = GetUnitRangeCalculator.Calculate(_unit.transform.position + (Vector3.right * ConfigMeta.it.RANGE_SKILL_POSITION), ConfigMeta.it.RANGE_SKILL_RADIUS, searchList, false);
						_allocTargets.AddRange(targetList);
					}
					break;

				case TargetingType.HighestHP:
					{
						var targetList = GetUnitRangeCalculator.Calculate(_unit.transform.position, _unit.SearchRange, searchList, false);

						// 체력이 가장 많은 적 찾기
						Unit target = _unit.target;

						foreach (var checkTarget in targetList)
						{
							if (checkTarget.Hp > target.Hp)
							{
								target = checkTarget;
							}
						}

						if (target != null)
						{
							_allocTargets.Add(_unit.target);
						}
					}
					break;

				case TargetingType.lowestHP:
					{
						var targetList = GetUnitRangeCalculator.Calculate(_unit.transform.position, _unit.SearchRange, searchList, false);

						// 체력이 적은 많은 적 찾기
						Unit target = _unit.target;

						foreach (var checkTarget in targetList)
						{
							if (checkTarget.Hp < target.Hp)
							{
								target = checkTarget;
							}
						}

						if (target != null)
						{
							_allocTargets.Add(_unit.target);
						}
					}
					break;

				case TargetingType.Random:
					if (searchList.Count > 0)
					{
						_allocTargets.Add(searchList[UnityEngine.Random.Range(0, searchList.Count)]);
					}
					break;
			}

			return _allocTargets;
		}
	}




	private static class GetUnitRangeCalculator
	{
		private static List<Unit> _allocUnitFront = new List<Unit>(100);

		public static List<Unit> Calculate(Vector2 _position, float _radius, List<Unit> _searchList, bool _checkFront)
		{
			_allocUnitFront.Clear();

			for (Int32 i = 0; i < _searchList.Count; i++)
			{
				Vector2 targetPosition = _searchList[i].transform.position;

				var direction = (_position - targetPosition).normalized;
				//var distance = Vector3.Distance(targetPosition, _position);
				float distance = Mathf.Abs(targetPosition.x - _position.x);
				if (distance < _radius)
				{
					if (_checkFront && _position.x <= targetPosition.x)
					{
						_allocUnitFront.Add(_searchList[i]);
					}
				}
			}

			return _allocUnitFront;
		}
	}
	public static HitInfo CreateHitInfo(UnitBase _attacker, bool isSkill, IdleNumber _attackPower, string _hitSound = "")
	{
		float criticalChanceMul = 1;
		float criticalX2ChangMul = 1;


		criticalX2ChangMul = _attacker.CriticalX2DamageMultifly;
		criticalChanceMul = _attacker.CriticalDamageMultifly;

		AttackerType attackerType = AttackerType.Unknown;
		if (_attacker is Pet)
		{
			attackerType = AttackerType.Pet;
		}
		else if (_attacker is PlayerUnit)
		{
			attackerType = AttackerType.Player;
		}
		else if (_attacker is EnemyUnit)
		{
			attackerType = AttackerType.Enemy;
		}
		else
		{
			attackerType = AttackerType.Unknown;
		}

		HitInfo hitInfo = new HitInfo(attackerType, _attackPower, isSkill);
		hitInfo.criticalChanceMul = criticalChanceMul;
		hitInfo.CriticalX2ChanceMul = criticalX2ChangMul;
		hitInfo.hitSound = _hitSound;

		return hitInfo;

	}
}
