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
	public const int tencubed = 1000;
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
		while (Math.Abs(Value) > tencubed)
		{
			Value /= tencubed;
			Exp += 3;
		}
		return Value;
	}

	public new string ToString()
	{
		IdleNumber a = new IdleNumber(this);
		return string.Format("{0:0.##}", a.GetTruncateValue()) + a.GetUnit();
	}

	public IdleNumber Normalize(long value)
	{
		return Normalize((double)value);
	}
	public IdleNumber Normalize(double value)
	{
		IdleNumber normalize = new IdleNumber();

		while (value > tencubed)
		{
			value /= tencubed;
			normalize.Exp += 3;
		}
		normalize.Value = value;
		return normalize;
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
		IdleNumber big = new IdleNumber();

		AligningIdleNumber(left, right, out big);
		result.Exp = big.Exp;
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
	#region operator
	public static bool operator <=(IdleNumber a, double b)
	{
		return a.Value <= b;
	}
	public static bool operator >=(IdleNumber a, double b)
	{
		return a.Value >= b;
	}
	public static bool operator >(IdleNumber a, double b)
	{
		return a.Value > b;
	}
	public static bool operator <(IdleNumber a, double b)
	{
		return a.Value < b;
	}
	public static bool operator ==(IdleNumber a, double b)
	{
		return a.Value == b;
	}
	public static bool operator !=(IdleNumber a, double b)
	{
		return a.Value != b;
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
		result.Value *= Mathf.Pow(10, result.Exp);
		result.Value += b;
		result.Value /= Mathf.Pow(10, result.Exp);
		return result;
	}

	public static IdleNumber operator -(IdleNumber a, double b)
	{
		IdleNumber result = new IdleNumber(a);
		result.Value *= Mathf.Pow(10, result.Exp);
		result.Value -= b;
		result.Value /= Mathf.Pow(10, result.Exp);
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
