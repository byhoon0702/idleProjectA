using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.Rendering;


[RequireComponent(typeof(SortingGroup))]
public class UnitAnimation : MonoBehaviour
{
	public Animator animator;
	public AnimationEventReceiver animationEventReceiver;

	[SerializeField] private Transform centerPivot;
	[SerializeField] private Transform headPivot;

	[SerializeField] private Transform hitPosition;
	public Transform HitPosition => hitPosition;

	[SerializeField] private GameObject hyperModeEffect;
	[SerializeField] private GameObject shadowObject;

	[SerializeField] private float m_attackedWhiteFlipTime = 0.1f;
	[SerializeField] private ParticleSystem stepEffect;
	[SerializeField] private SortingGroup sortingGroup;
	public Transform CenterPivot => centerPivot;
	public Transform HeadPivot => headPivot;

	private StateType currentState;

	private Dictionary<string, int> animatorHashDictionary = new Dictionary<string, int>();
	private IObjectPool<UnitAnimation> managedPool = null;

	private AnimatorOverrideController currentOverrideController;

	[SerializeField] private Material[] charMaterials;
	[SerializeField] private Renderer[] renderers;
	private MaterialPropertyBlock propertyBlock;
	private bool isReleased = false;

	private UnitCostume unitCostume;

	public void Init()
	{
		unitCostume = transform.GetComponent<UnitCostume>();
		if (animator != null)
		{
			animatorHashDictionary.Clear();
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				var param = animator.parameters[i];
				animatorHashDictionary.Add(param.name, param.nameHash);
			}
		}

		UpdateRenderer();
		SetMaterialTint(0);

		if (CenterPivot != null)
		{
			CenterPivot.localRotation = Quaternion.identity;
		}
		SwitchShadow(true);

		sortingGroup = GetComponent<SortingGroup>();


	}

	public void ChangeSortingLayer(string layerName, int order = 1)
	{
		sortingGroup.sortingLayerName = layerName;
		sortingGroup.sortingOrder = order;
	}
	public void UpdateRenderer()
	{
		propertyBlock = new MaterialPropertyBlock();
		renderers = transform.GetComponentsInChildren<Renderer>(true);
	}

	public void SetParameter(string name, float value)
	{
		if (animator.HasParameter(name))
		{
			animator.SetFloat(name, value);
		}
	}


	public void SwitchShadow(bool switchOn)
	{
		if (shadowObject != null)
		{
			shadowObject.SetActive(switchOn);
		}
	}

	public void Get()
	{
		isReleased = false;
	}


	public void OverrideAnimation(AnimatorOverrideController overrideController)
	{
		if (animator == null)
		{
			return;
		}
		if (overrideController == null)
		{
			return;
		}

		currentOverrideController = overrideController;
		animator.runtimeAnimatorController = currentOverrideController;

	}

	public void ResetOverrideController()
	{
		if (animator == null)
		{
			return;
		}
		if (currentOverrideController != null)
		{
			animator.runtimeAnimatorController = currentOverrideController.runtimeAnimatorController;
		}
	}


	public void Set(IObjectPool<UnitAnimation> _managedPool)
	{
		managedPool = _managedPool;
	}

	public void Release()
	{
		if (isReleased)
		{
			return;
		}
		if (managedPool != null)
		{
			isReleased = true;
			managedPool.Release(this);
		}
	}


	public void PlayAnimation(string name, float normalizedTime = 0)
	{
		animator.enabled = true;
		animator.Play(name, -1, normalizedTime);
	}

	public void PlayAnimation(StateType _stateType)
	{
		if (animator == null)
		{
			return;
		}
		currentState = _stateType;
		animator.enabled = true;
		UpdateAnimation();
	}

	public void PlayIdle()
	{
		if (animator == null)
		{
			return;
		}
		animator.Play("idle", -1, 1);
	}

	public void ResetAnimation()
	{
		if (animator == null)
		{
			return;
		}
		currentState = StateType.NONE;
		animator.SetTrigger("reset");

		InactiveHyperEffect();
	}

	public bool IsResetComplete()
	{
		if (animator == null)
		{
			return true;
		}
		// 사망 상태만 아니면 리셋 완료로 취급
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("death") == true)
		{
			return false;
		}
		return true;
	}

	public bool IsAttacking()
	{
		if (animator == null)
		{
			return false;
		}
		bool isCurrentStateAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
		bool isCurrentStateEnd = animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
		return isCurrentStateAttack && isCurrentStateEnd;
	}

	public float GetAttackStateDuration()
	{
		return 0;
	}

	private void UpdateAnimation()
	{
		if (animator == null)
		{
			return;
		}
		string stateName = GetCurrentStateName();

		// state 누락
		if (stateName == null)
		{
			return;
		}

		if (animatorHashDictionary.TryGetValue(stateName, out int parameterHash) == false)
		{
			return;
		}

		// 이미 현재 해당 state인 상태
		if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
		{
			return;
		}

		animator.SetTrigger(parameterHash);

	}

	private string GetCurrentStateName()
	{
		switch (currentState)
		{
			case StateType.IDLE:
				return "idle";
			case StateType.MOVE:
				return "move";
			case StateType.ATTACK:
				return "attack";
			case StateType.HIT:
				return "hit";
			case StateType.SKILL:
				return "skill";
			case StateType.DEATH:
				return "death";
			default:
				return null;
		}
	}
	Tweener tweener;
	Tweener dissolve;
	public void PlayDamageWhite()
	{
		float m_current_float = 50f;

		tweener = DOVirtual.Float(m_current_float, 0, 0.3f, (value) =>
		{
			SetMaterialTint(value);
		});
	}
	public void PlayDissolve(float value)
	{
		SetMaterialDissolve(value);
		//float m_current_float = 0f;

		//dissolve = DOVirtual.Float(m_current_float, 1.5f, 0.5f, (value) =>
		//{
		//	SetMaterialDissolve(value);
		//});
	}

	public void Death()
	{
		StartCoroutine(DeathLoop());
	}

	public IEnumerator DeathLoop()
	{
		float m_current_float = 50f;
		float time = 0.2f;
		for (int i = 0; i < 10; i++)
		{
			SetMaterialTint(m_current_float);
			yield return new WaitForSeconds(time);
			SetMaterialTint(0);
			yield return new WaitForSeconds(time);

			time *= 0.8f;
		}

	}

	private void OnDisable()
	{
		if (tweener != null)
		{
			tweener.Kill();
		}
	}

	public void PlayDamageWhite(float time)
	{
		float m_current_float = 0f;
		DOTween.To(() => m_current_float, x => SetMaterialTint(x), 1, time).SetLoops(1, LoopType.Yoyo);
	}


	private void SetMaterialTint(float value)
	{
		if (unitCostume != null)
		{
			unitCostume.SetMaterialTint(value);
			//return;
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			if (renderers[i] == null)
			{
				continue;
			}
			renderers[i].GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat("_TintAmount", value);
			renderers[i].SetPropertyBlock(propertyBlock);
		}
	}

	private void SetMaterialDissolve(float value)
	{
		if (unitCostume != null)
		{
			unitCostume.SetMaterialDissolve(value);
			//return;
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			if (renderers[i] == null)
			{
				continue;
			}
			renderers[i].GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat("_DissolveAmount", value);
			renderers[i].SetPropertyBlock(propertyBlock);
		}
	}

	public void ActiveHyperEffect()
	{
		if (hyperModeEffect != null)
		{
			hyperModeEffect.gameObject.SetActive(true);
		}
	}

	public void InactiveHyperEffect()
	{
		if (hyperModeEffect != null)
		{
			hyperModeEffect.gameObject.SetActive(false);
		}

	}

	public void PlayParticle()
	{
		if (stepEffect != null)
		{
			stepEffect.Play();
		}

	}
	public void StopAnimation()
	{
		animator.enabled = false;
	}

	public void StopParticle()
	{
		if (stepEffect != null)
		{
			stepEffect.Stop();
		}
	}

	public void SetModel()
	{
		animator = GetComponentInChildren<Animator>();
		animationEventReceiver = animator.GetComponent<AnimationEventReceiver>();

		if (animationEventReceiver == null)
		{
			animationEventReceiver = animator.gameObject.AddComponent<AnimationEventReceiver>();
		}

		//SortingGroup group = GetComponent<SortingGroup>();
		//group.sortingLayerName = "CharDepth1";

		centerPivot = transform.Find("ZoomInPivot").transform;
		headPivot = transform.Find("HeadPivot").transform;

		if (transform.Find("StepEffect") != null && transform.Find("StepEffect").Find("Step"))
		{
			stepEffect = transform.Find("StepEffect").Find("Step").GetComponent<ParticleSystem>();
		}
	}
}


