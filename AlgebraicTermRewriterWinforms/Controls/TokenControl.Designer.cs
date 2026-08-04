namespace AlgebraicTermRewriterWinforms.Controls
{
	partial class TokenControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			labelToken = new System.Windows.Forms.Label();
			SuspendLayout();
			// 
			// labelToken
			// 
			labelToken.AutoSize = true;
			labelToken.Location = new System.Drawing.Point(0, 0);
			labelToken.Margin = new System.Windows.Forms.Padding(0);
			labelToken.MinimumSize = new System.Drawing.Size(6, 35);
			labelToken.Name = "labelToken";
			labelToken.Size = new System.Drawing.Size(14, 35);
			labelToken.TabIndex = 0;
			labelToken.Text = "1";
			labelToken.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TokenControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			Controls.Add(labelToken);
			Margin = new System.Windows.Forms.Padding(0);
			Name = "TokenControl";
			Size = new System.Drawing.Size(14, 35);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label labelToken;
	}
}
