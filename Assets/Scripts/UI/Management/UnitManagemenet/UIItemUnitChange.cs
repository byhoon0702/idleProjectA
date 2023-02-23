using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIItemUnitChange : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI gradeText;
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private Button button;

	[Header("State")]
	[SerializeField] private GameObject selected;
	[SerializeField] private GameObject equipped;
	[SerializeField] private GameObject notPossessed;

	private UIUnitChange owner;


	public UIUnitData UIData { get; private set; }




	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	public void OnUpdate(UIUnitChange _owner, UIUnitData _uiData)
	{
		owner = _owner;
		UIData = _uiData;

		icon.sprite = Resources.Load<Sprite>($"Icon/{UIData.Icon}");

		nameText.text = UIData.UnitName;
		gradeText.text = UIData.UnitGradeText;

		OnRefresh();
	}

	public void OnRefresh()
	{
		levelText.text = UIData.UnitLevelText;

		var item = UIData.GetItem();
		if (item != null && item.Count > 0)
		{
			notPossessed.gameObject.SetActive(false);
		}
		else
		{
			notPossessed.gameObject.SetActive(true);
		}
	}

	public void SetSelect(bool _selected)
	{
		selected.SetActive(_selected);
	}

	public void SetEquipped(bool _equipped)
	{
		equipped.gameObject.SetActive(_equipped);
	}

	private void OnClick()
	{
		owner.ChangeCurrentUnit(UIData.ItemTid);
	}
}
