using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Number : INumber
	{
		public int Value { get { return _value; } }
		private int _value;
		public string Contents { get { return Value.ToString(); } }
		public TokenType Type { get { return TokenType.Number; } }

		public Number(int value)
		{
			_value = value;
		}

		public static Number Parse(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException($"{nameof(text)} cannot be null, empty or whitespace.");
			}
			string trimmedText = text.Trim();

			if (!trimmedText.All(c => Types.Numbers.Contains(c)))
			{
				throw new Exception($"{nameof(text)} can only contain numeric characters: {Types.Numbers}. {nameof(text)}: \"{text}\".");
			}

			return new Number(int.Parse(trimmedText));
		}

		public void Negate()
		{
			_value = -Value;
		}

		public IToken Clone()
		{
			return new Number(this.Value);
		}

		public bool Equals(IToken other)
		{
			return IToken.Equals(this, other);
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
