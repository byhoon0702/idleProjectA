
using UnityEngine;


[RequireComponent(typeof(UnitAnimation))]
public abstract class UnitCostume : MonoBehaviour
{


	protected UnitAnimation unitAnimation;
	public Transform rootBone;

	protected RuntimeAnimatorController runtimeAnimatorController;

	public abstract void Init();
	public abstract void ChangeCostume();
	public abstract void ChangeCostume(long tid, CostumeType type, bool isMasked = false);


	public abstract void SetMaterialTint(float value);

	public abstract void SetMaterialDissolve(float value);



	protected void DestroyObject(GameObject obj)
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
	public abstract void SyncAnimation();
}
