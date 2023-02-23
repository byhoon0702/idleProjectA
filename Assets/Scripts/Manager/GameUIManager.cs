using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;
using DG.Tweening;

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
		UnitStatusUI tmp = Instantiate(Resources.Load<UnitStatusUI>("UnitStatusBar"));
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


	public void FadeCurtain(bool fadeIn)
	{
		if (fadeIn)
		{
			fadeCurtain.DOFade(1, 0.5f);
		}
		else
		{
			fadeCurtain.DOFade(0, 0.5f);
		}
	}

	public Vector2 ToUIPosition(Vector3 worldPosition)
	{
		Vector2 uipos;

		Vector2 screenPosition;
		if (SceneCameraV2.it != null)
		{
			screenPosition = SceneCameraV2.it.WorldToScreenPoint(worldPosition);
		}
		else
		{
			screenPosition = SceneCamera.it.WorldToScreenPoint(worldPosition);
		}
		RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, screenPosition, null, out uipos);


		return uipos;
	}
	public void ShowUnitGauge(Unit _unit)
	{
		// UI초기화
		UnitStatusUI statusUI = unitStatusPool.Get();
		statusUI.gameObject.SetActive(false);
		statusUI.transform.SetParent(statusUIGroup.transform, false);
		statusUI.Init(_unit);

	}
	public void ShowFloatingText(string text, Color color, Vector3 position, CriticalType _criticalType, bool isPlayer = false)
	{
		//Vector2 uipos = ToUIPosition(position);

		var floatingtext = floatingTextPool.Get();
		floatingtext.Show(text, color, position, _criticalType, isPlayer);
	}

	private void ShowLevelupPopup(Int32 _beforeLv, Int32 _afterLv)
	{
		ToastUI.it.Create($"Levelup! {_beforeLv} -> {_afterLv}");
	}
}
