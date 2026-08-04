using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public class SubExpression : Expression, IToken
	{
		public static SubExpression Empty = new SubExpression();
		public string Contents { get { return this.ToString(); } }
		public TokenType Type { get { return TokenType.Subexpression; } }

		public SubExpression()
			: base()
		{ }

		public SubExpression(IEnumerable<IToken> tokens)
			: base(tokens)
		{ }

		public static SubExpression Parse(string subexpressionText)
		{
			if (string.IsNullOrWhiteSpace(subexpressionText))
			{
				throw new ArgumentException($"{nameof(subexpressionText)} cannot be null, empty or whitespace.");
			}

			if (subexpressionText.Any(c => Types.Comparison.Contains(c)))
			{
				throw new Exception($"{nameof(subexpressionText)} contains an equality or comparison operator (=, >, <, >=, <=). Perhaps you meant to parse it as an {nameof(Equation)} instead? {nameof(subexpressionText)}: \"{subexpressionText}\".");
			}

			Stack<char> stack = new Stack<char>(subexpressionText.Replace(" ", "").Reverse());

			IEnumerable<IToken> tokens = MathParser.ParseExpression(stack);

			return new SubExpression(tokens.ToArray());
		}

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
