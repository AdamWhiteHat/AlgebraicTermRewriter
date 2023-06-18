using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public class SubExpression : List<IToken>, ICloneable<SubExpression>
	{
		public SubExpression()
			: base()
		{ }
		public SubExpression(IToken[] tokens)
			: base(tokens)
		{ }

		public SubExpression Clone()
		{
			return new SubExpression(this.Select(tok => tok.Clone()).ToArray());
		}

		public override string ToString()
		{
			return string.Join(" ", this.Select(e => e.Contents));
		}
	}
}
