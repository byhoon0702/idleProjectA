using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[UnityEditor.CanEditMultipleObjects]
public class UnitAnimation : MonoBehaviour
{
	public Animator animator;

	[SerializeField] private Transform centerPivot;
	[SerializeField] private Transform headPivot;

	[SerializeField] private Transform rightFootPivot;
	[SerializeField] private Transform leftFootPivot;
	[SerializeField] private GameObject hyperModeEffect;
	[SerializeField] private Material m_thisCharMaterial;
	[SerializeField] private float m_attackedWhiteFlipTime = 1f;
	[SerializeField] private ParticleSystem stepEffect;

	public Transform CenterPivot => centerPivot;
	public Transform HeadPivot => headPivot;
	public Transform RightFootPivot => rightFootPivot;
	public Transform LeftFootPivot => leftFootPivot;

	public StateType _currentState;

	private Dictionary<string, int> animatorHashDictionary = new Dictionary<string, int>();

	private void Start()
	{
		for (int i = 0; i < animator.parameters.Length; i++)
		{
			var param = animator.parameters[i];
			animatorHashDictionary.Add(param.name, param.nameHash);
		}
	}

	private void LateUpdate()
	{
		UpdateAnimation();
	}

	public void PlayAnimation(StateType _stateType)
	{
		if (animator == null)
		{
			return;
		}
		_currentState = _stateType;
	}

	public void ResetAnimation()
	{
		_currentState = StateType.NONE;
		animator.SetTrigger("reset");

		InactiveHyperEffect();
	}

	public bool IsResetComplete()
	{
		// 사망 상태만 아니면 리셋 완료로 취급
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("death") == true)
		{
			return false;
		}
		return true;
	}

	public bool IsAttacking()
	{
		bool isCurrentStateAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
		bool isCurrentStateEnd = animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
		return isCurrentStateAttack && isCurrentStateEnd;
	}

	private void UpdateAnimation()
	{
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
			ResetAllTrigger();
			return;
		}

		ResetAllTrigger();
		animator.SetTrigger(parameterHash);
	}

	private string GetCurrentStateName()
	{
		switch (_currentState)
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

	private void ResetAllTrigger()
	{
		for (int i = 0; i < animator.parameters.Length; i++)
		{
			var param = animator.parameters[i];
			animator.ResetTrigger(param.nameHash);
		}
	}
	public void PlayDamageWhite()
	{
		float m_current_float = 0f;
		DOTween.To(() => m_current_float, x => SetMaterialTint(x), 1, m_attackedWhiteFlipTime).SetLoops(1, LoopType.Yoyo);
	}

	public void PlayDamageWhite(float time)
	{
		float m_current_float = 0f;
		DOTween.To(() => m_current_float, x => SetMaterialTint(x), 1, time).SetLoops(1, LoopType.Yoyo);
	}

	private void SetMaterialTint(float value)
	{
		if (m_thisCharMaterial != null)
			m_thisCharMaterial.SetFloat("_TintValue", value);
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

		centerPivot = transform.Find("ZoomInPivot").transform;
		headPivot = transform.Find("HeadPivot").transform;

		rightFootPivot = animator.transform.Find("leg2_R").transform;
		leftFootPivot = animator.transform.Find("leg2_L").transform;

		stepEffect = transform.Find("StepEffect").Find("Step").GetComponent<ParticleSystem>();
	}
}
