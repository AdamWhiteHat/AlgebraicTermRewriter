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

		public static Variable Parse(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException($"{nameof(text)} cannot be null, empty or whitespace.");
			}
			string trimmedText = text.Trim();

			if (trimmedText.Length != 1)
			{
				throw new ArgumentException($"{nameof(text)} must be a single character. {nameof(text)}: \"{text}\".");
			}

			char inputChar = trimmedText[0];

			if (!Types.Variables.Contains(inputChar))
			{
				throw new Exception($"{nameof(text)} must be an alpha character: {Types.Variables}. {nameof(text)}: \"{text}\".");
			}

			return new Variable(inputChar);
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

		public bool Equals(IToken other)
		{
			return IToken.Equals(this, other);
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
