using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

[Serializable]
public struct IdleNumber
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
	public const int ten = 10;
	public const int position = 3;
	public const int amountofletter = 26;
	public const int intChar = 'A';
	public const string regex = @"[A-Z]";
	//public IdleNumber()
	//{

	//}

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
		Value = 0;
		Exp = 0;
		NormalizeSelf(value);
	}

	public string GetUnit_ABC()
	{
		string unit = "";
		int magnitude = Exp / position;

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
	public float GetValueFloat()
	{
		return (float)GetValue();
	}

	public int GetValueToInt()
	{
		return (int)GetValue();
	}

	public long GetValueToLong()
	{
		return (long)GetValue();
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
			Value /= tencubed;
			Exp += 3;
		}
		return Value;
	}

	public new string ToString()
	{
		IdleNumber a = new IdleNumber(this);
		double turncateValue = a.GetTruncateValue();
		string unit = a.GetUnit_ABC();
		if (turncateValue >= 100)
		{
			return $"{Math.Floor(turncateValue)}{unit}";
		}
		else if (turncateValue >= 10)
		{
			return $"{Math.Floor(turncateValue * 10) / 10:0.#}{unit}";
		}
		else
		{
			return $"{Math.Floor(turncateValue * 100) / 100:0.##}{unit}";
		}
	}

	private void NormalizeSelf(double value)
	{
		if (value > 0)
		{
			while (value >= tencubed)
			{
				value /= tencubed;
				Exp += 3;
			}
		}
		else
		{
			Exp = 0;
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
				value /= tencubed;
				normalize.Exp += 3;
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
	private static IdleNumber AligningIdleNumber(ref IdleNumber left, ref IdleNumber right, out IdleNumber bigNumber, out IdleNumber smallNumber)
	{
		IdleNumber result = new IdleNumber();
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
				left.Value = left.Value / Mathf.Pow(ten, diff);
			}
			bigNumber = right;
			result.Exp = diff;
			smallNumber = left;
		}
		else
		{
			var diff = left.Exp - right.Exp;
			if (right.Value != 0)
			{
				right.Value = right.Value / Mathf.Pow(ten, diff);
			}
			bigNumber = left;
			result.Exp = diff;
			smallNumber = right;
		}
		return result;

	}

	public static explicit operator IdleNumber(string value)
	{
		if (value.IsNullOrEmpty())
		{
			return new IdleNumber();
		}

		IdleNumber idleNumber = new IdleNumber();
		double parseValue = 0;
		if (double.TryParse(value, out parseValue))
		{
			idleNumber = idleNumber.Normalize(parseValue);
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
			idleNumber.Exp = sum * 3;
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

		IdleNumber left = new IdleNumber(a);
		IdleNumber right = new IdleNumber(b);

		IdleNumber result = AligningIdleNumber(ref left, ref right, out IdleNumber big, out IdleNumber small);

		switch (operator_symbol)
		{
			case '+':
				result.Value = left.Value + right.Value;
				result.Exp = big.Exp;
				break;
			case '-':
				result.Value = left.Value - right.Value;
				result.Exp = big.Exp;
				break;
			case '*':
				{
					float diff = Mathf.Pow(ten, result.Exp);
					//같은 승수를 가지거나 승수가 없을 경우
					if (diff == 1)
					{
						//좌, 우의 승수를 더한다.
						result.Exp = left.Exp + right.Exp;
					}
					//AligningIdleNumber 함수에서 낮은 승수의 값을 소수점으로 변경시키기 때문에 결과에 최종 승수 만큼 곱한다.
					result.Value = (left.Value * right.Value) * diff;
				}
				break;
			case '/':
				{
					result.Value = left.Value / right.Value;
					result.Exp = 0;
					result.NormalizeSelf(result.Value);
				}
				break;
		}
		return result;
	}

	public static bool NullCheckAndThrow(IdleNumber _idleNumber)
	{
		if (_idleNumber.Equals(default))
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
		IdleNumber right = new IdleNumber(b);

		result += right;
		return result;
	}

	public static IdleNumber operator -(IdleNumber a, double b)
	{
		IdleNumber result = new IdleNumber(a);
		IdleNumber right = new IdleNumber(b);
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
			Debug.LogWarning("Can not divide by Zero");
			return a;
		}
		IdleNumber result = new IdleNumber(a);
		result.Value /= b;
		return result;
	}


	#endregion



}
