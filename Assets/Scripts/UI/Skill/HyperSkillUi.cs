using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HyperSkillUi : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI buttonText;
	[SerializeField] private Image[] hyperModeGauge;
	[SerializeField] private Image[] hyperBreakGauge;


	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => { UnitGlobal.it.hyperModule.auto = !UnitGlobal.it.hyperModule.auto; });
	}

	private void Update()
	{
		buttonText.text = $"auto: {UnitGlobal.it.hyperModule.auto}";
		if (UnitGlobal.it.WaveStated == false)
		{
			return;
		}

		SetProgressHyperMode(UnitGlobal.it.hyperModule.hyperModeGauge);
		SetProgressHyperBreak(UnitGlobal.it.hyperModule.hyperBreakGauge);
	}

	public void SetProgressHyperMode(float _value)
	{
		foreach (var v in hyperModeGauge)
		{
			v.fillAmount = _value;
		}
	}

	public void SetProgressHyperBreak(float _value)
	{
		foreach (var v in hyperBreakGauge)
		{
			v.fillAmount = _value;
		}
	}
}
