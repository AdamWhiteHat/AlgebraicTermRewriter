using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public static class EquationExtensionMethods
	{
		public static void Substitute(this Equation source, IVariable variable, INumber value)
		{
			source.LeftHandSide.Substitute(variable, value);
			source.RightHandSide.Substitute(variable, value);
		}

		public static void ApplyToBothSides(this Equation source, TermOperatorPair pair)
		{
			source.LeftHandSide.Insert(pair);
			source.RightHandSide.Insert(pair);
		}

		public static bool OnlyArithmeticElements(this Equation source)
		{
			return source.LeftHandSide.OnlyArithmeticElements() && source.RightHandSide.OnlyArithmeticElements();
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
