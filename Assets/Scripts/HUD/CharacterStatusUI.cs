using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CharacterStatusUI : MonoBehaviour
{
	[SerializeField] private Slider hpSlider = null;
	[SerializeField] private Slider skillGaugeSlider = null;


	private RectTransform rectTransform;
	private Unit observingCharacter;


	private IObjectPool<CharacterStatusUI> managedPool;


	public void SetManagedPool(IObjectPool<CharacterStatusUI> pool)
	{
		managedPool = pool;
	}
	public void Init(Unit _observingCharacter)
	{
		rectTransform = transform as RectTransform;
		observingCharacter = _observingCharacter;
		skillGaugeSlider.gameObject.SetActive(observingCharacter.skillModule.hasSkill);

		rectTransform.anchorMin = Vector2.one * 0.5f;
		rectTransform.anchorMax = Vector2.one * 0.5f;
		rectTransform.localScale = Vector3.one;

		Vector2 uipos = GameUIManager.it.ToUIPosition(observingCharacter.transform.position);
		rectTransform.sizeDelta = new Vector2(70, 60);
		rectTransform.anchoredPosition = uipos;
		gameObject.SetActive(true);

		//// 컨디션을 표시할 UI가 있을땐, 컨디션 변경상태를 체크한다 // check by myoung1 2023/01/02
		//if (this.conditionLayout != null)
		//{
		//	this.observingUnit.conditionModule.onAddCondition += OnAddCondition;
		//	this.observingUnit.conditionModule.onRemoveCondition += OnRemoveCondition;
		//}

		//// UI에 체력 표시
		//if (this.hpText != null)
		//{
		//	UpdateHP();
		//}
		//
		//// UI에 마력표시
		//if (this.mpText != null)
		//{
		//	UpdateMP();
		//}
	}
	private void OnRelease()
	{
		managedPool.Release(this);
	}

	private void Update()
	{
		// 관찰중인 유닛이 없으면 작동X
		if (observingCharacter == null)
		{
			OnRelease();
			return;
		}

		if (observingCharacter.IsAlive() == false)
		{
			OnRelease();
			return;
		}

		// 체력표시 관련 처리
		if (hpSlider != null)
		{
			IdleNumber gauge = (observingCharacter.info.hp / observingCharacter.info.rawHp);

			hpSlider.value = Mathf.Clamp01((float)gauge.Value);
		}

		// 스킬게이지 관련 처리
		if (skillGaugeSlider != null && observingCharacter.skillModule.hasSkill)
		{
			skillGaugeSlider.value = Mathf.Clamp01(observingCharacter.skillModule.skillAttack.uiSkillGauge);
		}

		Vector2 uipos = GameUIManager.it.ToUIPosition(observingCharacter.transform.position);

		rectTransform.anchoredPosition = uipos;
		// 컨디션 관련 처리
		//for (int i = 0 ; i < this.conditionElements.Count ; i++)
		//	this.conditionElements[i].UpdateUI();
	}
}
