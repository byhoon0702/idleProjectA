using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class UIStageClear : MonoBehaviour
{

	[SerializeField] private UIStageSuccess uiStageSuccess;
	[SerializeField] private UIStageFail uiStageFail;

	private void Awake()
	{
		uiStageSuccess.Init(this);
		uiStageFail.Init(this);

		uiStageSuccess.gameObject.SetActive(false);
		uiStageFail.gameObject.SetActive(false);
	}
	public void ShowResult(StageRule rule)
	{
		GameManager.GameStop = true;
		uiStageSuccess.Show(rule);
		uiStageFail.Show(rule);
	}

	public void Deactivate()
	{
		uiStageSuccess.gameObject.SetActive(false);
		uiStageFail.gameObject.SetActive(false);
	}
}


