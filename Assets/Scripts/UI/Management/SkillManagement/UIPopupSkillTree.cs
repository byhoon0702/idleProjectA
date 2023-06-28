using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPopupSkillTree : MonoBehaviour, ISelectListener
{
	[SerializeField] private UIManagementSkill parent;
	[SerializeField] private UISkillSlot itemPrefab;
	[SerializeField] private Button buttonClose;
	[SerializeField] private Button buttonLevelup;
	[SerializeField] private UISkillSlot uiSelectedSkillSlot;

	[SerializeField] private UISkillSlot[] uiSkillTreeSlots;
	[SerializeField] private GameObject objTreeRoot;
	[SerializeField] private Transform objTreeScrollContent;
	[SerializeField] private TextMeshProUGUI textSkillName;
	[SerializeField] private TextMeshProUGUI textSkillDescription;
	[SerializeField] private GameObject objSkillCooldown;
	[SerializeField] private TextMeshProUGUI textSkillCooldown;

	[SerializeField] private GameObject objLock;
	[SerializeField] private TextMeshProUGUI textLock;

	private RuntimeData.SkillInfo info;
	private SkillTreeData skillTreeData;

	private void Awake()
	{
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClickClose);
		buttonLevelup.onClick.RemoveAllListeners();
		buttonLevelup.onClick.AddListener(OnClickLevelUP);
	}
	public void OnUpdate(RuntimeData.SkillInfo _info)
	{
		gameObject.SetActive(true);
		info = _info;
		//skillTreeData = DataManager.Get<SkillTreeDataSheet>().GetSkillTreeData(info.rawData.detailData.rootSkillTid);

		//if (skillTreeData == null)
		//{
		//	skillTreeData = DataManager.Get<SkillTreeDataSheet>().GetSkillTreeData(info.Tid);
		//}


		OnUpdateSelectedSkill();
		OnUpdateTree();
	}
	private void OnUpdateSelectedSkill()
	{
		uiSelectedSkillSlot.OnUpdate(null, info, null);
		uiSelectedSkillSlot.OnUpdateLock();

		bool isAvailable = uiSelectedSkillSlot.IsAvailable(out string description);

		objLock.SetActive(isAvailable == false);
		textLock.text = description;

		textSkillName.text = $"{info.Name}";
		textSkillDescription.text = $"{info.Description}";
		objSkillCooldown.SetActive(info.CooldownType == SkillCooldownType.TIME);
		textSkillCooldown.text = $"{info.CooldownValue}";
	}

	private void OnUpdateTree()
	{
		if (skillTreeData == null)
		{
			for (int i = 0; i < objTreeScrollContent.childCount; i++)
			{

				var child = objTreeScrollContent.GetChild(i);
				child.gameObject.SetActive(false);
			}

			return;
		}
		List<RuntimeData.SkillInfo> itemList = new List<RuntimeData.SkillInfo>();
		for (int i = 0; i < skillTreeData.skillTreeList.Count; i++)
		{
			var _info = GameManager.UserDB.skillContainer.Get(skillTreeData.skillTreeList[i].skillTid);
			if (_info == null)
			{
				continue;
			}
			itemList.Add(_info);
		}

		var list = itemList;
		int countForMake = list.Count - objTreeScrollContent.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, objTreeScrollContent);
			}
		}


		for (int i = 0; i < objTreeScrollContent.childCount; i++)
		{

			var child = objTreeScrollContent.GetChild(i);
			if (i > list.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UISkillSlot slot = child.GetComponent<UISkillSlot>();

			var info = list[i];
			slot.OnUpdate(this, info, () =>
			{
				this.OnUpdate(info);
			});
			slot.OnUpdateLock();
		}
	}

	public void OnClickLevelUP()
	{
		if (info.IsMax())
		{
			return;
		}
		info.LevelUp();
		OnUpdate(info);
		parent.UpdateInfo();
	}

	public void OnClickClose()
	{
		gameObject.SetActive(false);
	}

	private long selectedTid;
	public void SetSelectedTid(long tid)
	{
		selectedTid = tid;
	}

	public void AddSelectListener(OnSelect callback)
	{

	}

	public void RemoveSelectListener(OnSelect callback)
	{

	}
}
