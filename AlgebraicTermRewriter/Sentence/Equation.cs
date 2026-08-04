using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AlgebraicTermRewriter
{
	/// <summary>
	/// An equation consists of two expressions with an equality or inequality symbol between them 
	/// </summary>
	public class Equation : ISentence, ICloneable<Equation>
	{
		public static Equation Empty = new Equation(Expression.Empty, ComparisonType.Equals, Expression.Empty);
		public Expression LeftHandSide { get; set; } = null;
		public ComparisonType ComparisonOperator { get; private set; }
		public Expression RightHandSide { get; set; } = null;

		public bool IsVariableIsolated { get { return (LeftHandSide.IsVariableIsolated || RightHandSide.IsVariableIsolated); } }

		public Equation(Expression leftExpression, ComparisonType comparison, Expression rightExpression)
		{
			LeftHandSide = leftExpression;
			ComparisonOperator = comparison;
			RightHandSide = rightExpression;

			LeftHandSide.Parent = this;
			RightHandSide.Parent = this;
		}

		public static Equation Parse(string equationText)
		{
			if (string.IsNullOrWhiteSpace(equationText))
			{
				throw new ArgumentException($"{nameof(equationText)} cannot be null, empty or whitespace.");
			}

			if (!equationText.Any(c => Types.Comparison.Contains(c)))
			{
				throw new Exception($"{nameof(equationText)} does not contain an equality or comparison operator (=, >, <, >=, <=), which is what defines an {nameof(Equation)}. Perhaps you meant to parse it as an {nameof(Expression)} instead? {nameof(equationText)}: \"{equationText}\".");
			}

			return MathParser.ParseEquation(equationText);
		}

		public Equation Simplify()
		{
			if (!this.CanSimplify())
			{
				return this;
			}
			return new Equation(LeftHandSide.Simplify(), ComparisonOperator, RightHandSide.Simplify());
		}

		public bool CanSimplify()
		{
			return LeftHandSide.CanSimplify() || RightHandSide.CanSimplify();
		}

		public bool Equals(Equation other)
		{
			return Equation.Equals(this, other);
		}

		public static bool Equals(Equation left, Equation right)
		{
			if (left == null)
			{
				return (right == null);
			}
			else if (right == null)
			{
				return false;
			}

			if (left.ComparisonOperator != right.ComparisonOperator)
			{
				return false;
			}
			if (!Expression.Equals(left.LeftHandSide, right.LeftHandSide))
			{
				return false;
			}
			if (!Expression.Equals(left.RightHandSide, right.RightHandSide))
			{
				return false;
			}
			return true;
		}

		public Equation Clone()
		{
			Expression lhs = LeftHandSide.Clone();
			Expression rhs = RightHandSide.Clone();

			return new Equation(lhs, ComparisonOperator, rhs);
		}

		public override string ToString()
		{
			return string.Join(" ", new string[] { LeftHandSide.ToString(), ComparisonOperator.AsString(), RightHandSide.ToString() });
		}
	}
}
