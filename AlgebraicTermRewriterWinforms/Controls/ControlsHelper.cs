using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgebraicTermRewriterWinforms.Controls
{
	public static class ControlsHelper
	{
		public static T GetParentControlOfType<T>(Control startingFrom)
			where T : Control
		{
			if (startingFrom == null)
			{
				return null;
			}

			Type targetType = typeof(T);

			T result = null;
			Control parent = startingFrom.Parent;
			while (parent != null)
			{
				result = parent as T;

				if (result != null)
				{
					return result;
				}
				parent = parent.Parent;
			}
			return null;
		}

		public static Control GetFirstParentControlOfType(Control startingFrom, List<Type> targetTypes)
		{
			if (startingFrom == null)
			{
				return null;
			}

			Control parent = startingFrom.Parent;
			while (parent != null)
			{
				foreach (Type targetType in targetTypes)
				{
					if (parent.GetType() == targetType)
					{
						return parent;
					}
				}
				parent = parent.Parent;
			}
			return null;
		}
	}
}
