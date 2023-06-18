using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Variable : IVariable, IEquatable<Variable>
	{
		public int? Value { get; private set; }
		public char Symbol { get; }
		public string Contents { get { if (Value.HasValue) { return Value.Value.ToString(); } else { return Symbol.ToString(); } } }
		public TokenType Type { get { return TokenType.Variable; } }

		public Variable(char symbol)
		{
			Value = null;
			if (!Types.Variables.Contains(symbol))
			{
				throw new ArgumentException($"{nameof(symbol)} does not match any of the valid variable symbols.");
			}
			Symbol = symbol;
		}

		public IToken Clone()
		{
			Variable result = new Variable(this.Symbol);
			if (this.Value.HasValue)
			{
				result.Value = this.Value.Value;
			}
			return result;
		}

		public bool Equals(Variable other)
		{
			return (this.Symbol == other.Symbol);
		}

		public override int GetHashCode()
		{
			return Symbol.GetHashCode();
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
