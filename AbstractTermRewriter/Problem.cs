using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public struct Problem
	{
		public ICollection<ISentence> Statements { get; private set; }

		private string[] _statements;

		public Problem(string[] statements)
		{
			if (statements == null || statements.Length < 1) throw new ArgumentException("statements");

			_statements = statements;
			Statements = _statements.Select(s => Parse(s)).ToList();
		}

		public static ISentence Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException();

			if (input.Any(c => Types.Inequality.Contains(c)))
			{
				string[] expressions = input.Split(Types.Inequality.Select(c => c).ToArray(), StringSplitOptions.RemoveEmptyEntries);
				if (expressions.Length != 2)
				{
					throw new ArgumentException("input must be of the form: [expression] = [expression]");
				}

				EqualityType equalityToken = Types.GetEqualityType(input);
				Expression lhs = new Expression(expressions[0]);
				Expression rhs = new Expression(expressions[1]);
				return (ISentence)new Equation(lhs, equalityToken, rhs);
			}
			else
			{
				return (ISentence)new Expression(input);
			}
		}
	}
}
