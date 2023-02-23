using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using sc_info;

public class ProjectileManager : VObjectPool<SkillObject>
{
	private static ProjectileManager instance;
	public static ProjectileManager it
	{
		get
		{
			if (instance == null)
			{
				GameObject manager = GameObject.Find("ProjectileManager");
				if (manager == null)
				{
					manager = new GameObject("ProjectileManager");
				}
				instance = manager.GetComponent<ProjectileManager>();
				if (instance == null)
				{
					instance = manager.AddComponent<ProjectileManager>();
				}

			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public SkillObject Create(Vector3 pos, Vector3 targetPos, SkillEffectInfo data, AffectedInfo info, float speed = 1)
	{

		SkillObject projectile = null;

		projectile = Get(data.objectResource.name);

		if (projectile == null)
		{
			return null;
		}

		projectile.Spawn(pos, targetPos, info, speed, data);
		return projectile;
	}
	public SkillObject Create(Vector3 pos, UnitBase target, SkillEffectInfo data, AffectedInfo info, float speed = 1)
	{
		SkillObject projectile = null;

		projectile = Get(data.objectResource.name);

		if (projectile == null)
		{
			return null;
		}

		projectile.Spawn(pos, target, info, speed, data);

		return projectile;
	}

	public SkillObject Create(UnitBase _unit, UnitBase target, SkillEffectInfo data, AffectedInfo info, float speed = 1)
	{

		return Create(_unit.unitAnimation.CenterPivot.position, target, data, info, speed);
	}
	public SkillObject Create(UnitBase _unit, UnitBase target, Vector3 spawnPos, Vector3 targetPos, SkillEffectInfo data, AffectedInfo info, float speed = 1)
	{

		return Create(_unit.unitAnimation.CenterPivot.position, target, data, info, speed);
	}

	public HitInfo CreateHitInfo(UnitBase _attacker, bool isSkill, IdleNumber _attackPower, string _hitSound = "")
	{
		float criticalChanceMul = 1;
		float criticalX2ChangMul = 1;


		criticalX2ChangMul = _attacker.CriticalX2DamageMultifly;
		criticalChanceMul = _attacker.CriticalDamageMultifly;

		AttackerType attackerType = AttackerType.Unknown;
		if (_attacker is Pet)
		{
			attackerType = AttackerType.Pet;
		}
		else if (_attacker is PlayerUnit)
		{
			attackerType = AttackerType.Player;
		}
		else if (_attacker is EnemyUnit)
		{
			attackerType = AttackerType.Enemy;
		}
		else
		{
			attackerType = AttackerType.Unknown;
		}

		HitInfo hitInfo = new HitInfo(attackerType, _attackPower, isSkill);
		hitInfo.criticalChanceMul = criticalChanceMul;
		hitInfo.CriticalX2ChanceMul = criticalX2ChangMul;
		hitInfo.hitSound = _hitSound;

		return hitInfo;

	}

	protected override void SetObject(SkillObject _object, IObjectPool<SkillObject> _pool)
	{
		_object.Set(_pool);
	}

	protected override string GetPath(string _name)
	{
		return $"Projectile/{_name}";
	}

	protected override SkillObject OnCreateObject(string _name)
	{
		SkillObject projectile = Instantiate(GetResource(_name), transform);
		projectile.name = _name;
		return projectile;
	}

	protected override void OnGetObject(SkillObject _object)
	{
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(SkillObject _object)
	{
		_object.gameObject.SetActive(false);
	}

	protected override void OnDestroyObject(SkillObject _object)
	{
		Destroy(_object.gameObject);
	}
}
