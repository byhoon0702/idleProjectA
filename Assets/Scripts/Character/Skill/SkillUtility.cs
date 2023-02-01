using System;
using System.Collections.Generic;
using UnityEngine;



public static class SkillUtility
{
	/// <summary>
	/// 타겟에게 대미지를 준다.
	/// </summary>
	public static void SimpleAttack(UnitBase _attacker, UnitBase _target, AttackType _attackType, IdleNumber _attackPower, Color _color, string _hitSound = "", bool _checkCritical = true)
	{
		float criticalChanceMul = 1;
		float criticalX2ChangMul = 1;

		if (_checkCritical)
		{
			CriticalType criticalType = _attacker.IsCritical();

			criticalX2ChangMul = _attacker.CriticalX2DamageMultifly();
			criticalChanceMul = _attacker.CriticalDamageMultifly();
		}
		AttackerType attackerType = AttackerType.Unknown;
		if(_attacker is Companion)
		{
			attackerType = AttackerType.Companion;
		}
		else if(_attacker is PlayerCharacter)
		{
			attackerType = AttackerType.Player;
		}
		else if(_attacker is BossCharacter)
		{
			attackerType = AttackerType.Enemy;
		}
		else if(_attacker is EnemyCharacter)
		{
			attackerType = AttackerType.Enemy;
		}
		else
		{
			attackerType = AttackerType.Unknown;
		}

		HitInfo hitInfo = new HitInfo(attackerType, _attackType, _attackPower);
		hitInfo.fontColor = _color;
		hitInfo.criticalChanceMul = criticalChanceMul;
		hitInfo.CriticalX2ChanceMul = criticalX2ChangMul;
		hitInfo.hitSound = _hitSound;

		_target.Hit(hitInfo);
	}

	/// <summary>
	/// 캐릭터 기준으로 타겟 찾기
	/// </summary>
	public static List<Unit> GetTargetCharacterNonAlloc(Unit _character, TargetingType _targetingType)
	{
		return GetTargetCharacterCalculator.Calculate(_character, _targetingType);
	}

	/// <summary>
	/// 포지션 위치에서 범위안에 있는 유닛들을 찾아줌(메모리할당X)
	/// _checkFront : 내 위치 기준 전방에 있는 유닛들만
	/// </summary>
	public static List<Unit> GetCharacterRangeNonAlloc(Vector2 _position, float _radius, List<Unit> _searchList, bool _checkFront)
	{
		return GetCharacterRangeCalculator.Calculate(_position, _radius, _searchList, _checkFront);
	}


	public static bool Cumulative(float probability)
	{
		return UnityEngine.Random.Range(0, 1f) <= probability;
	}





	private static class GetTargetCharacterCalculator
	{
		private static List<Unit> _allocTargetCharacter = new List<Unit>(100);
		private static readonly List<Unit> _allocEmpty = new List<Unit>(); // 빈 리스트 리턴용

		public static List<Unit> Calculate(Unit _character, TargetingType _targetingType)
		{
			_allocTargetCharacter.Clear();
			List<Unit> searchList = _allocEmpty;

			// 타겟을 찾을 적 리스트 추출
			switch (_character.info.controlSide)
			{
				case ControlSide.PLAYER:
					if (_targetingType == TargetingType.FriendlyAll)
					{
						searchList = CharacterManager.it.GetPlayerCharacters();
					}
					else
					{
						searchList = CharacterManager.it.GetEnemyCharacters();
					}
					break;


				case ControlSide.ENEMY:
					if (_targetingType == TargetingType.FriendlyAll)
					{
						searchList = CharacterManager.it.GetEnemyCharacters();
					}
					else
					{
						searchList = CharacterManager.it.GetPlayerCharacters();
					}
					break;
			}


			// 타게팅 타입에 맞는 적 리스트 추출
			switch (_targetingType)
			{
				case TargetingType.Default:
					{
						if (_character.target != null)
						{
							_allocTargetCharacter.Add(_character.target);
						}
					}
					break;

				case TargetingType.FriendlyAll:
				case TargetingType.EnemyAll:
					{
						_allocTargetCharacter.AddRange(searchList);
					}
					break;

				case TargetingType.FrontEnemy:
					{
						var targetList = GetCharacterRangeCalculator.Calculate(_character.transform.position, ConfigMeta.it.TARGET_SEARCH_FRONT_ENEMY_RANGE, searchList, true);
						_allocTargetCharacter.AddRange(targetList);
					}
					break;

				case TargetingType.ManyEnemy:
					{
						var targetList = GetCharacterRangeCalculator.Calculate(_character.transform.position, ConfigMeta.it.TARGET_SEARCH_MANY_ENEMY_RANGE, searchList, true);
						_allocTargetCharacter.AddRange(targetList);
					}
					break;

				case TargetingType.HighestHPEnemy:
					{
						var targetList = GetCharacterRangeCalculator.Calculate(_character.transform.position, _character.info.searchRange, searchList, false);

						// 체력이 가장 많은 적 찾기
						Unit target = _character.target;

						foreach (var checkTarget in targetList)
						{
							if (checkTarget.info.data.hp > target.info.data.hp)
							{
								target = checkTarget;
							}
						}

						if (target != null)
						{
							_allocTargetCharacter.Add(_character.target);
						}
					}
					break;
			}

			return _allocTargetCharacter;
		}
	}




	private static class GetCharacterRangeCalculator
	{
		private static List<Unit> _allocCharacterFront = new List<Unit>(100);

		public static List<Unit> Calculate(Vector2 _position, float _radius, List<Unit> _searchList, bool _checkFront)
		{
			_allocCharacterFront.Clear();

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
						_allocCharacterFront.Add(_searchList[i]);
					}
				}
			}

			return _allocCharacterFront;
		}
	}
}
