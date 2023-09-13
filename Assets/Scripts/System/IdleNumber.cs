using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;


[Serializable]
public struct IdleNumber
{
	public double Value;
	public int Exp;
	public const int tencubed = 1000;
	public const int ten = 10;
	public const int position = 3;
	public const int amountofletter = 26;
	public const int intChar = 'A';
	public const string regex = @"[A-Z]";

	public IdleNumber(IdleNumber idlenumber)
	{
		Value = idlenumber.Value;
		Exp = idlenumber.Exp;
	}

	public IdleNumber(double value)
	{
		Value = value;
		Exp = 0;
		NormalizeSelf();
	}

	public void Reset()
	{
		Value = 0;
		Exp = 0;
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
			unit = $"{Convert.ToChar(unitInt + char_a - 1)}";
		}

		return unit;
	}

	//public string GetUnit()
	//{
	//	string unit = "";
	//	int magnitude = Exp / position;
	//	if (magnitude < IndexToMagnitude.Length)
	//	{
	//		return unit = IndexToMagnitude[magnitude];
	//	}

	//	var unitInt = magnitude - IndexToMagnitude.Length;
	//	var secondUnit = unitInt % amountofletter;
	//	var firstUnit = unitInt / amountofletter;

	//	int char_a = intChar;

	//	unit = $"{Convert.ToChar(firstUnit + char_a)}{Convert.ToChar(secondUnit + char_a)}";

	//	return unit;
	//}

	/// <summary>
	/// 해당 함수로 10의 25승 이상의 값을 호출 할 시 앱이 멈출 수 있기 때문에 가급적 사용하지 않는다.
	/// </summary>
	/// <returns></returns>
	public double GetValue()
	{
		var value = Value;
		value = value * Mathf.Pow(10, Exp);
		if (double.IsInfinity(value))
		{
			value = double.MaxValue;
		}
		return value;
	}
	public float GetValueFloat()
	{
		double value = GetValue();
		if (value >= float.MaxValue)
		{
			return float.MaxValue;
		}
		return (float)value;
	}

	public int GetValueToInt()
	{
		double value = GetValue();
		if (value >= int.MaxValue)
		{
			return int.MaxValue;
		}
		return (int)value;
	}

	public void Check()
	{
		if (Value == 0)
		{
			Exp = 0;
		}
	}
	public void Turncate()
	{
		if (Exp > 2)
		{
			return;
		}

		Exp = 0;
		Value = Math.Min(Value * Mathf.Pow(10, Exp), double.MaxValue);
	}

	public long GetValueToLong()
	{
		double value = GetValue();
		if (value >= long.MaxValue)
		{
			return long.MaxValue;
		}
		return (long)value;
	}
	public string ToFloatingString()
	{
		IdleNumber a = new IdleNumber(this);
		a.NormalizeSelf();
		a.Turncate();
		double turncateValue = a.Value;
		string unit = a.GetUnit_ABC();

		int left = Exp % 3;
		return $"{turncateValue * Mathf.Pow(10, left):0.##}{unit}";
	}

	public override string ToString()
	{
		IdleNumber a = new IdleNumber(this);
		a.NormalizeSelf();
		a.Turncate();
		double turncateValue = a.Value;
		string unit = a.GetUnit_ABC();

		//if (format.IsNullOrEmpty() == false)
		//{
		//	return $"{string.Format(format, Math.Floor(turncateValue))}{unit}";
		//}

		if (Exp >= 3)
		{
			int left = Exp % 3;
			return $"{turncateValue * Mathf.Pow(10, left):0.##}{unit}";
		}
		else
		{
			return $"{Math.Floor(turncateValue)}{unit}";
		}


	}

	public void NormalizeSelf()
	{

		if (double.IsInfinity(Value))
		{
			Value = long.MaxValue;
		}
		if (Value < 0)
		{
			Exp = 0;
		}
		else if (Value < 1)
		{
			if (Exp >= position)
			{
				Exp -= position;
				Value *= 1000;
			}
		}
		else
		{
			int left = Exp % position;
			if (left > 0)
			{
				Value *= Mathf.Pow(10, left);
			}
			Exp -= left;
			while (Value >= tencubed)
			{
				Value /= tencubed;
				Exp += position;
			}
		}
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
				normalize.Exp += position;
			}
		}
		normalize.Value = value;
		return normalize;
	}

	public static int CompareTo(IdleNumber left, IdleNumber right)
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

	#region operator
	public static explicit operator IdleNumber(string value)
	{
		if (value.IsNullOrEmpty())
		{
			return new IdleNumber();
		}

		IdleNumber idleNumber = new IdleNumber();
		double parseValue = 0;

		int exp = 0;

		if (value.Length > 7)
		{
			exp = value.Length - 7;
			value = value.Remove(7);
		}

		if (double.TryParse(value, out parseValue))
		{
			idleNumber = idleNumber.Normalize(parseValue);
			idleNumber.Exp += exp;
		}
		else
		{
			value = value.ToUpper();
			string numbers = Regex.Replace(value, @"[A-Z]", "");
			string unit = value.Replace(numbers, "");

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
			idleNumber.Exp = sum * position;
		}
		return idleNumber;
	}

	public static implicit operator float(IdleNumber number)
	{
		return number.GetValueFloat();
	}

	public static implicit operator double(IdleNumber number)
	{
		return number.GetValue();
	}

	//public static implicit operator IdleNumber(int value)
	//{
	//	IdleNumber idlenumber = new IdleNumber(value);
	//	return idlenumber;
	//}

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
	#endregion


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

	public static IdleNumber operator %(IdleNumber a, IdleNumber b)
	{
		IdleNumber result = Calculate(a, b, '%');
		return result;
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

		result.NormalizeSelf();
		return result;
	}

	public static IdleNumber operator /(IdleNumber a, double b)
	{
		if (a.Value == 0 || b == 0)
		{
			//Debug.LogWarning("Can not divide by Zero");
			return a;
		}
		IdleNumber result = new IdleNumber(a);
		result.Value /= b;
		result.NormalizeSelf();
		return result;
	}



	#endregion
	private static IdleNumber Calculate(IdleNumber a, IdleNumber b, char operator_symbol)
	{
		a.NormalizeSelf();
		b.NormalizeSelf();
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

					}
					result.Exp = left.Exp + right.Exp;

					//AligningIdleNumber 함수에서 낮은 승수의 값을 소수점으로 변경시키기 때문에 결과에 최종 승수 만큼 곱한다.
					result.Value = (left.Value * right.Value) * diff;
				}
				break;
			case '/':
				{
					result.Value = left.Value / right.Value;
					result.Exp = 0;
					result.NormalizeSelf();
				}
				break;
			case '%':
				{
					result.Value = left.Value % right.Value;
					result.Exp = 0;
					result.NormalizeSelf();
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

	public static bool TryConvert(string value, out IdleNumber result)
	{
		result = (IdleNumber)0;
		if (value.IsNullOrEmpty())
		{
			return false;
		}

		IdleNumber idleNumber = new IdleNumber();
		double parseValue = 0;

		int exp = 0;

		if (value.Length > 7)
		{
			exp = value.Length - 7;
			value = value.Remove(7);
		}

		if (double.TryParse(value, out parseValue))
		{
			idleNumber = idleNumber.Normalize(parseValue);
			idleNumber.Exp += exp;
		}
		else
		{
			value = value.ToUpper();
			string numbers = Regex.Replace(value, @"[A-Z]", "");
			string unit = value.Replace(numbers, "");

			if (double.TryParse(numbers, out parseValue) == false)
			{
				VLog.LogError("숫자가 없는 문자열");
				return false;
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
			idleNumber.Exp = sum * position;
		}
		result = idleNumber;
		return false;
	}

	public override bool Equals(object obj)
	{
		return obj is IdleNumber number &&
			   Value == number.Value &&
			   Exp == number.Exp;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Value, Exp);
	}
}

public class IdleRandom
{
	public static IdleNumber Random(IdleNumber min, IdleNumber max)
	{

		if (min > max)
		{
			Debug.LogWarning("min bigger than max");
			return min;
		}


		IdleNumber differ = max - min;

		if (differ.Exp > 10)
		{
			Debug.LogWarning("Range is Too Big");
			return min;
		}

		var randomValue = UnityEngine.Random.Range(0, differ.GetValueFloat());

		IdleNumber value = new IdleNumber(min + randomValue);
		value.Turncate();
		return value;
	}
}
