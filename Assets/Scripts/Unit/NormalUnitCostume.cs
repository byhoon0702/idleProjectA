
using UnityEngine;


[RequireComponent(typeof(UnitAnimation))]
public class NormalUnitCostume : UnitCostume
{

	private CostumeItemObject head;
	private CostumeItemObject body;
	private CostumeItemObject weapon;


	[SerializeField] private Transform headRoot;

	[SerializeField] private Transform bodyRoot;


	[SerializeField] private Transform weaponRoot;

	[SerializeField] private CostumeSkin headCostume;
	[SerializeField] private CostumeSkin bodyCostume;
	[SerializeField] private CostumeSkin weaponCostume;


	public override void Init()
	{
		unitAnimation = GetComponent<UnitAnimation>();
		runtimeAnimatorController = unitAnimation.animator.runtimeAnimatorController;
	}
	public override void ChangeCostume()
	{
		var headinfo = GameManager.UserDB.costumeContainer[CostumeType.HEAD];
		var bodyinfo = GameManager.UserDB.costumeContainer[CostumeType.BODY];
		var weaponinfo = GameManager.UserDB.costumeContainer[CostumeType.WEAPON];

		ChangeCostume(headinfo.item, bodyinfo.item, weaponinfo.item);
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
			case CostumeType.HEAD:
				head = info.itemObject;
				ChangeHead(isMasked);
				break;
			case CostumeType.BODY:
				body = info.itemObject;
				ChangeBody(isMasked);
				break;
			case CostumeType.WEAPON:
				weapon = info.itemObject;
				ChangeWeapon(isMasked);
				break;
		}
		SyncAnimation();
	}

	public void EquipeWeapon(RuntimeData.EquipItemInfo _weapon)
	{
		if (_weapon == null || _weapon.itemObject == null)
		{
			return;
		}
		if (weaponCostume != null)
		{
			DestroyObject(weaponCostume.gameObject);
			weapon = null;
		}

		GameObject costume = Instantiate(_weapon.itemObject.equipObject, weaponRoot);

		weaponCostume = costume.GetComponent<CostumeSkin>();
		weaponCostume.name = weaponCostume.name.Replace("(Clone)", "");
		weaponCostume.transform.localPosition = Vector3.zero;
		weaponCostume.transform.localScale = Vector3.one;
		weaponCostume.transform.localRotation = Quaternion.identity;

		if (weaponCostume.OverrideController != null)
		{
			unitAnimation.animator.runtimeAnimatorController = weaponCostume.OverrideController;
		}
		else
		{
			unitAnimation.animator.runtimeAnimatorController = runtimeAnimatorController;
		}
		SyncAnimation();
	}

	public void ChangeCostume(CostumeItemObject _head, CostumeItemObject _body, CostumeItemObject _weapon)
	{
		head = _head;
		body = _body;
		weapon = _weapon;

		ChangeHead();
		ChangeBody();
		ChangeWeapon();

		SyncAnimation();
	}
	public void ChangeCostume(RuntimeData.CostumeInfo _head, RuntimeData.CostumeInfo _body, RuntimeData.CostumeInfo _weapon)
	{
		head = _head != null ? _head.itemObject : null;
		body = _body != null ? _body.itemObject : null;
		weapon = _weapon != null ? _weapon.itemObject : null;

		ChangeCostume(head, body, weapon);
	}

	public void ChangeHead(bool isMasked = false)
	{
		if (head == null)
		{
			return;
		}

		if (headCostume != null)
		{
			if (head.CostumeObject != null && headCostume.name == head.CostumeObject.name)
			{
				headCostume.SetMaterialMask(isMasked);
				return;
			}
			DestroyObject(headCostume.gameObject);
		}

		if (head.CostumeObject == null)
		{
			return;
		}
		GameObject costume = Instantiate(head.CostumeObject, headRoot);

		headCostume = costume.GetComponent<CostumeSkin>();
		headCostume.name = headCostume.name.Replace("(Clone)", "");
		headCostume.transform.localPosition = Vector3.zero;
		headCostume.transform.localScale = Vector3.one;
		headCostume.transform.localRotation = Quaternion.identity;

		headCostume.SetMaterialMask(isMasked);
	}

	public void ChangeBody(bool isMasked = false)
	{
		if (body == null)
		{
			return;
		}
		if (bodyCostume != null)
		{
			if (body.CostumeObject != null && bodyCostume.name == body.CostumeObject.name)
			{
				bodyCostume.SetMaterialMask(isMasked);
				return;
			}
			DestroyObject(bodyCostume.gameObject);
		}
		if (body.CostumeObject == null)
		{
			return;
		}
		GameObject costume = Instantiate(body.CostumeObject, bodyRoot);

		bodyCostume = costume.GetComponent<CostumeSkin>();
		bodyCostume.name = bodyCostume.name.Replace("(Clone)", "");
		bodyCostume.transform.localPosition = Vector3.zero;
		bodyCostume.transform.localScale = Vector3.one;
		bodyCostume.transform.localRotation = Quaternion.identity;
		bodyCostume.SetMaterialMask(isMasked);


	}

	public void ChangeWeapon(bool isMasked = false)
	{
		if (unitAnimation)
			if (weapon == null)
			{
				return;
			}
		if (weaponCostume != null)
		{
			if (weapon.CostumeObject != null && weaponCostume.name == weapon.CostumeObject.name)
			{
				weaponCostume.SetMaterialMask(isMasked);
				return;
			}

			DestroyObject(weaponCostume.gameObject);
		}
		if (weapon.CostumeObject == null)
		{
			return;
		}

		GameObject costume = Instantiate(weapon.CostumeObject, weaponRoot);

		weaponCostume = costume.GetComponent<CostumeSkin>();
		weaponCostume.name = weaponCostume.name.Replace("(Clone)", "");
		weaponCostume.transform.localPosition = Vector3.zero;
		weaponCostume.transform.localScale = Vector3.one;
		weaponCostume.transform.localRotation = Quaternion.identity;
		weaponCostume.SetMaterialMask(isMasked);

		if (weaponCostume.OverrideController != null)
		{
			unitAnimation.animator.runtimeAnimatorController = weaponCostume.OverrideController;
		}
		else
		{
			unitAnimation.animator.runtimeAnimatorController = runtimeAnimatorController;
		}
	}

	public override void SetMaterialTint(float value)
	{
		headCostume?.SetMaterialTint(value);
		bodyCostume?.SetMaterialTint(value);
		weaponCostume?.SetMaterialTint(value);

	}

	public override void SetMaterialDissolve(float value)
	{
		headCostume?.SetMaterialDissolve(value);
		bodyCostume?.SetMaterialDissolve(value);
		weaponCostume?.SetMaterialDissolve(value);

	}

	public override void SyncAnimation()
	{
		//var stateinfo = unitAnimation.animator.GetCurrentAnimatorStateInfo(0);

		headCostume?.SetRootBone(rootBone);
		bodyCostume?.SetRootBone(rootBone);
		weaponCostume?.SetRootBone(rootBone);

		//unitAnimation.UpdateRenderer();
	}
}
