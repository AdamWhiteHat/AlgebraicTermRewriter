namespace AlgebraicTermRewriterWinforms.Controls
{
	partial class EquationControl
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
			flowLHS = new System.Windows.Forms.FlowLayoutPanel();
			flowRHS = new System.Windows.Forms.FlowLayoutPanel();
			labelEquals = new System.Windows.Forms.Label();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// flowLHS
			// 
			flowLHS.Location = new System.Drawing.Point(3, 3);
			flowLHS.Margin = new System.Windows.Forms.Padding(0);
			flowLHS.MinimumSize = new System.Drawing.Size(10, 0);
			flowLHS.Name = "flowLHS";
			flowLHS.Padding = new System.Windows.Forms.Padding(3);
			flowLHS.Size = new System.Drawing.Size(10, 35);
			flowLHS.TabIndex = 0;
			// 
			// flowRHS
			// 
			flowRHS.Location = new System.Drawing.Point(27, 3);
			flowRHS.Margin = new System.Windows.Forms.Padding(0);
			flowRHS.MinimumSize = new System.Drawing.Size(10, 0);
			flowRHS.Name = "flowRHS";
			flowRHS.Padding = new System.Windows.Forms.Padding(3);
			flowRHS.Size = new System.Drawing.Size(10, 35);
			flowRHS.TabIndex = 1;
			// 
			// labelEquals
			// 
			labelEquals.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
			labelEquals.AutoSize = true;
			labelEquals.Location = new System.Drawing.Point(13, 3);
			labelEquals.Margin = new System.Windows.Forms.Padding(0);
			labelEquals.Name = "labelEquals";
			labelEquals.Size = new System.Drawing.Size(14, 35);
			labelEquals.TabIndex = 2;
			labelEquals.Text = "=";
			labelEquals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			tableLayoutPanel1.Controls.Add(labelEquals, 1, 0);
			tableLayoutPanel1.Controls.Add(flowRHS, 2, 0);
			tableLayoutPanel1.Controls.Add(flowLHS, 0, 0);
			tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
			tableLayoutPanel1.RowCount = 1;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel1.Size = new System.Drawing.Size(40, 41);
			tableLayoutPanel1.TabIndex = 3;
			// 
			// EquationControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			Controls.Add(tableLayoutPanel1);
			Name = "EquationControl";
			Size = new System.Drawing.Size(40, 41);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLHS;
		private System.Windows.Forms.FlowLayoutPanel flowRHS;
		private System.Windows.Forms.Label labelEquals;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
