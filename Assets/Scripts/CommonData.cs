using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct LevelupConsumeData
{
	public Grade grade;
	public int basicConsume;
	public int starWeight;
	public int increaseRange;
	public int gradeWeight;
}

[System.Serializable]
public struct PetEvolutionData
{
	public int evolutionLevel;
	public int consumeCount;
}

[System.Serializable]
public struct PetDismentleData
{
	public Grade grade;
	public int needCount;
}

/// <summary>
/// 공통적으로 사용되는 데이터 집합체 Config 는 좀 더 설정 값에 가까운 느낌이라 새로 만듦
/// </summary>
[CreateAssetMenu]
[System.Serializable]
public class CommonData : ScriptableObject
{
	/// <summary>
	/// 강화 요구치
	/// </summary>
	[Tooltip("강화 요구치")]
	public List<LevelupConsumeData> LevelUpConsumeDataList;

	public List<PetEvolutionData> PetEvolutionDataList;

	public List<PetDismentleData> PetDismentleDataList;

}
