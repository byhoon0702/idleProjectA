using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStageInfo : MonoBehaviour
{
	[Header("이름")]
	[SerializeField] private GameObject nameRoot;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private Button bossButton;
	[SerializeField] private Button exitButton;

	[Header("웨이브 게이지")]
	[SerializeField] private GameObject waveGaugeRoot;
	[SerializeField] private Slider waveSlider;
	[SerializeField] private GameObject waveRewordNormal;
	[SerializeField] private GameObject waveRewordSpecial;

	[Header("HP 게이지")]
	[SerializeField] private GameObject hpGaugeRoot;
	[SerializeField] private Slider hpSlider;
	[SerializeField] private TextMeshProUGUI hpBossNameText;
	[SerializeField] private TextMeshProUGUI hpGaugeText;

	[Header("시간 게이지")]
	[SerializeField] private GameObject timeGaugeRoot;
	[SerializeField] private Slider timeSlider;
	[SerializeField] private TextMeshProUGUI timeGaugeText;

	[Header("누적 DPS")]
	[SerializeField] private GameObject dpsRoot;
	[SerializeField] private TextMeshProUGUI dpsText;

	[Header("처치수")]
	[SerializeField] private GameObject killRoot;
	[SerializeField] private TextMeshProUGUI killText;



	private GameStageInfo stageInfo;


	private void Awake()
	{
		//bossButton.onClick.RemoveAllListeners();
		//bossButton.onClick.AddListener(OnClickPlayBoss);

		//exitButton.onClick.RemoveAllListeners();
		//exitButton.onClick.AddListener(OnClickExit);

		//gameObject.SetActive(false);
	}

	public void OnUpdate(GameStageInfo _stageInfo)
	{
		//stageInfo = _stageInfo;

		//gameObject.SetActive(true);

		//UpdateName();
		//UpdateButton();
		//UpdateWaveGauge();
		//UpdateHPGauge();
		//UpdateTimeGauge();
		//UpdateKill();
		//UpdateDPS();
	}

	private void UpdateName()
	{
		if (stageInfo.IsShowStageNameUI == false)
		{
			nameRoot.SetActive(false);
			return;
		}

		nameRoot.SetActive(true);
		nameText.text = $"{stageInfo.StageName} {stageInfo.StageSubTitle}";
	}

	private void UpdateButton()
	{
		//bossButton.gameObject.SetActive(stageInfo.WaveType == WaveType.Normal);
		//exitButton.gameObject.SetActive(stageInfo.WaveType != WaveType.Normal);
	}

	private void UpdateWaveGauge()
	{
		//if (stageInfo.IsShowWaveGaugeUI == false)
		//{
		//	waveGaugeRoot.SetActive(false);
		//	return;
		//}

		//waveGaugeRoot.SetActive(true);
		//waveRewordNormal.SetActive(stageInfo.IsShowNormalRewardIcon);
		//waveRewordSpecial.SetActive(stageInfo.IsShowSpecialRewardIcon);

		//SetWaveGauge(0);
	}

	private void UpdateHPGauge()
	{
		if (stageInfo.IsShowHPGaugeUI == false)
		{
			hpGaugeRoot.SetActive(false);
			return;
		}

		hpGaugeRoot.SetActive(true);

		SetBossName(stageInfo.spawnLast.unitData.name, stageInfo.spawnLast.spawnLevel);
		SetHpGauge(1);
	}

	public void SetBossName(string _bossName, int _level)
	{
		// hpBossNameText.text = $"{_bossName} Lv. {_level}";
	}

	private void UpdateTimeGauge()
	{
		if (stageInfo.IsShowTimeGauge == false)
		{
			timeGaugeRoot.SetActive(false);
			return;
		}

		timeGaugeRoot.SetActive(true);
		SetTimeGauge(stageInfo.TimeLimit);
	}

	private void UpdateKill()
	{
		if (stageInfo.IsShowKillUI == false)
		{
			killRoot.SetActive(false);
			return;
		}


		killRoot.SetActive(true);
		killText.text = $"누적처치수\n{0}";
	}

	private void UpdateDPS()
	{
		if (stageInfo.IsShowDPSUI == false)
		{
			dpsRoot.SetActive(false);
			return;
		}


		dpsRoot.SetActive(true);
		dpsText.text = $"누적딜량\n{0}";
	}

	public void RefreshKillCount()
	{
		killText.text = $"누적처치수\n{VGameManager.it.battleRecord.killCount}";
	}

	public void RefreshDPSCount()
	{
		var totalDamage = VGameManager.it.battleRecord.playerDPS.attackPower + VGameManager.it.battleRecord.petDPS.attackPower;
		dpsText.text = $"누적딜량\n{totalDamage.ToString()}";
	}

	public void SetWaveGauge(float _ratio)
	{
		waveSlider.value = _ratio;
	}

	public void SetTimeGauge(float remainTime)
	{
		float time = Mathf.Clamp(remainTime, 0, remainTime);

		timeSlider.value = time / stageInfo.TimeLimit;
		timeGaugeText.text = $"{remainTime.ToString("F1")}s";
	}

	public void SetHpGauge(float _ratio)
	{
		hpSlider.value = _ratio;
		hpGaugeText.text = $"{Mathf.CeilToInt(_ratio * 100).ToString()}%";
	}

	private void OnClickPlayBoss()
	{
		// 현재레벨의 보스 스테이지 찾기
		var bossStageInfo = StageManager.it.metaGameStage.GetStage(WaveType.NormalBoss, StageManager.it.CurrentStage.StageLv);
		StageManager.it.PlayStage(bossStageInfo);
	}

	private void OnClickExit()
	{
		var playStage = StageManager.it.metaGameStage.GetStage(WaveType.Normal, UserInfo.stage.PlayingStageLv(WaveType.Normal));
		StageManager.it.PlayStage(playStage);
	}
}
