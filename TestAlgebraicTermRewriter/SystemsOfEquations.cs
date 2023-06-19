using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;
using AlgebraicTermRewriter;

namespace RewriterTests
{
	[TestFixture]
	public class SystemsOfEquations
	{
		/*
		    (y^2) / (5*x) = -4
			2*x + y = 0
			5 - x = 10
			--------------------
			x = -5
			y = 10
		*/

		/*
		    x + 2*y = -3
			2*x - 5 = -3
		    x - y = 3		    
	        --------------------
		    y = -2
			x = 1
		    
		 */

		/*
			x + 2*y = 2*x - 5 
			x - y = 3
			--------------------
			y = -2
			x = 1
		*/

		/*
			x + y   = 0
			x + 2*y + z = 5
			2 * y = 4
			--------------------
			x = -2
			y = 2
			z = 3
		*/

		public void Print(object obj) { Print("{0}", obj); }

		public void Print(string message = " ") { TestContext.WriteLine(message); }

		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }

		private TestContext testContextInstance;

		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }
	}


}
