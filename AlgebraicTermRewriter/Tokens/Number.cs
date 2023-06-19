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
