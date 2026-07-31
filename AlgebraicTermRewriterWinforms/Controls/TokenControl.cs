using AlgebraicTermRewriter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgebraicTermRewriterWinforms.Controls
{
	public partial class TokenControl : UserControl
	{
		public IToken Token
		{
			get
			{
				return _token;
			}
			set
			{
				_token = value;
				PopulateControl();
			}
		}
		private IToken _token = null;

		public TokenControl()
		{
			InitializeComponent();
		}

		public TokenControl(IToken token)
			: this()
		{
			Token = token;
		}

		protected virtual void PopulateControl()
		{
			labelToken.Text = _token.ToString();
			labelToken.Tag = _token;
		}
	}
}
