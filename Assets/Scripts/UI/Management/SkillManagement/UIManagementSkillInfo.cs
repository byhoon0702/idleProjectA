using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class UIManagementSkillInfo : MonoBehaviour
{
	[SerializeField] private Button buttonEquip;
	public Button ButtonEquip => buttonEquip;
	[SerializeField] private GameObject objEquip;
	[SerializeField] private GameObject objUnequip;
	[SerializeField] private GameObject objUnableEquip;

	[SerializeField] private Button buttonLevelup;
	public Button ButtonLevelup => buttonLevelup;
	[SerializeField] private Button buttonEvolution;
	public Button ButtonEvolution => buttonEvolution;

	[SerializeField] private UISkillSlot skillSlot;
	[SerializeField] private TextMeshProUGUI textSkillName;
	[SerializeField] private TextMeshProUGUI textSkillCooldown;
	[SerializeField] private TextMeshProUGUI textSkillDescription;

	UIManagementSkill parent;
	RuntimeData.SkillInfo info;
	void Awake()
	{
		buttonEquip.onClick.RemoveAllListeners();
		buttonEquip.onClick.AddListener(OnClickEquip);
		buttonLevelup.onClick.RemoveAllListeners();
		buttonLevelup.onClick.AddListener(OnClickLevelUP);
		buttonEvolution.onClick.RemoveAllListeners();
		buttonEvolution.onClick.AddListener(OnClickEvolution);
	}

	public void OnUpdate(UIManagementSkill _parent, RuntimeData.SkillInfo _info)
	{
		parent = _parent;
		info = _info;

		UpdateInfo();
	}

	private void UpdateInfo()
	{
		bool isAvailable = info.Level > 0;

		objEquip.SetActive(isAvailable && info.isEquipped == false);
		objUnequip.SetActive(isAvailable && info.isEquipped);
		objUnableEquip.SetActive(isAvailable == false);
		buttonLevelup.gameObject.SetActive(isAvailable);
		buttonEvolution.gameObject.SetActive(isAvailable);

		textSkillName.text = info.ItemName;
		textSkillCooldown.text = $"{info.CooldownValue}s";

		textSkillDescription.text = info.Description;

		skillSlot.OnUpdate(null, info, null);
		skillSlot.ShowSlider(true);
	}

	public void OnClickEquip()
	{
		if (info.Level == 0 || info.unlock == false)
		{
			return;
		}
		if (info.isEquipped)
		{
			parent.UnEquipSkill();
		}
		else
		{
			parent.EquipSkill();

		}
	}

	public void OnClickLevelUP()
	{
		if (info.IsMax())
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_max_level"]);
			return;
		}
		parent.OnClickLevelUp();
	}

	public void OnClickEvolution()
	{
		if (info.EvolutionLevel == info.EvolutionMaxLevel)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_max_evolution"]);
			return;
		}
		parent.OnClickEvolution();
	}
}
