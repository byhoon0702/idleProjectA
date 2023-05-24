using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemYouthOption : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textGrade;
	[SerializeField] private TextMeshProUGUI textDescription;
	[SerializeField] private TextMeshProUGUI textValue;

	[SerializeField] private GameObject objLock;
	[SerializeField] private GameObject objNotOpen;
	[SerializeField] private TextMeshProUGUI textNotOpen;

	[SerializeField] private Button buttonLock;

	private YouthOptionSlot slot;
	private UIPopupYouthOption parent;
	private void Awake()
	{
		buttonLock.onClick.RemoveAllListeners();
		buttonLock.onClick.AddListener(OnClickLock);
	}
	public void OnUpdate(UIPopupYouthOption _parent, YouthOptionSlot _slot)
	{
		parent = _parent;
		slot = _slot;

		bool notOpen = slot.grade > GameManager.UserDB.youthContainer.currentGrade;

		objNotOpen.SetActive(notOpen);
		if (notOpen)
		{
			textNotOpen.text = $"회춘 등급 {slot.grade} 달성시 해제";
			textGrade.text = "";
			textValue.text = "";
			textDescription.text = "";
		}
		else
		{
			if (slot.info == null || slot.info.type == StatsType.None)
			{
				textGrade.text = "";
				textValue.text = "";
				textDescription.text = "옵션을 부여해주세요.";
			}
			else
			{
				textGrade.text = slot.info.grade.ToString();
				textValue.text = $"+{slot.info.currentValue.ToString()}%";
				textDescription.text = slot.info.type.ToUIString();
			}
		}
		objLock.SetActive(slot.isLock && !notOpen);
	}

	public void OnClickLock()
	{
		slot.isLock = !slot.isLock;
		objLock.SetActive(slot.isLock);
	}
}
