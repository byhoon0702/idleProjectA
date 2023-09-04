using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Testss : MonoBehaviour
{
	public UnitAnimation a;
	public UnitAnimation b;

	public Transform s;
	void Start()
	{

	}

	public void OnTest()
	{

		float distA = a.collider2d.bounds.SqrDistance(s.position);
		float distB = b.collider2d.bounds.SqrDistance(s.position);
		Debug.Log($"A : {distA} / B : {distB}");


	}


}
