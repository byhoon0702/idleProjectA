using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

/* Juvenescence */
public class UIPageJuvenescence : MonoBehaviour
{
	[Header("Info")]
	[SerializeField] private TextMeshProUGUI textJuvenescenceLevel;
	[SerializeField] private Toggle[] toggleJuvenescencePoint;
	[SerializeField] private Button buttonPrevJuvenescence;
	[SerializeField] private Button buttonNextJuvenescence;
	[SerializeField] private Button buttonReset;

	[SerializeField] private UIStatsInfoCell[] statsInfoCells;

	[SerializeField] private TextMeshProUGUI textTitleStats;
	[SerializeField] private TextMeshProUGUI textPoint;


	[Header("Element")]
	[SerializeField] private UIItemJuvenescenceElement[] uiItemJuvenescence;


	private UIManagementJuvenescence parent;

	List<RuntimeData.JuvenescenceInfo> infos;

	RuntimeData.JuvenescenceInfo currentInfo;
	public void Init(UIManagementJuvenescence _parent)
	{
		parent = _parent;
	}

	private void Awake()
	{
		buttonPrevJuvenescence.onClick.RemoveAllListeners();
		buttonPrevJuvenescence.onClick.AddListener(PrevInfo);
		buttonNextJuvenescence.onClick.RemoveAllListeners();
		buttonNextJuvenescence.onClick.AddListener(NextInfo);
	}
	public void OnEnable()
	{
		OnUpdate();
	}

	public void Reset()
	{

	}
	public void NextInfo()
	{
		if (currentInfo.page == infos.Count)
		{
			return;
		}

		currentInfo = infos.Find(x => x.page == currentInfo.page + 1);
		UpdateInfo();
	}

	public void PrevInfo()
	{
		int index = currentInfo.page - 1;

		if (index - 1 < 0)
		{
			return;
		}
		currentInfo = infos.Find(x => x.page == currentInfo.page - 1);
		UpdateInfo();
	}

	private void UpdateInfo()
	{
		textJuvenescenceLevel.text = currentInfo.page.ToString();

		for (int i = 0; i < toggleJuvenescencePoint.Length; i++)
		{
			toggleJuvenescencePoint[i].isOn = i < currentInfo.point;
		}

		List<AbilityInfo> abilities = new List<AbilityInfo>();

		foreach (var element in currentInfo.rawData.elements)
		{
			foreach (var stat in element.stats)
			{
				abilities.Add(new AbilityInfo(stat));
			}
		}
		for (int i = 0; i < statsInfoCells.Length; i++)
		{
			statsInfoCells[i].gameObject.SetActive(false);
		}

		int index = 0;
		foreach (var element in currentInfo.infos)
		{
			foreach (var stat in element.StatsInfo)
			{
				statsInfoCells[index].gameObject.SetActive(true);
				statsInfoCells[index].OnUpdate(stat.stats.type, $"+{stat.GetValue().ToString()}%");
				index++;
			}
		}

		for (int i = 0; i < uiItemJuvenescence.Length; i++)
		{
			if (i < currentInfo.infos.Count)
			{
				uiItemJuvenescence[i].gameObject.SetActive(true);
				uiItemJuvenescence[i].OnUpdate(this, currentInfo.infos[i]);
			}
			else
			{
				uiItemJuvenescence[i].gameObject.SetActive(false);
			}
		}

		buttonNextJuvenescence.gameObject.SetActive(currentInfo.page < infos.Count);
		buttonPrevJuvenescence.gameObject.SetActive(currentInfo.page > 1);
	}

	public void OnUpdate()
	{
		infos = new List<RuntimeData.JuvenescenceInfo>(GameManager.UserDB.juvenescenceContainer.juvenescenceInfos);

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].unlock == false)
			{
				break;
			}
			currentInfo = infos[i];
		}

		if (currentInfo == null)
		{
			currentInfo = infos[0];
		}
		UpdateInfo();

	}

	public void LevelUp(RuntimeData.JuvenescenceElementInfo _info)
	{
		currentInfo.LevelUp(_info);
		UpdateInfo();
	}
}
