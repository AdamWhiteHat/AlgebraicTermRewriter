using AlgebraicTermRewriter;

namespace AlgebraicTermRewriterWinforms
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			equationControl1 = new AlgebraicTermRewriterWinforms.Controls.EquationControl();
			textBoxInput = new System.Windows.Forms.TextBox();
			buttonGo = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			errorProviderInput = new System.Windows.Forms.ErrorProvider(components);
			((System.ComponentModel.ISupportInitialize)errorProviderInput).BeginInit();
			SuspendLayout();
			// 
			// equationControl1
			// 
			equationControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
			equationControl1.AutoSize = true;
			equationControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			equationControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			equationControl1.Location = new System.Drawing.Point(119, 87);
			equationControl1.Margin = new System.Windows.Forms.Padding(0);
			equationControl1.Name = "equationControl1";
			equationControl1.Padding = new System.Windows.Forms.Padding(3);
			equationControl1.Size = new System.Drawing.Size(94, 49);
			equationControl1.TabIndex = 0;
			// 
			// textBoxInput
			// 
			textBoxInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			textBoxInput.Location = new System.Drawing.Point(28, 35);
			textBoxInput.Name = "textBoxInput";
			textBoxInput.Size = new System.Drawing.Size(210, 21);
			textBoxInput.TabIndex = 1;
			textBoxInput.Text = "(X + 45) * 7 = 21";
			textBoxInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			textBoxInput.KeyUp += textBoxInput_KeyUp;
			// 
			// buttonGo
			// 
			buttonGo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			buttonGo.Location = new System.Drawing.Point(244, 32);
			buttonGo.Name = "buttonGo";
			buttonGo.Size = new System.Drawing.Size(75, 25);
			buttonGo.TabIndex = 2;
			buttonGo.Text = "Go";
			buttonGo.UseVisualStyleBackColor = true;
			buttonGo.Click += buttonGo_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(11, 17);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(59, 15);
			label1.TabIndex = 3;
			label1.Text = "Equation:";
			// 
			// errorProviderInput
			// 
			errorProviderInput.ContainerControl = this;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(332, 159);
			Controls.Add(label1);
			Controls.Add(buttonGo);
			Controls.Add(textBoxInput);
			Controls.Add(equationControl1);
			MaximumSize = new System.Drawing.Size(1900, 200);
			MinimumSize = new System.Drawing.Size(350, 200);
			Name = "MainForm";
			Padding = new System.Windows.Forms.Padding(10);
			Text = "Algebraic Term Rewriter";
			((System.ComponentModel.ISupportInitialize)errorProviderInput).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Controls.EquationControl equationControl1;
		private System.Windows.Forms.TextBox textBoxInput;
		private System.Windows.Forms.Button buttonGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ErrorProvider errorProviderInput;
	}
}