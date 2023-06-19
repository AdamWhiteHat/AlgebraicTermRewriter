using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public class SimplifyEquation : IRewriteRule
	{
		public static int ApplyOrder => 100;

		public static bool ShouldApplyRule(Equation equation)
		{
			return equation.OnlyArithmeticTokens();
		}

		public static Equation ApplyRule(Equation equation)
		{
			Expression lhs = equation.LeftHandSide;
			Expression rhs = equation.RightHandSide;

			lhs = Simplify(lhs);
			rhs = Simplify(rhs);

			return new Equation(lhs, equation.ComparisonOperator, rhs);
		}

		public static Expression Simplify(Expression input)
		{
			Expression result = input;

			Range range = result.Tokens.GetLongestArithmeticRange();

			if (range == Range.Empty)
			{
				return result;
			}

			List<IToken> arithmeticExpression = result.Tokens.ToList().GetRange(range.StartIndex, range.Count);


			// Check if left of extracted expression is a subtraction operator.
			// If so, replace with addition and negate the left-most INumber in the extracted expression
			IToken leftOfExpression = result.TokenAt(range.StartIndex - 1);
			if (leftOfExpression != Token.None
				&& leftOfExpression.Type == TokenType.Operator
				&& arithmeticExpression.First().Type == TokenType.Number)
			{
				IOperator opLeftOfExpression = leftOfExpression as IOperator;
				INumber numberFirstOfExpression = arithmeticExpression.First() as INumber;
				if (opLeftOfExpression.Contents == "-")
				{
					result.ReplaceAt(range.StartIndex - 1, Operator.GetInverse(opLeftOfExpression));
					((INumber)arithmeticExpression[0]).Negate();
				}
			}

			string toEvaluate = string.Join("", arithmeticExpression.Select(e => e.Contents));

			INumber newValue = new Number(InfixNotationEvaluator.Evaluate(toEvaluate));
			result.RemoveRange(range.StartIndex, range.Count);

			int insertIndex = range.StartIndex;

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
					return result;
				}
			}

			result.Insert(insertIndex, newValue);
			return result;
		}
	}
}
