using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIGacha : MonoBehaviour, IUIClosable
{
	[SerializeField] private UIGachaProbabilityPopup probabilityPopup;

	[SerializeField] private Button closeButton;
	[SerializeField] private Button equipButton;
	[SerializeField] private Button skillButton;
	[SerializeField] private Button petButton;

	[SerializeField] private UIItemGacha equipGacha;
	[SerializeField] private UIItemGacha skillGacha;
	[SerializeField] private UIItemGacha petGacha;



	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		equipButton.onClick.RemoveAllListeners();
		equipButton.onClick.AddListener(OnEquipButtonClick);

		skillButton.onClick.RemoveAllListeners();
		skillButton.onClick.AddListener(OnSkillButtonClick);

		petButton.onClick.RemoveAllListeners();
		petButton.onClick.AddListener(OnPetButtonClick);
	}

	public void OnUpdate()
	{
		if(equipGacha.gameObject.activeSelf == false && skillGacha.gameObject.activeSelf == false && petGacha.gameObject.activeSelf == false)
		{
			OnEquipButtonClick();
		}
		else if(equipGacha.gameObject.activeSelf)
		{
			OnEquipButtonClick();
		}
		else if(skillGacha.gameObject.activeSelf)
		{
			OnSkillButtonClick();
		}
		else if(petGacha.gameObject.activeSelf)
		{
			OnPetButtonClick();
		}
		

	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	private void OnEquipButtonClick()
	{
		InactiveAlllUI();
		var sheet = DataManager.Get<GachaDataSheet>();
		var gachaData = sheet.GetByType(GachaType.Equip);

		UIGachaData uiData = new UIGachaData();
		VResult result = uiData.Setup(gachaData);
		if(result.Fail())
		{
			PopAlert.it.Create(result, () => { Close(); });
			return;
		}

		equipGacha.gameObject.SetActive(true);
		equipGacha.OnUpdate(this, uiData);
	}

	private void OnSkillButtonClick()
	{
		InactiveAlllUI();
		var sheet = DataManager.Get<GachaDataSheet>();
		var gachaData = sheet.GetByType(GachaType.Skill);

		UIGachaData uiData = new UIGachaData();
		VResult result = uiData.Setup(gachaData);
		if (result.Fail())
		{
			PopAlert.it.Create(result, () => { Close(); });
			return;
		}

		skillGacha.gameObject.SetActive(true);
		skillGacha.OnUpdate(this, uiData);
	}

	private void OnPetButtonClick()
	{
		InactiveAlllUI();
		var sheet = DataManager.Get<GachaDataSheet>();
		var gachaData = sheet.GetByType(GachaType.Pet);

		UIGachaData uiData = new UIGachaData();
		VResult result = uiData.Setup(gachaData);
		if (result.Fail())
		{
			PopAlert.it.Create(result, () => { Close(); });
			return;
		}

		petGacha.gameObject.SetActive(true);
		petGacha.OnUpdate(this, uiData);

	}

	private void InactiveAlllUI()
	{
		equipGacha.gameObject.SetActive(false);
		skillGacha.gameObject.SetActive(false);
		petGacha.gameObject.SetActive(false);
	}

	public void ShowProbabilityPopup(int _level, UIGachaData _uiData)
	{
		probabilityPopup.gameObject.SetActive(true);
		probabilityPopup.OnUpdate(_level, _uiData);
	}
}
