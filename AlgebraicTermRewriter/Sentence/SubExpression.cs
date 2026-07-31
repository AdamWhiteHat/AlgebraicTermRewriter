using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public class SubExpression : List<IToken>, IToken
	{
		public string Contents { get { return this.ToString(); } }
		public TokenType Type { get { return TokenType.Subexpression; } }

		public SubExpression()
			: base()
		{ }
		public SubExpression(IToken[] tokens)
			: base(tokens)
		{ }

		public bool Equals(IToken? other)
		{
			return SubExpression.Equals(this, other);
		}

		public static bool Equals(SubExpression left, SubExpression right)
		{
			if (left == null)
			{
				return (right == null);
			}
			else if (right == null)
			{
				return false;
			}

			if (left.Count != right.Count)
			{
				return false;
			}

			int index = 0;
			while (index < left.Count)
			{
				if (!IToken.Equals(left[index], right[index]))
				{
					return false;
				}
				index++;
			}

			return true;
		}

		public IToken Clone()
		{
			return new SubExpression(this.Select(tok => tok.Clone()).ToArray());
		}

		public override string ToString()
		{
			return $"({string.Join(" ", this.Select(e => e.ToString()))})";
		}
	}
}
