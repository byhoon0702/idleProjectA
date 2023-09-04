using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialType
{
	//버튼
	SELECTABLE,
	//리스트 아이템
	LIST,

}




public class TutorialManager : MonoBehaviour
{
	public Camera uiCamera;
	public RectTransform obj;
	public static TutorialManager instance;

	public RuntimeData.QuestInfo questinfo;
	private RectTransform targetRect;

	[SerializeField] private TutorialObject[] tutorialObjs;
	public TutorialObject currentTutorial;
	private List<Vector3> pos;
	int index;
	private void Awake()
	{
		instance = this;

		tutorialObjs = Resources.LoadAll<TutorialObject>("Tutorials");
	}

	public bool begin = false;
	public void BeginGuide(RectTransform target, RuntimeData.QuestInfo quest)
	{
		targetRect = target;
		if (questinfo != null && questinfo.Tid == quest.Tid)
		{
			return;
		}
		questinfo = quest;
		currentTutorial = null;

		if (questinfo == null)
		{
			return;
		}
		for (int i = 0; i < tutorialObjs.Length; i++)
		{
			if (tutorialObjs[i].questGoalType == questinfo.GoalType)
			{
				currentTutorial = tutorialObjs[i];
				currentTutorial.BeginTutorial(questinfo);
				break;
			}
		}
	}

	public void QuestComplete()
	{
		begin = false;
		targetRect = null;
		questinfo = null;
		Reset();
	}
	public void SetPosition(Transform rect)
	{
		SetPosition(rect as RectTransform);

	}

	public void Reset()
	{
		obj.gameObject.SetActive(false);
		obj.transform.SetParent(transform);
		obj.anchorMin = new Vector2(0.5f, 0.5f);
		obj.anchorMax = new Vector2(0.5f, 0.5f);
		obj.anchoredPosition = Vector2.zero;
	}
	public void SetPosition(RectTransform rect)
	{
		if (rect == null)
		{
			Reset();
			return;
		}

		obj.gameObject.SetActive(true);
		obj.transform.SetParent(rect);
		obj.anchorMin = rect.pivot;
		obj.anchorMax = rect.pivot;
		obj.anchoredPosition = Vector2.zero;
		//Vector3 position = new Vector3(rect.position.x, rect.position.y, 0);

		//Vector3 screenPos = uiCamera.WorldToScreenPoint(position);
		//Vector2 viewport = uiCamera.ScreenToViewportPoint(screenPos);

		//obj.gameObject.SetActive(true);
		//obj.anchorMin = new Vector2(viewport.x, viewport.y);
		//obj.anchorMax = new Vector2(viewport.x, viewport.y);
		//obj.anchoredPosition = Vector2.zero;
	}

	public bool TutorialFulfilled()
	{
		return false;
	}

	private void Update()
	{
		if (questinfo == null)
		{
			return;
		}
		if (questinfo.progressState == QuestProgressState.ONPROGRESS)
		{
			if (currentTutorial != null)
			{
				currentTutorial.OnUpdate(Time.deltaTime);
			}
		}
		else
		{
			SetPosition(targetRect);
		}
	}
}
