using System;
using System.Collections.Generic;

public class LevelExpInfo
{
	public Int32 level;
	public Int64 beginExp;
	public Int64 nextExp;
	public Int32 propertyPoint;
}

public static class UserDataCalculator
{
	/// <summary>
	/// 레벨 데이터 전체 리스트
	/// </summary>
	private static LevelExpInfo[] _levelExpInfo = new LevelExpInfo[0];
	private static LevelExpInfo[] levelExpInfo
	{
		get
		{
			if (_levelExpInfo == null || _levelExpInfo.Length == 0)
			{
				CreateLevelExpInfo();
			}

			return _levelExpInfo;
		}
	}


	// 마지막 레벨 계산 데이터 캐싱
	private const Int32 CACHE_COUNT = 5;
	private static List<Int32> cacheLevelData = new List<Int32>(CACHE_COUNT);



	public static LevelExpInfo GetLevelInfo(Int64 _exp)
	{
		// 1레벨 체크
		if (levelExpInfo[1].nextExp > _exp)
		{
			return levelExpInfo[1];
		}

		// 마지막레벨 체크
		if (levelExpInfo[levelExpInfo.Length - 1].nextExp <= _exp)
		{
			return levelExpInfo[levelExpInfo.Length - 1];
		}

		// 캐시된 데이터가 있으면 해당 데이터 사용
		LevelExpInfo cacheLevel = GetCachedLevel(_exp);
		if (cacheLevel != null)
		{
			return cacheLevel;
		}

		// 없으면 검색후 데이터 캐싱
		for (Int32 i = 1 ; i < levelExpInfo.Length ; i++)
		{
			if (_exp < levelExpInfo[i].nextExp)
			{
				cacheLevelData.Add(i);

				// 개수초과는 제거
				if (CACHE_COUNT < cacheLevelData.Count)
				{
					cacheLevelData.RemoveAt(0);
				}

				return levelExpInfo[i];
			}
		}

		return cacheLevel;
	}

	private static LevelExpInfo GetCachedLevel(Int64 _exp)
	{
		for (Int32 i = 0 ; i < cacheLevelData.Count ; i++)
		{
			Int32 index = cacheLevelData[i];
			if (levelExpInfo[index].beginExp <= _exp && _exp < levelExpInfo[index].nextExp)
			{
				return levelExpInfo[index];
			}
		}

		return null;
	}


	public static Int32 GetPropertyPoint(Int32 _level)
	{
		return _levelExpInfo[UnityEngine.Mathf.Clamp(_level, 0, _levelExpInfo.Length)].propertyPoint;
	}

	private static void CreateLevelExpInfo()
	{
		UserLevelDataSheet dataSheet = DataManager.Get<UserLevelDataSheet>();

		_levelExpInfo = new LevelExpInfo[dataSheet.infos.Count + 1]; // <-- 레벨은 1부터 시작하기에 + 1해줌

		_levelExpInfo[0] = new LevelExpInfo();
		_levelExpInfo[0].level = 0;
		_levelExpInfo[0].nextExp = 0;
		_levelExpInfo[0].propertyPoint = 0;

		foreach (var data in dataSheet.infos)
		{
			_levelExpInfo[data.level] = new LevelExpInfo();

			_levelExpInfo[data.level].level = data.level;
			_levelExpInfo[data.level].nextExp = data.nextExp;
			_levelExpInfo[data.level].propertyPoint = data.propertyPoint;
		}

		for (Int32 i = 1 ; i < _levelExpInfo.Length ; i++)
		{
			_levelExpInfo[i].beginExp = _levelExpInfo[i - 1].nextExp;
		}

		// 데이터 검증
		CheckLevelExpValid();
	}

	private static void CheckLevelExpValid()
	{
		for (Int32 i = 1 ; i < levelExpInfo.Length ; i++)
		{
			if (levelExpInfo[i] == null)
			{
				VLog.LogError($"[데이터오류] 레벨정보 없음. Lv: {i}");
			}
		}
	}
}
