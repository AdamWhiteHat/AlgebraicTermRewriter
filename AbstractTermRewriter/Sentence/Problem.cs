using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public struct Problem
	{
		public List<ISentence> Statements { get; private set; }

		private string[] _statements;

		public Problem(string[] statements)
		{
			Statements = new List<ISentence>();

			if (statements == null || statements.Length < 1) throw new ArgumentException("You must pass at least one statement.");

			_statements = statements;
			Statements = _statements.Select(s => Parse(s)).ToList();
		}
		
		public static ISentence Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException();

			if (input.Any(c => Types.Comparative.Contains(c)))
			{
				return (ISentence)new Equation(input);
			}
			else
			{
				return (ISentence)new Expression(input);
			}
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			foreach (ISentence statement in Statements)
			{
				if (statement == null) continue;

				if (statement.GetType() == typeof(Expression))
				{
					Expression ex = (Expression)statement;
					result.AppendLine(ex.ToString());
				}
				else if (statement.GetType() == typeof(Equation))
				{
					Equation eq = (Equation)statement;
					result.AppendLine(eq.ToString());
				}
			}

			return result.ToString();
		}
	}
}
