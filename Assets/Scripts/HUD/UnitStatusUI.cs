using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UnitStatusUI : MonoBehaviour
{
	[SerializeField] private Slider hpSlider = null;
	//[SerializeField] private Slider skillGaugeSlider = null;


	private RectTransform rectTransform;
	private HittableUnit observingUnit;


	private IObjectPool<UnitStatusUI> managedPool;


	public void SetManagedPool(IObjectPool<UnitStatusUI> pool)
	{
		managedPool = pool;
	}

	public void Init(HittableUnit _observingUnit)
	{
		rectTransform = transform as RectTransform;
		observingUnit = _observingUnit;
		if (_observingUnit is Unit)
		{
			//skillModule = (_observingUnit as Unit).skillModule;
			//skillGaugeSlider.gameObject.SetActive(skillModule.mainSkill != null);
		}
		else
		{
			//skillGaugeSlider.gameObject.SetActive(false);
		}

		rectTransform.anchorMin = Vector2.one * 0.5f;
		rectTransform.anchorMax = Vector2.one * 0.5f;
		rectTransform.localScale = Vector3.one;

		Vector2 uipos = GameUIManager.it.ToUIPosition(observingUnit.HeadPosition);
		//rectTransform.sizeDelta = new Vector2(70, 60);
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
		if (observingUnit == null)
		{
			OnRelease();
			return;
		}

		if (observingUnit.IsAlive() == false)
		{
			OnRelease();
			return;
		}

		// 체력표시 관련 처리
		if (hpSlider != null)
		{
			IdleNumber gauge = (observingUnit.Hp / observingUnit.MaxHp);

			hpSlider.value = Mathf.Clamp01((float)gauge.Value);
		}

		//// 스킬게이지 관련 처리
		//if (skillGaugeSlider != null && skillModule != null && skillModule.mainSkill != null)
		//{
		//	skillGaugeSlider.value = Mathf.Clamp01(skillModule.mainSkill.uiSkillGauge);
		//}

		Vector2 uipos = GameUIManager.it.ToUIPosition(observingUnit.HeadPosition);

		rectTransform.anchoredPosition3D = uipos;
		// 컨디션 관련 처리
		//for (int i = 0 ; i < this.conditionElements.Count ; i++)
		//	this.conditionElements[i].UpdateUI();
	}
}
