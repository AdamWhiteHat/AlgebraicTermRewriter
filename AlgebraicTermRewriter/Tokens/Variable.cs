using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Variable : IVariable
	{
		public char Value { get; private set; }
		public string Contents { get; }
		public TokenType Type { get { return TokenType.Variable; } }

		public Variable(char symbol)
		{
			if (!Types.Variables.Contains(symbol))
			{
				throw new ArgumentException($"{nameof(symbol)} does not match any of the valid variable symbols.");
			}
			Value = symbol;
			Contents = symbol.ToString();
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
