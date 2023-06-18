using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public class RewriteRuleCollection : Collection<IRewriteRule>
	{
		public RewriteRuleCollection(IEnumerable<IRewriteRule> collection)
			: base(collection.OrderBy(r => r.ApplyOrder).ToList())
		{
		}

		protected override void InsertItem(int index, IRewriteRule item)
		{
			int newIndex = Items.Select(r => r.ApplyOrder).ToList().FindLastIndex(i => i <= item.ApplyOrder);
			if (newIndex != -1)
			{
				base.InsertItem(newIndex, item);
			}
			else
			{
				base.InsertItem(0, item);
			}
		}
	}
}
