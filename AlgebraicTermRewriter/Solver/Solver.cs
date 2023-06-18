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
					IVariable variable = from.Variables.First();
					MoveVariableTerm(variable, from, to);
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

		public static bool MoveVariableTerm(IVariable variable, Expression from, Expression to)
		{
			SubExpression variableGroup = from.GetVariableProductSubExpression(variable);
			OperatorExpressionPair extracted = from.Extract(new Operator('+'), variableGroup);

			if (extracted != null)
			{
				to.Insert(extracted);
				return true;
			}
			return false;
		}

		public static bool MoveNumberTerm(Expression from, Expression to)
		{
			List<Tuple<IOperator, ITerm>> OperatorTermIndexList = from.GetOperatorTermIndexPairs();
			if (OperatorTermIndexList.Any())
			{
				Tuple<IOperator, ITerm> next = OperatorTermIndexList.First();

				OperatorExpressionPair extracted = from.Extract(next.Item1, next.Item2);
				if (extracted != null)
				{
					to.Insert(extracted);
					return true;
				}
			}
			return false;
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
			SolvedVariables.Add(variable.Symbol, numericValue.Value);
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
