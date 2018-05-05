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
		public string Symbol { get { return Value.ToString(); } }
		public ElementType Type { get { return ElementType.Number; } }

		public Number(int value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Symbol;
		}
	}
}
