using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public enum EqualityType
	{
		Equals,
		LessThan,
		GreaterThan,
		LessThanOrEquals,
		GreaterThanOrEquals
	}

	public enum ElementType
	{
		Number,
		Variable,
		Operator,
		Equality
	}

	public static class Types
	{
		public static readonly string Equality = "=";
		public static readonly string Inequality = "=<>";
		public static readonly string Parenthesis = "()";
		public static readonly string Operators = "+-*/^";
		public static readonly string Numbers = "0123456789";
		public static readonly string Variables = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public static readonly string All = Inequality + Parenthesis + Numbers + Operators + Variables;

		public static EqualityType GetEqualityType(string input)
		{
			if (input.Contains("<="))
			{
				return EqualityType.LessThanOrEquals;
			}
			else if (input.Contains(">="))
			{
				return EqualityType.GreaterThanOrEquals;
			}
			else if (input.Contains("<"))
			{
				return EqualityType.LessThan;
			}
			else if (input.Contains(">"))
			{
				return EqualityType.GreaterThan;
			}
			else if (input.Contains("="))
			{
				return EqualityType.Equals;
			}
			else
			{
				throw new ArgumentException("input not an (in)equality expression");
			}
		}

		public static class Convert
		{
			public static ElementType ToElementType(char symbol)
			{
				if (Types.Operators.Contains(symbol))
				{
					return ElementType.Operator;
				}
				else if (Types.Numbers.Contains(symbol))
				{
					return ElementType.Number;
				}
				else if (Types.Variables.Contains(symbol))
				{
					return ElementType.Variable;
				}
				else if (Types.Inequality.Contains(symbol))
				{
					return ElementType.Equality;
				}
				else
				{
					throw new NotSupportedException(symbol.ToString());
				}
			}
		}
	}
}
