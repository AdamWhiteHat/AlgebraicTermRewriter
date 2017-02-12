using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public enum TermType
	{
		Variable,
		Constant
	}

	public class Term : IElement
	{
		public char Symbol { get; }
		public TermType Type { get; private set; }
		public static readonly string AllowedSymbols = Types.Numbers + Types.Variables;

		public Term(char symbol)
		{
			Symbol = symbol;
			if (Types.Numbers.Contains(Symbol))
			{
				Type = TermType.Constant;
			}
			else if (Types.Variables.Contains(Symbol))
			{
				Type = TermType.Variable;
			}
		}
	}
}
