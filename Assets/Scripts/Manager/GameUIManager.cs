﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;
using DG.Tweening;

public delegate void OnClose();

public class GameUIManager : MonoBehaviour
{
	private static GameUIManager instance;
	public static GameUIManager it => instance;

	public Canvas mainCanvas;
	public FloatingText resource;

	public GameObject mainUIObject;
	public Image fadeCurtain;

	public Transform floatingUIGroup;
	public RectTransform statusUIGroup;

	public IObjectPool<FloatingText> floatingTextPool;
	public IObjectPool<UnitStatusUI> unitStatusPool;
	public List<Sprite> spriteGradeList = new List<Sprite>();
	public List<Sprite> spriteGradeFrameList = new List<Sprite>();

	public Sprite spriteAds;
	public Sprite spriteExp;

	public UIController uiController;

	public Camera renderTexureCamera;
	public Transform renderTexureStand;


	public ContentOpenMessageSystem contentOpenMessageSystem;

	[SerializeField] private UIStageClear uiStageClear;
	[SerializeField] private UIStageStart uiStageStart;

	public Stack<IUIClosable> uIClosables = new Stack<IUIClosable>();

	public UIContentButton[] buttons;

	public event OnClose onClose;

	private void Awake()
	{
		instance = this;
		floatingTextPool = new ObjectPool<FloatingText>(CreateFloatingText, OnGetFloatingtext, OnReleaseFloatingText, OnDestroyFloatingText);
		unitStatusPool = new ObjectPool<UnitStatusUI>(CreateUnitStatusUI, OnGetUnitStatusUI, OnReleaseUnitStatusUI, OnDestroyUnitStatusUI);
	}

	private void Start()
	{
		buttons = GameObject.FindObjectsOfType<UIContentButton>(true);
	}

	private void OnEnable()
	{
		//EventCallbacks.onLevelupChanged += ShowLevelupPopup;
	}

	private void OnDisable()
	{
		//EventCallbacks.onLevelupChanged -= ShowLevelupPopup;
	}

	#region FloatingTextPool

	private FloatingText CreateFloatingText()
	{
		FloatingText tmp = Instantiate(resource);
		tmp.gameObject.SetActive(false);
		tmp.transform.SetParent(floatingUIGroup);
		(tmp.transform as RectTransform).anchoredPosition3D = Vector3.zero;
		tmp.SetManagedPool(floatingTextPool);

		return tmp;
	}

	private void OnGetFloatingtext(FloatingText floatingtext)
	{
		floatingtext.gameObject.SetActive(true);
	}

	private void OnReleaseFloatingText(FloatingText floatingtext)
	{
		floatingtext.gameObject.SetActive(false);
	}
	private void OnDestroyFloatingText(FloatingText floatingtext)
	{
		Destroy(floatingtext.gameObject);
	}
	#endregion

	#region UnitStatusPool

	private UnitStatusUI CreateUnitStatusUI()
	{
		UnitStatusUI tmp = Instantiate(Resources.Load<UnitStatusUI>("UnitHealthBar"));
		tmp.gameObject.SetActive(false);
		tmp.transform.SetParent(statusUIGroup.transform);
		tmp.SetManagedPool(unitStatusPool);

		return tmp;
	}

	private void OnGetUnitStatusUI(UnitStatusUI _unitStatusUI)
	{
		_unitStatusUI.gameObject.SetActive(false);
	}

	private void OnReleaseUnitStatusUI(UnitStatusUI _unitStatusUI)
	{
		_unitStatusUI.gameObject.SetActive(false);
	}
	private void OnDestroyUnitStatusUI(UnitStatusUI _unitStatusUI)
	{
		Destroy(_unitStatusUI.gameObject);
	}
	#endregion
	public void ReleaseAllPool()
	{
		unitStatusPool.Clear();
	}

	public void ShowAdRewardBox(bool show)
	{
		uiController.ShowAdRewardChest(show);

	}

	public void FadeCurtain(bool fadeIn, bool instant = false)
	{

		if (fadeIn)
		{
			if (instant)
			{
				fadeCurtain.DOFade(1, 0);
			}
			else
			{
				fadeCurtain.DOFade(1, 0.5f);
			}

		}
		else
		{
			if (instant)
			{
				fadeCurtain.DOFade(0, 0);
			}
			else
			{
				fadeCurtain.DOFade(0, 0.5f);
			}
		}
	}


	public Vector2 ToUIPosition(Vector3 worldPosition)
	{
		Vector2 uiPos;

		Vector2 screenPosition = SceneCamera.it.WorldToScreenPoint(worldPosition);
		//uiPos = mainCanvas.worldCamera.ScreenToWorldPoint(screenPosition);
		//uiPos.x -= ((mainCanvas.transform) as RectTransform).sizeDelta.x / 2;
		//uiPos.y -= ((mainCanvas.transform) as RectTransform).sizeDelta.y / 2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(statusUIGroup, screenPosition, mainCanvas.worldCamera, out uiPos);


		return uiPos;
	}
	public void ShowUnitGauge(HittableUnit _unit)
	{
		// UI초기화
		UnitStatusUI statusUI = unitStatusPool.Get();
		statusUI.gameObject.SetActive(false);
		statusUI.transform.SetParent(statusUIGroup.transform, false);
		statusUI.Init(_unit);
	}


	public void ShowStageStart()
	{
		uiStageStart.Activate();
	}

	public void ShowStageResult(StageRule rule)
	{
		uiStageClear.gameObject.SetActive(true);
		uiStageClear.ShowResult(rule);
	}
	public void HideStageResult()
	{
		uiStageClear.gameObject.SetActive(false);
	}

	public void ShowFloatingText(IdleNumber value, Vector3 position, Vector3 endPosition, TextType _textType, Sprite sprite)
	{
		var floatingtext = floatingTextPool.Get();
		floatingtext.Show(value, position, endPosition, _textType, sprite);
	}

	public void AddContentOpenMessage(ContentOpenMessage message)
	{
		contentOpenMessageSystem.AddMessage(message);
	}
	public void Close()
	{
		onClose?.Invoke();
		uIClosables.Clear();
	}

	public GameObject CreateUnitForUI(GameObject _costume)
	{
		var obj = Instantiate(_costume);

		obj.transform.SetParent(renderTexureStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;


		obj.ChangeLayer(LayerMask.NameToLayer("UI"));

		return obj;

	}
	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (uIClosables.Count > 0)
			{
				if (uIClosables.Peek() != null)
				{
					var closable = uIClosables.Pop();
					closable.Close();
				}
			}
		}
	}
}
