using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class UIQuestTracker : MonoBehaviour
{
	[SerializeField] private RectTransform group;
	[SerializeField] private QuestType type;
	[SerializeField] private Image imageIcon;
	[SerializeField] private TextMeshProUGUI textCount;

	[SerializeField] private GameObject rewardEffect;
	[SerializeField] private UITextMeshPro uiTextQuestType;
	[SerializeField] private UITextMeshPro uiTextQuestTitle;
	[SerializeField] private TextMeshProUGUI textProgress;

	private RuntimeData.QuestInfo questInfo;
	private bool hideUI = false;
	private float originX = 10f;
	private float hideX = -220f;
	private void Start()
	{
		hideUI = false;

		group.anchoredPosition = new Vector2(originX, 0);
	}
	private void OnEnable()
	{
		hideUI = false;

		QuestContainer.OnUpdate += OnUpdate;
	}

	private void OnDisable()
	{
		QuestContainer.OnUpdate -= OnUpdate;
	}

	public void OnClickReward()
	{
		if (questInfo.progressState == QuestProgressState.COMPLETE)
		{
			if (questInfo.Type == QuestType.MAIN)
			{
				TutorialManager.instance.QuestComplete();
				PlatformManager.UserDB.questContainer.ReceiveMainQuestReward();
			}
			else
			{
				questInfo.OnGetReward(true, true);
			}

			return;
		}

		hideUI = !hideUI;
		float xPos = hideUI ? hideX : originX;
		group.DOAnchorPosX(xPos, 0.2f);
	}
	public void OnClickNavigate()
	{
		if (hideUI)
		{
			return;
		}
	}

	public void OnUpdate()
	{
		RuntimeData.QuestInfo temp = null;
		if (type == QuestType.MAIN)
		{
			temp = PlatformManager.UserDB.questContainer.CurrentMainQuest;
		}
		else
		{
			temp = PlatformManager.UserDB.questContainer.GetNonMainQuest();
		}

		if (questInfo == null || (questInfo != null && questInfo.progressState != QuestProgressState.COMPLETE))
		{
			questInfo = temp;
		}

		if (questInfo == null)
		{
			gameObject.SetActive(false);
			return;
		}
		if (gameObject.activeInHierarchy == false)
		{
			gameObject.SetActive(true);
		}
		if (type == QuestType.MAIN)
		{
			TutorialManager.instance.BeginGuide(transform as RectTransform, questInfo);
		}

		questInfo.UpdateAccumulateQuest();

		rewardEffect.SetActive(questInfo.progressState == QuestProgressState.COMPLETE);
		string questTitle = "";
		switch (questInfo.Type)
		{
			case QuestType.MAIN:
				questTitle = questInfo.rawData.questTitle;
				break;
			case QuestType.REPEAT:
				questTitle = "str_ui_quest_repeat";
				break;

			case QuestType.DAILY:
				questTitle = "str_ui_quest_daily";
				break;
		}

		uiTextQuestType.SetKey(questTitle);

		uiTextQuestTitle.SetKey(questInfo.rawData.questName);

		textProgress.text = $"{questInfo.count.ToString()}/{questInfo.GoalCount.ToString()}";

		if (questInfo.rewardInfos.Count > 0)
		{
			imageIcon.sprite = questInfo.rewardInfos[0].iconImage;
			textCount.text = questInfo.rewardInfos[0].fixedCount.ToString();
		}
	}

	float time = 0;
	private void Update()
	{
		time += Time.deltaTime;
		if (time > 1)
		{
			OnUpdate();
			time = 0;
		}
	}
}
