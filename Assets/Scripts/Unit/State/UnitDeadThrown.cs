
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Unit Dead Thrown", menuName = "Unit State/Dead Action/Thrown", order = 1)]
public class UnitDeadThrown : UnitDeadStateAction
{
	private const float gravity = 9.8f;
	public float angle;
	public float speed;
	public float rotationSpeed;

	private float velocityX;
	private float vy;
	public override void OnEnter(UnitBase unitBase)
	{
		velocityX = Mathf.Sqrt(speed) * Mathf.Cos(angle * Mathf.Deg2Rad);
		vy = Mathf.Sqrt(speed) * Mathf.Sin(angle * Mathf.Deg2Rad);

		unitBase.unitAnimation.SwitchShadow(false);
	}

	public override void OnUpdate(UnitBase unitBase, float delta, float elapsedTime)
	{
		unitBase.unitAnimation.CenterPivot.Rotate(Vector3.forward, angle * delta * rotationSpeed, Space.Self);

		Vector3 calculateVector = Vector3.right;
		calculateVector.x = velocityX;
		calculateVector.y = (vy - (gravity * elapsedTime));
		//calculateVector.z = projectile.velocityXZ.z;

		unitBase.unitAnimation.transform.parent.Translate(calculateVector * delta);
	}
}
