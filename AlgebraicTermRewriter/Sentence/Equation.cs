using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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

		public Equation(Expression leftExpression, ComparisonType comparison, Expression rightExpression)
		{
			LeftHandSide = leftExpression;
			ComparisonOperator = comparison;
			RightHandSide = rightExpression;
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
