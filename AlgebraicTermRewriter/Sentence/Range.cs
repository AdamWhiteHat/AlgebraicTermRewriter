using System;
using System.Linq;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public class Range
	{
		public static Range Empty = new Range(-1, 0);

		public int StartIndex { get; set; }
		public int Count { get; set; }
		public int EndIndex { get { return StartIndex + Count; } }

		public Range(int startIndex, int count)
		{
			StartIndex = startIndex;
			Count = count;
		}

		public static bool operator ==(Range left, Range right)
		{
			if (left.StartIndex != right.StartIndex)
			{
				return false;
			}
			return left.Count == right.Count;
		}

		public static bool operator !=(Range left, Range right) => !(left == right);

		public bool Equals(Range other) => this == other;

		public override bool Equals(object obj) => (obj == null || GetType() != obj.GetType()) ? false : Equals(obj as Range);

		public override int GetHashCode() => new Tuple<int, int>(StartIndex, Count).GetHashCode();

		public override string ToString() => $"[{StartIndex} -> {EndIndex}]";
	}
}
