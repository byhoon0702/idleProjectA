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

	public GameObject introObject;
	public GameObject mainUIObject;
	public Image fadeCurtain;

	public Transform floatingUIGroup;
	public RectTransform statusUIGroup;
	public Transform StepForGroup;

	public StepFog stepFogResource;

	public IObjectPool<FloatingText> floatingTextPool;
	public IObjectPool<CharacterStatusUI> characterStatusPool;

	private void Awake()
	{
		instance = this;
		floatingTextPool = new ObjectPool<FloatingText>(CreateFloatingText, OnGetFloatingtext, OnReleaseFloatingText, OnDestroyFloatingText);
		characterStatusPool = new ObjectPool<CharacterStatusUI>(CreateCharacterStatusUI, OnGetCharacterStatusUI, OnReleaseCharacterStatusUI, OnDestroyCharacterStatusUI);
	}

	private void OnEnable()
	{
		UserInfo.onLevelupChanged += ShowLevelupPopup;
	}

	private void OnDisable()
	{
		UserInfo.onLevelupChanged -= ShowLevelupPopup;
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

	#region CharacterStatusPool

	private CharacterStatusUI CreateCharacterStatusUI()
	{
		CharacterStatusUI tmp = Instantiate(Resources.Load<CharacterStatusUI>("CharacterStatusBar"));
		tmp.gameObject.SetActive(false);
		tmp.transform.SetParent(statusUIGroup.transform);
		tmp.SetManagedPool(characterStatusPool);

		return tmp;
	}

	private void OnGetCharacterStatusUI(CharacterStatusUI characterstatusui)
	{
		characterstatusui.gameObject.SetActive(false);
	}

	private void OnReleaseCharacterStatusUI(CharacterStatusUI characterstatusui)
	{
		characterstatusui.gameObject.SetActive(false);
	}
	private void OnDestroyCharacterStatusUI(CharacterStatusUI characterstatusui)
	{
		Destroy(characterstatusui.gameObject);
	}
	#endregion
	public void ReleaseAllPool()
	{

		// floatingTextPool.Clear();

		characterStatusPool.Clear();

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

		Vector2 screenPosition = SceneCamera.it.WorldToScreenPoint(worldPosition);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, screenPosition, null, out uipos);


		return uipos;
	}
	public void ShowCharacterGauge(Unit character)
	{
		// UI초기화
		CharacterStatusUI statusUI = characterStatusPool.Get();
		statusUI.gameObject.SetActive(false);
		statusUI.transform.SetParent(statusUIGroup.transform, false);
		statusUI.Init(character);

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

	public void ShowStepFog(UnitBase _unit, bool _isRight)
	{
		StepFog newStepFog = Instantiate(stepFogResource, StepForGroup);
		if (_isRight)
		{
			newStepFog.Show(_unit.characterAnimation.RightFootPivot.gameObject);
		}
		else
		{
			newStepFog.Show(_unit.characterAnimation.LeftFootPivot.gameObject);
		}
	}
}
