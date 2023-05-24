using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperModule : MonoBehaviour
{
	private bool isHyper;
	public bool IsHyper => isHyper;
	public static readonly float[] hyperPhaseStepNumber = new float[] { 0f, 10f, 15f, 20f };
	private float hyperFullCharge => hyperPhaseStepNumber[hyperPhaseStepNumber.Length - 1];
	private float hyperGaugePerHit
	{
		get
		{
			return hyperFullCharge / 50f;
		}
	}
	private float hyperGauge = 0f;

	public delegate void OnHyperProgress(float ratio, float current);
	public OnHyperProgress onHyperProgress;

	private Unit unit;

	private void Start()
	{
		UIController.it.HyperSkill.AddEvent(this);
		unit = GetComponent<Unit>();
	}
	float time = 0f;
	float maxTime = 0f;

	Action onComplete;
	void Update()
	{

		if (isHyper == false)
		{
			return;
		}

		if (GameManager.UserDB.skillContainer.isAutoHyper)
		{
			ActivateHyper();
		}

		ConsumeHyperGauge(Time.deltaTime);
	}

	public bool ActivateHyper()
	{
		var phaseNumbers = hyperPhaseStepNumber;

		int phaseIndex = 0;
		for (int i = phaseNumbers.Length - 1; i >= 0; i--)
		{
			if (hyperGauge >= phaseNumbers[i])
			{
				phaseIndex = i;
				break;
			}
		}

		if (phaseIndex == 0)
		{
			return false;
		}

		maxTime = hyperPhaseStepNumber[phaseIndex];
		time = maxTime;

		isHyper = true;

		(unit as PlayerUnit).hyperPhase = phaseIndex;
		unit.ActiveHyperEffect();

		return true;
	}

	public void StackHyperGauge()
	{
		if (isHyper)
		{
			return;
		}

		hyperGauge += hyperGaugePerHit;
		if (hyperGauge > hyperFullCharge)
		{
			hyperGauge = hyperFullCharge;
		}
		onHyperProgress?.Invoke(hyperGauge / hyperFullCharge, hyperGauge);
	}

	public void ConsumeHyperGauge(float delta)
	{
		time -= delta;
		onHyperProgress?.Invoke(time / hyperFullCharge, time);
		if (time <= 0f)
		{
			hyperGauge = 0f;
			unit.InactiveHyperEffect();
			isHyper = false;
			UIController.it.HyperSkill.SetHyperMode(false);
		}

	}


}
