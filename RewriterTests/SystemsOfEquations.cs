using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgebraicTermRewriter;

namespace RewriterTests
{
	[TestClass]
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
	}
}
