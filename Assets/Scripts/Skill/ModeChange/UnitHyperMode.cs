
using System.Threading.Tasks;
using UnityEngine;


//[CreateAssetMenu(fileName = "Unit Hyper Mode", menuName = "ScriptableObject/Unit Hyper Mode", order = 1)]
public class UnitHyperMode : UnitModeBase
{

	public SkillObject hyperEnterAttack;
	public SkillObject hyperAttack;
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

		hyperEffect.SetActive(true);
		if (hyperEnterAttack != null)
		{
			var attack = Instantiate(hyperEnterAttack).GetComponent<SkillObject>();
			HitInfo hitinfo = new HitInfo(unit.AttackPower);
			attack.Trigger(unit, unit.position, hitinfo, null);
		}
	}
	public override void OnSpawn(string _path, string _resource)
	{
		if (modelAnimation != null)
		{
			modelAnimation.Release();
		}
		//resource = _resource;
		//modelAnimation = UnitModelPoolManager.it.Get(path, resource);

		var gameObject = Instantiate(GameManager.UserDB.costumeContainer.GetHyperCostume().CostumeObject);
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
