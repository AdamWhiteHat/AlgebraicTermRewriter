using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public static class TypeEnumExtensionMethods
	{
		public static string AsString(this ComparisonType source)
		{
			switch (source)
			{
				case ComparisonType.Equals:
					return "=";
				case ComparisonType.GreaterThan:
					return ">"; ;
				case ComparisonType.LessThan:
					return "<";
				case ComparisonType.GreaterThanOrEquals:
					return ">=";
				case ComparisonType.LessThanOrEquals:
					return "<=";
				default:
					throw new NotImplementedException();
			}
		}

		public static TokenType GetTokenType(this TermType source)
		{
			if (source == TermType.Number) return TokenType.Number;
			else return TokenType.Variable;
		}
	}

	public static class SideOfEquationExtensionMethods
	{
		public static SideOfEquation Other(this SideOfEquation side)
		{
			if (side == SideOfEquation.Left) { return SideOfEquation.Right; }
			if (side == SideOfEquation.Right) { return SideOfEquation.Left; }
			throw new Exception($"Did you add a new {typeof(SideOfEquation)} enum value?");
		}
	}
}
