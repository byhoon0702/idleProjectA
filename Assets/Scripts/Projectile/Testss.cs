using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testss : MonoBehaviour
{
	//public string a;
	//public string b;

	public Transform a;
	public Transform b;
	// Start is called before the first frame update
	void Start()
	{

	}

	private void Update()
	{
		Debug.Log(Vector3.Distance(a.position, b.position));
	}

	//private void OnGUI()
	//{
	//	a = GUI.TextField(new Rect(200, 100, 100, 100), a);
	//	b = GUI.TextField(new Rect(200, 200, 100, 100), b);
	//	if (GUI.Button(new Rect(200, 300, 100, 100), "dddd"))
	//	{
	//		Calculate();
	//	}
	//}

	//private void Calculate()
	//{
	//	IdleNumber ia = (IdleNumber)a;
	//	IdleNumber ib = (IdleNumber)b;


	//	IdleNumber c = ia * ib;
	//	Debug.Log(c.ToString());
	//}
}
