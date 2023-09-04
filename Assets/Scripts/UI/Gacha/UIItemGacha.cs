using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;
using System.Linq;

public class UIItemGacha : MonoBehaviour
{

	[SerializeField] ContentType contentType;
	[SerializeField] private Material grayScale;
	[Header("Main")]
	[SerializeField] private Image imageMain;
	[SerializeField] private UITextMeshPro uiTextTitle;

	[Header("Reward")]
	[SerializeField] private GameObject objReward;
	[SerializeField] private TextMeshProUGUI textSlider;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI textLevel;
	[SerializeField] private Button buttonGachaReward;
	[SerializeField] private GameObject objGachaRewardOn;

	[Header("Gacha Button")]
	[SerializeField] private Button buttonTenGacha;
	public Button ButtonTenGacha => buttonTenGacha;
	[SerializeField] private Image imageTenCost;
	[SerializeField] private UITextMeshPro textTenLabel;
	[SerializeField] private TextMeshProUGUI textTenCost;

	[SerializeField] private Button buttonThirtyGacha;
	public Button ButtonThirtyGacha => buttonThirtyGacha;
	[SerializeField] private Image imageThirtyCost;
	[SerializeField] private UITextMeshPro textThirtyLabel;
	[SerializeField] private TextMeshProUGUI textThirtyCost;

	[SerializeField] private Button buttonADGacha;
	public Button ButtonADGacha => buttonADGacha;
	[SerializeField] private Image imageAdsCost;
	[SerializeField] private UITextMeshPro textAdsLabel;
	[SerializeField] private TextMeshProUGUI textAdsCount;

	[SerializeField] private Button buttonChanceInfo;


	private RuntimeData.GachaInfo gachaInfo;
	public RuntimeData.GachaInfo GachaInfo => gachaInfo;
	private UIManagementGacha parent;


	RuntimeData.CurrencyInfo currency10;
	RuntimeData.CurrencyInfo currency30;
	private System.Action<RuntimeData.GachaInfo> _onClickInfo;

	private void Awake()
	{
		buttonTenGacha.onClick.AddListener(OnClick10);
		buttonThirtyGacha.onClick.AddListener(OnClick30);
		buttonADGacha.onClick.AddListener(OnClickAds);
		buttonChanceInfo.onClick.AddListener(OnClickGachaInfo);
		buttonGachaReward.onClick.AddListener(OnClickGachaReward);
	}

	private void OnDisable()
	{
		gachaInfo.OnLevelUp -= OnUpdateReward;
	}

	private void OnUpdateReward()
	{
		if (gachaInfo.currentLevelInfo != null)
		{
			bool isEmpty = gachaInfo.currentLevelInfo.reward.tid == 0;
			objReward.SetActive(!isEmpty);

			textSlider.text = $"{gachaInfo.Exp}/{gachaInfo.currentLevelInfo.exp}";
			slider.value = gachaInfo.Exp / (float)gachaInfo.currentLevelInfo.exp;
			textLevel.text = $"Lv. {gachaInfo.Level}";

			objGachaRewardOn.SetActive(isEmpty == false && gachaInfo.CanGetReward());
		}
		else
		{
			objGachaRewardOn.SetActive(false);
		}
	}

	bool _isOpen;
	string _contentMessage;
	public void OnUpdate(UIManagementGacha _parent, RuntimeData.GachaInfo _gachaInfo, System.Action<RuntimeData.GachaInfo> onClickInfo)
	{
		parent = _parent;
		gachaInfo = _gachaInfo;

		var content = PlatformManager.UserDB.contentsContainer.Get(contentType);
		_isOpen = content != null ? content.IsOpen : false;
		_contentMessage = content != null ? content.Description : "";


		_onClickInfo = onClickInfo;
		gachaInfo.OnLevelUp += OnUpdateReward;
		uiTextTitle.SetKey(gachaInfo.rawData.name);
		imageMain.sprite = gachaInfo.IconImage;
		if (_isOpen)
		{
			imageMain.material = null;
		}
		else
		{
			imageMain.material = grayScale;
		}

		OnUpdateReward();

		currency10 = PlatformManager.UserDB.inventory.FindCurrency(gachaInfo.gacha10.itemTid);
		currency30 = PlatformManager.UserDB.inventory.FindCurrency(gachaInfo.gacha30.itemTid);
		imageTenCost.sprite = currency10.itemObject.ItemIcon;
		imageThirtyCost.sprite = currency30.itemObject.ItemIcon;
		//imageAdsCost.sprite = null;

		textTenLabel.SetKey("str_ui_10_gacha");
		textThirtyLabel.SetKey("str_ui_30_gacha");
		textAdsLabel.SetKey("str_ui_ads");

		textTenCost.text = $"{gachaInfo.gacha10.cost}";
		textThirtyCost.text = $"{gachaInfo.gacha30.cost}";

		textTenCost.color = currency10.Check(gachaInfo.gacha10.cost) ? Color.white : Color.red;
		textThirtyCost.color = currency30.Check(gachaInfo.gacha30.cost) ? Color.white : Color.red;

		buttonADGacha.gameObject.SetActive(gachaInfo.gachaAds != null);
		if (gachaInfo.gachaAds != null)
		{
			int count = Mathf.Max(gachaInfo.gachaAds.summonMaxCount - gachaInfo.ViewAdsCount, 0);
			textAdsCount.text = $"{count}/{gachaInfo.gachaAds.summonMaxCount}";
			if (count == 0)
			{
				textAdsCount.color = Color.red;
			}
			else
			{
				textAdsCount.color = Color.white;
			}
		}

	}

	public void OnClick10()
	{
		if (_isOpen == false)
		{
			ToastUI.Instance.Enqueue(_contentMessage);
			return;
		}

		if (currency10.Pay(gachaInfo.gacha10.cost) == false)
		{
			ToastUI.Instance.Enqueue("다이아가 부족합니다.");
			return;
		}
		gachaInfo.OnClickGacha(GachaButtonType.Gacha10);
		parent.OnUpdate();
	}

	public void OnClick30()
	{
		if (_isOpen == false)
		{
			ToastUI.Instance.Enqueue(_contentMessage);
			return;
		}
		if (currency30.Pay(gachaInfo.gacha30.cost) == false)
		{
			ToastUI.Instance.Enqueue("다이아가 부족합니다.");
			return;
		}
		gachaInfo.OnClickGacha(GachaButtonType.Gacha30);

		parent.OnUpdate();
	}

	public void OnClickAds()
	{
		if (_isOpen == false)
		{
			ToastUI.Instance.Enqueue(_contentMessage);
			return;
		}
		int count = gachaInfo.gachaAds.summonMaxCount - gachaInfo.ViewAdsCount;
		if (count <= 0)
		{
			ToastUI.Instance.Enqueue("광고 횟수가 없어요.");
			return;
		}

		MobileAdsManager.Instance.ShowAds(() =>
		{
			gachaInfo.OnClickGachaAds();
			gachaInfo.OnClickGacha(GachaButtonType.Ads);
			parent.OnUpdate();
		});

	}

	public void OnClickGachaInfo()
	{
		_onClickInfo?.Invoke(gachaInfo);
	}

	public void OnClickGachaReward()
	{
		if (_isOpen == false)
		{
			ToastUI.Instance.Enqueue(_contentMessage);
			return;
		}
		gachaInfo.OnReceiveReward();
	}
}
