using UnityEngine.Pool;
using UnityEngine;
using System.Collections;
using System;


public enum EndActionType
{
	REMOVE = 0,
	REMAIN,
	REMOVEWITHEFFECT,
	REMAINWITHEFFECT,
}


public class SkillObject : MonoBehaviour
{
	[Header("===Common===")]
	public Transform projectileView;
	protected SkillActionBehavior behavior;
	protected AppliedDamageBehavior applyDamageBehavior;

	public float speed
	{
		get;
		protected set;
	}

	public EndActionType endActionType;
	public float remainTime;
	[Header("===Common===")]

	[Space(1)]

	[Header("===Bezier===")]
	public Vector3[] points = new Vector3[4];
	[Header("===Bezier===")]

	[HideInInspector]
	public float duration;
	[HideInInspector]
	public float vy;
	[HideInInspector]
	public Vector3 velocityXZ;

	protected IdleNumber attackPower;

	public AffectedInfo hitInfo
	{
		get;
		private set;
	}
	public UnitBase targetUnit
	{
		get; private set;
	}

	protected Vector3 targetPos;
	protected Vector3 originPos;

	protected Vector3 lookRotation;
	protected float elapsedTime = 0;
	protected bool dontmove = false;

	public Transform cachedTransform
	{ get; private set; }

	protected IObjectPool<SkillObject> managedPool = null;

	public string hitEffect;

	private SkillEffectInfo attackdata;
	private Action<AppliedDamageBehavior, Vector3, AffectedInfo> onReachedTarget;

	public delegate bool OnCheckDistance(Vector3 a, Vector3 b);
	public OnCheckDistance onCheckDistance;


	public void Set(IObjectPool<SkillObject> _managedPool)
	{
		this.managedPool = _managedPool;
	}
	public void SetAction(Action<AppliedDamageBehavior, Vector3, AffectedInfo> _onReachedTarget, OnCheckDistance _onCheckDistance)
	{
		onReachedTarget = _onReachedTarget;
		onCheckDistance = _onCheckDistance;
	}
	public virtual void Spawn(Vector3 _origin, Vector3 _targetPos, AffectedInfo _hitInfo, float _speed, SkillEffectInfo _attackData = null)
	{
		dontmove = false;
		int direction = 1;
		if (_origin.x > _targetPos.x)
		{
			direction = -1;
		}
		transform.localScale = new Vector3(direction, 1, 1);
		transform.position = _origin;

		targetPos = _targetPos;

		attackdata = _attackData;
		hitEffect = attackdata.hitFxResource.name;
		hitInfo = _hitInfo;
		this.speed = _speed;
		if (behavior == null)
		{
			behavior = _attackData.actionBehavior;
		}
		if (behavior == null)
		{
			behavior = (SkillActionBehavior)Resources.Load($"{PathHelper.projectileBehaviorPath}/Straight Movement");
		}

		isFire = true;

		behavior?.SetPostionAndTarget(this, _origin, _targetPos);
	}


	public virtual void Spawn(Vector3 _origin, UnitBase _targetUnit, AffectedInfo _hitInfo, float _speed, SkillEffectInfo _attackData = null)
	{
		targetUnit = _targetUnit;

		Spawn(_origin, targetUnit.unitAnimation.CenterPivot.position, _hitInfo, _speed, _attackData);
	}


	private bool PlayFromPool(bool atTargetPos)
	{
		if (HitEffectPoolManager.it != null)
		{
			var effect = HitEffectPoolManager.it.Get(hitEffect);
			if (effect == null)
			{
				return false;
			}
			effect.transform.position = transform.position;
			if (atTargetPos && targetUnit != null)
			{
				effect.transform.position = targetUnit.position;
			}

			effect.Play();
			return true;
		}
		return false;
	}
	public void ShowHitEffect(bool atTargetPos, Vector3 pos)
	{
		if (hitEffect.IsNullOrEmpty() == false)
		{
			if (PlayFromPool(atTargetPos) == false)
			{
				GameObject effect = (GameObject)Instantiate(Resources.Load($"{PathHelper.hyperCasualFXPath}/{hitEffect}"));
				effect.transform.position = pos;
				effect.transform.localScale = Vector3.one * 0.7f;
				if (atTargetPos && targetUnit != null)
				{
					effect.transform.position = targetUnit.position;
				}

				HitEffect comp = effect.GetComponent<HitEffect>();
				if (comp != null)
				{
					comp.Play();
				}
			}
		}
	}

	public void Release(bool isShowEffect)
	{
		dontmove = true;
		elapsedTime = 0;

		switch (endActionType)
		{
			case EndActionType.REMOVE:
				gameObject.SetActive(false);
				managedPool?.Release(this);
				break;
			case EndActionType.REMOVEWITHEFFECT:
				if (isShowEffect)
				{
					ShowHitEffect(false, transform.position);
				}
				gameObject.SetActive(false);
				managedPool?.Release(this);
				break;
			case EndActionType.REMAIN:
				StartCoroutine(RemoveRoutine());
				break;
			case EndActionType.REMAINWITHEFFECT:
				if (isShowEffect)
				{
					ShowHitEffect(false, transform.position);
				}
				StartCoroutine(RemoveRoutine());
				break;
		}
	}

	private IEnumerator RemoveRoutine()
	{
		yield return new WaitForSeconds(remainTime);
		gameObject.SetActive(false);
		managedPool?.Release(this);
	}

	private bool CheckTarget()
	{
		if (targetUnit == null)
		{
			return false;
		}

		if (onCheckDistance.Invoke(transform.position, targetUnit.position))
		{

			return true;
		}

		return false;
	}
	protected void Update()
	{
		if (dontmove)
		{
			return;
		}

		OnFire();
	}

	private void OnFire()
	{
		if (isFire == false)
		{
			return;
		}

		if (CheckTarget())
		{
			TargetHit();
			isFire = false;
			return;
		}

		if (elapsedTime < duration)
		{
			behavior?.OnUpdate(this, elapsedTime, Time.deltaTime);
			elapsedTime += Time.deltaTime;
		}
		else
		{
			TargetHit();


			isFire = false;
		}
	}
	public void TargetHit()
	{
		Vector3 pos = transform.position;
		if (targetUnit == null)
		{
			Release(true);
			return;
		}

		pos = targetUnit.position;
		ShowHitEffect(true, pos);

		onReachedTarget?.Invoke(attackdata.appliedDamageBehavior, transform.position, hitInfo);
		Release(false);
	}

	private bool isFire = false;


}
