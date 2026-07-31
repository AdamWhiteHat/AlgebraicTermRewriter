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
			labelToken.Location = new System.Drawing.Point(3, 3);
			labelToken.Margin = new System.Windows.Forms.Padding(0);
			labelToken.Name = "labelToken";
			labelToken.Padding = new System.Windows.Forms.Padding(3);
			labelToken.Size = new System.Drawing.Size(20, 21);
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
			Name = "TokenControl";
			Padding = new System.Windows.Forms.Padding(3);
			Size = new System.Drawing.Size(26, 27);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label labelToken;
	}
}
