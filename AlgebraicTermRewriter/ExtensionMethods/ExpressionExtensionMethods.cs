using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public static class ExpressionExtensionMethods_Checks
	{
		public static void CombineArithmeticTokens(this Expression source)
		{
			Tuple<int, int> range = source.Tokens.GetLongestArithmeticRange();

			if (range == null)
			{
				return;
			}

			List<IToken> arithmeticExpression = source.Tokens.GetRange(range.Item1, range.Item2);

			string toEvaluate = string.Join("", arithmeticExpression.Select(e => e.Contents));

			INumber newValue = new Number(InfixNotationEvaluator.Evaluate(toEvaluate));

			source.Tokens.RemoveRange(range.Item1, range.Item2);

			int insertIndex = range.Item1 == 0 ? 0 : range.Item1;

			if (newValue.Value == 0)
			{
				if (source.TokenCount == 0)
				{
					source.Tokens.Insert(0, new Number(0));
					return;
				}
				IToken op = source.TokenAt(insertIndex);
				if (op.Contents != "*")
				{
					source.Tokens.RemoveAt(insertIndex);
				}
			}
			else
			{
				source.Tokens.Insert(insertIndex, newValue);
			}
		}

		public static void Substitute(this Expression source, IVariable variable, IToken[] expression)
		{
			if (source.Tokens.Any(e => e.Contents == variable.Contents))
			{
				int index = -1;
				int tokenCount = source.TokenCount;
				while (index++ < tokenCount)
				{
					IToken currentToken = source.TokenAt(index);

					if (currentToken.Contents == variable.Contents)
					{
						source.Tokens.RemoveAt(index);
						source.Tokens.InsertRange(index, expression);
					}
				}
			}
		}

		public static bool OnlyArithmeticTokens(this Expression source)
		{
			return (!source.Variables.Any()
				&& source.Numbers.Any());
		}

		public static int RankComplexity(this Expression source)
		{
			int tokenCount = source.Terms.Count();

			if (tokenCount == 1)
			{
				TokenType singletonType = source.Tokens.First().Type;
				if (singletonType == TokenType.Number) return 1;
				else if (singletonType == TokenType.Variable) return 2;
			}
			else if (tokenCount == 2)
			{
				TokenType firstType = source.Tokens[0].Type;
				TokenType secondType = source.Tokens[1].Type;

				if (firstType == secondType)
				{
					if (firstType == TokenType.Number) return 1;
					else return 3;
				}
				else return 2;
			}
			else if (tokenCount == 3)
			{
				int variables = source.Variables.Count();
				if (variables >= 2) return 4;
				else return 3;
			}
			return 5;
		}

		public static TermOperatorPair Extract(this Expression source, ITerm term, IOperator op)
		{
			IOperator oper = op;

			int tIndex = source.Tokens.IndexOf(term);


			InsertOrientation orientation = InsertOrientation.Either;
			if (tIndex == 0)
			{
				if (op.Symbol != '+')
				{
					source.Tokens.Remove(op);
				}
				
				oper = Operator.GetInverse(oper);
				orientation = InsertOrientation.Right;
				
			}
			else
			{
				source.Tokens.Remove(op);
				oper = Operator.GetInverse(oper);
				orientation = InsertOrientation.Right;
			}

			source.Tokens.Remove(term);

			return new TermOperatorPair(term, oper, orientation);
		}

		public static void SetToMultiplicativeInverse2(this Expression source)
		{
			string expression = source.ToString()?.Replace(" ", "") ?? "";

			if (expression.StartsWith("0"))
			{
				source.Tokens.RemoveAt(0);
				source.Tokens.RemoveAt(0);
				return;
			}

			if (expression.StartsWith("-"))
			{
				source.Tokens.RemoveAt(0);
			}
			else
			{
				if (source.TokenCount > 0)
				{
					INumber num = source.Tokens[0] as INumber;
					if (num != null)
					{
						int newNum = -num.Value;
						source.Tokens[0] = new Number(newNum);
						return;
					}
				}
			}
		}

		public static void SetToMultiplicativeInverse(this Expression source)
		{
			bool isNegative = false;
			IToken first = null;
			IToken second = null;

			first = source.Tokens.FirstOrDefault();
			if (first != null && first.Contents == "0")
			{
				second = source.Tokens.Skip(1).FirstOrDefault();
				if (second != null && second.Contents == "-")
				{
					isNegative = true;
				}
			}


			if (!isNegative)
			{
				first = source.Tokens.FirstOrDefault();
				if (first != null && first.Contents == "-")
				{
					second = source.Tokens.Skip(1).FirstOrDefault();
					if (second != null && second is IVariable)
					{
						isNegative = true;
					}
				}
			}

			if (isNegative)
			{
				source.Tokens.Remove(first);
				source.Tokens.Remove(second);
			}

		}
	}



	public static class ExpressionExtensionMethods_Manipulations
	{
		public static IToken LeftOfToken(this Expression source, IToken token)
		{
			int index = source.Tokens.IndexOf(token);
			if (index == 0) return Token.None;
			else return source.TokenAt(index - 1);
		}

		public static IToken RightOfToken(this Expression source, IToken token)
		{
			int index = source.Tokens.IndexOf(token);
			if (index == source.TokenCount - 1) return Token.None;
			else return source.TokenAt(index + 1);
		}

		public static bool OperatorsAllSame(this Expression source)
		{
			var operators = source.Tokens.Where(e => e is IOperator);

			int count = operators.Select(o => o.Type).Distinct().Count();

			return (count == 1 && operators.Count() != 1);
		}
	}

	#region Unused / Dead code
	//public static void CombineVariables(this Expression source)
	//{
	//	var vars = source.Variables.Select(e => (e as IVariable).Value).ToList();
	//	var distinctVars = vars.Distinct();
	//
	//	if (distinctVars.Count() == vars.Count())
	//	{
	//		return;
	//	}
	//
	//	string tokens = string.Join("", source.Tokens.Select(e => e.Symbol[0]));
	//
	//	var found = new List<Tuple<char, List<int>>>();
	//	foreach (char distinct in distinctVars)
	//	{
	//		List<int> indices = Enumerable.Range(0, tokens.Length - 1).Where(i => distinct.Equals(tokens[i])).ToList();
	//
	//		if (indices.Count > 1)
	//		{
	//			found.Add(new Tuple<char, List<int>>(distinct, indices));
	//		}
	//	}
	//
	//	if (!found.Any()) return;
	//
	//	found = found.OrderBy(tup => tup.Item2.Count).ToList();
	//
	//	Tuple<char, List<int>> first = found.First();
	//
	//	int value = 0;
	//	int insertPosition = -1;
	//	foreach (int index in first.Item2)
	//	{
	//		if (index == 0)
	//		{
	//			value = 1;
	//			insertPosition = 0;
	//			continue;
	//		}
	//
	//		IToken token = source.TokenAt(index);
	//
	//		IOperator left = source.LeftOfToken(token) as IOperator;
	//
	//		if (left.Symbol == "+")
	//		{
	//			value += 1;
	//			insertPosition = index;
	//
	//		}
	//		else if (left.Symbol == "-")
	//		{
	//			value -= 1;
	//			insertPosition = index - 1;
	//		}
	//		else if (left.Symbol == "*")
	//		{
	//			IToken leftLeft = source.LeftOfToken(left);
	//			if (leftLeft != Token.None && leftLeft.Type == TokenType.Number)
	//			{
	//				int multiplyer = (leftLeft as INumber).Value;
	//
	//				IToken leftLeftLeft = source.LeftOfToken(leftLeft);
	//
	//				if (leftLeftLeft.Symbol == "-")
	//				{
	//					value -= multiplyer;
	//					insertPosition = index - 3;
	//				}
	//				else //if (leftLeftLeft == Token.None || leftLeftLeft.Symbol == "+")
	//				{
	//					value += multiplyer;
	//					insertPosition = index - 2;
	//				}
	//			}
	//			else
	//			{
	//				value += 1;
	//				insertPosition = index;
	//			}
	//		}
	//		else
	//		{
	//			value += 1;
	//			insertPosition = index;
	//		}
	//	}
	//}
	#endregion

}
