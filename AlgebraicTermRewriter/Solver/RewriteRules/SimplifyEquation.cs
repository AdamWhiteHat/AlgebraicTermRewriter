using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public class SimplifyEquation : IRewriteRule
	{
		public int ApplyOrder => 100;

		public bool ShouldApplyRule(Equation equation)
		{
			return equation.OnlyArithmeticTokens();
		}

		public Equation ApplyRule(Equation equation)
		{
			return new Equation(equation.LeftHandSide.Simplify(), equation.ComparisonOperator, equation.RightHandSide.Simplify());
		}
	}
}
