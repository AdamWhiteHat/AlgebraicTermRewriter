using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace AlgebraicTermRewriter
{
	public enum SideOfEquation
	{
		Left,
		Right
	}

	public enum TermType
	{
		Number,
		Variable
	}

	public enum TokenType
	{
		None,
		Number,
		Variable,
		Operator,
		Comparison
	}

	public enum ComparisonType
	{
		Equals,
		LessThan,
		GreaterThan,
		LessThanOrEquals,
		GreaterThanOrEquals
	}

	public static class Types
	{
		public static readonly string Equality = "=";
		public static readonly string Inequality = "<>";
		public static readonly string Comparison = Equality + Inequality;
		public static readonly string Parenthesis = "()";
		public static readonly string Operators = "+-*/^";
		public static readonly string Numbers = "0123456789";
		public static readonly string Variables = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public static readonly string All = Comparison + Parenthesis + Numbers + Operators + Variables;
	}

	public static class ConvertTo
	{
		public static ComparisonType ComparisonTypeEnum(string input)
		{
			if (input == "=") return ComparisonType.Equals;
			else if (input == ">") return ComparisonType.GreaterThan;
			else if (input == "<") return ComparisonType.LessThan;
			else if (input == "<=") return ComparisonType.LessThanOrEquals;
			else if (input == ">=") return ComparisonType.GreaterThanOrEquals;
			else throw new ArgumentException($"{nameof(input)} is not a {typeof(ComparisonType)}.");
		}

		public static TokenType TokenTypeEnum(char symbol)
		{
			if (Types.Operators.Contains(symbol)) return TokenType.Operator;
			else if (Types.Numbers.Contains(symbol)) return TokenType.Number;
			else if (Types.Variables.Contains(symbol)) return TokenType.Variable;
			else if (Types.Comparison.Contains(symbol)) return TokenType.Comparison;
			else throw new ArgumentException($"{symbol} is not a TokenType.");
		}
	}
}
