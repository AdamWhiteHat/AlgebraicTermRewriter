using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public interface ISentence
	{
	}

	public interface INumber : ITerm
	{
	}

	public interface IVariable : ITerm
	{
	}

	public interface ITerm : IElement
	{
	}

	public interface IOperator : IElement
	{
	}

	public interface IElement
	{
		string Symbol { get; }
		ElementType Type { get; }
		string ToString();
	}
}
