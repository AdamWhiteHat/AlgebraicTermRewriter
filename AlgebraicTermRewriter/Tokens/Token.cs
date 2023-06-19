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

		public IToken Clone()
		{
			return new Token() { Contents = this.Contents, Type = this.Type };
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
