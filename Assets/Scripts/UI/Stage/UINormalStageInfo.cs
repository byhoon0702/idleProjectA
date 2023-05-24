using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINormalStageInfo : MonoBehaviour
{
	[SerializeField] private UIStageInfo parent;
	[SerializeField] private TextMeshProUGUI textStageTitle;
	[SerializeField] private Slider sliderStageProgress;
	[SerializeField] private Button buttonBossEnter;

	StageInfo stageInfo;
	public void OnUpdate(StageInfo _stageInfo)
	{
		stageInfo = _stageInfo;
		textStageTitle.text = $"{stageInfo.data.stageNumber} {stageInfo.data.stageNumber}";
	}

	public void OnClickBoss()
	{
		parent.OnClickPlayBoss();
	}

	//public void SetWaveGauge(float _ratio)
	//{
	//	sliderStageProgress.value = _ratio;
	//}
}
