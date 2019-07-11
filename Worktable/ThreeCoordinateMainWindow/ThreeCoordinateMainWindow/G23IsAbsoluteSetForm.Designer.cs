namespace ThreeCoordinateMainWindow
{
	partial class G23IsAbsoluteSetForm
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbRelative = new System.Windows.Forms.RadioButton();
			this.rbAbsolute = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(113, 86);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(194, 86);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbRelative);
			this.groupBox1.Controls.Add(this.rbAbsolute);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(256, 67);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "G2/G3插补设置";
			// 
			// rbRelative
			// 
			this.rbRelative.AutoSize = true;
			this.rbRelative.Location = new System.Drawing.Point(6, 42);
			this.rbRelative.Name = "rbRelative";
			this.rbRelative.Size = new System.Drawing.Size(71, 16);
			this.rbRelative.TabIndex = 1;
			this.rbRelative.TabStop = true;
			this.rbRelative.Text = "相对圆心";
			this.rbRelative.UseVisualStyleBackColor = true;
			// 
			// rbAbsolute
			// 
			this.rbAbsolute.AutoSize = true;
			this.rbAbsolute.Location = new System.Drawing.Point(6, 20);
			this.rbAbsolute.Name = "rbAbsolute";
			this.rbAbsolute.Size = new System.Drawing.Size(71, 16);
			this.rbAbsolute.TabIndex = 0;
			this.rbAbsolute.TabStop = true;
			this.rbAbsolute.Text = "绝对圆心";
			this.rbAbsolute.UseVisualStyleBackColor = true;
			// 
			// G23IsAbsoluteSetForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(281, 118);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "G23IsAbsoluteSetForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "圆弧插补设置";
			this.Load += new System.EventHandler(this.G23IsAbsoluteSetForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbRelative;
		private System.Windows.Forms.RadioButton rbAbsolute;
	}
}