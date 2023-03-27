using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UserInfo
{
	public class StageInfo
	{
		public int VitalityKillCount => userData.VitalityKillCount;
		public IdleNumber ImmortalDPS => userData.ImmortalDPS;


		/// <summary>
		/// 현재 플레이해야할(클리어해야할) 최근 스테이지 레벨
		/// </summary>
		public int RecentStageLv(WaveType _waveType)
		{
			for(int i=0 ; i< userData.recentSaveData.Count ; i++)
			{
				if(userData.recentSaveData[i].key == _waveType)
				{
					return userData.recentSaveData[i].value;
				}
			}

			return 1;
		}

		public void SetRecentStageLv(WaveType _waveType, int _value)
		{
			for (int i = 0 ; i < userData.recentSaveData.Count ; i++)
			{
				if (userData.recentSaveData[i].key == _waveType)
				{
					userData.recentSaveData[i].value = _value;
					return;
				}
			}

			userData.recentSaveData.Add(new StageSaveData() { key = _waveType, value = _value });
		}

		/// <summary>
		/// 현재 진행중인 스테이지 레벨
		/// </summary>
		public int PlayingStageLv(WaveType _waveType)
		{
			for (int i = 0 ; i < userData.playingSaveData.Count ; i++)
			{
				if (userData.playingSaveData[i].key == _waveType)
				{
					return userData.playingSaveData[i].value;
				}
			}

			return 1;
		}

		public void SetPlayingStageLv(WaveType _waveType, int _value)
		{
			for (int i = 0 ; i < userData.playingSaveData.Count ; i++)
			{
				if (userData.playingSaveData[i].key == _waveType)
				{
					userData.playingSaveData[i].value = _value;
					return;
				}
			}

			userData.playingSaveData.Add(new StageSaveData() { key = _waveType, value = _value });
		}

		public void SetVitalityKillCount(int _killCount)
		{
			userData.VitalityKillCount = VGameManager.it.battleRecord.killCount;
		}

		public void SetImmortalDPS(IdleNumber _totalDPS)
		{
			userData.ImmortalDPS = _totalDPS;
		}
	}
}
