using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using AbstractTermRewriter;

namespace RewriterTests
{
	[TestClass]
	public class SimpleEquations
	{
		private string Equation1 = "x+5=10";
		private string Equation2 = "3*x=8";
		private string Equation3 = "2/y=7";


		[TestMethod]
		public void TestSimpleEquation1()
		{
			Equation exp = new Equation(Equation1);

			
			

			//exp.ApplyToBothSides(pair);

		}


















		[TestMethod]
		public void TestSimpleEquation2()
		{
		}









		public void Print(string message = " ") { Print("{0}", message); }


		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }


		private TestContext testContextInstance;
		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }


		[ClassInitializeAttribute]
		public static void Initialize(TestContext context) { }
	}
}
