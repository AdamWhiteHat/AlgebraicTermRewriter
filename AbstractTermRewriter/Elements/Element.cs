using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public class Element : IElement
	{
		public static Element None = new Element() { Symbol = "{None}", Type = ElementType.None };

		public string Symbol { get; private set; }
		public ElementType Type { get; private set; }
				
		private Element()
		{
		}
	}
}
