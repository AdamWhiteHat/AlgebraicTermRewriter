using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public class Token : IToken
	{
		public static Token None = new Token() { Contents = "{None}", Type = TokenType.None };

		public string Contents { get; private set; }
		public TokenType Type { get; private set; }

		private Token()
		{
		}

		public override string ToString()
		{
			return Contents;
		}
	}
}
