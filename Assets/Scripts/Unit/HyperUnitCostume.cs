
using UnityEngine;


[RequireComponent(typeof(UnitAnimation))]
public class HyperUnitCostume : UnitCostume
{
	private CostumeItemObject face;

	[SerializeField] private Transform bodyRoot;
	[SerializeField] private CostumeSkin faceCostume;

	public override void Init()
	{
		unitAnimation = GetComponent<UnitAnimation>();
		runtimeAnimatorController = unitAnimation.animator.runtimeAnimatorController;
	}
	public override void ChangeCostume()
	{
		var unit = unitAnimation.GetComponentInParent<PlayerUnit>();
		face = GameManager.UserDB.costumeContainer.GetHyperFace(unit.hyperPhase);

		ChangeCostume(face);
	}

	public override void ChangeCostume(long tid, CostumeType type, bool isMasked = false)
	{
		var info = GameManager.UserDB.costumeContainer.FindCostumeItem(tid, type);

		if (info == null)
		{
			return;
		}

		switch (type)
		{
			//case CostumeType.WHOLEBODY:
			//	body = info.itemObject;
			//	ChangeBody(isMasked);
			//	break;
			case CostumeType.WHOLEFACE:
				face = info.itemObject;
				ChangeFace(isMasked);
				break;
		}
		SyncAnimation();
	}


	public void ChangeCostume(CostumeItemObject _face)
	{
		//body = _body;
		face = _face;
		//ChangeBody();
		ChangeFace();
		SyncAnimation();
	}
	public void ChangeCostume(RuntimeData.CostumeInfo _face)
	{
		face = _face != null ? _face.itemObject : null;
		//body = _body != null ? _body.itemObject : null;

		ChangeCostume(face);
	}



	//public void ChangeBody(bool isMasked = false)
	//{
	//	if (body == null)
	//	{
	//		return;
	//	}
	//	if (bodyCostume != null)
	//	{
	//		if (body.CostumeObject != null && bodyCostume.name == body.CostumeObject.name)
	//		{
	//			bodyCostume.SetMaterialMask(isMasked);
	//			return;
	//		}
	//		DestroyObject(bodyCostume.gameObject);
	//	}
	//	if (body.CostumeObject == null)
	//	{
	//		return;
	//	}
	//	GameObject costume = Instantiate(body.CostumeObject, bodyRoot);

	//	bodyCostume = costume.GetComponent<CostumeSkin>();
	//	bodyCostume.name = bodyCostume.name.Replace("(Clone)", "");
	//	bodyCostume.transform.localPosition = Vector3.zero;
	//	bodyCostume.transform.localScale = Vector3.one;
	//	bodyCostume.transform.localRotation = Quaternion.identity;
	//	bodyCostume.SetMaterialMask(isMasked);


	//}

	public void ChangeFace(bool isMasked = false)
	{
		if (face == null)
		{
			return;
		}
		if (faceCostume != null)
		{
			if (face.CostumeObject != null && faceCostume.name == face.CostumeObject.name)
			{
				faceCostume.SetMaterialMask(isMasked);
				return;
			}
			DestroyObject(faceCostume.gameObject);
		}
		if (face.CostumeObject == null)
		{
			return;
		}
		GameObject costume = Instantiate(face.CostumeObject, bodyRoot);

		faceCostume = costume.GetComponent<CostumeSkin>();
		faceCostume.name = faceCostume.name.Replace("(Clone)", "");
		faceCostume.transform.localPosition = Vector3.zero;
		faceCostume.transform.localScale = Vector3.one;
		faceCostume.transform.localRotation = Quaternion.identity;
		faceCostume.SetMaterialMask(isMasked);


	}

	public override void SetMaterialTint(float value)
	{
		faceCostume?.SetMaterialTint(value);

	}

	public override void SetMaterialDissolve(float value)
	{
		faceCostume?.SetMaterialDissolve(value);

	}

	public override void SyncAnimation()
	{
		faceCostume?.SetRootBone(rootBone);
	}
}
