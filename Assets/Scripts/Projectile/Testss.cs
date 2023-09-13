using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Testss : MonoBehaviour
{

	public Transform target;
	public Transform left;
	public Transform right;

	public float angle;
	public int seperate;

	void Start()
	{

	}

	private void Update()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Vector3 angled = direction.GetAngledVector3(angle);
		left.position = angled.normalized;

		angled = direction.GetAngledVector3(angle, true);
		right.position = angled.normalized;
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Vector3 direction = (target.position - transform.position).normalized;
		float sep_angle = angle / Mathf.Max(1, seperate - 1);
		for (int i = 0; i < seperate; i += 2)
		{
			if (i + 1 < seperate)
			{
				Vector3 angled = direction.GetAngledVector3(angle - (sep_angle * i));
				Gizmos.DrawLine(transform.position, angled);
				angled = direction.GetAngledVector3(angle - (sep_angle * i), true);
				Gizmos.DrawLine(transform.position, angled);
			}
			else
			{
				Vector3 angled = direction.GetAngledVector3(0);
				Gizmos.DrawLine(transform.position, angled);
			}
		}
	}
}
