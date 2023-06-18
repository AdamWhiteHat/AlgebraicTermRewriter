﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Number : INumber
	{
		public int Value { get; }
		public string Contents { get { return Value.ToString(); } }
		public TokenType Type { get { return TokenType.Number; } }

		public Number(int value)
		{
			Value = value;
		}

		public IToken Clone()
		{
			return new Number(this.Value);
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
