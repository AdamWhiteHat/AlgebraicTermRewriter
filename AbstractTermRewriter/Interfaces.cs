using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public interface ISentence
	{
		List<Element> Elements { get; }
	}

	public interface IElement
	{
		char Symbol { get; }
		ElementType Type { get; }
	}
}
