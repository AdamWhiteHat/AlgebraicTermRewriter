using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public class Variable : IVariable
	{
		public string Symbol { get; }
		public ElementType Type { get { return ElementType.Variable; } }

		public Variable(char symbol)
		{
			if (!Types.Variables.Contains(symbol))
			{
				throw new ArgumentException($"{nameof(symbol)} does not match any of the valid variable symbols.");
			}
			Symbol = symbol.ToString();
		}

		public override string ToString()
		{
			return Symbol;
		}
	}
}
