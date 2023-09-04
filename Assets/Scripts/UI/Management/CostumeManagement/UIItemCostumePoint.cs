using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemCostumePoint : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMeshPoint;
	[SerializeField] private TextMeshProUGUI textMeshValue;
	[SerializeField] private Button buttonReward;
	[SerializeField] private Button buttonRewardCant;
	[SerializeField] private Button buttonRewardComplete;
	[SerializeField] private UITextMeshPro textButtonReward;

	RuntimeData.CostumePointInfo info;

	UIPopupCostumePoint parent;
	private void Awake()
	{
		buttonReward.SetButtonEvent(OnClickReward);
		buttonRewardCant.SetButtonEvent(OnClickCannot);
		buttonRewardComplete.SetButtonEvent(OnClickComplete);
	}

	public void OnClickCannot()
	{
		ToastUI.Instance.Enqueue("포인트가 부족합니다.");
	}
	public void OnClickComplete()
	{
		ToastUI.Instance.Enqueue("이미 획득하였습니다.");
	}
	public void OnUpdate(UIPopupCostumePoint _parent, RuntimeData.CostumePointInfo _info)
	{
		parent = _parent;

		info = _info;

		textMeshPoint.text = info.needPoint.ToString();
		textMeshValue.text = $" +{info.rewardAbility.type.ToUIString()}{info.rewardAbility.Value.ToString()}%";


		bool needPoint = PlatformManager.UserDB.costumeContainer.TotalCostumePoints < info.needPoint;

		buttonRewardCant.gameObject.SetActive(needPoint && !info.IsGet);
		buttonRewardComplete.gameObject.SetActive(info.IsGet);
		buttonReward.gameObject.SetActive(!info.IsGet && !needPoint);

		//if (PlatformManager.UserDB.costumeContainer.TotalCostumePoints < info.needPoint)
		//{
		//	buttonReward.interactable = false;
		//}
		//else
		//{
		//	buttonReward.interactable = true;
		//}

		//if (info.IsGet)
		//{
		//	buttonReward.interactable = false;
		//	textButtonReward.SetKey("str_ui_complete");
		//}
		//else
		//{
		//	textButtonReward.SetKey("str_ui_reward");
		//}

	}

	public void OnClickReward()
	{
		info.GetCostumeReward();
		parent.OnUpdate();
	}
}
