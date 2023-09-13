using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIPopupGachaInfo : UIBase
{
	[SerializeField] TextMeshProUGUI textSelectedLevel;
	[SerializeField] private UIListItemGachaInfo[] listItems;

	[SerializeField] private GameObject objButtonNext;
	[SerializeField] private GameObject objButtonPrev;

	[SerializeField] private GameObject objStars;
	[SerializeField] private UIListItemStarChance[] listStars;
	[SerializeField] private TextMeshProUGUI textDescription;

	RuntimeData.GachaInfo _info;
	private int _level = 0;
	public void SetData(RuntimeData.GachaInfo info)
	{
		_info = info;
		_level = info.Level;
		Refresh();

		objStars.SetActive(_info.rawData.gachaType == GachaType.Equip);
		if (_info.rawData.gachaType == GachaType.Equip)
		{
			textDescription.text = PlatformManager.Language["str_ui_gacha_equip_info"];
			for (int i = 0; i < listStars.Length; i++)
			{
				listStars[i].SetData(i + 1, PlatformManager.UserDB.gachaContainer.gachaStarChance[i]);
			}
		}
	}

	private void OnUpdateButton()
	{
		objButtonNext.SetActive(_level + 1 < _info.MaxLevel);
		objButtonPrev.SetActive(_level >= 0);
	}
	public void Refresh()
	{
		OnUpdateButton();
		textSelectedLevel.text = $"Level {_level}";
		if (_level == _info.MaxLevel && _info.MaxLevel == 1)
		{
			textSelectedLevel.text = $"Max Level";
		}

		for (int i = 0; i < listItems.Length; i++)
		{
			listItems[i].gameObject.SetActive(false);
			if (i < _info.rawData.chances.Count)
			{
				listItems[i].SetData(_info.rawData.chances[i], _level);
				listItems[i].gameObject.SetActive(true);
			}
		}
	}

	public void OnClickNextLevel()
	{
		if (_level + 1 == _info.MaxLevel)
		{
			return;
		}
		_level++;
		Refresh();
	}

	public void OnClickPrevLevel()
	{
		if (_level - 1 < 0)
		{
			return;
		}
		_level--;
		Refresh();
	}
}
