using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class HyperModule : MonoBehaviour
{
	public TimelineAsset hyperChangeTimeline;
	private bool isHyper;
	public bool IsHyper => isHyper;
	public static readonly float[] hyperPhaseStepNumber = new float[] { 0f, 12f };
	private float hyperFullCharge => hyperPhaseStepNumber[hyperPhaseStepNumber.Length - 1];
	private float hyperGaugePerHit
	{
		get
		{
			return hyperFullCharge / 10f;
		}
	}
	private float hyperGauge = 0f;

	public delegate void OnHyperProgress(float ratio, float current);
	public OnHyperProgress onHyperProgress;
	private SkillSlot hyperFinishSkill;
	private Unit unit;

	private void Start()
	{
		UIController.it.HyperSkill.AddEvent(this);
		unit = GetComponent<Unit>();


		hyperFinishSkill = new SkillSlot();
		hyperFinishSkill.item = GameManager.UserDB.skillContainer.skillList.Find(x => x.Tid == 1701600010);
	}
	float time = 0f;
	float maxTime = 0f;

	Action onComplete;
	public PlayerUnit player => unit as PlayerUnit;
	void Update()
	{
		if (unit.IsAlive() == false)
		{
			return;
		}
		if (unit is PlayerUnit)
		{
			if (player.info.HyperAvailable == false)
			{
				return;
			}
		}
		if (isHyper == false)
		{
			if (GameManager.UserDB.skillContainer.isAutoHyper)
			{
				ActivateHyper();
			}
			return;
		}

		ConsumeHyperGauge(Time.deltaTime);
	}

	public void ShowHyperEnterEffect(GameObject particlePrefab)
	{
		if (particlePrefab == null)
		{
			return;
		}
		GameObject effect = Instantiate(particlePrefab);
		effect.transform.position = UnitManager.it.Player.position;
	}


	public bool ActivateHyper()
	{
		if (unit.IsAlive() == false)
		{
			return false;
		}

		if (unit is PlayerUnit)
		{
			if (player.info.HyperAvailable == false)
			{
				return false;
			}
		}

		useFinish = false;
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
		if (unit is PlayerUnit)
		{
			if (player.info.HyperAvailable == false)
			{
				return;
			}
		}
		hyperGauge += hyperGaugePerHit;
		if (hyperGauge > hyperFullCharge)
		{
			hyperGauge = hyperFullCharge;
		}
		onHyperProgress?.Invoke(hyperGauge / hyperFullCharge, hyperGauge);
	}


	bool useFinish = false;
	public void ConsumeHyperGauge(float delta)
	{
		if (GameManager.GameStop)
		{
			return;
		}
		if (unit is PlayerUnit)
		{
			PlayerUnit player = unit as PlayerUnit;
			time -= delta;
			onHyperProgress?.Invoke(time / hyperFullCharge, time);
			if (time <= 0f)
			{
				if (useFinish == false)
				{
					//필살기 시전

					var hyperClassObject = GameManager.UserDB.awakeningContainer.selectedInfo.hyperClassObject;
					player.PlayHyperFinishTimeline(hyperClassObject.FinishTimeline);
					hyperFinishSkill = new SkillSlot();
					hyperFinishSkill.item = GameManager.UserDB.skillContainer.skillList.Find(x => x.Tid == hyperClassObject.FinishSkillTid);
					player.TriggerHyperFinishSkill(hyperFinishSkill);
					player.ChangeState(StateType.HYPER_FINISH, true);
					useFinish = true;
				}
			}
		}
	}
	public void EndHyper()
	{

		UnitManager.it.Player.unitAnimation.ChangeSortingLayer("CharDepth1");
		hyperGauge = 0f;
		unit.InactiveHyperEffect();
		isHyper = false;
		UIController.it.HyperSkill.SetHyperMode(false);
		UnitManager.it.Player.ChangeState(StateType.IDLE, true);
	}

}
