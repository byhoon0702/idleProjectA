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

	RuntimeData.StageInfo stageInfo;
	public void OnUpdate(RuntimeData.StageInfo _stageInfo)
	{
		stageInfo = _stageInfo;
		textStageTitle.text = $"{stageInfo.stageListData.stageNumber} {stageInfo.stageListData.stageNumber}";
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
