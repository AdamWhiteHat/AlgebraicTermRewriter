using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using AlgebraicTermRewriter;

namespace RewriterTests
{
	[TestFixture]
	public class BasicFunctions
	{
		static string[] problemStatement =
			   new string[]
			   {
					" 2 * x + 4 = 10",
					" 3 * y ^ 2 - 2 = 20",
			   };

		static Problem testProblem;
		static Equation testEquation;
		static Expression exp;

		[SetUp]
		public static void Initialize()
		{
			testProblem = new Problem(problemStatement);
			testEquation = (Equation)testProblem.Statements[0];
			exp = testEquation.LeftHandSide;
		}

		[Test]
		public void TestInitializedSucceeded()
		{
			Assert.IsNotNull(testProblem);
			Assert.IsNotNull(testEquation);
			Assert.IsNotNull(exp);
			Print(problemStatement[0]);
			Print("=");
			Print(testEquation);
			Print();
			Print(exp);
			Print();
		}


		[Test]
		public void TestIsIsolatedMethod()
		{
			Equation expr_true1 = Equation.Parse("x = 10");
			Equation expr_true2 = Equation.Parse("x = 2 * 5");
			Equation expr_true3 = Equation.Parse("x = 4");

			Equation expr_false1 = Equation.Parse("x + 4 = 14");
			Equation expr_false2 = Equation.Parse("x - 3 = 7");
			Equation expr_false3 = Equation.Parse("x + y = 13");
			Equation expr_false4 = Equation.Parse("x - 4 = 2 * y");

			Assert.IsTrue(expr_true1.IsVariableIsolated, expr_true1.ToString());
			Assert.IsTrue(expr_true2.IsVariableIsolated, expr_true2.ToString());
			Assert.IsTrue(expr_true3.IsVariableIsolated, expr_true3.ToString());

			Assert.IsFalse(expr_false1.IsVariableIsolated, expr_false1.ToString());
			Assert.IsFalse(expr_false2.IsVariableIsolated, expr_false2.ToString());
			Assert.IsFalse(expr_false3.IsVariableIsolated, expr_false3.ToString());
			Assert.IsFalse(expr_false4.IsVariableIsolated, expr_false4.ToString());
		}

		[Test]
		public void TestExpressionExtensionMethods()
		{
			Expression expr = Expression.Parse("2 * x + 3");

			IToken elm = expr.TokenAt(2);
			Assert.AreEqual(TokenType.Variable, elm.Type);
			Variable var = (Variable)elm;

			Print(expr);
			Print("---");
			Print();
			Print($"Selected token: {var}");
			Print();
			Print($"RightOfToken: {expr.RightOfToken(var)}");
			Print($"LeftOfToken:  {expr.LeftOfToken(var)}");
			Print();
			Print("---");
			Print();

			IToken firstElm = expr.TokenAt(0);
			IToken lastElm = expr.TokenAt(expr.TokenCount - 1);

			Print(expr.LeftOfToken(firstElm));
			Print(expr.RightOfToken(lastElm));
		}


		[Test]
		public void TestGatherOperatorNumberPairs()
		{
			Expression leftHandSide = Expression.Parse("2 * x + 4");
			Print(leftHandSide);
			Print();

			List<Tuple<IOperator, INumber>> pairs = leftHandSide.GetOperatorTermIndexPairs();

			Print(string.Join(Environment.NewLine, pairs.Select(tup => $"[op = {tup.Item1} ; num = {tup.Item2}]")));
		}

		[Test]
		public void TestSimplify_AddVariableGroups()
		{
			// "5 * x + 3 * x"
			string expected = "8 * x";
			Expression expr = new Expression(new IToken[] { new Number(5), new Operator('*'), new Variable('x'), new Operator('+'), new Number(3), new Operator('*'), new Variable('x') });

			var result = SimplifyEquation.Simplify(expr);
			TestAssertResults(result, expected);
		}

		[Test]
		public void TestSimplify_UnitaryOperations()
		{
			string expected = "14";
			Expression expr = new Expression(new IToken[] { new Operator('+'), new Number(14) });

			var result = SimplifyEquation.Simplify(expr);
			TestAssertResults(result, expected);
		}

		[Test]
		public void TestExtractToken()
		{
			Expression e1 = exp.Clone();
			Expression e2 = exp.Clone();
			Expression e3 = exp.Clone();

			Print(ExtractInfo(e1, 0));
			Print();
			Print(ExtractInfo(e2, 4));
			Print();
			Print(ExtractInfo(e3, 2));
			Print();
		}

		string ExtractInfo(Expression e, int index)
		{
			return
			$"Index: {index}{Environment.NewLine}{Environment.NewLine}" +
			$"Before: {e}{Environment.NewLine}" +
			//$"Extract: {index.AsString()}{Environment.NewLine}" +
			$"After: {e}{Environment.NewLine}" +
			$"---{Environment.NewLine}";
		}

		[Test]
		public void TestTypeResolution()
		{
			IToken isNum = new Number(3);
			IToken isOp = new Operator('*');
			IToken isVar = new Variable('x');

			INumber number = new Number(2);
			IToken isToken = Token.None;

			Assert.AreEqual(isNum.Type, TokenType.Number, "isNum.Type, TokenType.Number  ");
			Assert.AreEqual(isOp.Type, TokenType.Operator, "isOp.Type,  TokenType.Operator");
			Assert.AreEqual(isVar.Type, TokenType.Variable, "isVar.Type, TokenType.Variable");


			IToken tokenFromNumber = number as IToken;
			Assert.IsNotNull(tokenFromNumber, "IToken tokenFromNumber = number as IToken");


			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(INumber)), "	typeof(IToken).IsAssignableFrom(typeof(INumber))");
			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(IOperator)), "typeof(IToken).IsAssignableFrom(typeof(IOperator)) ");
			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(IVariable)), "typeof(IToken).IsAssignableFrom(typeof(IVariable))");
			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(ITerm)), "typeof(IToken).IsAssignableFrom(typeof(ITerm))");
			Assert.IsTrue(typeof(ITerm).IsAssignableFrom(typeof(INumber)), "typeof(ITerm).IsAssignableFrom(typeof(INumber))");
			Assert.IsTrue(typeof(ITerm).IsAssignableFrom(typeof(IVariable)), "typeof(ITerm).IsAssignableFrom(typeof(IVariable))");


			IOperator opMulti = new Operator('*');
			IVariable varY = new Variable('y');
			INumber numSeven = new Number(7);

			List<IToken> mixedTokens = new List<IToken>();
			mixedTokens.Add(opMulti);
			mixedTokens.Add(varY);
			mixedTokens.Add(numSeven);

			Assert.IsTrue(mixedTokens[0] is IOperator, "tokens[0] is IOperator");
			Assert.IsTrue(mixedTokens[1] is IVariable, "tokens[1] is IVariable");
			Assert.IsTrue(mixedTokens[2] is INumber, "tokens[2] is INumber  ");
			Assert.IsTrue(mixedTokens[2] is IToken, "tokens[2] is IToken  ");


			mixedTokens.Add(number);
			mixedTokens.Add(isToken);
			mixedTokens.Add(isNum);
			mixedTokens.Add(isOp);
			mixedTokens.Add(isVar);

			int termCount = mixedTokens.Where(e => e is ITerm).Count();

			Assert.AreEqual(5, termCount);


			IEnumerable<IToken> col = mixedTokens.ToList();

			int complexity = exp.RankComplexity();
			Print($"Complexity: {complexity}");
		}

		private void TestAssertResults(Expression expr, string expected)
		{
			string actual = expr.ToString();

			Print($"Result: [{actual}]           Expecting: ({expected})");
			Print();
			Print("-----");
			Print();

			Assert.AreEqual(expected, actual);
		}

		public void Print(object obj) { Print("{0}", obj); }

		public void Print(string message = " ") { TestContext.WriteLine(message); }

		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }

		private TestContext testContextInstance;

		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

	}
}
