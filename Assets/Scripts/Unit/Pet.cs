using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class Pet : MonoBehaviour
{
	public PlayerUnit follow;
	public int index;
	public float speed = 5f;
	public UnitAnimation unitAnimation;
	public new Rigidbody2D rigidbody;
	protected virtual void FixedUpdate()
	{
		Move();
	}

	public void Spawn(PetSlot petSlot, int _index, PlayerUnit _follow)
	{
		index = _index;
		follow = _follow;
		GameObject go = Instantiate(petSlot.item.PetObject, transform);
		Vector3 pos = new Vector3(index * -0.8f, 0, 0);
		if (follow != null)
		{
			pos += follow.position;
		}
		transform.position = pos;
		transform.localScale = Vector3.one;

		Camera sceneCam = SceneCamera.it.sceneCamera;
		go.transform.LookAt(go.transform.position + sceneCam.transform.rotation * Vector3.forward, sceneCam.transform.rotation * Vector3.up);
		UnitAnimation petAnimation = go.GetComponent<UnitAnimation>();
		petAnimation.Init();
		unitAnimation = petAnimation;
		rigidbody = GetComponent<Rigidbody2D>();

		speed = Random.Range(1f, 3f);
	}


	void Move()
	{
		if (follow == null)
		{
			return;
		}
		Vector3 targetPos = follow.transform.position + (follow.headingDirection * -1 * (index * 0.5f));


		Vector3 myPos = transform.position;


		if (targetPos.x - myPos.x > 0.1f)
		{
			unitAnimation.transform.localScale = new Vector3(1, 1, 1);
		}
		else if (targetPos.x - myPos.x < -0.1f)
		{
			unitAnimation.transform.localScale = new Vector3(-1, 1, 1);
		}

		if (Vector3.Distance(targetPos, myPos) > 0.05f)
		{
			rigidbody.MovePosition(myPos + (targetPos - myPos).normalized * Time.deltaTime * speed);
			unitAnimation.PlayAnimation(StateType.MOVE);
		}
		else
		{
			rigidbody.position = Vector3.Lerp(myPos, targetPos, 0.05f);
			unitAnimation.PlayAnimation(StateType.IDLE);
		}


	}
}
