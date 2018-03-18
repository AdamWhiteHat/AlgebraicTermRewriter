using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public static class IEnumerableExtensionMethods
	{
		public static bool Same<T>(this IEnumerable<T> source)
		{
			if (source.Count() > 1)
			{
				return source.Distinct().Count() == 1;
			}
			else
			{
				return false;
			}
		}
	}
}
