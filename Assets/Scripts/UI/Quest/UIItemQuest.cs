using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIItemQuest : MonoBehaviour
{
	[SerializeField] private Image imageIcon;
	[SerializeField] private TextMeshProUGUI textRewardCount;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI textProgress;
	[SerializeField] private Button buttonReward;
	[SerializeField] private UITextMeshPro uiTextButton;
	[SerializeField] private UITextMeshPro uiTextTitle;

	private RuntimeData.QuestInfo questInfo;
	private Action onClickReward;

	private void Awake()
	{
		buttonReward.onClick.RemoveAllListeners();
		buttonReward.onClick.AddListener(OnClickReward);
	}

	private void OnEnable()
	{

		QuestContainer.OnUpdate += OnUpdate;
	}
	private void OnDisable()
	{
		QuestContainer.OnUpdate -= OnUpdate;
	}

	public void OnUpdate()
	{
		uiTextTitle.SetKey(questInfo.rawData.questName);

		slider.value = questInfo.count / questInfo.GoalCount;

		textProgress.text = $"{questInfo.count.ToString()} / {questInfo.GoalCount.ToString()}";

		buttonReward.interactable = questInfo.progressState == QuestProgressState.COMPLETE;
		switch (questInfo.progressState)
		{
			case QuestProgressState.ONPROGRESS:
			case QuestProgressState.ACTIVE:
				uiTextButton.SetKey("str_ui_onprogress");
				break;
			case QuestProgressState.END:
				uiTextButton.SetKey("str_ui_complete");
				break;
			case QuestProgressState.COMPLETE:
				uiTextButton.SetKey("str_ui_get_reward");
				break;
		}

		if (questInfo.rewardInfos.Count > 0)
		{
			IdleNumber count = (IdleNumber)0;
			count = questInfo.rewardInfos[0].fixedCount;
			imageIcon.sprite = questInfo.rewardInfos[0].iconImage;
			textRewardCount.text = count.ToString();
		}
	}
	public void SetData(RuntimeData.QuestInfo info, Action action)
	{
		questInfo = info;
		onClickReward = action;
		OnUpdate();
	}

	public void OnClickReward()
	{
		questInfo.OnGetReward(true, true);
		onClickReward?.Invoke();
	}

}
