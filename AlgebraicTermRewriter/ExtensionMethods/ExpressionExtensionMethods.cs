using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public static class ExpressionExtensionMethods_Checks
	{
		public static bool Substitute(this Expression source, IVariable variable, IToken[] expression)
		{
			if (source.Tokens.Any(e => e.Contents == variable.Contents))
			{
				int index = -1;
				int tokenCount = source.TokenCount;
				while (index++ < tokenCount)
				{
					IToken currentToken = source.TokenAt(index);

					if (IToken.Equals(currentToken, variable))
					{
						source.RemoveAt(index);
						source.InsertRange(index, new SubExpression(expression));
						return true;
					}
				}
			}
			return false;
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

		public static OperatorExpressionPair Extract(this Expression source, IVariable variable)
		{
			return Extract(source, (IToken)variable);
		}

		public static OperatorExpressionPair Extract(this Expression source, INumber number)
		{
			return Extract(source, (IToken)number);
		}

		public static OperatorExpressionPair Extract(this Expression source, SubExpression subExpression)
		{
			return Extract(source, (IToken)subExpression);
		}

		private static OperatorExpressionPair Extract(this Expression source, IToken token)
		{
			IToken left = source.LeftOfToken(token);
			IToken right = source.RightOfToken(token);

			IToken opCandidate = null;
			InsertOrientation insertOrientation = InsertOrientation.Right;

			if (source.TokenCount == 1)
			{
				source.Remove(token);
				source.Add(new Number(0));
				return new OperatorExpressionPair(new Operator('-'), new SubExpression(new IToken[] { token }), insertOrientation);
			}
			else if (left.Equals(Token.None))
			{
				opCandidate = right;
				insertOrientation = InsertOrientation.Left;
			}
			else if (right.Equals(Token.None))
			{
				opCandidate = left;
				insertOrientation = InsertOrientation.Right;
			}
			else if (left.Contents == "*")
			{
				opCandidate = left;
				insertOrientation = InsertOrientation.Right;
			}
			else if (right.Contents == "*")
			{
				opCandidate = right;
				insertOrientation = InsertOrientation.Left;
			}

			IOperator oper = opCandidate as IOperator;
			if (oper == null)
			{
				throw new FormatException("The token adjacent to the variable must be an operator.");
			}

			source.Remove(oper);
			source.Remove(token);
			IOperator inverseOper = Operator.GetInverse(oper);

			/*
			SubExpression subExpression = null;

			if (token is SubExpression)
			{
				subExpression = token as SubExpression;
			}

			if (subExpression == null)
			{
				subExpression = new SubExpression(new IToken[] { token });
			}
			*/

			return new OperatorExpressionPair(inverseOper, new SubExpression(new IToken[] { token }), insertOrientation);
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

			IOperator oper = (IOperator)op.Clone();

			int tIndex = source.IndexOf(terms[0]);

			InsertOrientation orientation = InsertOrientation.Right;

			if (op.Symbol == '^')
			{
				int opIndex = source.IndexOf(op);
				if (opIndex != tIndex - 1)
				{
					throw new Exception("Term of the exponentiation operator ('^') must be immediately right of the operator.");
				}
			}

			source.Remove(op);
			IOperator inverseOper = Operator.GetInverse(oper);

			foreach (IToken term in terms)
			{
				source.Remove(term);
			}

			return new OperatorExpressionPair(inverseOper, new SubExpression(terms), orientation);
		}

		public static void SetToAdditiveInverse(this Expression source)
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
					INumber num = source.Numbers.First();
					if (num != null)
					{
						num.Negate();
						return;
					}
				}
			}
		}

		public static void SetToAdditiveInverse_Old(this Expression source)
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
