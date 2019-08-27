using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgebraicTermRewriter;

namespace RewriterTests
{
	[TestClass]
	public class SimpleEquations
	{

		[TestMethod]
		public void TestSolveBasicEquations()
		{
			string[] EquationA = new string[] { "x-5=10", "x = 15" };
			string[] EquationB = new string[] { "10=x-5", "x = 15" };
			string[] EquationC = new string[] { "5-x=10", "x = -5" };
			string[] EquationD = new string[] { "10=5-x", "x = -5" };


			TestSolveArithmeticHelper(EquationA);
			TestSolveArithmeticHelper(EquationB);
			TestSolveArithmeticHelper(EquationC);
			TestSolveArithmeticHelper(EquationD);

			Print("---------");

			string[] EquationP = new string[] { "2/y=6", "y = 0" };
			string[] EquationQ = new string[] { "6=2/y", "y = 0" };
			string[] EquationR = new string[] { "y/2=6", "y = 12" };
			string[] EquationS = new string[] { "6=y/2", "y = 12" };
			TestSolveArithmeticHelper(EquationP);
			TestSolveArithmeticHelper(EquationQ);
			TestSolveArithmeticHelper(EquationR);
			TestSolveArithmeticHelper(EquationS);


			Print("---------");


			string[] EquationW = new string[] { "2*x=8", "x = 4" };
			string[] EquationX = new string[] { "x/2=8", "x = 16" };
			string[] EquationY = new string[] { "2/x=8", "x = 0" };
			string[] EquationZ = new string[] { "x-2=8", "x = 10" };
			TestSolveArithmeticHelper(EquationW);
			TestSolveArithmeticHelper(EquationX);
			TestSolveArithmeticHelper(EquationY);
			TestSolveArithmeticHelper(EquationZ);

			Print("---------");

			string[] Equation1 = new string[] { "2/y=6", "y = 0" };
			string[] Equation2 = new string[] { "y-2=6", "y = 8" };
			string[] Equation3 = new string[] { "2-y=6", "y = -4" };
			string[] Equation4 = new string[] { "y/2=6", "y = 12" };
			TestSolveArithmeticHelper(Equation1);
			TestSolveArithmeticHelper(Equation2);
			TestSolveArithmeticHelper(Equation3);
			TestSolveArithmeticHelper(Equation4);

			Print("---------");


		}

		[TestMethod]
		public void TestSolveStandardEquations()
		{
			string[] EquationA = new string[] { "2 * x + 4 = 10", "x = 3" };
			TestSolveArithmeticHelper(EquationA);
		}

		/*	NOT IMPLEMENTED YET
		[TestMethod]
		public void TestSolveRootsEquations()
		{
			string[] EquationB = new string[] { "2 * y ^ 2 - 2 = 48", "y = 5" };
			TestSolveArithmeticHelper(EquationB);
		}
		*/

		/*	NOT IMPLEMENTED YET
		
		[TestMethod]
		public void TestSolveForVariablesOnBothSide()
		{
			// Not Implemented yet
			string[] EquationA = new string[] { "5 * x - 6 = 3 * x - 8", "x = -1" };

			TestSolveArithmeticHelper(EquationA);
		}
		*/

		private void TestSolveArithmeticHelper(string[] equation)
		{
			Problem prob = new Problem(new string[] { equation[0] });
			Print(prob.ToString());

			Solver solver = new Solver(prob, Print);
			solver.Solve();

			string resultExpected = equation[1];
			string resultActual = "";

			if (solver.Solutions.Any())
			{
				resultActual = solver.Solutions[0];
			}
			else
			{
				resultActual = prob.ToString();
			}

			Print($"Result: [{resultActual}]           Expecting: ({equation[1]})");
			Print();
			Print("-----");
			Print();

			Assert.AreEqual(resultExpected, resultActual);
		}



		[TestMethod]
		public void TestCombineArithmeticTokens()
		{
			string equation0 = "x+5*2";
			string expected0 = "x + 10";

			string equation1 = "-8*2+7";
			string expected1 = "-9";

			string equation2 = "1+9";
			string expected2 = "10";

			string equation3 = "2*7^y";
			string expected3 = "14 ^ y";

			string equation4 = "x+13+7^y";
			string expected4 = "x + 20 ^ y";

			string equation5 = "y+x+4+9*3+2*y^2";
			string expected5 = "y + x + 33 * y ^ 2";

			string equation6 = "y+x+4+9*3+2";
			string expected6 = "y + x + 33";

			string equation7 = "7*4+9*3+2-56/8";
			string expected7 = "50";

			string equation8 = "y+4*x-9/z+2*y^2";
			string expected8 = "y + 4 * x - 9 / z + 2 * y ^ 2";

			CombineArithmeticTokens(equation0, expected0);
			CombineArithmeticTokens(equation1, expected1);
			CombineArithmeticTokens(equation2, expected2);
			CombineArithmeticTokens(equation3, expected3);
			CombineArithmeticTokens(equation4, expected4);
			CombineArithmeticTokens(equation5, expected5);
			CombineArithmeticTokens(equation6, expected6);
			CombineArithmeticTokens(equation7, expected7);
			CombineArithmeticTokens(equation8, expected8);
		}


		public void CombineArithmeticTokens(string expressionString, string expected)
		{
			Expression exp = MathParser.ParseExpression(expressionString);

			Print("-----");
			Print();
			Print($"Input: {exp.ToString()}");
			Print();

			exp.CombineArithmeticTokens();
			Print($"Result: [{exp.ToString()}]           Expecting: ({expected})");
			Print();

			Assert.AreEqual(expected, exp.ToString());
		}


		[TestMethod]
		public void TestUnaryOperators()
		{
			string equation1 = "-8*2+7";
			string expected1 = "-9";

			CombineArithmeticTokens(equation1, expected1);
		}


		[TestMethod]
		public void TestExtractionInsertion_Addition()
		{
			string eEquation1 = "x+5=10";
			string expected = "x = 5";

			Equation exp = MathParser.ParseEquation(eEquation1);
			Print(exp.ToString());

			TermOperatorPair pair = exp.LeftHandSide.Extract((ITerm)exp.LeftHandSide.TokenAt(2), (IOperator)exp.LeftHandSide.TokenAt(1));

			Print(exp.ToString());
			Print("Extracted: " + pair.ToString());

			exp.RightHandSide.Insert(pair);
			Print(exp.ToString());

			exp.RightHandSide.CombineArithmeticTokens();
			Print(exp.ToString());

			Assert.AreEqual(expected, exp.ToString());
		}

		[TestMethod]
		public void TestExtractionInsertion_Multiplication()
		{
			string eEquation1 = "2*x=8";
			string expected = "x = 4";

			Equation exp = MathParser.ParseEquation(eEquation1);
			Print(exp.ToString());

			TermOperatorPair pair = exp.LeftHandSide.Extract((ITerm)exp.LeftHandSide.TokenAt(0), (IOperator)exp.LeftHandSide.TokenAt(1));

			Print(exp.ToString());
			Print("Extracted: " + pair.ToString());

			exp.RightHandSide.Insert(pair);
			Print(exp.ToString());

			exp.RightHandSide.CombineArithmeticTokens();
			Print(exp.ToString());

			Assert.AreEqual(expected, exp.ToString());
		}






		[TestMethod]
		public void TestLongestSubsequenceOfArithmeticTokens()
		{
			string equation1 = "x+5";
			string equation2 = "y+3*4-x";
			string equation3 = "x*z/2-1/4+y";

			ReportLongestSubsequence(equation1);

			ReportLongestSubsequence(equation2);

			ReportLongestSubsequence(equation3);
		}

		private void ReportLongestSubsequence(string equation)
		{
			Expression parsed = MathParser.ParseExpression(equation);
			Print();
			Print("----------------------------");
			Print(parsed.ToString());
			ReportSubsequence(parsed);
		}

		private void ReportSubsequence(Expression parsed)
		{

			Print();
			Tuple<List<IToken>, int> resultString = parsed.Tokens.FindLongestSubsequenceOfArithmeticTokens();
			if (resultString != null && resultString.Item1.Any())
			{
				Print($"{resultString.Item1.AsString()}				(position {resultString.Item2})");
			}
			Print();
		}








		public void Print(string message = " ") { Print("{0}", message); }


		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }


		private TestContext testContextInstance;
		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }


		[ClassInitializeAttribute]
		public static void Initialize(TestContext context) { }
	}
}
