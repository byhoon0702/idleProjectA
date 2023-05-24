using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGlobal : MonoBehaviour
{
	private static UnitGlobal instance;
	public static UnitGlobal it => instance;


	private bool waveStarted;
	public bool WaveStated => waveStarted;



	private void Awake()
	{
		instance = this;
	}

	public void ResetModule()
	{
	}

	public void WaveStart()
	{
		waveStarted = true;
		//skillModule.SetUnit(UnitManager.it.Player);

	}

	public void WaveFinish()
	{
		waveStarted = false;
	}

	private void Update()
	{
		if (waveStarted == false)
		{
			return;
		}


	}
}
