using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Number : INumber
	{
		public int Value { get; private set; }
		public string Contents { get { return Value.ToString(); } }
		public TokenType Type { get { return TokenType.Number; } }

		public Number(int value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
