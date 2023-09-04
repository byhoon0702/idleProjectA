using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIBattlePassReward : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI _textContext;
	[SerializeField] private Animator _animation;
	[Header("무료")]
	[SerializeField] private UIItemReward _freeReward;
	[SerializeField] private GameObject _objFreeCheckMark;
	[SerializeField] private GameObject _objFreeReceive;

	[Header("유료")]
	[SerializeField] private UIItemReward _passReward;
	[SerializeField] private GameObject _objPassCheckMark;
	[SerializeField] private GameObject _objPassReceive;
	RuntimeData.BattlePassTierInfo _tierInfo;
	UIPopupBattlePass _parent;
	bool _passPurchased;
	public void SetData(UIPopupBattlePass parent, RuntimeData.BattlePassTierInfo tierInfo, bool passPurchased)
	{
		_parent = parent;
		_tierInfo = tierInfo;

		_passPurchased = passPurchased;

		_textContext.text = _tierInfo.condition.parameter2.ToString();

		_freeReward.Set(new AddItemInfo(_tierInfo.freeReward));
		_passReward.Set(new AddItemInfo(_tierInfo.passReward));

		bool conditionOk = _tierInfo.CheckCondition().IsNullOrEmpty();

		if (conditionOk && (_tierInfo.isFreeRawardClaim == false || (_passPurchased && _tierInfo.isPassRawardClaim == false)))
		{
			_animation.enabled = true;

		}
		else
		{
			_animation.enabled = false;
		}

		_objFreeReceive.SetActive(conditionOk && _tierInfo.isFreeRawardClaim == false);
		_objPassReceive.SetActive(conditionOk && _passPurchased && _tierInfo.isPassRawardClaim == false);

		_objFreeCheckMark.SetActive(_tierInfo.isFreeRawardClaim);
		_objPassCheckMark.SetActive(_tierInfo.isPassRawardClaim);
	}

	public void OnClickFreeReward()
	{
		string message = _tierInfo.CheckCondition();
		if (message.IsNullOrEmpty() == false)
		{
			ToastUI.Instance.Enqueue(message);
			return;
		}

		if (_tierInfo.isFreeRawardClaim)
		{
			return;
		}

		_tierInfo.GetFreeReward();
		_parent.RefreshView();
	}

	public void OnClickPassReward()
	{
		if (_passPurchased == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_buy_pass"]);
			return;
		}
		string message = _tierInfo.CheckCondition();
		if (message.IsNullOrEmpty() == false)
		{
			ToastUI.Instance.Enqueue(message);
			return;
		}
		if (_tierInfo.isPassRawardClaim)
		{
			return;
		}
		///결제 여부 확인 코드 필요

		_tierInfo.GetPassReward();
		_parent.RefreshView();
	}

	public void OnClickReward()
	{
		OnClickFreeReward();
		OnClickPassReward();

	}
}
