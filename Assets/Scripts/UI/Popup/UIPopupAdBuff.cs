using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupAdBuff : UIBase
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private Button buttonBuyAdFree;
	[SerializeField] private TextMeshProUGUI textFreeAds;

	[SerializeField] private UIItemAdBuff[] uiItemAdBuff;
	public UIItemAdBuff[] UiItemAdBuff => uiItemAdBuff;


	List<RuntimeData.AdBuffInfo> buffList = new List<RuntimeData.AdBuffInfo>();

	public void Open()
	{
		Activate();
		gameObject.SetActive(true);
		buffList = PlatformManager.UserDB.buffContainer.adBuffList;
		OnUpdate();
	}


	public void OnClickShop()
	{
		Close();
		var uiController = GameUIManager.it.uiController;
		uiController.BottomMenu.ShopToggle.isOn = true;
		//uiController.ToggleShop(() => { uiController.BottomMenu.ShopToggle.isOn = false; });
	}
	public void OnUpdate()
	{

		var item = PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid);

		buttonBuyAdFree.gameObject.SetActive(!item.unlock);
		textFreeAds.gameObject.SetActive(item.unlock);

		for (int i = 0; i < uiItemAdBuff.Length; i++)
		{
			uiItemAdBuff[i].OnUpdate(this, buffList[i]);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		if (GameManager.it != null)
		{
			PlatformManager.UserDB.buffContainer.GainAdExp += OnUpdate;
		}

	}
	protected override void OnDisable()
	{
		base.OnDisable();
		if (GameManager.it != null)
		{
			PlatformManager.UserDB.buffContainer.GainAdExp -= OnUpdate;
		}
	}

}
