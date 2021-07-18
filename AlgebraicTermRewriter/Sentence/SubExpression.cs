using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public class SubExpression : List<IToken>
	{
		public SubExpression()
			: base()
		{ }
		public SubExpression(IToken[] tokens)
			: base(tokens)
		{ }

		public override string ToString()
		{
			return string.Join(" ", this.Select(e => e.Contents));
		}
	}
}
