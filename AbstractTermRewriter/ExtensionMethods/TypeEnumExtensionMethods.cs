using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public static class TypeEnumExtensionMethods
	{
		public static string AsString(this ComparativeType source)
		{
			switch (source)
			{
				case ComparativeType.Equals:
					return "=";
				case ComparativeType.GreaterThan:
					return ">"; ;
				case ComparativeType.LessThan:
					return "<";
				case ComparativeType.GreaterThanOrEquals:
					return ">=";
				case ComparativeType.LessThanOrEquals:
					return "<=";
				default:
					throw new NotImplementedException();
			}
		}

		public static ElementType GetElementType(this TermType source)
		{
			if (source == TermType.Number) return ElementType.Number;
			else return ElementType.Variable;
		}
	}
}
