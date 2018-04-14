using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public interface ISentence
	{
	}

	public interface INumber : ITerm
	{
		int Value { get; }
	}

	public interface IVariable : ITerm
	{
		char Value { get; }
	}

	public interface ITerm : IElement
	{
	}

	public interface IOperator : IElement
	{
		char Value { get; }
	}

	public interface IElement
	{
		string Symbol { get; }
		ElementType Type { get; }
		string ToString();
	}
}
