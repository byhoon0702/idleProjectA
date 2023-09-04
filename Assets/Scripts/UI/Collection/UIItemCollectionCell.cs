using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public struct CollectionCellData
{
	public int level;
	public Grade grade;
	public Sprite icon;
}

public class UIItemCollectionCell : MonoBehaviour
{
	[SerializeField] GameObject objLock;
	[SerializeField] private Image imageGrade;
	[SerializeField] private TextMeshProUGUI textLevel;
	[SerializeField] private Image icon;
	[SerializeField] private Image bg;

	public void OnUpdate(CollectionCellData data)
	{

		objLock.SetActive(data.level == 0);
		imageGrade.sprite = GameUIManager.it.spriteGradeFrameList[(int)data.grade];
		bg.sprite = GameUIManager.it.spriteGradeList[(int)data.grade];
		textLevel.text = $"LV {data.level}";
		icon.sprite = data.icon;
	}
}
