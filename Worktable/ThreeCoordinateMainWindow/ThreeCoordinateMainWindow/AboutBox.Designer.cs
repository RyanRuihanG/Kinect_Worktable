namespace ThreeCoordinateMainWindow
{
	partial class AboutBox
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.btnBuy = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(185, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "第4组、第7组三坐标工作台上位机";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "SY1707621  郭子为";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "SY1707624  吴晓聪";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 12);
			this.label4.TabIndex = 3;
			this.label4.Text = "SY1707623  张军";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 78);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(107, 12);
			this.label5.TabIndex = 4;
			this.label5.Text = "SY1707603  赵晨希";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(13, 124);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(95, 12);
			this.label6.TabIndex = 5;
			this.label6.Text = "ZY1707604  周骥";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 108);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(101, 12);
			this.label7.TabIndex = 6;
			this.label7.Text = "SY1707607 储瑞航";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(13, 93);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(101, 12);
			this.label8.TabIndex = 7;
			this.label8.Text = "SY1707601 张啸宇";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(13, 139);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(101, 12);
			this.label9.TabIndex = 8;
			this.label9.Text = "SY1707606 杨子豪";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(374, 147);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 9;
			this.button1.Text = "确定";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnBuy
			// 
			this.btnBuy.Location = new System.Drawing.Point(374, 118);
			this.btnBuy.Name = "btnBuy";
			this.btnBuy.Size = new System.Drawing.Size(75, 23);
			this.btnBuy.TabIndex = 10;
			this.btnBuy.Text = "感谢支持";
			this.btnBuy.UseVisualStyleBackColor = true;
			this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
			// 
			// AboutBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(461, 179);
			this.Controls.Add(this.btnBuy);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "关于";
			this.Load += new System.EventHandler(this.AboutBox_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnBuy;
	}
}