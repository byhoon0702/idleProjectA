using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IdleNumber
{
	public static readonly string[] IndexToMagnitude =
	{
		"",
		"K",
		"M",
		"B",
		"T"
	};

	public double Value;
	public int Exp;
	public const int tencubed = 10000;
	public const int ten = 10;
	public const int position = 3;
	public const int amountofletter = 26;
	public IdleNumber()
	{

	}
	public IdleNumber(IdleNumber idlenumber)
	{
		Value = idlenumber.Value;
		Exp = idlenumber.Exp;
	}
	public IdleNumber(double value, int exp)
	{
		Value = value;
		Exp = exp;
	}

	public string GetUnit_ABC()
	{
		string unit = "";
		int magnitude = Exp / 1;

		// 4승부터 A로 표기
		if (Exp == 0)
		{
			return "";
		}

		var unitInt = magnitude;
		var secondUnit = unitInt % (amountofletter);
		var firstUnit = (unitInt - 1) / amountofletter;

		int char_a = Convert.ToInt32('A');

		if (firstUnit != 0)
		{
			unit = $"{Convert.ToChar(firstUnit + char_a - 1)}{Convert.ToChar(secondUnit + char_a - 1)}";
		}
		else
		{
			unit = $"{Convert.ToChar(secondUnit + char_a - 1)}";
		}

		return unit;
	}

	public string GetUnit()
	{
		string unit = "";
		int magnitude = Exp / position;
		if (magnitude < IndexToMagnitude.Length)
		{
			return unit = IndexToMagnitude[magnitude];
		}

		var unitInt = magnitude - IndexToMagnitude.Length;
		var secondUnit = unitInt % amountofletter;
		var firstUnit = unitInt / amountofletter;

		int char_a = Convert.ToInt32('A');

		unit = $"{Convert.ToChar(firstUnit + char_a)}{Convert.ToChar(secondUnit + char_a)}";

		return unit;
	}

	/// <summary>
	/// 해당 함수로 10의 25승 이상의 값을 호출 할 시 앱이 멈출 수 있기 때문에 가급적 사용하지 않는다.
	/// </summary>
	/// <returns></returns>
	public double GetValue()
	{
		var value = Value;
		value = value * Mathf.Pow(10, Exp);

		return value;
	}

	private double GetTruncateValue()
	{
		Value = Value * Mathf.Pow(10, Exp);
		Exp = 0;
		while (Math.Abs(Value) >= tencubed)
		{
			Value /= ten;
			Exp += 1;
		}
		return Value;
	}

	public new string ToString()
	{
		IdleNumber a = new IdleNumber(this);
		return string.Format("{0:0.##}", a.GetTruncateValue()) + a.GetUnit_ABC();
	}

	public IdleNumber Normalize(long value)
	{
		return Normalize((double)value);
	}
	public IdleNumber Normalize(double value)
	{
		IdleNumber normalize = new IdleNumber();

		while (value >= tencubed)
		{
			value /= ten;
			normalize.Exp += 1;
		}
		normalize.Value = value;
		return normalize;
	}

	private static int CompareTo(IdleNumber left, IdleNumber right)
	{
		var comparison = left.Exp.CompareTo(right.Exp);
		if (comparison == 0)
		{
			comparison = left.Value.CompareTo(right.Value);

			return comparison;
		}

		if (comparison < 0)
		{
			var diff = right.Exp - left.Exp;
			left.Value = left.Value / Mathf.Pow(10, diff);
		}
		else
		{
			var diff = left.Exp - right.Exp;
			right.Value = right.Value / Mathf.Pow(10, diff);
		}
		comparison = left.Value.CompareTo(right.Value);

		return comparison;
	}
	private static void AligningIdleNumber(IdleNumber left, IdleNumber right, out IdleNumber big_number)
	{
		var comparison = left.Exp.CompareTo(right.Exp);
		if (comparison == 0)
		{
			comparison = left.Value.CompareTo(right.Value);
		}

		if (comparison < 0)
		{
			var diff = right.Exp - left.Exp;
			left.Value = left.Value / Mathf.Pow(10, diff);
			big_number = right;
		}
		else
		{
			var diff = left.Exp - right.Exp;
			right.Value = right.Value / Mathf.Pow(10, diff);
			big_number = left;
		}
	}

	public static explicit operator IdleNumber(double value)
	{
		IdleNumber idlenumber = new IdleNumber();
		idlenumber.Normalize(value);
		return idlenumber;
	}

	public static explicit operator IdleNumber(long value)
	{
		IdleNumber idlenumber = new IdleNumber();
		idlenumber.Normalize(value);
		return idlenumber;
	}

	private static IdleNumber Calculate(IdleNumber a, IdleNumber b, char operator_symbol)
	{
		IdleNumber result = new IdleNumber();
		IdleNumber left = new IdleNumber(a);
		IdleNumber right = new IdleNumber(b);


		AligningIdleNumber(left, right, out result);

		switch (operator_symbol)
		{
			case '+':
				result.Value = left.Value + right.Value;
				break;
			case '-':
				result.Value = left.Value - right.Value;
				break;
			case '*':
				result.Value = left.Value * right.Value;
				break;
			case '/':
				result.Value = left.Value / right.Value;
				break;
		}
		return result;
	}
	#region operator double 
	public static bool operator <=(IdleNumber a, double b)
	{
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		return a <= right;
	}
	public static bool operator >=(IdleNumber a, double b)
	{
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		return a >= right;
	}
	public static bool operator >(IdleNumber a, double b)
	{
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		return a > right;
	}
	public static bool operator <(IdleNumber a, double b)
	{
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		return a < right;
	}
	public static bool operator ==(IdleNumber a, double b)
	{
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		return a == right;
	}
	public static bool operator !=(IdleNumber a, double b)
	{
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		return a != right;
	}
	#endregion

	#region  operator compare
	public static bool operator <=(IdleNumber a, IdleNumber b)
	{
		int comparison = CompareTo(a, b);

		return comparison == 0 || comparison == -1;
	}
	public static bool operator >=(IdleNumber a, IdleNumber b)
	{
		int comparison = CompareTo(a, b);

		return comparison == 0 || comparison == 1;
	}
	public static bool operator >(IdleNumber a, IdleNumber b)
	{
		int comparison = CompareTo(a, b);

		return comparison == 1;
	}
	public static bool operator <(IdleNumber a, IdleNumber b)
	{
		int comparison = CompareTo(a, b);

		return comparison == -1;
	}
	public static bool operator ==(IdleNumber a, IdleNumber b)
	{
		int comparison = CompareTo(a, b);

		return comparison == 0;
	}
	public static bool operator !=(IdleNumber a, IdleNumber b)
	{
		int comparison = CompareTo(a, b);

		return comparison != 0;
	}

	public static IdleNumber operator +(IdleNumber a, IdleNumber b)
	{
		IdleNumber result = Calculate(a, b, '+');
		return result;
	}

	public static IdleNumber operator -(IdleNumber a, IdleNumber b)
	{
		IdleNumber result = Calculate(a, b, '-');
		return result;
	}
	public static IdleNumber operator *(IdleNumber a, IdleNumber b)
	{
		IdleNumber result = Calculate(a, b, '*');
		return result;
	}
	public static IdleNumber operator /(IdleNumber a, IdleNumber b)
	{
		IdleNumber result = Calculate(a, b, '/');
		return result;
	}

	public static IdleNumber operator +(IdleNumber a, double b)
	{
		IdleNumber result = new IdleNumber(a);
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);

		result += right;

		return result;
	}

	public static IdleNumber operator -(IdleNumber a, double b)
	{
		IdleNumber result = new IdleNumber(a);
		int quotient = (int)(b / 10);
		IdleNumber right = new IdleNumber(b / quotient, quotient);
		result -= right;
		return result;
	}

	public static IdleNumber operator *(IdleNumber a, double b)
	{
		IdleNumber result = new IdleNumber(a);
		result.Value *= b;
		return result;
	}

	public static IdleNumber operator /(IdleNumber a, double b)
	{
		IdleNumber result = new IdleNumber(a);
		result.Value /= b;
		return result;
	}


	#endregion



}
