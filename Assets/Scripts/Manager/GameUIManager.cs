using System;
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
	public UIController uiController;


	[SerializeField] private UIStageClear uiStageClear;
	[SerializeField] private UIStageStart uiStageStart;


	public event OnClose onClose;

	private void Awake()
	{
		instance = this;
		floatingTextPool = new ObjectPool<FloatingText>(CreateFloatingText, OnGetFloatingtext, OnReleaseFloatingText, OnDestroyFloatingText);
		unitStatusPool = new ObjectPool<UnitStatusUI>(CreateUnitStatusUI, OnGetUnitStatusUI, OnReleaseUnitStatusUI, OnDestroyUnitStatusUI);
	}

	private void OnEnable()
	{
		EventCallbacks.onLevelupChanged += ShowLevelupPopup;
	}

	private void OnDisable()
	{
		EventCallbacks.onLevelupChanged -= ShowLevelupPopup;
	}

	#region FloatingTextPool

	private FloatingText CreateFloatingText()
	{
		FloatingText tmp = Instantiate(resource);
		tmp.gameObject.SetActive(false);
		tmp.transform.SetParent(floatingUIGroup);
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

		// floatingTextPool.Clear();

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

	public void ShowFloatingText(IdleNumber value, /*Color color,*/ Vector3 position, Vector3 endPosition, TextType _textType)
	{
		var floatingtext = floatingTextPool.Get();
		floatingtext.Show(value, position, endPosition, _textType);
	}

	private void ShowLevelupPopup(Int32 _beforeLv, Int32 _afterLv)
	{
		ToastUI.it.Enqueue($"Levelup! {_beforeLv} -> {_afterLv}");
	}

	public void OnClose()
	{
		onClose?.Invoke();
	}
}
