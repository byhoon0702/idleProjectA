using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;


[RequireComponent(typeof(UnitAnimation))]
public class UnitCostume : MonoBehaviour
{

	public CostumeItemObject head;
	public CostumeItemObject body;
	public CostumeItemObject weapon;

	public Transform headRoot;
	public Transform bodyRoot;

	public Transform weaponRoot;

	[SerializeField] private CostumeSkin headCostume;
	[SerializeField] private CostumeSkin bodyCostume;
	[SerializeField] private CostumeSkin weaponCostume;

	private UnitAnimation unitAnimation;

	public Transform rootBone;
	private AnimatorOverrideController overrideController;
	//public SpriteResolver[] spriteResolver;

	//public SpriteLibrary[] spriteLibrary;
	//public SpriteLibraryAsset asset;


	//public void Init()
	//{
	//	spriteResolver = transform.GetComponentsInChildren<SpriteResolver>(true);
	//	spriteLibrary = transform.GetComponentsInChildren<SpriteLibrary>(true);
	//}


	//public void ChangeLibrary()
	//{
	//	for (int i = 0; i < spriteLibrary.Length; i++)
	//	{
	//		spriteLibrary[i].spriteLibraryAsset = asset;
	//	}

	//	for (int i = 0; i < spriteResolver.Length; i++)
	//	{
	//		spriteResolver[i].SetCategoryAndLabel(spriteResolver[i].GetCategory(), spriteResolver[i].GetLabel());

	//		var sprite = spriteResolver[i].GetComponent<SpriteRenderer>().sprite;
	//		if (sprite == null)
	//		{
	//			continue;
	//		}
	//		SpriteSkin skin = spriteResolver[i].GetComponent<SpriteSkin>();

	//		FindBone(sprite, skin);
	//	}
	//}



	public void Init()
	{
		unitAnimation = GetComponent<UnitAnimation>();
	}
	public void ChangeCostume()
	{
		var headinfo = VGameManager.it.userDB.costumeContainer[CostumeType.HEAD];
		var bodyinfo = VGameManager.it.userDB.costumeContainer[CostumeType.BODY];
		var weaponinfo = VGameManager.it.userDB.costumeContainer[CostumeType.WEAPON];

		ChangeCostume(headinfo.item, bodyinfo.item, weaponinfo.item);
	}

	public void ChangeCostume(long tid, CostumeType type, bool isMasked = false)
	{
		var info = VGameManager.it.userDB.inventory.FindCostumeItem(tid, type);

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

	public void ChangeCostume(RuntimeData.CostumeInfo _head, RuntimeData.CostumeInfo _body, RuntimeData.CostumeInfo _weapon)
	{
		head = _head.itemObject;
		body = _body.itemObject;
		weapon = _weapon.itemObject;

		ChangeHead();
		ChangeBody();
		ChangeWeapon();

		SyncAnimation();
	}

	public void ChangeHead(bool isMasked = false)
	{
		if (head == null)
		{
			return;
		}

		if (headCostume != null)
		{
			if (headCostume.name == head.CostumeObject.name)
			{
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

		if (isMasked)
		{
			headCostume.SetMaterialMask(new Color32(44, 44, 44, 255));
		}
	}

	public void ChangeBody(bool isMasked = false)
	{
		if (body == null)
		{
			return;
		}
		if (bodyCostume != null)
		{
			if (bodyCostume.name == body.CostumeObject.name)
			{
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


		if (isMasked)
		{
			bodyCostume.SetMaterialMask(new Color32(44, 44, 44, 255));
		}
	}

	public void ChangeWeapon(bool isMasked = false)
	{
		if (weapon == null)
		{
			return;
		}
		if (weaponCostume != null)
		{
			if (weaponCostume.name == weapon.CostumeObject.name)
			{
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

		if (isMasked)
		{
			weaponCostume.SetMaterialMask(new Color32(44, 44, 44, 255));
		}


		if (weapon.OverrideAnimator != null)
		{
			overrideController = weapon.OverrideAnimator;
			unitAnimation.animator.runtimeAnimatorController = overrideController;
		}
		else
		{
			if (overrideController != null)
			{
				unitAnimation.animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
				overrideController = null;
			}
		}
	}



	private void DestroyObject(GameObject obj)
	{
		if (Application.isPlaying)
		{
			Destroy(obj);
		}
		else
		{
			DestroyImmediate(obj);
		}
	}
	public void SyncAnimation()
	{
		//var stateinfo = unitAnimation.animator.GetCurrentAnimatorStateInfo(0);

		headCostume?.SetRootBone(rootBone);
		bodyCostume?.SetRootBone(rootBone);
		weaponCostume?.SetRootBone(rootBone);
	}
}
