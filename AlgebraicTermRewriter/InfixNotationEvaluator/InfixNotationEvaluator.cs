using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public static class InfixNotationEvaluator
	{
		public static int Evaluate(string infixNotationString)
		{
			string postfixNotationString = InfixToPostfixConverter.Convert(infixNotationString);
			return PostfixNotationEvaluator.Evaluate(postfixNotationString);
		}
	}
}
