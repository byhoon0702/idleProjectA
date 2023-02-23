using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemEquipChange : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;
	[SerializeField] private Button button;

	[Header("State")]
	[SerializeField] private GameObject selected;
	[SerializeField] private GameObject equipped;
	[SerializeField] private GameObject notPossessed;


	private UIEquipChange owner;
	public UIEquipData UIData { get; private set; }


	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	public void OnUpdate(UIEquipChange _owner, UIEquipData _uiData)
	{
		owner = _owner;
		UIData = _uiData;

		icon.sprite = Resources.Load<Sprite>($"Icon/{UIData.Icon}");

		OnRefresh();
	}

	public void OnRefresh()
	{
		levelText.text = UIData.ItemLevelText;
		expSlider.value = UIData.ExpRatio;
		textExp.text = UIData.ExpText;

		var item = UIData.GetItem();
		if (item == null || item.Count == 0)
		{
			notPossessed.SetActive(true);
		}
		else
		{
			notPossessed.SetActive(false);
		}
	}

	public void SetSelect(bool _selected)
	{
		selected.SetActive(_selected);
	}

	public void SetEquipped(bool _equiped)
	{
		equipped.SetActive(_equiped);
	}

	private void OnClick()
	{
		owner.ChangeCurrentItem(UIData.ItemTid);
	}
}
