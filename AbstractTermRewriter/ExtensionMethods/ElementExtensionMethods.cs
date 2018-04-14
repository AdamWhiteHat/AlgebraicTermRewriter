using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public static class ElementCollectionExtensionMethods
	{
		public static Tuple<int, int> GetLongestArithmeticRange(this IEnumerable<IElement> source)
		{
			int startIndex = -1;

			int currentIndex = -1;
			
			bool isSequence = false;
			int sequenceCount = 0;

			List<Tuple<int, int>> results = new List<Tuple<int, int>>();
			foreach (IElement e in source)
			{
				currentIndex++;

				if (e.Type == ElementType.Number)
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
				else if (e.Type == ElementType.Operator)
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

		public static Tuple<List<IElement>, int> FindLongestSubsequenceOfArithmeticElements(this IEnumerable<IElement> source)
		{
			return source.FindLongestSubsetWhere((e) => e.Type == ElementType.Number || e.Type == ElementType.Operator);
		}

		public static Tuple<List<IElement>, int> FindLongestSubsetWhere(this IEnumerable<IElement> source, Func<IElement, bool> predicate)
		{
			int counter = 0;
			int length = source.Count();

			List<Tuple<List<IElement>, int>> subSets = new List<Tuple<List<IElement>, int>>();
			List<IElement> sequence = new List<IElement>();

			var acceptResultBehavior = new Action<List<IElement>, int>((lst, cntr) =>
			{
				if (lst.Any())
				{
					subSets.Add(new Tuple<List<IElement>, int>(lst, cntr - lst.Count()));
				}
			});

			foreach (IElement e in source)
			{
				counter++;

				if (predicate.Invoke(e))
				{
					if (sequence.Count() != 0 || e.Type == ElementType.Number)
					{
						sequence.Add(e);
					}
				}
				else
				{
					acceptResultBehavior.Invoke(sequence, counter);
					sequence = new List<IElement>();
				}
			}

			acceptResultBehavior.Invoke(sequence, counter);


			int longest = subSets.Count < 1 ? 0 : subSets.Max(l => l.Item1.Count());

			if (longest > 0)
			{

				Tuple<List<IElement>, int> result = subSets.Where(l => l.Item1.Count() == longest).FirstOrDefault();

				if (result != null)
				{
					var lst = result.Item1;
					if (lst.Last().Type == ElementType.Operator)
					{
						lst.Remove(lst.Last());
						return new Tuple<List<IElement>, int>(lst, result.Item2);
					}
				}

				return result;
			}

			return new Tuple<List<IElement>, int>(new List<IElement>(), -1);
		}
	}

	public static class ElementExtensionMethods
	{
		public static string AsString(this IElement source)
		{
			return source.Symbol;
		}

		public static string AsString(this IEnumerable<IElement> source)
		{
			return string.Join(" ", source.Select(e => e.AsString()));
		}
	}

}