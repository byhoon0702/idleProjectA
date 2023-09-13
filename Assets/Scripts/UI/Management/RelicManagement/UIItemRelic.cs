using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemRelic : MonoBehaviour
{
	[SerializeField] private UITextMeshPro uiTextTitle;
	[SerializeField] private TextMeshProUGUI uiTextDescription;
	[SerializeField] private TextMeshProUGUI textLevel;
	[SerializeField] private TextMeshProUGUI textCost;
	[SerializeField] private Image imageIcon;
	[SerializeField] private Image imageGrade;
	[SerializeField] private Image imageBg;
	[SerializeField] private Image imageCost;
	[SerializeField] private Button buttonLevelUp;
	[SerializeField] private TextMeshProUGUI textChance;

	private RuntimeData.RelicInfo info;
	public RuntimeData.RelicInfo RelicInfo => info;

	private void Awake()
	{
		buttonLevelUp.onClick.RemoveAllListeners();
		buttonLevelUp.onClick.AddListener(OnClickLevelUp);
	}
	public void SetData(RuntimeData.RelicInfo _info)
	{
		info = _info;
		OnUpdate();
	}
	public void OnUpdate()
	{
		uiTextTitle.SetKey(info.rawData.name);
		uiTextDescription.text = info.Description();

		textLevel.text = $"{info.Level}/{info.rawData.maxLevel}";
		textCost.text = info.Count.ToString();

		if (info.Count == 0)
		{
			textChance.color = Color.red;
		}
		else
		{
			textChance.color = Color.white;
		}
		textChance.text = $"{info.chance}%";


		imageBg.sprite = GameUIManager.it.spriteGradeList[(int)info.rawData.itemGrade];
		imageGrade.sprite = GameUIManager.it.spriteGradeFrameList[(int)info.rawData.itemGrade];

		imageIcon.sprite = info.IconImage;
		imageCost.sprite = info.IconImage;
	}

	public void OnClickLevelUp()
	{
		if (info.Count == 0)
		{
			return;
		}
		PlatformManager.UserDB.relicContainer.OnClickLevelUp(info.Tid);
		OnUpdate();
	}

}
