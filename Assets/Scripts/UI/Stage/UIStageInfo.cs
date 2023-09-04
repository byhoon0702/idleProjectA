using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIStageInfo : MonoBehaviour
{
	[SerializeField] private GameObject stageNameRoot;
	[SerializeField] private TextMeshProUGUI textStageName;

	[SerializeField] private Button btnExit;
	[SerializeField] private Button btnPlayBoss;
	public Button BtnPlayBoss => btnPlayBoss;
	[SerializeField] private Toggle toggleContinue;

	[SerializeField] private GameObject objBossIcon;
	[SerializeField] private GameObject objNormalIcon;


	[SerializeField] private GameObject btnContinueChallange;
	[SerializeField] private GameObject objNormalButtonGroup;

	[SerializeField] private Slider hpSlider;
	[SerializeField] private TextMeshProUGUI textHpSlider;

	[Header("타이머")]
	[SerializeField] private GameObject objectTimer;
	[SerializeField] private Slider sliderTimeAttack;
	[SerializeField] private TextMeshProUGUI textTimeAttack;

	private RuntimeData.StageInfo stageInfo;

	public void ShowNormalButtonGroup(bool isTrue)
	{
		objNormalButtonGroup.SetActive(isTrue);
	}

	public void ToggleContinueChallenge(bool isTrue)
	{
		StageManager.it.continueBossChallenge = isTrue;
	}

	public void SwitchBossMode(bool isTrue)
	{
		objBossIcon.SetActive(isTrue);
		objNormalIcon.SetActive(isTrue == false);

		if (isTrue)
		{
			textHpSlider.text = "100%";
			hpSlider.value = 1f;
		}
		else
		{
			textHpSlider.text = "";
			hpSlider.value = 0f;
		}
	}
	public void TurnOffUI()
	{
		objectTimer.SetActive(false);
		stageNameRoot.SetActive(false);
		objNormalButtonGroup.SetActive(false);
	}

	public void ShowStageName()
	{
		var curStage = StageManager.it.CurrentStage;
		hpSlider.value = 0f;
		stageNameRoot.SetActive(true);
		textStageName.text = $"STAGE {curStage.StageNumber} {PlatformManager.Language[curStage.Name]}";
	}

	public void ShowBossName()
	{
		stageNameRoot.SetActive(true);
		textStageName.text = "";
		if (UnitManager.it.Boss != null)
		{
			textStageName.text = PlatformManager.Language[UnitManager.it.Boss.info.rawData.name];
		}
	}
	public void SetBossHpGauge(float _ratio)
	{
		hpSlider.value = _ratio;
		textHpSlider.text = $"{_ratio * 100f:0.##}%";
	}

	public void OnUpdate(RuntimeData.StageInfo _stageInfo)
	{
		stageInfo = _stageInfo;
		toggleContinue.isOn = StageManager.it.continueBossChallenge;
	}

	public void SetTimer(float time)
	{
		objectTimer.SetActive(true);
		sliderTimeAttack.value = 1;

		textTimeAttack.text = $"{time}s";
	}
	public void UpdateTimer(float time, float maxTime)
	{
		sliderTimeAttack.value = time / maxTime;
		textTimeAttack.text = $"{time:0.##}s";
	}

	public void RefreshKillCount()
	{
		if (StageManager.it.bossSpawn)
		{
			return;
		}

		StringBuilder sb = new StringBuilder();

		if (StageManager.it.CurrentStage.CountLimit > 0)
		{
			if (sb.Length != 0)
			{
				sb.Append("\n");
			}
			sb.Append($"{GameManager.it.battleRecord.killCount}/{StageManager.it.CurrentStage.CountLimit}");
		}

		textHpSlider.text = sb.ToString();
		if (StageManager.it.CurrentStage.CountLimit > 0)
		{
			hpSlider.value = (float)(GameManager.it.battleRecord.killCount) / (float)(StageManager.it.CurrentStage.CountLimit);
		}
	}

	public void RefreshDPSCount()
	{
		var totalDamage = StageManager.it.cumulativeDamage;
		textHpSlider.text = $"누적딜량\n{totalDamage.ToString()}";
	}


	public void SwitchContentExitButton(bool isContentEnter)
	{
		btnExit.gameObject.SetActive(isContentEnter);
		btnPlayBoss.gameObject.SetActive(isContentEnter == false);
	}


	public void OnClickPlayBoss()
	{
		// 현재레벨의 보스 스테이지 찾기
		var bossStageInfo = PlatformManager.UserDB.stageContainer.GetStage(StageType.Normal, StageManager.it.CurrentStage.StageNumber);
		StageManager.it.OnClickBoss(bossStageInfo);

	}

	public void OnClickExit()
	{
		StageManager.it.ReturnNormalStage();
		//SwitchContentExitButton(false);
	}
}
