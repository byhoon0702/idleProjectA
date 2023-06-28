
using System.Threading.Tasks;
using Ink.Parsed;
using UnityEngine;


//[CreateAssetMenu(fileName = "Unit Hyper Mode", menuName = "ScriptableObject/Unit Hyper Mode", order = 1)]
public class UnitHyperMode : UnitModeBase
{
	public GameObject hyperEffect;

	private GameObject model;
	private HyperUnitCostume hyperUnitCostume;
	public override void OnModeEnter(StateType state)
	{

		path = "";
		resource = "";
		OnSpawn(path, resource);
		unit.unitAnimation = modelAnimation;

		hyperUnitCostume = modelAnimation.GetComponent<HyperUnitCostume>();
		if (hyperUnitCostume != null)
		{
			hyperUnitCostume.Init();
			hyperUnitCostume.ChangeCostume();
		}
		unit.ChangeState(state, true);

		OnSpawnEffect(unit.position);

		unit.skillModule.Init(unit, GameManager.UserDB.awakeningContainer.selectedInfo.hyperData.skillTid);
		unit.skillModule.ChangeSkillSet(GameManager.UserDB.skillContainer.skillSlot);

		GameManager.UserDB.HyperStats.stats.Clear();
		var hyperStats = GameManager.UserDB.awakeningContainer.selectedInfo.abilityInfos;

		for (int i = 0; i < hyperStats.Count; i++)
		{
			var stat = hyperStats[i];
			GameManager.UserDB.AddModifiers(false, stat.type, new StatsModifier(stat.Value, StatModeType.PercentAdd, unit.hyperModule));
		}

	}
	public override void OnSpawn(string _path, string _resource)
	{
		if (modelAnimation != null)
		{
			modelAnimation.Release();
		}

		GameObject costume = null;// GameManager.UserDB.awakeningContainer.selectedInfo.hyperClassObject.HyperCostumes[0];
		if (unit is PlayerUnit)
		{
			PlayerUnit player = unit as PlayerUnit;
			player.hitEffectObject = GameManager.UserDB.awakeningContainer.selectedInfo.hyperClassObject.HitEffectObject;
			player.attackEffectObject = GameManager.UserDB.awakeningContainer.selectedInfo.hyperClassObject.AttackEffectObject;
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
		unit.unitAnimation = modelAnimation;
		unit.unitFacial = modelAnimation.GetComponent<UnitFacial>();
		model = modelAnimation.gameObject;
		modelAnimation.animationEventReceiver.Init(unit);
		modelAnimation.PlayDissolve(1.5f);
	}

	public override void OnModeExit()
	{
		//if (modelAnimation != null)
		//{
		//	modelAnimation.Release();
		//}
		if (hyperEffect != null)
		{
			hyperEffect.SetActive(false);
		}
		//for (int i = 0; i < GameManager.UserDB.HyperStats.stats.Count; i++)
		//{
		//	var stat = GameManager.UserDB.HyperStats.stats[i];
		//	GameManager.UserDB.RemoveModifiers(false, stat.type, unit.hyperModule);
		//}

		if (model != null)
		{
			Destroy(model);
		}
	}

	public override void OnAttackNormal()
	{
		unit.PlayAnimation(StateType.ATTACK);
	}

	public override void OnAttackSkill()
	{

	}

	public override void OnMove()
	{

	}

	public override void OnHit(HitInfo hit)
	{
		//unit.Hit(hit);
	}
}
