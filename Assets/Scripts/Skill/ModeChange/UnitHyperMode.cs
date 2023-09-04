
using System.Threading.Tasks;
using Ink.Parsed;
using UnityEngine;

public class UnitHyperMode : UnitModeBase
{
	public GameObject hyperEffect;

	private HyperUnitCostume hyperUnitCostume;
	public override void OnModeEnter(StateType state)
	{

		path = "";
		resource = "";
		OnSpawn();
		unit.unitAnimation = modelAnimation;

		hyperUnitCostume = modelAnimation.GetComponent<HyperUnitCostume>();
		if (hyperUnitCostume != null)
		{
			hyperUnitCostume.Init();
			hyperUnitCostume.ChangeCostume();
		}
		unit.ChangeState(state, true);

		OnSpawnEffect(unit.position);

		var hyperSlot = PlatformManager.UserDB.costumeContainer[CostumeType.HYPER];

		unit.skillModule.Init(unit, hyperSlot.item.hyperData.skillTid);
		unit.skillModule.ChangeSkillSet(PlatformManager.UserDB.skillContainer.skillSlot);

		foreach (var stats in PlatformManager.UserDB.HyperStats)
		{
			PlatformManager.UserDB.AddModifiers(stats.Key, new StatsModifier(stats.Value.Value, StatModeType.Hyper, hyperSlot));
		}
	}

	public override void OnSpawn()
	{
		if (modelAnimation != null)
		{
			modelAnimation.Release();
		}

		var hyperSlot = PlatformManager.UserDB.costumeContainer[CostumeType.HYPER];
		GameObject costume = hyperSlot.costume;
		if (unit is PlayerUnit)
		{
			PlayerUnit player = unit as PlayerUnit;
			player.hitEffectObject = hyperSlot.item.hyperClassObject.HitEffectObject;
			player.attackEffectObject = hyperSlot.item.hyperClassObject.AttackEffectObject;
		}

		var gameObject = Instantiate(costume);
		modelAnimation = gameObject.GetComponent<UnitAnimation>();
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
		modelAnimation.SetMaskInteraction();
		unit.unitAnimation = modelAnimation;
		unit.unitFacial = modelAnimation.GetComponent<UnitFacial>();
		modelAnimation.animationEventReceiver.Init(unit);
		modelAnimation.PlayDissolve(1.5f);
	}

	public override void OnModeExit()
	{

		if (hyperEffect != null)
		{
			hyperEffect.SetActive(false);
		}

		var hyperSlot = PlatformManager.UserDB.costumeContainer[CostumeType.HYPER];
		foreach (var stats in PlatformManager.UserDB.HyperStats)
		{
			PlatformManager.UserDB.RemoveModifiers(stats.Key, hyperSlot);
		}

		if (modelAnimation != null)
		{
			Destroy(modelAnimation.gameObject);
		}
	}

	public override void OnAttackNormal()
	{
		unit.PlayAnimation(StateType.ATTACK);
	}



	public override void OnHit(HitInfo hit)
	{
		//unit.Hit(hit);
	}
}
