using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ProjectileManager : MonoBehaviour
{
	private static ProjectileManager instance;
	public static ProjectileManager it => instance;

	private Dictionary<string, List<Projectile>> projectilePool = new Dictionary<string, List<Projectile>>();
	private List<GuidedProjectile> guidedProjectileList = new List<GuidedProjectile>();
	private List<StraightProjectile> straightProjectileList = new List<StraightProjectile>();
	private List<ParabolaProjectile> parabolaProjectileList = new List<ParabolaProjectile>();
	private List<BezierProjectile> bezierProjectileList = new List<BezierProjectile>();


	public ProjectileBehaviorGroup projectileBehaviorGroup;
	private void Awake()
	{
		instance = this;
	}


	public Projectile Create(UnitBase character)
	{
		Projectile projectile = null;

		projectile = GetProjectile(character.GetProjectileName());

		if (projectile == null)
		{
			return null;
		}

		projectile.Spawn(character.characterAnimation.CenterPivot, character, character.target, character.AttackPower());

		return projectile;
	}
	// Start is called before the first frame update

	private Projectile GetProjectile<T>(T _resource, List<T> _projectileList) where T : Projectile
	{
		T projectile = null;

		for (int i = 0; i < _projectileList.Count; i++)
		{
			projectile = _projectileList[i];
			if (projectile.gameObject.activeSelf == false)
			{
				projectile.gameObject.SetActive(true);
				return projectile;
			}
		}

		projectile = Instantiate(_resource);
		_projectileList.Add(projectile);
		projectile.gameObject.SetActive(true);
		return projectile;
	}
	private Projectile GetProjectile(string _resource)
	{
		Projectile projectile = null;
		if (projectilePool.ContainsKey(_resource) == false)
		{
			projectilePool.Add(_resource, new List<Projectile>());
		}
		var _projectileList = projectilePool[_resource];
		for (int i = 0; i < _projectileList.Count; i++)
		{
			projectile = _projectileList[i];
			if (projectile.gameObject.activeSelf == false)
			{
				projectile.gameObject.SetActive(true);
				return projectile;
			}
		}
		var od = Resources.Load($"Projectile/{_resource}");
		projectile = Instantiate(od).GetComponent<Projectile>();
		if (projectile == null)
		{
			return null;
		}
		projectilePool[_resource].Add(projectile);
		projectile.gameObject.SetActive(true);
		return projectile;
	}

	public void ClearProjectiles()
	{

		foreach (var dd in projectilePool)
		{
			while (dd.Value.Count > 0)
			{
				Destroy(dd.Value[0].gameObject);
				dd.Value.RemoveAt(0);
			}
		}
		projectilePool.Clear();
		for (int i = 0; i < guidedProjectileList.Count; i++)
		{
			Projectile projectile = guidedProjectileList[i];
			if (projectile != null)
			{
				Destroy(projectile.gameObject);
			}
		}
		guidedProjectileList.Clear();

		for (int i = 0; i < straightProjectileList.Count; i++)
		{
			Projectile projectile = straightProjectileList[i];
			if (projectile != null)
			{
				Destroy(projectile.gameObject);
			}
		}
		straightProjectileList.Clear();

		for (int i = 0; i < parabolaProjectileList.Count; i++)
		{
			Projectile projectile = parabolaProjectileList[i];
			if (projectile != null)
			{
				Destroy(projectile.gameObject);
			}
		}
		parabolaProjectileList.Clear();

		for (int i = 0; i < bezierProjectileList.Count; i++)
		{
			Projectile projectile = bezierProjectileList[i];
			if (projectile != null)
			{
				Destroy(projectile.gameObject);
			}
		}
		bezierProjectileList.Clear();
	}
}
