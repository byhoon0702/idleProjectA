using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupBattlePass : UIBase
{

	[SerializeField] private Toggle toggleLevel;
	[SerializeField] private Toggle toggleStage;
	[SerializeField] private Toggle toggleHunting;

	[SerializeField] private TextMeshProUGUI _textPrice;
	[SerializeField] private TextMeshProUGUI _textPassName;
	[SerializeField] private TextMeshProUGUI _textBuyInfo;
	[SerializeField] private TextMeshProUGUI _textPassInfo;
	[SerializeField] private TextMeshProUGUI _textPassContext;
	[SerializeField] private TextMeshProUGUI _textProgress;

	[SerializeField] private Transform _contentBattlePass;
	[SerializeField] private GameObject _uiItemBattlePass;

	[SerializeField] private Transform _contentReward;
	[SerializeField] private GameObject _uiItemPassReward;

	[SerializeField] private UIBattlePassReward _lastReward;

	private BattlePassType currentType;
	private List<RuntimeData.BattlePassInfo> passList;
	private RuntimeData.BattlePassInfo _selectedInfo;

	public void OnToggleLevelPass(bool isOn)
	{
		if (isOn)
		{
			currentType = BattlePassType.LEVEL;
			TabChanged();
		}
	}

	public void OnToggleStagePass(bool isOn)
	{
		if (isOn)
		{
			currentType = BattlePassType.STAGE;
			TabChanged();
		}
	}

	public void OnToggleHuntingPass(bool isOn)
	{
		if (isOn)
		{
			currentType = BattlePassType.HUNTINNG;
			TabChanged();
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();

	}
	protected override void OnDisable()
	{
		base.OnDisable();


	}


	public void Open()
	{
		if (Activate() == false)
		{
			return;
		}
		gameObject.SetActive(true);
		toggleLevel.SetIsOnWithoutNotify(true);
		OnToggleLevelPass(true);
	}


	public void TabChanged()
	{
		passList = PlatformManager.UserDB.battlePassContainer.infoList.FindAll(x => x.rawData.type == currentType);
		_contentBattlePass.CreateListCell(passList.Count, _uiItemBattlePass);
		for (int i = 0; i < _contentBattlePass.childCount; i++)
		{
			var child = _contentBattlePass.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < passList.Count)
			{
				UIToggleBattlePass togglePass = child.GetComponent<UIToggleBattlePass>();
				togglePass.SetData(this, passList[i]);

				child.gameObject.SetActive(true);
			}
		}

		switch (currentType)
		{
			case BattlePassType.LEVEL:
				_textProgress.text = $"{PlatformManager.UserDB.userInfoContainer.userInfo.UserLevel}";
				_textPassInfo.text = PlatformManager.Language["str_ui_levelpass_tab_desc"];
				break;
			case BattlePassType.STAGE:
				var stageInfo = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage();
				_textProgress.text = $"{stageInfo.StageNumber}";
				_textPassInfo.text = PlatformManager.Language["str_ui_stagepass_tab_desc"];
				break;
			case BattlePassType.HUNTINNG:
				_textProgress.text = $"{PlatformManager.UserDB.userInfoContainer.dailyKillCount}";
				_textPassInfo.text = PlatformManager.Language["str_ui_huntingpass_tab_desc"];
				break;
		}

		UIToggleBattlePass first = _contentBattlePass.GetChild(0).GetComponent<UIToggleBattlePass>();
		first.Toggle(true);
	}

	public void OnClickPurchase()
	{
		if (_selectedInfo.PersistentItem.unlock)
		{
			return;
		}

		var product = PurchaseManager.Instance.GetProductID(_selectedInfo.ShopInfo.rawData.productIDs);

		PurchaseManager.Instance.BuyProduct(product.id);

		PurchaseManager.Instance.PurchaseCompleted = OnPurchaseCompleted;
	}

	public void OnPurchaseCompleted()
	{
		_selectedInfo.Purchase();

		ToastUI.Instance.Enqueue("패스 활성화");
		RefreshView();
	}

	public void RefreshView()
	{
		SetData(_selectedInfo);
	}

	public void SetData(RuntimeData.BattlePassInfo info)
	{
		_selectedInfo = info;

		_textPassName.text = PlatformManager.Language[_selectedInfo.rawData.name];
		_textPassContext.text = PlatformManager.Language[_selectedInfo.rawData.subText];

		int count = _selectedInfo.PersistentItem.unlock ? 0 : 1;
		_textBuyInfo.text = $"{count}/1";

		var product = PurchaseManager.Instance.GetProduct(info.ShopInfo.rawData.productIDs);
		_textPrice.text = product.metadata.localizedPriceString;

		bool isPassPurchased = _selectedInfo.PersistentItem.unlock;

		int lastIndex = _selectedInfo.TierInfoList.Count - 1;
		_contentReward.CreateListCell(lastIndex, _uiItemPassReward);

		for (int i = 0; i < _contentReward.childCount; i++)
		{
			var child = _contentReward.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < lastIndex)
			{
				child.gameObject.SetActive(true);
				child.GetComponent<UIBattlePassReward>().SetData(this, _selectedInfo.TierInfoList[i], isPassPurchased);
			}

		}
		_lastReward.SetData(this, _selectedInfo.TierInfoList[lastIndex], isPassPurchased);

	}
}
