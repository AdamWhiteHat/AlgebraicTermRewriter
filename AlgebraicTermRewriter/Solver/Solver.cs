using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public class Solver
	{
		private Equation currentEquation;
		private Problem equations;
		private Action<string> LoggingMethod;

		private enum OperatorLocation
		{
			Left,
			Right
		}

		public List<string> Solutions { get; private set; }
		public Dictionary<char, int> SolvedVariables { get; private set; }


		private Expression Left { get { return (currentEquation == null) ? Expression.Empty : currentEquation.LeftHandSide; } }
		private Expression Right { get { return (currentEquation == null) ? Expression.Empty : currentEquation.RightHandSide; } }

		private bool LeftHasVariables { get { return Left.Variables.Any(); } }
		private bool RightHasVariables { get { return Right.Variables.Any(); } }
		public Solver(Problem problem)
			: this(problem, null)
		{
		}

		public Solver(Problem problem, Action<string> loggingMethod)
		{
			LoggingMethod = loggingMethod;
			Solutions = new List<string>();
			SolvedVariables = new Dictionary<char, int>();
			equations = problem;
		}

		public void Solve()
		{
			foreach (ISentence sentence in equations.Statements)
			{
				currentEquation = sentence as Equation;
				if (sentence is Equation)
				{
					SolveEquation(sentence as Equation);
				}
				else if (sentence is Expression)
				{
					SolveExpression(sentence as Expression);
				}
			}
		}

		private void SolveEquation(Equation eq)
		{
			if (eq.OnlyArithmeticTokens())
			{
				Solutions.Add(IsArithmeticEquasionTrue(eq).ToString());
				return;
			}

			if (LeftHasVariables && RightHasVariables)
			{
				SolveForVariablesOnBothSide(eq);
				Solutions.Add(eq.ToString());
				return;
			}

			eq.EnsureVariableOnLeft();

			if (LeftHasVariables)
			{
				if (Left.Variables.Count() > 1)
				{
					SolveForMultipleVariables(eq);
				}
				else if (!Left.IsVariableIsolated)
				{
					IsolateSingleVariable(eq);
				}

				if (!Left.IsVariableIsolated)
				{
					throw new Exception($"Failed to isolate LeftHandSide. Equation: '{eq.ToString()}'");
				}

				if (!Right.IsSimplified)
				{
					throw new Exception($"Failed to simplify RightHandSide. Equation: '{eq.ToString()}'");
				}

				Solutions.Add(eq.ToString());
				AddSolvedVariable(Left.Variables.Single(), Right.Numbers.Single());
				return;
			}

			throw new Exception("Not sure what to do here. Equations should have been solved.");
		}

		/// <summary>
		/// Finds all numbers and their associated operations.
		/// For each number it creates a tuple with the operator's precedence and the number's index, in that order.
		/// Returns a list of such tuples, ordered by precedence in ascending order.
		/// </summary>
		/// <returns>A list of tuples of the form: (precedence, index), ordered by precedence in ascending order.</returns>
		private static List<Tuple<IOperator, ITerm>> GetOperatorTermIndexPairs(Expression from)
		{
			var results = new List<Tuple<IOperator, ITerm>>();

			foreach (INumber candidate in from.Numbers)
			{
				ITerm term = candidate;

				int termIndex = from.IndexOf(term);

				IToken op = null;

				if (termIndex == 0)
				{
					op = from.RightOfToken(term);

					if (op.Contents == "/")
					{
						IToken alternative = from.RightOfToken(op);
						term = (ITerm)alternative;
					}
					else if (op.Contents == "+" || op.Contents == "-")
					{
						op = new Operator('+');
					}
				}
				else
				{
					op = from.LeftOfToken(term);
				}

				IOperator operation = op as IOperator;
				if (operation == null)
				{
					throw new Exception("Was expecting to find Operator.");
				}

				results.Add(new Tuple<IOperator, ITerm>(operation, term));
			}

			return results.OrderBy(tup => GetOperatorSolveOrder(tup.Item1))
							.ToList();
		}

		private static int GetOperatorSolveOrder(IOperator operation)
		{
			int weight = ParserTokens.PrecedenceDictionary[operation.Symbol];

			if (operation.Symbol == '/') weight += 1; // Prefer other operations first
			if (operation.Symbol == '^') weight += 2; // One does not simply negate an exponent and move it to the other side...

			return weight;
		}

		private static int GetTermSolveOrder(ITerm term, IOperator operation)
		{
			if (term.Type == TokenType.Variable)
			{
				if (operation.Symbol == '*' || operation.Symbol == '/')
				{
					return 0;
				}
				else
				{
					return 1;
				}
			}
			else if (term.Type == TokenType.Number)
			{
				if (operation.Symbol == '*' || operation.Symbol == '/')
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			return 2;
		}

		private static void IsolateSingleVariable(Equation eq)
		{
			Expression from = null;
			Expression to = null;

			eq.EnsureVariableOnLeft();

			while (true)
			{
				from = eq.LeftHandSide;
				to = eq.RightHandSide;

				if (!from.Numbers.Any())
				{
					break;
				}

				bool moveVariableMultiple = false;
				if ((from.Variables.Any() && to.Variables.Any()) /*|| eq.GetDistinctVariableCount() > 1*/)
				{
					if (
						!(from.Contains("-") || from.Contains("+"))
						&&
						from.Contains("*")
						&&
						from.Numbers.Any()
						&&
						to.Contains("*")
						&&
						to.Numbers.Any()
						)
					{
						moveVariableMultiple = true;
					}
				}

				if (moveVariableMultiple)
				{
					from = eq.RightHandSide;
					to = eq.LeftHandSide;
					MoveVariableTerm(from, to);
				}
				else
				{
					MoveNumberTerm(from, to);
				}

				to.Simplify();
				from.Simplify();

				IToken leadingToken = from.Tokens.First();
				if (leadingToken.Contents == "-")
				{
					to.SetToMultiplicativeInverse();
					from.SetToMultiplicativeInverse();
				}

				eq.EnsureVariableOnLeft();
			}
		}

		private static void MoveVariableTerm(Expression from, Expression to)
		{
			IVariable variable = from.Variables.First();

			SubExpression variableGroup = from.GetVariableProductSubExpression(variable);
			OperatorExpressionPair extracted = from.Extract(new Operator('+'), variableGroup);

			if (extracted != null)
			{
				to.Insert(extracted);
			}
		}

		private static void MoveNumberTerm(Expression from, Expression to)
		{
			List<Tuple<IOperator, ITerm>> OperatorTermIndexList = GetOperatorTermIndexPairs(from);
			if (OperatorTermIndexList.Any())
			{
				Tuple<IOperator, ITerm> next = OperatorTermIndexList.First();

				OperatorExpressionPair extracted = from.Extract(next.Item1, next.Item2);
				to.Insert(extracted);
			}
		}

		private void SolveForMultipleVariables(Equation eq)
		{
			//int leftVarsCount = Left.Variables.Count();
			//int rightVarsCount = Right.Variables.Count();

			//IVariable firstVar = Left.Variables.First();

			IsolateSingleVariable(eq);
		}

		private void SolveForVariablesOnBothSide(Equation eq)
		{
			//IVariable firstVar = Left.Variables.First();

			IsolateSingleVariable(eq);
		}

		private void AddSolvedVariable(IVariable variable, INumber numericValue)
		{
			SolvedVariables.Add(variable.Value, numericValue.Value);
		}

		private bool IsArithmeticEquasionTrue(Equation eq)
		{
			eq.LeftHandSide.Simplify();
			eq.RightHandSide.Simplify();

			var left = eq.LeftHandSide;
			var right = eq.RightHandSide;

			if (!left.IsSimplified || !right.IsSimplified) throw new Exception("Expected both sides of the equation were arithmetic tokens only, but failed to simplify one or both sides.");

			switch (eq.ComparisonOperator)
			{
				case ComparisonType.Equals:
					return left.Value == right.Value;
				case ComparisonType.GreaterThan:
					return left.Value > right.Value;
				case ComparisonType.LessThan:
					return left.Value < right.Value;
				case ComparisonType.GreaterThanOrEquals:
					return left.Value >= right.Value;
				case ComparisonType.LessThanOrEquals:
					return left.Value <= right.Value;
				default:
					throw new Exception();
			}
		}



		/* EXPRESSIONS */

		private void SolveExpression(Expression ex)
		{
			if (ex.OnlyArithmeticTokens())
			{
				ex.Simplify();
				if (!ex.IsSimplified) throw new Exception("Expected the expression was arithmetic tokens only, but failed to simplify.");
				Solutions.Add(ex.ToString());
				PrintStatus();
			}
		}

		private void PrintStatus()
		{
			if (LoggingMethod != null)
			{
				LoggingMethod.Invoke(equations.ToString());
			}
		}

	}
}
