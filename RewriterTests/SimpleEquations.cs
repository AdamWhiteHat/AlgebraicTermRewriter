using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbstractTermRewriter;

namespace RewriterTests
{
	[TestClass]
	public class SimpleEquations
	{

		[TestMethod]
		public void TestCombineArithmeticElements()
		{
			string[] EquationA = new string[] { "x-5=10", "x = 15" };
			string[] EquationB = new string[] { "10=x-5", "x = 15" };
			string[] EquationC = new string[] { "5-x=10", "x = -5" };
			string[] EquationD = new string[] { "10=5-x", "x = -5" };


			TestCombineArithmeticHelper(EquationA);
			TestCombineArithmeticHelper(EquationB);
			TestCombineArithmeticHelper(EquationC);
			TestCombineArithmeticHelper(EquationD);

			Print("---------");

			string[] EquationP = new string[] { "2/y=6", "y = 0" };
			string[] EquationQ = new string[] { "6=2/y", "y = 0" };
			string[] EquationR = new string[] { "y/2=6", "y = 12" };
			string[] EquationS = new string[] { "6=y/2", "y = 12" };
			TestCombineArithmeticHelper(EquationP);
			TestCombineArithmeticHelper(EquationQ);
			TestCombineArithmeticHelper(EquationR);
			TestCombineArithmeticHelper(EquationS);


			Print("---------");


			string[] EquationW = new string[] { "2*x=8", "x = 4" };
			string[] EquationX = new string[] { "x/2=8", "x = 16" };
			string[] EquationY = new string[] { "2/x=8", "x = 0" };
			string[] EquationZ = new string[] { "x-2=8", "x = 10" };
			TestCombineArithmeticHelper(EquationW);
			TestCombineArithmeticHelper(EquationX);
			TestCombineArithmeticHelper(EquationY);
			TestCombineArithmeticHelper(EquationZ);

			Print("---------");

			string[] Equation1 = new string[] { "2/y=6", "y = 0" };
			string[] Equation2 = new string[] { "y-2=6", "y = 8" };
			string[] Equation3 = new string[] { "2-y=6", "y = -4" };
			string[] Equation4 = new string[] { "y/2=6", "y = 12" };
			TestCombineArithmeticHelper(Equation1);
			TestCombineArithmeticHelper(Equation2);
			TestCombineArithmeticHelper(Equation3);
			TestCombineArithmeticHelper(Equation4);

			Print("---------");


		}

		private void TestCombineArithmeticHelper(string[] equation)
		{
			Problem prob = new Problem(new string[] { equation[0] });
			Print(prob.ToString());
			Solver solver = new Solver(prob, Print);
			solver.Solve();

			string resultExpected = equation[1].Replace(" ", "");
			string resultActual = "";

			if (solver.Solutions.Any())
			{
				resultActual = solver.Solutions[0].ToString();
			}
			else
			{
				resultActual = prob.ToString();
			}

			Print($"Result: [{resultActual}]           Expecting: ({equation[1]})");
			Print("---");
			Print();

			Assert.AreEqual(resultExpected, resultActual);
		}


		/*
		[TestMethod]
		public void TestCombineArithmeticElements()
		{

			string equation0 = "1+9";
			string equation1 = "-8*2+7";
			string equation2 = "x+5*2";
			string equation3 = "2*7^y";
			string equation4 = "x+13+7^y";


			string equation5 = "y+x+4+9*3+2*y^2";
			string equation6 = "y+x+4+9*3+2";
			string equation7 = "7*4+9*3+2-7/8";

			string equation8 = "y+4*x-9/z+2*y^2";

			CombineArithmeticElements(equation0);
			Print("----");
			CombineArithmeticElements(equation1);
			Print("----");
			CombineArithmeticElements(equation2);
			Print("----");
			CombineArithmeticElements(equation3);
			Print("----");
			CombineArithmeticElements(equation4);
			Print("----");
			CombineArithmeticElements(equation5);
			Print("----");
			CombineArithmeticElements(equation6);
			Print("----");
			CombineArithmeticElements(equation7);
			Print("----");
			CombineArithmeticElements(equation8);
		}


		public void CombineArithmeticElements(string expression)
		{
			Expression exp = new Expression(expression);
			Print(exp.ToString());
			Print();
			exp.CombineArithmeticElements();
			Print();
			Print(exp.ToString());
			Print();
		}
		*/

		/*
		[TestMethod]
		public void TestNegativeNumbers()
		{
			string equation1 = "-8*2+7";

			CombineArithmeticElements(equation1);
		}
		*/

		/*
		[TestMethod]
		public void TestExtractionInsertion_Addition()
		{
			string eEquation1 = "x+5=10";

			Equation exp = new Equation(eEquation1);
			Print(exp.ToString());

			TermOperatorPair pair = exp.LeftHandSide.Extract(2);

			Print(exp.ToString());
			Print("Extracted: " + pair.ToString());

			exp.RightHandSide.Insert(pair);
			Print(exp.ToString());

			exp.RightHandSide.CombineArithmeticElements();
			Print(exp.ToString());
		}

		[TestMethod]
		public void TestExtractionInsertion_Multiplication()
		{
			string eEquation1 = "2*x=8";

			Equation exp = new Equation(eEquation1);
			Print(exp.ToString());

			TermOperatorPair pair = exp.LeftHandSide.Extract(2);

			Print(exp.ToString());
			Print("Extracted: " + pair.ToString());

			exp.RightHandSide.Insert(pair);
			Print(exp.ToString());

			exp.RightHandSide.CombineArithmeticElements();
			Print(exp.ToString());
		}


		*/


		/*
		[TestMethod]
		public void TestLongestSubsequenceOfArithmeticElements()
		{
			string equation1 = "x+5";
			string equation2 = "y+3*4-x";
			string equation3 = "x*z/2-1/4+y";

			ReportLongestSubsequence(equation1);

			ReportLongestSubsequence(equation2);

			ReportLongestSubsequence(equation3);
		}

		private void ReportLongestSubsequence(string equasion)
		{
			Expression parsed = new Expression(equasion);
			Print();
			Print("----------------------------");
			Print(equasion.ToString());
			ReportSubsequence(parsed);
		}

		private void ReportSubsequence(Expression parsed)
		{

			Print();
			Tuple<List<IElement>, int> resultString = parsed.Elements.FindLongestSubsequenceOfArithmeticElements();
			if (resultString != null && resultString.Item1.Any())
			{
				Print($"{resultString.Item1.AsString()}				(position {resultString.Item2})");
			}
			Print();
		}
		*/







		public void Print(string message = " ") { Print("{0}", message); }


		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }


		private TestContext testContextInstance;
		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }


		[ClassInitializeAttribute]
		public static void Initialize(TestContext context) { }
	}
}
