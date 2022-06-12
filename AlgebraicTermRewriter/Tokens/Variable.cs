using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Variable : IVariable, IEquatable<Variable>
	{
		public char Value { get; private set; }
		public string Contents { get { return Value.ToString(); } }
		public TokenType Type { get { return TokenType.Variable; } }

		public Variable(char symbol)
		{
			if (!Types.Variables.Contains(symbol))
			{
				throw new ArgumentException($"{nameof(symbol)} does not match any of the valid variable symbols.");
			}
			Value = symbol;
		}

		public bool Equals(Variable other)
		{
			return (this.Value == other.Value);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
