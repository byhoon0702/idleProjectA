




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;





public class UIDungeonDetailImmortalHub : MonoBehaviour, IUIClosable
{
	[Header("Info")]
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI combatPowerText;

	[Header("보상")]
	[SerializeField] private UIItemBase[] uiItemRewardList;
	[SerializeField] private TextMeshProUGUI[] probabilityText;

	[Header("버튼")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button moveButton;
	[SerializeField] private Button autoButton;
	[SerializeField] private TextMeshProUGUI moveButtonText;
	[SerializeField] private TextMeshProUGUI autoButtonText;


	private GameImmortalHubStageInfo stageInfo;


	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		moveButton.onClick.RemoveAllListeners();
		moveButton.onClick.AddListener(OnMoveButtonClick);

		autoButton.onClick.RemoveAllListeners();
		autoButton.onClick.AddListener(OnAutoButtonClick);
	}

	public void OnUpdate()
	{
		stageInfo = StageManager.it.metaGameStage.GetStage(WaveType.ImmortalHub, 1) as GameImmortalHubStageInfo;
		UpdateInfo();
		UpdateButton();
	}

	private void UpdateInfo()
	{
		titleText.text = $"{stageInfo.StageName} {stageInfo.StageSubTitle}";
		combatPowerText.text = $"최대: {stageInfo.MaxHitDamage.ToString()}";

		var rewards = stageInfo.GetAllRewards();
		for(int i=0 ; i<uiItemRewardList.Length ; i++)
		{
			if(rewards.Count <= i)
			{
				uiItemRewardList[i].gameObject.SetActive(false);
			}
			else
			{
				uiItemRewardList[i].gameObject.SetActive(true);
				uiItemRewardList[i].OnUpdate(rewards[i]);
			}
		}
	}

	private void UpdateButton()
	{
		var item = Inventory.it.FindItemByTid(stageInfo.ConsumeItemTid);

		if(item != null)
		{
			if(item is ItemMoney)
			{
				var itemMoney = item as ItemMoney;

				moveButtonText.text = $"이동 {itemMoney.Count.ToString()} / {itemMoney.SystemMax.ToString()}";
				autoButtonText.text = $"소탕 {itemMoney.Count.ToString()} / {itemMoney.SystemMax.ToString()}";
			}
			else
			{
				moveButtonText.text = $"이동 {item.Count.ToString()}";
				autoButtonText.text = $"소탕 {item.Count.ToString()}";
			}
		}
		else
		{
			moveButtonText.text = $"이동";
			autoButtonText.text = $"소탕";
		}
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	private void OnMoveButtonClick()
	{
		var playResult = stageInfo.Play();
		if(playResult.Fail())
		{
			PopAlert.Create(playResult);
			return;
		}

		UIController.it.UIDungeonList.Close();
	}

	private void OnAutoButtonClick()
	{

	}
}
