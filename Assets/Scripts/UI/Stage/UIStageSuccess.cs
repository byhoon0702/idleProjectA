using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class UIStageSuccess : UIStageResult
{
	[SerializeField] private TextMeshProUGUI textTitle;

	[SerializeField] private Button buttonLeft;
	[SerializeField] private UITextMeshPro textButtonLeft;

	[SerializeField] private Button buttonRight;
	[SerializeField] private UITextMeshPro textButtonRight;

	[SerializeField] private GameObject rewardPrefab;
	[SerializeField] private Transform rewardRoot;

	[SerializeField] private Toggle toggleBossContinue;

	[SerializeField] private ScrollRect scrollRect;
	private Action onClickLeft;
	private Action onClickRight;
	private Action onAutoClick;
	private RuntimeData.StageInfo currentStage;

	private float autoClickTimer = 0;
	private float autoClickTime = 0;

	private float autoClickBossTimer = 0;
	private float autoClickBossTime = 0;

	private string leftButtonKey;
	private string rightButtonKey;
	private bool bossTimer;
	private void Awake()
	{
		buttonLeft.SetButtonEvent(OnClickLeft);
		buttonRight.SetButtonEvent(OnClickRight);

		toggleBossContinue.onValueChanged.RemoveAllListeners();
		toggleBossContinue.onValueChanged.AddListener(OnToggleContinue);
	}

	private void OnToggleContinue(bool isOn)
	{
		bossTimer = isOn;
		StageManager.it.continueBossChallenge = isOn;
		SwitchButton(isOn);

		autoClickTimer = 0;
	}

	private void SwitchButton(bool isTrue)
	{
		if (isTrue)
		{
			autoClickTime = 5;
			textButtonLeft.SetKey(leftButtonKey);
			textButtonRight.SetKey(rightButtonKey).Append($"{autoClickTime} 초");
			onAutoClick = onClickRight;
		}
		else
		{
			autoClickTime = 15;
			onAutoClick = onClickLeft;
			textButtonLeft.SetKey(leftButtonKey).Append($"{autoClickTime} 초");
			textButtonRight.SetKey(rightButtonKey);
		}
	}

	public override void Show(StageRule _rule)
	{
		base.Show(_rule);

		if (rule.isWin == false)
		{
			gameObject.SetActive(false);
			return;
		}

		currentStage = StageManager.it.CurrentStage;

		gameObject.SetActive(true);
		toggleBossContinue.gameObject.SetActive(false);
		buttonRight.gameObject.SetActive(false);
		leftButtonKey = "str_ui_ok";
		textButtonLeft.SetKey(leftButtonKey);
		int showCount = 0;

		if (rule is StageNormal)
		{
			toggleBossContinue.gameObject.SetActive(true);
			buttonRight.gameObject.SetActive(true);

			onClickLeft = () => { rule.End(); StageManager.it.playBossStage = false; StageManager.it.continueBossChallenge = false; };
			onClickRight = () => { rule.End(); StageManager.it.playBossStage = true; };
			toggleBossContinue.SetIsOnWithoutNotify(StageManager.it.continueBossChallenge);

			rightButtonKey = "str_ui_to_boss";

			SwitchButton(toggleBossContinue.isOn);

		}
		else if (rule is StageInfinity)
		{
			autoClickTime = 15;
			onClickLeft = rule.End;
			onAutoClick = onClickLeft;

		}
		else
		{
			autoClickTime = 15;
			onClickLeft = rule.End;
			onAutoClick = onClickLeft;

		}
		showCount = ShowReward(rule.displayRewardList);
		autoClickTimer = 0;

		StartCoroutine(ShowRewardEffect(showCount));
	}

	public int ShowReward(List<RuntimeData.RewardInfo> reward)
	{
		int differ = reward.Count - rewardRoot.childCount;

		if (differ > 0)
		{
			for (int i = 0; i < differ; i++)
			{
				GameObject go = Instantiate(rewardPrefab, rewardRoot);
				go.SetActive(false);
			}
		}

		for (int i = 0; i < rewardRoot.childCount; i++)
		{
			var child = rewardRoot.GetChild(i);
			child.gameObject.SetActive(false);

			if (i < reward.Count)
			{
				if (reward[i] == null)
				{
					continue;
				}
				UIItemReward uiItemReward = child.GetComponent<UIItemReward>();
				uiItemReward.Set(new AddItemInfo(reward[i]));
			}
		}
		//PlatformManager.UserDB.AddRewards(reward, false);
		return reward.Count;
	}


	public int ShowReward()
	{
		var list = currentStage.GetStageRewardList();
		int differ = list.Count - rewardRoot.childCount;

		if (differ > 0)
		{
			for (int i = 0; i < differ; i++)
			{
				GameObject go = Instantiate(rewardPrefab, rewardRoot);
				go.SetActive(false);
			}
		}

		for (int i = 0; i < rewardRoot.childCount; i++)
		{
			var child = rewardRoot.GetChild(i);
			if (i < list.Count)
			{
				child.gameObject.SetActive(false);
				UIItemReward uiItemReward = child.GetComponent<UIItemReward>();
				uiItemReward.Set(new AddItemInfo(list[i]));
			}
			else
			{
				child.gameObject.SetActive(false);
			}
		}
		return list.Count;
	}

	private IEnumerator ShowRewardEffect(int count)
	{
		for (int i = 0; i < rewardRoot.childCount; i++)
		{
			var child = rewardRoot.GetChild(i);
			if (i < count)
			{
				UIItemReward uiItemReward = child.GetComponent<UIItemReward>();
				uiItemReward.ShowEffect();
			}
			scrollRect.horizontalNormalizedPosition = 1f;
			yield return new WaitForSeconds(0.1f);

		}
	}

	void Update()
	{
		autoClickTimer += Time.deltaTime;
		if (autoClickTimer > autoClickTime)
		{
			onAutoClick?.Invoke();
			onAutoClick = null;
			gameObject.SetActive(false);
			autoClickTimer = 0;
		}


		if (rule is StageNormal)
		{
			if (toggleBossContinue.isOn)
			{

				textButtonRight.SetKey(rightButtonKey).Append($" {Mathf.RoundToInt(autoClickTime - autoClickTimer)}초");
			}
			else
			{

				textButtonLeft.SetKey(leftButtonKey).Append($" {Mathf.RoundToInt(autoClickTime - autoClickTimer)}초");
			}
		}
		else
		{

		}

	}

	public void OnClickLeft()
	{
		onClickLeft?.Invoke();
		gameObject.SetActive(false);
	}
	public void OnClickRight()
	{
		onClickRight?.Invoke();
		gameObject.SetActive(false);
	}
}
