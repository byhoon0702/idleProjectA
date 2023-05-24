using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitModeBase : MonoBehaviour
{

	[SerializeField] protected GameObject spawnEffect;

	protected UnitAnimation modelAnimation;

	//protected SkillEffectObject skillObject;
	protected Unit unit;
	protected string resource;
	protected string path;
	public void OnInit(Unit _unit)
	{
		unit = _unit;
	}

	public virtual void OnSpawnEffect(Vector3 pos)
	{
		if (spawnEffect == null)
		{
			return;
		}

		GameObject spawnEffectObj = Instantiate(spawnEffect);
		spawnEffectObj.transform.position = pos;
		spawnEffectObj.transform.localScale = Vector3.one;

	}
	public virtual void OnSpawn(string path, string _resource)
	{
		if (modelAnimation != null)
		{
			modelAnimation.Release();
		}
		resource = _resource;
		modelAnimation = UnitModelPoolManager.it.Get(path, resource);

		modelAnimation.transform.SetParent(unit.transform);
		modelAnimation.transform.localPosition = Vector3.zero;

		Vector3 scale = modelAnimation.transform.localScale;
		scale.x = Mathf.Abs(scale.x) * unit.currentDir;
		modelAnimation.transform.localScale = scale;

		if (SceneCamera.it != null)
		{
			Camera sceneCam = SceneCamera.it.sceneCamera;
			modelAnimation.transform.LookAt(modelAnimation.transform.position + sceneCam.transform.rotation * Vector3.forward, sceneCam.transform.rotation * Vector3.up);
		}
		modelAnimation.Init();
		unit.unitAnimation = modelAnimation;
		unit.unitFacial = modelAnimation.GetComponent<UnitFacial>();
	}

	public abstract void OnModeEnter(StateType state);
	public abstract void OnModeExit();

	public abstract void OnAttackSkill();
	public abstract void OnAttackNormal();

	public abstract void OnMove();

	public abstract void OnHit(HitInfo hit);
}
