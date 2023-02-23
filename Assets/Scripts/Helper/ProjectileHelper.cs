using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileHelper
{
	public static float FourPointBezierCurve(float _p1, float _p2, float _p3, float p4, float t)
	{
		float a = Mathf.Pow((1 - t), 3) * _p1;
		float b = Mathf.Pow((1 - t), 2) * 3 * t * _p2;
		float c = Mathf.Pow(t, 2) * 3 * (1 - t) * _p3;
		float d = Mathf.Pow(t, 3) * p4;

		return a + b + c + d;
	}
	public static float ThreePointBezierCurve(float _p1, float _p2, float _p3, float t)
	{
		float a = Mathf.Pow((1 - t), 2) * _p1;
		float b = (2 * (1 - t)) * t * _p2;
		float c = Mathf.Pow(t, 2) * _p3;

		return a + b + c;
	}
}


