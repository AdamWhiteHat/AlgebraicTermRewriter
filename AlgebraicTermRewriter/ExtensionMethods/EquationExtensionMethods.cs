using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public static class EquationExtensionMethods
	{
		public static int GetDistinctVariableCount(this Equation source)
		{
			return source.LeftHandSide.Variables.Concat(source.RightHandSide.Variables).Distinct().Count();
		}

		public static void Substitute(this Equation source, IVariable variable, Token[] expression)
		{
			source.LeftHandSide.Substitute(variable, expression);
			source.RightHandSide.Substitute(variable, expression);
		}

		public static void ApplyToBothSides(this Equation source, TermOperatorPair pair)
		{
			source.LeftHandSide.Insert(pair);
			source.RightHandSide.Insert(pair);
		}

		public static bool OnlyArithmeticTokens(this Equation source)
		{
			return source.LeftHandSide.OnlyArithmeticTokens() && source.RightHandSide.OnlyArithmeticTokens();
		}

		public static void EnsureVariableOnLeft(this Equation source)
		{
			Expression left = source.LeftHandSide;
			Expression right = source.RightHandSide;

			bool leftHasVariables = left.Variables.Any();
			bool rightHasVariables = right.Variables.Any();

			if (leftHasVariables && rightHasVariables)
			{
				return;
			}

			if (leftHasVariables)
			{
				return;
			}

			if (rightHasVariables)
			{
				Expression temp = right;
				source.RightHandSide = left;
				source.LeftHandSide = temp;
			}
		}

		public static Expression PickExpression(this Equation source)
		{
			Expression l = source.LeftHandSide;
			Expression r = source.RightHandSide;
			int lComplexity = l.RankComplexity();
			int rComplexity = r.RankComplexity();

			return (lComplexity < rComplexity) ? r : l;
		}
	}
}
