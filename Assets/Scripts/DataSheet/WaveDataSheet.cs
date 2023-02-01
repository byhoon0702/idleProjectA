using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData : BaseData
{
	//전체 웨이브 수
	public int totalWaveCount;
	//웨이브당 적 등장 수 
	public int countPerWave;
	//웨이브 간 거리
	public float waveDistance;
	//웨이브에 등장하는 적 리스트
	public List<long> enemyTidList;
}

[System.Serializable]
public class WaveDataSheet : DataSheetBase<WaveData>
{
	public WaveData Get(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}

		}

		return null;
	}
}
