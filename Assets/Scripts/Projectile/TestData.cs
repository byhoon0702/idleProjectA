using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestData : MonoBehaviour
{

	public Vector3 pos;
	public float angle;

	private void Start()
	{

	}

	private void Update()
	{
		float sin = Mathf.Sin(angle) * Mathf.Deg2Rad;
		Debug.Log($"{sin} , {pos.y * sin}");

	}
}
