using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementYouth : MonoBehaviour
{
	[SerializeField] private UIManagementMastery masteryUI;
	[SerializeField] private UIManagementCore coreUI;
	[SerializeField] private UIManagementCoreAbility coreAbilUI;

	[SerializeField] private Button masteryButton;
	[SerializeField] private Button coreButton;
	[SerializeField] private Button coreAbilityButton;


	private void Awake()
	{
		masteryButton.onClick.RemoveAllListeners();
		masteryButton.onClick.AddListener(OnMasteryButtonClick);

		coreButton.onClick.RemoveAllListeners();
		coreButton.onClick.AddListener(OnCoreButtonClick);

		coreAbilityButton.onClick.RemoveAllListeners();
		coreAbilityButton.onClick.AddListener(OnCoreAbilityButtonClick);
	}

	public void OnUpdate()
	{
		if (masteryUI.gameObject.activeInHierarchy == false &&
			coreUI.gameObject.activeInHierarchy == false &&
			coreAbilUI.gameObject.activeInHierarchy == false)
		{
			OnMasteryButtonClick();
		}
		else
		{
			if (masteryUI.gameObject.activeInHierarchy)
			{
				masteryUI.OnUpdate(false);
			}
			else if (coreUI.gameObject.activeInHierarchy)
			{
				coreUI.OnUpdate(false);
			}
			else if (coreAbilUI.gameObject.activeInHierarchy)
			{
				coreAbilUI.OnUpdate();
			}
		}
	}

	private void OnMasteryButtonClick()
	{
		InactiveAll();

		masteryUI.gameObject.SetActive(true);
		masteryUI.OnUpdate(false);
	}

	private void OnCoreButtonClick()
	{
		InactiveAll();

		coreUI.gameObject.SetActive(true);
		coreUI.OnUpdate(false);
	}

	private void OnCoreAbilityButtonClick()
	{
		InactiveAll();

		coreAbilUI.gameObject.SetActive(true);
		coreAbilUI.OnUpdate();
	}

	private void InactiveAll()
	{
		masteryUI.gameObject.SetActive(false);
		coreUI.gameObject.SetActive(false);
		coreAbilUI.gameObject.SetActive(false);
	}
}
