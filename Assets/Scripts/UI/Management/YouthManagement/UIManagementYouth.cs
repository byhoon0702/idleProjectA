using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementYouth : MonoBehaviour
{
	[SerializeField] private UIPopupYouthOption uiPopupYouthOption;
	//[SerializeField] private UIPopupYouth uiPopupYouth;

	[SerializeField] private Transform contentRoot;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private TextMeshProUGUI textCurrentGrade;

	[SerializeField] private Button buttonOption;
	[SerializeField] private Button buttonYouth;


	private void Awake()
	{
		buttonOption.onClick.RemoveAllListeners();
		buttonOption.onClick.AddListener(OnClickShowPopupYouthOption);
		buttonYouth.onClick.RemoveAllListeners();
		buttonYouth.onClick.AddListener(OnClickShowPopupYouth);

	}

	private void OnDisable()
	{
		uiPopupYouthOption.Close();
	}

	public void OnUpdate()
	{
		textCurrentGrade.text = GameManager.UserDB.youthContainer.currentGrade.ToString();
		UpdateItem();
	}

	public void UpdateItem()
	{
		var list = GameManager.UserDB.youthContainer.info;
		int countForMake = list.Count - contentRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, contentRoot);
			}
		}

		//list.Sort((a, b) => { return b.isOpen.CompareTo(a.isOpen); });

		for (int i = 0; i < contentRoot.childCount; i++)
		{

			var child = contentRoot.GetChild(i);
			if (i > list.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UIItemYouth slot = child.GetComponent<UIItemYouth>();
			var info = list[i];
			slot.OnUpdate(this, info);

			//slot.OnUpdate(this, info);
		}
	}

	public void OnClickShowPopupYouthOption()
	{
		uiPopupYouthOption.gameObject.SetActive(true);
		uiPopupYouthOption.OnUpdate(this);
	}
	public void OnClickShowPopupYouth()
	{
		if (GameManager.UserDB.youthContainer.UpdateGrade() == false)
		{
			return;
		}
		OnUpdate();
	}
}
