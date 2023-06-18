using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	/// <summary>
	/// A Term Rewriting Rule
	/// </summary>
	public interface IRewriteRule
	{
		/// <summary>
		/// A numerical value that controls what order Rewrite Rules get applied in.
		/// Rewrite rules are ordered from smallest to largest.
		/// </summary>
		int ApplyOrder { get; }

		/// <summary>
		/// Called for every Equation to see if this rewrite rule should be ran against it.
		/// Place your logic here testing the supplied equation for criteria upon which this rule applies.
		/// </summary>
		/// <param name="equation">The equation to text.</param>
		/// <returns><c>true</c> if this rule should be applied to the equation supplied in the parameter, or <c>false</c> otherwise.</returns>
		bool ShouldApplyRule(Equation equation);

		/// <summary>
		/// Applies the rewrite rule to a qualifying equation.
		/// Place your rewrite logic here.
		/// </summary>
		/// <param name="equation">The equation.</param>
		Equation ApplyRule(Equation equation);
	}
}
