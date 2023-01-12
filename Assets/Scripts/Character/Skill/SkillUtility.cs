using System;
using System.Collections.Generic;
using UnityEngine;



public static class SkillUtility
{
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
	/// 캐릭터 기준으로 타겟 찾기
	/// </summary>
	public static List<Character> GetTargetCharacterNonAlloc(Character _character, TargetingType _targetingType)
	{
		return GetTargetCharacterCalculator.Calculate(_character, _targetingType);
	}

	/// <summary>
	/// 포지션 위치에서 범위안에 있는 유닛들을 찾아줌(메모리할당X)
	/// _checkFront : 내 위치 기준 전방에 있는 유닛들만
	/// </summary>
	public static List<Character> GetCharacterRangeNonAlloc(Vector2 _position, float _radius, List<Character> _searchList, bool _checkFront)
	{
		return GetCharacterRangeCalculator.Calculate(_position, _radius, _searchList, _checkFront);
	}


	public static bool Cumulative(float probability)
	{
		return UnityEngine.Random.Range(0, 1f) <= probability;
	}





	private static class GetTargetCharacterCalculator
	{
		private static List<Character> _allocTargetCharacter = new List<Character>(100);
		private static readonly List<Character> _allocEmpty = new List<Character>(); // 빈 리스트 리턴용

		public static List<Character> Calculate(Character _character, TargetingType _targetingType)
		{
			_allocTargetCharacter.Clear();
			List<Character> searchList = _allocEmpty;

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
						var targetList = GetCharacterRangeCalculator.Calculate(_character.transform.position, _character.info.jobData.attackRange, searchList, false);

						// 체력이 가장 많은 적 찾기
						Character target = _character.target;

						foreach (var checkTarget in targetList)
						{
							if (checkTarget.info.data.hp.GetValue() > target.info.data.hp.GetValue())
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
		private static List<Character> _allocCharacterFront = new List<Character>(100);

		public static List<Character> Calculate(Vector2 _position, float _radius, List<Character> _searchList, bool _checkFront)
		{
			_allocCharacterFront.Clear();

			for (Int32 i = 0 ; i < _searchList.Count ; i++)
			{
				Vector2 targetPosition = _searchList[i].transform.position;

				var direction = (_position - targetPosition).normalized;
				var distance = Vector3.Distance(targetPosition, _position);
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
