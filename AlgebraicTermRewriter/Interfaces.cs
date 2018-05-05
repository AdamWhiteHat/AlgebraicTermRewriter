using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	/// <summary>
	/// A mathematical sentence is either an expression or an equation
	/// (which consists of two expressions, separated by a comparative symbol, such as an equals).
	/// Both the Expression class and the Equation class derive from ISentence,
	/// because they are mathematical sentences.
	/// </summary>
	public interface ISentence
	{
	}

	/// <summary>
	/// A term in a mathematical sentence that holds numeric value,
	/// and can be directly operated upon by a mathematical operation.
	/// For our purposes, this will always be whole number integer.
	/// </summary>
	public interface INumber : ITerm
	{
		int Value { get; }
	}

	/// <summary>
	/// A variable, such as X or Y, that stand in for a numeric value in a mathematical expression or equation.
	/// </summary>
	public interface IVariable : ITerm
	{
		char Value { get; }
	}

	/// <summary>
	/// A ITerm is an IToken that is either an INumber or an IVariable,
	/// but is not an IOperator, which is the third type of IToken.
	/// </summary>
	public interface ITerm : IToken
	{
	}

	/// <summary>
	/// A mathematical operator such as addition, subtraction, multiplication or division.
	/// </summary>
	public interface IOperator : IToken
	{
		char Symbol { get; }
	}

	/// <summary>
	/// The most basic token in a mathematical sentence.
	/// Could also be thought of as a Token, from a parsing perspective.
	/// </summary>
	public interface IToken
	{
		string Contents { get; }
		TokenType Type { get; }
		string ToString();
	}

	/// <summary>
	/// A Generic ICloneable interface. This avoids conversion and boxing.
	/// </summary>
	/// <typeparam name="T">The type of the class that is cloneable.</typeparam>
	public interface ICloneable<T>
	{
		T Clone();
	}
}
