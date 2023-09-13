using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemAdBuff : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private Image icon;
	[SerializeField] private Button buttonLevelUp;
	public Button ButtonLevelUp => buttonLevelUp;

	[SerializeField] private Button buttonWatchAd;
	public Button ButtonWatchAd => buttonWatchAd;

	[SerializeField] private Button buttonLock;
	[SerializeField] private TextMeshProUGUI textButtonLock;
	[SerializeField] private Slider sliderExp;
	[SerializeField] private TextMeshProUGUI textSliderValue;
	[SerializeField] private TextMeshProUGUI textLevel;

	private UIPopupAdBuff _parent;
	private RuntimeData.AdBuffInfo _info;

	bool free;

	public void OnUpdate(UIPopupAdBuff parent, RuntimeData.AdBuffInfo info)
	{
		_parent = parent;
		_info = info;

		textTitle.text = $"{info.Ability.type.ToUIString()} +{info.Ability.Value.ToString()}%";

		if (info.IsActive)
		{
			textTitle.color = Color.yellow;
		}
		else
		{
			textTitle.color = Color.gray;
		}
		textLevel.text = $"LV. {info.Level}";
		OnUpdateSlider();

		buttonWatchAd.gameObject.SetActive(!_info.IsActive);
		buttonLock.gameObject.SetActive(_info.IsActive);


		var item = PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid);
		free = item.unlock;

		textButtonLock.text = PlatformManager.Language["str_ui_unlimit"];
	}

	public void OnUpdateSlider()
	{
		IdleNumber needExp = _info.NeedExp();
		sliderExp.value = _info.Exp / needExp;
		textSliderValue.text = $"{_info.Exp.GetValueToLong()}/{needExp.GetValueToLong()}";
	}

	public void WatchAd()
	{
		if (free)
		{
			return;
		}

		_info.WatchAd();
	}

	public void OnClickLevelUP()
	{
		if (_info.Exp < _info.NeedExp())
		{
			return;
		}
		_info.LevelUp();
		_parent.OnUpdate();
	}

	void Update()
	{
		if (buttonLock.gameObject.activeInHierarchy && !free)
		{
			System.TimeSpan ts = _info.EndTime - TimeManager.Instance.UtcNow;

			textButtonLock.text = $"{ts.Minutes}:{ts.Seconds}";
		}
	}
	public void OnClickLock()
	{
		ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_ad_buff_cooldown"]);
	}
}
