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
	public class Equation : ISentence
	{
		public Expression LeftHandSide { get; set; } = null;
		public Expression RightHandSide { get; set; } = null;
		public List<Element> Elements { get { return LeftHandSide.Elements.Concat(RightHandSide.Elements).ToList(); } }

		public EqualityType Equality { get; private set; }
		public bool IsVariableIsolated { get; private set; } = false;
		public bool IsFullyReduced { get { return LeftHandSide != null && RightHandSide != null ? LeftHandSide.IsFullyReduced && RightHandSide.IsFullyReduced : false; } }

		public Equation(Expression lhs, EqualityType equality, Expression rhs)
		{
			LeftHandSide = lhs;
			Equality = equality;
			RightHandSide = rhs;
			LeftHandSide.SideOfEqualitySymbol = EqualitySide.Left;
			RightHandSide.SideOfEqualitySymbol = EqualitySide.Right;
		}
	}
}
