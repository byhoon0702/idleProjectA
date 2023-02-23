using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemPetSlot : MonoBehaviour
{
	[SerializeField] private Image icon;

	[SerializeField] private TextMeshProUGUI levelText;

	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;

	[SerializeField] private Button plusButton;
	[SerializeField] private Button minusButton;

	[SerializeField] private Button infoButton;

	[SerializeField] private GameObject equipped;
	[SerializeField] private GameObject noPossessed;

	private UIPet owner;
	private UIPetData uiData;




	private void Awake()
	{
		plusButton.onClick.RemoveAllListeners();
		plusButton.onClick.AddListener(OnClickPlus);
		minusButton.onClick.RemoveAllListeners();
		minusButton.onClick.AddListener(OnClickMinus);
		infoButton.onClick.RemoveAllListeners();
		infoButton.onClick.AddListener(OnClickSkillInfo);
	}


	public void OnUpdate(UIPet _owner, UIPetData _uiData)
	{
		owner = _owner;
		uiData = _uiData;

		icon.sprite = Resources.Load<Sprite>($"Icon/{uiData.Icon}");

		OnRefresh();
	}

	public void OnRefresh()
	{
		var item = uiData.GetItem();

		if (item != null)
		{
			bool hasItem = item.Count > 0;
			bool isEquipSkill = UserInfo.IsEquipPet(item.Tid);

			expSlider.gameObject.SetActive(true);
			noPossessed.SetActive(hasItem == false);
			equipped.SetActive(isEquipSkill);
			plusButton.gameObject.SetActive(isEquipSkill == false);
			minusButton.gameObject.SetActive(isEquipSkill);

		}
		else
		{
			expSlider.gameObject.SetActive(false);
			noPossessed.SetActive(true);
			equipped.SetActive(false);
			plusButton.gameObject.SetActive(false);
			minusButton.gameObject.SetActive(false);

		}

		expSlider.value = uiData.ExpRatio;
		textExp.text = uiData.ExpText;
		levelText.text = uiData.LevelText;
	}

	private void OnClickPlus()
	{
		owner.EquipPet(uiData.ItemTid);
	}

	private void OnClickMinus()
	{
		owner.UnEquipPet(uiData.ItemTid);
	}

	private void OnClickSkillInfo()
	{
		owner.ShowItemInfo(uiData);
	}
}
