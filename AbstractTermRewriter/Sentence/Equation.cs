using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	/// <summary>
	/// An equation consists of two expressions with an equality or inequality symbol between them 
	/// </summary>
	public class Equation : ISentence, ICloneable<Equation>
	{
		public static Equation Empty = new Equation(Expression.Empty, ComparativeType.Equals, Expression.Empty);
		public Expression LeftHandSide { get; set; } = null;
		public ComparativeType ComparativeOperator { get; private set; }
		public Expression RightHandSide { get; set; } = null;

		public Equation(Expression leftExpression, ComparativeType comparative, Expression rightExpression)
		{
			LeftHandSide = leftExpression;
			ComparativeOperator = comparative;
			RightHandSide = rightExpression;
		}

		public Equation Clone()
		{
			Expression lhs = LeftHandSide.Clone();
			Expression rhs = RightHandSide.Clone();

			return new Equation(lhs, ComparativeOperator, rhs);
		}

		public override string ToString()
		{
			return LeftHandSide.ToString() + ComparativeOperator.AsString() + RightHandSide.ToString();
		}
	}
}
