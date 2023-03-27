using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemMasterySlot : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI pointText;
	[SerializeField] private GameObject noPossessed;
	[SerializeField] private Button button;


	private UIManagementMastery owner;
	private UIMasteryData uiData;



	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	public void OnUpdate(UIManagementMastery _owner, UIMasteryData _uiData)
	{
		owner = _owner;
		uiData = _uiData;

		icon.sprite = Resources.Load<Sprite>($"Icon/{uiData.Icon}");

		UpdatePoint();
	}

	public void UpdatePoint()
	{
		pointText.text = uiData.PointText;

		bool isUnlock = uiData.IsUnlock;
		noPossessed.SetActive(isUnlock == false);
	}

	public void OnRefresh()
	{
		UpdatePoint();
	}

	private void OnClick()
	{
		owner.ShowMasteryInfo(uiData);
	}
}
