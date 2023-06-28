using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class UIManagementSkillInfo : MonoBehaviour
{
	[SerializeField] private Button buttonEquip;
	[SerializeField] private GameObject objEquip;
	[SerializeField] private GameObject objUnequip;
	[SerializeField] private GameObject objUnableEquip;

	[SerializeField] private Button buttonLevelup;
	[SerializeField] private Button buttonSkillLearn;

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
		buttonSkillLearn.onClick.RemoveAllListeners();
		buttonSkillLearn.onClick.AddListener(OnClickLearn);

	}
	public void OnUpdate(UIManagementSkill _parent, RuntimeData.SkillInfo _info)
	{
		parent = _parent;
		info = _info;

		UpdateInfo();
	}

	private void UpdateInfo()
	{
		bool isAvailable = info.level > 0;

		buttonSkillLearn.gameObject.SetActive(false);
		objEquip.SetActive(isAvailable && info.isEquipped == false);
		objUnequip.SetActive(isAvailable && info.isEquipped);
		objUnableEquip.SetActive(isAvailable == false);
		//buttonLevelup.gameObject.SetActive(isAvailable);
		buttonLevelup.gameObject.SetActive(false);

		textSkillName.text = info.Name;
		textSkillCooldown.text = $"{info.CooldownValue}s";
		textSkillDescription.text = info.Description;

		skillSlot.OnUpdate(null, info, null);
	}

	public void OnClickEquip()
	{
		if (info.level == 0 || info.unlock == false)
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
		//info.LevelUp();
		//UpdateInfo();
		parent.OnClickShowTree();
	}

	public void OnClickLearn()
	{
		if (info.IsMax())
		{
			return;
		}
		info.LevelUp();
		UpdateInfo();
		parent.OnUpdate(info.Tid);
	}
}
