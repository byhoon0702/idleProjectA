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
	[SerializeField] private Slider sliderExp;
	[SerializeField] private TextMeshProUGUI textSliderValue;
	[SerializeField] private TextMeshProUGUI textLevel;

	private UIPopupAdBuff _parent;
	private RuntimeData.AdBuffInfo _info;

	bool free;
	private void Awake()
	{

	}

	public void OnUpdate(UIPopupAdBuff parent, RuntimeData.AdBuffInfo info)
	{
		_parent = parent;
		_info = info;

		textTitle.text = $"{info.Ability.type.ToUIString()} +{info.Ability.Value.ToString()}%";

		textLevel.text = $"LV. {info.Level}";
		OnUpdateSlider();


		var item = PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid);
		free = item.unlock;
	}

	public void OnUpdateSlider()
	{
		IdleNumber needExp = _info.NeedExp();
		sliderExp.value = _info.Exp / needExp;
		textSliderValue.text = $"{_info.Exp.ToString()}/{needExp.ToString()}";
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
}
