using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public static class ParserTokens
	{
		public static string Numbers = "0123456789";
		public static string Operators = "+-*/^";

		public static string AllowedCharacters = Numbers + Operators + "()";

		public enum Associativity
		{
			Left, Right
		}

		public static Dictionary<char, int> PrecedenceDictionary = new Dictionary<char, int>()
		{
			{'(', 0}, {')', 0},
			{'+', 1}, {'-', 1},
			{'*', 2}, {'/', 2},
			{'^', 3}
		};

		public static Dictionary<char, Associativity> AssociativityDictionary = new Dictionary<char, Associativity>()
		{
			{'+', Associativity.Left}, {'-', Associativity.Left}, {'*', Associativity.Left}, {'/', Associativity.Left},
			{'^', Associativity.Right}
		};

		public static bool IsOperatorOrNull(char symbol)
		{
			if (symbol == '\0')
			{
				return true;
			}
			if (ParserTokens.Operators.Contains(symbol) || symbol == '(')
			{
				return true;
			}
			return false;
		}

		public static bool IsNumericOrUnary(char symbol)
		{
			if (IsNumeric(symbol.ToString()) || symbol == '-' || symbol == ')') { return true; }
			return false;
		}

		public static bool IsNumeric(string text)
		{
			if (string.IsNullOrWhiteSpace(text)) { return false; }

			int parseOut = 0;
			if (int.TryParse(text, out parseOut)) //text.All(c => Numbers.Contains(c));
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
