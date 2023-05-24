using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using System.Runtime.CompilerServices;

public static class FourArithmeticCalculator
{
	public const string DEFAULT_VALUE = "def";
	public const string SKILL_LEVEL = "lv";

	/// <summary>
	/// 예약어들을 사칙연산으로 사용할 수 있게 변환
	/// </summary>
	/// <returns></returns>
	public static string ReplaceReservedWord(string _data, float _defaultValue, Int32 _skillLv)
	{
		string outResult = _data;

		outResult = outResult.Replace(DEFAULT_VALUE, _defaultValue.ToString());
		outResult = outResult.Replace(SKILL_LEVEL, _skillLv.ToString());

		return outResult;
	}

	/// <summary>
	/// 사칙연산(사용전에 Replace로 변환 한번 해주셔야 합니다)
	/// </summary>
	public static double CalculateFourArithmetic(string _data)
	{
		double result = FourArithmeticOperations.Calculate(_data);
		return result;
	}

	/// <summary>
	/// 스페이스 무시하는 계산식
	/// </summary>

	public static IdleNumber CalculateFourArithmetics(string _data)
	{
		if (_data.IsNullOrEmpty())
		{
			return new IdleNumber(0);
		}
		List<string> tokens = FourArithmeticOperations.Seperate(_data);

		IdleNumber result = FourArithmeticOperations.Calculate(tokens.ToArray());
		return result;
	}


	public static double Calculate(string _data, float _defaultValue, Int32 _skillLv)
	{
		if (string.IsNullOrEmpty(_data))
		{
			return _defaultValue;
		}


		string replaceAttackPower = FourArithmeticCalculator.ReplaceReservedWord(_data, _defaultValue, _skillLv);
		double result = FourArithmeticCalculator.CalculateFourArithmetic(replaceAttackPower);

		return result;
	}




	/// <summary>
	/// 사칙연산 계산기
	/// </summary>
	private static class FourArithmeticOperations
	{
		private static List<string> allockList = new List<string>(10);
		private static Stack<string> allockStack = new Stack<string>(10);
		private static string[] operatorList = new string[] { "+", "-", "*", "/", "(", ")" };
		private static Dictionary<string, double> precs = new Dictionary<string, double>
		{
			["*"] = 2,
			["/"] = 2,
			["+"] = 1,
			["-"] = 1,
			["("] = 0,
		};

		public static IdleNumber Calculate(string[] _tokens)
		{
			ConvertToPostFix(_tokens);
			IdleNumber outResult = CalculateIdleNumber();

			return outResult;
		}


		public static double Calculate(string _data)
		{
			ConvertToPostFix(_data);

			double outResult = Calculate();

			return outResult;
		}
		private static IdleNumber CalculateIdleNumber()
		{
			Stack<IdleNumber> stack = new Stack<IdleNumber>();

			try
			{
				foreach (string token in allockList)
				{
					switch (token)
					{
						case "+":
							stack.Push(stack.Pop() + stack.Pop());
							break;

						case "-":
							stack.Push((stack.Pop() - stack.Pop()) * -1);
							break;

						case "*":
							stack.Push(stack.Pop() * stack.Pop());
							break;

						case "/":
							IdleNumber rv = stack.Pop();
							stack.Push(stack.Pop() / rv);
							break;

						default:
							stack.Push((IdleNumber)token);
							break;
					}
				}
				return stack.Pop();
			}
			catch
			{
				return new IdleNumber();
			}
		}
		private static double Calculate()
		{
			Stack<double> stack = new Stack<double>();

			try
			{
				foreach (string token in allockList)
				{
					switch (token)
					{
						case "+":
							stack.Push(stack.Pop() + stack.Pop());
							break;

						case "-":
							stack.Push(-(stack.Pop() - stack.Pop()));
							break;

						case "*":
							stack.Push(stack.Pop() * stack.Pop());
							break;

						case "/":
							double rv = stack.Pop();
							stack.Push(stack.Pop() / rv);
							break;

						default:
							stack.Push((IdleNumber)token);
							break;
					}
				}
				return stack.Pop();
			}
			catch
			{
				return 0;
			}
		}

		public static List<string> Seperate(string cal)
		{
			List<string> ss = new List<string>();

			string number = "";
			for (int i = 0; i < cal.Length; i++)
			{
				//스페이스 바는 무시
				if (cal[i] == ' ')
				{
					continue;
				}

				if ((cal[i] >= '0' && cal[i] <= '9')
					|| (cal[i] >= 'A' && cal[i] <= 'Z')
					|| cal[i] == '.')
				{
					//문자를 저장 , 연속된 문자가 숫자 또는 '.' 일 경우는 계속해서 이어준다.
					number += cal[i].ToString();
				}
				else
				{
					//문자가 연산자일 경우 이전에 저장된 문자열(숫자)을 리스트에 먼저 추가 후 연산자를 추가한다.
					if (number.IsNullOrEmpty() == false)
					{
						ss.Add(number);
					}
					number = cal[i].ToString();
					ss.Add(number);
					number = "";
				}

				if (i == cal.Length - 1)
				{
					if (number.IsNullOrEmpty() == false)
					{
						ss.Add(number);
					}
				}
			}

			return ss;
		}
		private static void ConvertToPostFix(string[] _tokens)
		{
			allockList.Clear();
			allockStack.Clear();


			// 1. 수식을 각 토큰별로 구분하여 읽어들인다
			string[] tokens = _tokens;

			foreach (string item in tokens)
			{
				if (operatorList.Contains(item) == false)
				{
					// 2. 토큰이 피 연산자이면 출력 리스트에 넣는다.
					allockList.Add(item);
				}
				else if (item == "(")
				{
					// 3. 토큰이 왼쪽 괄호이면 스택에 푸시한다.
					allockStack.Push(item);
				}
				else if (item == ")")
				{
					// 5. 토큰이 오른쪽 괄호이면 왼쪽 괄호가 나올 때까지 스택에서 팝하여 순서대로 리스트에 넣는다.
					while (allockStack.Peek() != "(")
					{
						allockList.Add(allockStack.Pop());
					}

					// 왼쪽 괄호 자체는 버린다.
					allockStack.Pop();
				}
				else
				{
					// 4. 토큰이 연산자이면, 
					while (allockStack.Count != 0)
					{
						if (precs[allockStack.Peek()] >= precs[item])
						{
							// 스택에 있는 연산자의 우선 순위가 자신보다 높거나 같다면 출력 리스트에 이어 붙여준다.
							allockList.Add(allockStack.Pop());
						}
						else
						{
							break;
						}
					}

					allockStack.Push(item);
				}
			}

			// 6. 더 이상 읽을 토큰이 없다면, 스택에서 연산자를 팝하여 붙인다.
			while (allockStack.Count != 0)
			{
				allockList.Add(allockStack.Pop());
			}
		}
		private static void ConvertToPostFix(string _expStr)
		{
			allockList.Clear();
			allockStack.Clear();


			// 1. 수식을 각 토큰별로 구분하여 읽어들인다
			string[] tokens = _expStr.Split(' ');

			foreach (string item in tokens)
			{
				if (operatorList.Contains(item) == false)
				{
					// 2. 토큰이 피 연산자이면 출력 리스트에 넣는다.
					allockList.Add(item);
				}
				else if (item == "(")
				{
					// 3. 토큰이 왼쪽 괄호이면 스택에 푸시한다.
					allockStack.Push(item);
				}
				else if (item == ")")
				{
					// 5. 토큰이 오른쪽 괄호이면 왼쪽 괄호가 나올 때까지 스택에서 팝하여 순서대로 리스트에 넣는다.
					while (allockStack.Peek() != "(")
					{
						allockList.Add(allockStack.Pop());
					}

					// 왼쪽 괄호 자체는 버린다.
					allockStack.Pop();
				}
				else
				{
					// 4. 토큰이 연산자이면, 
					while (allockStack.Count != 0)
					{
						if (precs[allockStack.Peek()] >= precs[item])
						{
							// 스택에 있는 연산자의 우선 순위가 자신보다 높거나 같다면 출력 리스트에 이어 붙여준다.
							allockList.Add(allockStack.Pop());
						}
						else
						{
							break;
						}
					}

					allockStack.Push(item);
				}
			}

			// 6. 더 이상 읽을 토큰이 없다면, 스택에서 연산자를 팝하여 붙인다.
			while (allockStack.Count != 0)
			{
				allockList.Add(allockStack.Pop());
			}
		}
	}
}
