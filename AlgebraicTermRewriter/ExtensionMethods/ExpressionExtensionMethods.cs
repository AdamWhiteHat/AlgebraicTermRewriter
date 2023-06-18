using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public static class ExpressionExtensionMethods_Checks
	{
		public static Expression Simplify(this Expression source)
		{
			Expression result = source.Clone();
			// Same variable appears more than once.
			if (result.Variables.Count() != result.Variables.Distinct().Count())
			{
				var variables = result.Variables.ToList();

				List<SubExpression> toCombine = new List<SubExpression>();
				foreach (IVariable variable in variables)
				{
					toCombine.Add(result.GetVariableProductSubExpression(variable));
				}

				string debugSource = result.ToString();

				int leftIndex = result.IndexOf(toCombine[0].Last());
				int rightIndex = result.IndexOf(toCombine[1].First());

				var rightOfLeftToken = result.RightOfToken(result.TokenAt(leftIndex));
				var leftOfRightToken = result.LeftOfToken(result.TokenAt(rightIndex));

				if (rightOfLeftToken == leftOfRightToken)
				{
					if (rightOfLeftToken.Type == TokenType.Operator)
					{
						IOperator operationToken = (IOperator)rightOfLeftToken;

						if (operationToken.Symbol == '+')
						{

						}
						else if (operationToken.Symbol == '-')
						{

						}
						else if (operationToken.Symbol == '*')
						{

						}
						else if (operationToken.Symbol == '/')
						{

						}
					}
				}

			}

			Tuple<int, int> range = result.Tokens.GetLongestArithmeticRange();

			if (range == null)
			{
				return result;
			}

			List<IToken> arithmeticExpression = result.Tokens.ToList().GetRange(range.Item1, range.Item2);

			string toEvaluate = string.Join("", arithmeticExpression.Select(e => e.Contents));

			INumber newValue = new Number(InfixNotationEvaluator.Evaluate(toEvaluate));
			result.RemoveRange(range.Item1, range.Item2);

			int insertIndex = range.Item1 == 0 ? 0 : range.Item1;

			if (newValue.Value == 0)
			{
				if (result.TokenCount == 0)
				{
					result.Insert(0, new Number(0));
					return result;
				}
				IToken op = result.TokenAt(insertIndex);
				if (op.Contents != "*")
				{
					result.RemoveAt(insertIndex);
				}
			}
			else
			{
				result.Insert(insertIndex, newValue);
			}
			return result;
		}

		public static Expression Substitute(this Expression source, IVariable variable, IToken[] expression)
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
						source.RemoveAt(index);
						source.InsertRange(index, new SubExpression(expression.ToArray()));
					}
				}
			}
			return source;
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
				TokenType firstType = source.TokenAt(0).Type;
				TokenType secondType = source.TokenAt(1).Type;

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

		public static int RankIsolationComplexity(this Expression source)
		{
			int score = source.Terms.Count();
			score += source.Variables.Count();
			score += source.Operators.Select(o => ParserTokens.PrecedenceDictionary[o.Symbol]).Sum();
			score += source.Operators.Count(o => o.Symbol == '^') * 2;
			return score;
		}

		public static SubExpression GetVariableProductSubExpression(this Expression source, IVariable variable)
		{
			List<IToken> collection = new List<IToken>();
			collection.Add(variable.Clone());

			IToken current = variable.Clone();
			IToken left = Token.None;
			while (true)
			{
				left = source.LeftOfToken(current);
				current = source.LeftOfToken(left);
				if (current.Equals(Token.None) || left.Contents != "*")
				{
					break;
				}
				collection.Insert(0, left);
				collection.Insert(0, current);
			}

			current = variable.Clone();
			IToken right = Token.None;
			while (true)
			{
				right = source.RightOfToken(current);
				current = source.RightOfToken(right);
				if (current.Equals(Token.None) || right.Contents != "*")
				{
					break;
				}
				collection.Add(right);
				collection.Add(current);
			}


			SubExpression result = new SubExpression(collection.ToArray());

			int start = source.IndexOf(collection.First());
			int count = collection.Count;

			source.RemoveRange(start, count);
			source.InsertRange(start, result);

			return result;
		}

		public static OperatorExpressionPair Extract(this Expression source, IOperator op, SubExpression subExpr)
		{
			return Extract(source, op, subExpr.ToArray());
		}

		public static OperatorExpressionPair Extract(this Expression source, IOperator op, params IToken[] terms)
		{
			if (!terms.Any())
			{
				return null;
			}

			IOperator oper = op;

			int tIndex = source.IndexOf(terms[0]);

			InsertOrientation orientation = InsertOrientation.Either;
			if (tIndex == 0)
			{
				if (op.Symbol != '+')
				{
					source.Remove(op);
				}
				orientation = InsertOrientation.Left;

			}
			else
			{
				source.Remove(op);
				orientation = InsertOrientation.Right;
			}

			oper = Operator.GetInverse(oper);

			foreach (IToken term in terms)
			{
				source.Remove(term);
			}

			return new OperatorExpressionPair(oper, new SubExpression(terms), orientation);
		}

		public static void SetToMultiplicativeInverse(this Expression source)
		{
			string expression = source.ToString()?.Replace(" ", "") ?? "";

			if (expression.StartsWith("0"))
			{
				source.RemoveAt(0);
				source.RemoveAt(0);
				return;
			}

			if (expression.StartsWith("-"))
			{
				source.RemoveAt(0);
			}
			else
			{
				if (source.TokenCount > 0)
				{
					INumber num = source.Tokens.First() as INumber;
					if (num != null)
					{
						int newNum = -num.Value;
						source.SubExpressions[0][0] = new Number(newNum);
						return;
					}
				}
			}
		}

		public static void SetToMultiplicativeInverse_Old(this Expression source)
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
				source.Remove(first);
				source.Remove(second);
			}
		}

	}



	public static class ExpressionExtensionMethods_Manipulations
	{
		public static IToken LeftOfToken(this Expression source, IToken token)
		{
			int index = source.IndexOf(token);
			if (index == 0) return Token.None;
			else return source.TokenAt(index - 1);
		}

		public static IToken RightOfToken(this Expression source, IToken token)
		{
			int index = source.IndexOf(token);
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

}
