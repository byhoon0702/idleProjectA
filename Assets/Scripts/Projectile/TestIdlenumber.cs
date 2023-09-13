using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdlenumber : MonoBehaviour
{

	public string _A;
	public double _B;

	public char symbol;
	public void OnCalculate()
	{
		IdleNumber A = (IdleNumber)_A;
		double B = _B;
		switch (symbol)
		{
			case '+':
				Debug.Log((A + B).ToString());
				break;
			case '-':
				Debug.Log((A - B).ToString());
				break;
			case '*':
				Debug.Log((A * B).ToString());
				break;
			case '/':
				Debug.Log((A / B).ToString());
				break;
			case '%':
				Debug.Log((A % B).ToString());
				break;

		}


	}
	// Start is called before the first frame update
	void Start()
	{


	}

	// Update is called once per frame
	void Update()
	{

	}
}
