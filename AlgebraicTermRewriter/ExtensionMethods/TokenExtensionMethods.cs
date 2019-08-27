using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public static class TokenCollectionExtensionMethods
	{
		public static Tuple<int, int> GetLongestArithmeticRange(this IEnumerable<IToken> source)
		{
			int startIndex = -1;

			int currentIndex = -1;
			
			bool isSequence = false;
			int sequenceCount = 0;

			List<Tuple<int, int>> results = new List<Tuple<int, int>>();
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
					//continue;
				}
				else
				{
					if (isSequence == true)
					{
						results.Add(new Tuple<int, int>(startIndex, sequenceCount));
					}

					startIndex = -1;
					sequenceCount = 0;
					isSequence = false;
				}				
			}

			if (isSequence == true)
			{
				results.Add(new Tuple<int, int>(startIndex, sequenceCount));
			}

			if (results.Any())
			{
				List<int> lengths = results.Select(t => t.Item2 - t.Item1).ToList();

				int maxLength = lengths.Max();

				return results[lengths.IndexOf(maxLength)];
			}

			return null;
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


			int longest = subSets.Count < 1 ? 0 : subSets.Max(l => l.Item1.Count());

			if (longest > 0)
			{

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

			return new Tuple<List<IToken>, int>(new List<IToken>(), -1);
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

}