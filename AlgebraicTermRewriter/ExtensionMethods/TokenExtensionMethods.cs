
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public static class TokenCollectionExtensionMethods
	{
		public static Range GetLongestArithmeticRange(this IEnumerable<IToken> source)
		{
			int startIndex = -1;

			int currentIndex = -1;

			bool isSequence = false;
			int sequenceCount = 0;

			List<Range> results = new List<Range>();
			foreach (IToken e in source)
			{
				currentIndex++;

				if (e.Type == TokenType.Number)
				{
					if (isSequence != true)
					{
						if (startIndex == -1)
						{
							startIndex = currentIndex;

							sequenceCount = 1;
							continue;
						}
						else
						{
							isSequence = true;
						}
					}
					sequenceCount += 2;
				}
				else if (e.Type == TokenType.Operator)
				{
					if (currentIndex == 0)
					{
						if (e.Contents == "-" || e.Contents == "+")
						{
							startIndex = currentIndex;
						}
					}
					continue;
				}
				else
				{
					if (isSequence == true)
					{
						results.Add(new Range(startIndex, sequenceCount));
					}

					startIndex = -1;
					sequenceCount = 0;
					isSequence = false;
				}
			}

			if (isSequence == true)
			{
				results.Add(new Range(startIndex, sequenceCount));
			}

			if (results.Any())
			{
				int maxCount = results.Max(r => r.Count);
				return results.Find(r => r.Count == maxCount);
			}

			return Range.Empty;
		}

		public static Tuple<List<IToken>, int> FindLongestSubsequenceOfArithmeticTokens(this IEnumerable<IToken> source)
		{
			return source.FindLongestSubsetWhere((e) => e.Type == TokenType.Number || e.Type == TokenType.Operator);
		}

		public static Tuple<List<IToken>, int> FindLongestSubsetWhere(this IEnumerable<IToken> source, Func<IToken, bool> predicate)
		{
			int counter = 0;

			List<Tuple<List<IToken>, int>> subSets = new List<Tuple<List<IToken>, int>>();
			List<IToken> sequence = new List<IToken>();

			var acceptResultBehavior = new Action<List<IToken>, int>((lst, cntr) =>
			{
				if (lst.Any())
				{
					subSets.Add(new Tuple<List<IToken>, int>(lst, cntr - lst.Count()));
				}
			});

			foreach (IToken e in source)
			{
				counter++;

				if (predicate.Invoke(e))
				{
					if (sequence.Count() != 0 || e.Type == TokenType.Number)
					{
						sequence.Add(e);
					}
				}
				else
				{
					acceptResultBehavior.Invoke(sequence, counter);
					sequence = new List<IToken>();
				}
			}

			acceptResultBehavior.Invoke(sequence, counter);



			if (!subSets.Any())
			{
				return new Tuple<List<IToken>, int>(new List<IToken>(), -1);
			}

			int longest = subSets.Max(l => l.Item1.Count());

			Tuple<List<IToken>, int> result = subSets.Where(l => l.Item1.Count() == longest).FirstOrDefault();

			if (result != null)
			{
				var lst = result.Item1;
				if (lst.Last().Type == TokenType.Operator)
				{
					lst.Remove(lst.Last());
					return new Tuple<List<IToken>, int>(lst, result.Item2);
				}
			}

			return result;
		}
	}

	public static class TokenExtensionMethods
	{
		public static string AsString(this IToken source)
		{
			return source.Contents;
		}

		public static string AsString(this IEnumerable<IToken> source)
		{
			return string.Join(" ", source.Select(e => e.AsString()));
		}
	}

	public static class OperatorExtensionMethods
	{
		public static int RankOperator(this IOperator source)
		{
			if (source.Symbol == '+' || source.Symbol == '-')
			{
				return 0;
			}
			else if (source.Symbol == '*' || source.Symbol == '/')
			{
				return 1;
			}
			else if (source.Symbol == '^')
			{
				return 3;
			}
			else
			{
				throw new Exception($"Did you add a new {typeof(Operator)}?");
			}
		}
	}
}