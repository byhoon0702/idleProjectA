using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HyperSkillUi : MonoBehaviour
{

	[SerializeField] GameObject objHyperPhase;
	[SerializeField] private TextMeshProUGUI buttonAutoSkillText;
	[SerializeField] private TextMeshProUGUI buttonAutoHyperText;
	[SerializeField] private Image[] hyperModeGauge;
	[SerializeField] private Image[] hyperBreakGauge;

	[SerializeField] private Button buttonHyper;
	[SerializeField] private Toggle toggleAutoHyper;
	[SerializeField] private Toggle toggleAutoSkill;

	private HyperModule target;
	bool isHyper;
	private void Awake()
	{
		toggleAutoHyper.onValueChanged.RemoveAllListeners();
		toggleAutoHyper.onValueChanged.AddListener(OnClickAutoHyper);

		toggleAutoSkill.onValueChanged.RemoveAllListeners();
		toggleAutoSkill.onValueChanged.AddListener(OnClickAutoSkill);


		buttonHyper.onClick.RemoveAllListeners();
		buttonHyper.onClick.AddListener(OnClickHyper);
	}

	private void Start()
	{
		objHyperPhase.SetActive(false);
		foreach (var v in hyperModeGauge)
		{
			v.fillAmount = 0;
		}



	}

	public void AddEvent(HyperModule module)
	{
		target = module;
		module.onHyperProgress = SetProgressHyperMode;
	}

	public void SetHyperMode(bool isTrue)
	{
		isHyper = isTrue;
	}
	public void OnClickHyper()
	{
		if (PlatformManager.UserDB.awakeningContainer.HyperActivate == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_awakening_open_warning"]);
			return;
		}
		if (target == null)
		{
			return;
		}

		if (target.IsHyper)
		{
			return;
		}

		if (target.ActivateHyper())
		{
			SetHyperMode(true);
		}
		else
		{
			SetHyperMode(false);
		}

	}

	public void OnClickAutoSkill(bool isTrue)
	{
		PlatformManager.UserDB.skillContainer.isAutoSkill = isTrue;
		if (isTrue)
		{
			buttonAutoSkillText.color = Color.yellow;
		}
		else
		{
			buttonAutoSkillText.color = Color.white;
		}
	}
	public void OnClickAutoHyper(bool isTrue)
	{
		if (PlatformManager.UserDB.awakeningContainer.HyperActivate == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_awakening_open_warning"]);
			toggleAutoHyper.SetIsOnWithoutNotify(false);
			return;
		}


		if (isTrue)
		{
			buttonAutoHyperText.color = Color.yellow;
		}
		else
		{
			buttonAutoHyperText.color = Color.white;
		}
		PlatformManager.UserDB.skillContainer.isAutoHyper = isTrue;
	}

	public void SetProgressHyperMode(float _ratio, float _value)
	{
		foreach (var v in hyperModeGauge)
		{
			v.fillAmount = _ratio;
		}

		var phaseNumbers = HyperModule.hyperPhaseStepNumber;

		int phaseIndex = 0;
		for (int i = phaseNumbers.Length - 1; i >= 0; i--)
		{
			if (_value >= phaseNumbers[i])
			{
				phaseIndex = i;
				break;
			}
		}

		objHyperPhase.SetActive(!isHyper && phaseIndex == 1);
	}

	public void SetProgressHyperBreak(float _value)
	{
		foreach (var v in hyperBreakGauge)
		{
			v.fillAmount = _value;
		}
	}
}
