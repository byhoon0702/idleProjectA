using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageWaveData : BaseData
{
	/// <summary>
	/// 웨이브 타입
	/// </summary>
	public WaveType waveType;
	/// <summary>
	/// 전체 웨이브 수
	/// </summary>
	public int waveCount;
	/// <summary>
	/// 일반 몬스터 등장여부
	/// </summary>
	public bool spawnNormal;
	/// <summary>
	/// 보물상자 등장여부
	/// </summary>
	public bool spawnTreasureBox;
	/// <summary>
	/// 보스 등장여부
	/// </summary>
	public bool spawnBoss;
	/// <summary>
	/// 죽지않는 보스몬스터
	/// </summary>
	public bool spawnImmotal;
	/// <summary>
	/// 웨이브 간 거리
	/// </summary>
	public float waveDistance;
	/// <summary>
	/// 제한시간(0이면 무제한)
	/// </summary>
	public float timeLimit;
	/// <summary>
	/// 기본 생명력
	/// </summary>
	public string defaultHP;
	[NonSerialized] private IdleNumber _defaultHP;
	public IdleNumber DefaultHP
	{
		get
		{
			if (_defaultHP == 0)
			{
				_defaultHP = (IdleNumber)defaultHP;
			}

			return _defaultHP;
		}
	}
	/// <summary>
	/// 기본 공격력
	/// </summary>
	public string defaultAttackPower;
	[NonSerialized] private IdleNumber _defaultAttackPower;
	public IdleNumber DefaultAttackPower
	{
		get
		{
			if (_defaultAttackPower == 0)
			{
				_defaultAttackPower = (IdleNumber)defaultAttackPower;
			}

			return _defaultAttackPower;
		}
	}
	/// <summary>
	/// 기본 공격속도
	/// </summary>
	public float attackSpeed;
}

[System.Serializable]
public class StageWaveDataSheet : DataSheetBase<StageWaveData>
{

}


public enum StageDifficulty
{
	Easy,
	Normal,
	Hard,
	Nightmare
}


public enum WaveType
{
	_None = 0,

	/// <summary>
	/// 일반 스테이지
	/// </summary>
	Normal = 1,

	/// <summary>
	/// 일반스테이지 보스전
	/// </summary>
	NormalBoss = 2,








	/*********************
	 *  던전모드
	 *********************/


	/// <summary>
	/// 참깨동굴
	/// </summary>
	Sesame = 3,

	/// <summary>
	/// 용사의 무덤
	/// </summary>
	Tomb = 4,

	/// <summary>
	/// 오염된 부화장
	/// </summary>
	Hatchery = 5,

	/// <summary>
	/// 불로초 평원
	/// </summary>
	ImmortalHub = 6,

	/// <summary>
	/// 회춘던전
	/// </summary>
	Youth = 7,




	/*********************
	 *  도전모드
	 *********************/


	/// <summary>
	/// 악몽의 탑
	/// </summary>
	NightmareTower = 101,

	/// <summary>
	/// 넘치는 활력의 CA
	/// </summary>
	Vitality = 102,

	/// <summary>
	/// 보스러쉬-냉기지대
	/// </summary>
	BossRush_Ice = 103,
	/// <summary>
	/// 보스러쉬-화염지대
	/// </summary>
	BossRush_Fire = 104,
	/// <summary>
	/// 보스러쉬-바위지대
	/// </summary>
	BossRush_Stone = 105,
	/// <summary>
	/// 보스러쉬-어둠지대
	/// </summary>
	BossRush_Dark = 106,
	/// <summary>
	/// 보스러쉬-부서진배
	/// </summary>
	BossRush_Ship = 107,
	/// <summary>
	/// 보스러쉬-화산지대
	/// </summary>
	BossRush_Volcano = 108,
}
