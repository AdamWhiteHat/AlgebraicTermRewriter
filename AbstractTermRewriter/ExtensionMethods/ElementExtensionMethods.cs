using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public static class ElementExtensionMethods
	{
		public static string AsString(this IElement source)
		{
			return source.Symbol;
		}

		public static string AsString(this IElement[] source)
		{
			return string.Join(" ", source.Select(e => e.AsString()));
		}
	}
}
