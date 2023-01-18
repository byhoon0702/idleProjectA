using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

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
	public const int intChar = 'A';
	public const string regex = @"[A-Z]";
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
	public IdleNumber(double value)
	{
		NormalizeSelf(value);
	}

	public string GetUnit_ABC()
	{
		string unit = "";
		int magnitude = Exp;

		// 4승부터 A로 표기
		if (Exp == 0)
		{
			return "";
		}

		var unitInt = magnitude;
		var secondUnit = unitInt % (amountofletter);
		var firstUnit = (unitInt - 1) / amountofletter;

		int char_a = intChar;

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

		int char_a = intChar;

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
		if (Value == 0)
		{
			return 0;
		}
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

	private void NormalizeSelf(double value)
	{
		if (value > 0)
		{
			while (value >= tencubed)
			{
				value /= ten;
				Exp += 1;
			}
		}
		Value = value;
	}

	public IdleNumber Normalize(long value)
	{
		return Normalize((double)value);
	}
	public IdleNumber Normalize(double value)
	{

		IdleNumber normalize = new IdleNumber();

		if (value > 0)
		{
			while (value >= tencubed)
			{
				value /= ten;
				normalize.Exp += 1;
			}
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
			if (left.Value != 0)
			{
				left.Value = left.Value / Mathf.Pow(10, diff);
			}
		}
		else
		{
			var diff = left.Exp - right.Exp;
			if (right.Value != 0)
			{
				right.Value = right.Value / Mathf.Pow(10, diff);
			}
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
			if (left.Value != 0)
			{
				left.Value = left.Value / Mathf.Pow(10, diff);
			}
			big_number = right;
		}
		else
		{
			var diff = left.Exp - right.Exp;
			if (right.Value != 0)
			{
				right.Value = right.Value / Mathf.Pow(10, diff);
			}
			big_number = left;
		}
	}

	public static explicit operator IdleNumber(string value)
	{
		IdleNumber idleNumber = new IdleNumber();
		double parseValue = 0;
		if (double.TryParse(value, out parseValue))
		{
			idleNumber.Normalize(parseValue);
		}
		else
		{

			value = value.ToUpper();
			string numbers = Regex.Replace(value, @"[A-Z]", "");
			string unit = value.Replace(numbers, "");
			//unit = unit.ToUpper();

			if (double.TryParse(numbers, out parseValue) == false)
			{
				VLog.LogError("숫자가 없는 문자열");
				return idleNumber;
			}

			int[] resultExponential = new int[unit.Length];

			int converted = intChar - 1;
			for (int i = 0; i < unit.Length; i++)
			{
				int diff = Convert.ToInt32(unit[i]) - converted;
				resultExponential[i] = diff;
			}
			int sum = 1;

			if (resultExponential.Length > 1)
			{
				sum = (resultExponential[0] * amountofletter) + resultExponential[1];
			}
			else
			{
				sum = resultExponential[0];
			}

			idleNumber.Value = parseValue;
			idleNumber.Exp = sum;
		}
		return idleNumber;
	}

	public static explicit operator IdleNumber(int value)
	{
		IdleNumber idlenumber = new IdleNumber(value);
		return idlenumber;
	}
	public static explicit operator IdleNumber(float value)
	{
		IdleNumber idlenumber = new IdleNumber(value);
		return idlenumber;
	}

	public static explicit operator IdleNumber(double value)
	{
		IdleNumber idlenumber = new IdleNumber(value);
		return idlenumber;
	}

	public static explicit operator IdleNumber(long value)
	{
		IdleNumber idlenumber = new IdleNumber(value);
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

	public static bool NullCheckAndThrow(IdleNumber _idleNumber)
	{
		if (_idleNumber is null)
		{
			return true;
		}

		return false;
	}

	#region operator double 
	public static bool operator <=(IdleNumber a, double b)
	{
		if (NullCheckAndThrow(a))
		{
			return false;
		}

		if (b == 0)
		{
			return a.Value <= 0;
		}
		IdleNumber right = new IdleNumber();
		right = right.Normalize(b);
		return a <= right;
	}
	public static bool operator >=(IdleNumber a, double b)
	{
		if (b == 0)
		{
			return a.Value >= 0;
		}
		IdleNumber right = new IdleNumber();
		right = right.Normalize(b);
		return a >= right;
	}
	public static bool operator >(IdleNumber a, double b)
	{
		if (b == 0)
		{
			return a.Value > 0;
		}
		IdleNumber right = new IdleNumber();
		right = right.Normalize(b);
		return a > right;
	}
	public static bool operator <(IdleNumber a, double b)
	{
		if (b == 0)
		{
			return a.Value < 0;
		}
		IdleNumber right = new IdleNumber();
		right = right.Normalize(b);
		return a < right;
	}
	public static bool operator ==(IdleNumber a, double b)
	{
		if (b == 0)
		{
			return a.Value == 0;
		}
		IdleNumber right = new IdleNumber();
		right = right.Normalize(b);
		return a == right;
	}
	public static bool operator !=(IdleNumber a, double b)
	{
		if (b == 0)
		{
			return a.Value != 0;
		}
		IdleNumber right = new IdleNumber();
		right = right.Normalize(b);
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
		if (a.IsNull())
		{
			return b.IsNull();
		}

		int comparison = CompareTo(a, b);

		return comparison == 0;
	}
	public static bool operator !=(IdleNumber a, IdleNumber b)
	{
		if (a.IsNull())
		{
			return b.IsNull() == false;
		}

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
		if (a.Value == 0 || b == 0)
		{
			Debug.LogError("Can not divide Zero");
			return a;
		}
		IdleNumber result = new IdleNumber(a);
		result.Value /= b;
		return result;
	}


	#endregion



}
