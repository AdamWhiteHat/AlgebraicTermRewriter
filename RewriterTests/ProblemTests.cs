using System;
using AbstractTermRewriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace RewriterTests
{
	[TestClass]
	public class ProblemTests
	{
		[TestMethod]
		public void TestBasicProblem()
		{
			string[] problemStatement = 
				new string[]
				{
					"2x+4=10",
					"3y^2-2=20"
				};

			Problem test = new Problem(problemStatement);
		}

		//[TestMethod]
		//public void TestTokenize()
		//{
		//}

		//[TestMethod]
		//public void TestSerialize()
		//{
		//}		
	}
}
