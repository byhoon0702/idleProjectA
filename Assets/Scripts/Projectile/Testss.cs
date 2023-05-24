using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testss : MonoBehaviour
{
	public new Rigidbody rigidbody;
	public float power;
	public FieldItem fieldItem;
	//public string a;
	//public string b;

	public UITalkBubble talkbuble;
	//// Start is called before the first frame update
	void Start()
	{
		//talkbuble.Show("alsjdflkajsdlfgjalsjdflahgalsjdfljalsdjflalsjdflkajsdlfgjalsjdflahgalsjdfljalsdjflaalsjdflkajsdlfgjalsjdflahgalsjdfljalsdjflalsjdflkajsdlfgjalsjdflahgalsjdfljalsdjflaaa");
		//float time = 0;
		//string a = "1 - ( 2.2 + 3 )";
		//string b = "1-(2.2+ 3)";

		//time = Time.realtimeSinceStartup;
		//double d = FourArithmeticCalculator.CalculateFourArithmetics(b);
		//Debug.Log($"{b}={d}, {Time.realtimeSinceStartup - time}");

		//time = Time.realtimeSinceStartup;
		//double c = FourArithmeticCalculator.CalculateFourArithmetic(a);

		//Debug.Log($"{a}={c}, {Time.realtimeSinceStartup - time}");


	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	rigidbody.AddForce(Vector3.left * power);
		//}
		//if (Input.GetKeyDown(KeyCode.D))
		//{
		//	rigidbody.AddForce(Vector3.right * power);
		//}
		if (Input.GetKeyDown(KeyCode.D))
		{
			var field = Instantiate(fieldItem);
			field.Appear(0, Vector3.zero, null);
		}
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
