
using UnityEngine;

using UnityEngine.Pool;

public class SkillEffectObject : MonoBehaviour
{
	[SerializeField] private SkillEffectData mData;
	public SkillEffectData data
	{
		get
		{
			return mData;
		}
	}

	[HideInInspector]
	public int index;

	public Vector3 pos
	{ get; private set; }
	public Vector3 targetPos
	{ get; private set; }

	public UnitBase targetUnit
	{ get; private set; }

	public AffectedInfo hitinfo
	{
		private set;
		get;
	}
	public UnitBase caster
	{
		private set;
		get;
	}

	public float duration
	{
		get;
		private set;
	}

	//공격, 버프, 힐에 대한 부분
	public BaseAbilitySO baseAbilitySO;

	public bool isFire;
	private bool isDependent;

	private float elapsedTime;


	private IObjectPool<SkillEffectObject> managedPool;

	private bool isCancel;

	public void SetManagedPool(IObjectPool<SkillEffectObject> _managedPool)
	{
		managedPool = _managedPool;
	}

	public void SetData(SkillEffectData _data)
	{
		if (_data == null)
		{
			return;
		}
		mData = _data;

		isDependent = mData.isDependent;
		duration = mData.duration > 0 ? mData.duration : 1;
	}

	public void Cancel()
	{
		isCancel = true;
	}

	public void Reset()
	{
		isFire = false;
		elapsedTime = 0;
		isCancel = false;
		index = 0;
	}

	public bool isTargetAlive()
	{

		if (targetUnit == null)
		{
			return false;
		}

		return targetUnit.IsAlive();
	}

	public void UpdateFromOutSide(float time)
	{
		if (isDependent == false)
		{
			return;
		}
		if (isFire)
		{
			baseAbilitySO?.OnUpdate(this, time);
		}
	}

	private void Update()
	{
		if (isDependent)
		{
			return;
		}

		if (isFire)
		{
			if (elapsedTime > duration || isCancel)
			{
				Reset();
				Release();
				return;
			}

			baseAbilitySO?.OnUpdate(this, elapsedTime);

			elapsedTime += Time.deltaTime;
		}
	}

	private void OnSkillStart(Vector3 pos, Vector3 targetPos, IdleNumber power)
	{
		Reset();
		isFire = true;
		this.pos = pos;
		this.targetPos = targetPos;

		targetUnit = null;
		transform.position = pos;
		if (caster != null)
		{

			targetUnit = caster.target;

			transform.position = caster.position;
		}

		if ((baseAbilitySO == null) || (baseAbilitySO != null && baseAbilitySO.name != data.abilitySO))
		{
			baseAbilitySO = (BaseAbilitySO)Resources.Load($"{PathHelper.skillAbilitySOPath}/{data.abilitySO}");
		}
		baseAbilitySO?.OnSet(this, power);
		//skillEffectActionSO?.OnSet(this);
	}
	public void SetAffectedInfo(AffectedInfo _affectedInfo)
	{
		hitinfo = _affectedInfo;
	}

	public void OnSkillStart(UnitBase _attacker, Vector3 pos, Vector3 targetPos, IdleNumber power)
	{
		if (isFire)
		{
			return;
		}
		gameObject.SetActive(true);
		caster = _attacker;
		targetUnit = caster.target;
		OnSkillStart(pos, targetPos, power);

	}

	public void OnSkillStart(UnitBase _attacker, UnitBase _target, IdleNumber power)
	{
		if (isFire)
		{
			return;
		}

		caster = _attacker;
		targetPos = _target.unitAnimation.CenterPivot.position;
		OnSkillStart(caster.unitAnimation.CenterPivot.position, targetPos, power);
	}


	public void OnSkillStart(UnitBase _attacker, Vector3 targetPos, IdleNumber power)
	{
		if (isFire)
		{
			return;
		}

		caster = _attacker;

		OnSkillStart(caster.unitAnimation.CenterPivot.position, targetPos, power);
	}

#if UNITY_EDITOR
	public void OnSkillStartEditor(UnitBase _attacker, Vector3 pos, Vector3 targetPos)
	{
		if (isFire)
		{
			return;
		}

		isDependent = false;
		caster = _attacker;

		OnSkillStart(pos, targetPos, caster != null ? caster.AttackPower : new IdleNumber(1000));
	}
#endif
	public void Release()
	{
		gameObject.SetActive(false);
		//managedPool?.Release(this);
	}
}
