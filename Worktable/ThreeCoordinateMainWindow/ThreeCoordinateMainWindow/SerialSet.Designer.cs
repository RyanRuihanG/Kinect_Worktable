namespace ThreeCoordinateMainWindow
{
	partial class SerialSet
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
			this.label1 = new System.Windows.Forms.Label();
			this.cbSerial = new System.Windows.Forms.ComboBox();
			this.cbBaudRate = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbDataBits = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbStopBits = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbParity = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnFind = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(241, 106);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(322, 106);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "串口号：";
			// 
			// cbSerial
			// 
			this.cbSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSerial.FormattingEnabled = true;
			this.cbSerial.Location = new System.Drawing.Point(72, 10);
			this.cbSerial.Name = "cbSerial";
			this.cbSerial.Size = new System.Drawing.Size(81, 20);
			this.cbSerial.TabIndex = 3;
			// 
			// cbBaudRate
			// 
			this.cbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBaudRate.FormattingEnabled = true;
			this.cbBaudRate.Items.AddRange(new object[] {
            "1382400",
            "921600",
            "460800",
            "256000",
            "230400",
            "128000",
            "115200",
            "76800",
            "57600",
            "43000",
            "38400",
            "19200",
            "14400",
            "9600",
            "4800",
            "2400",
            "1200"});
			this.cbBaudRate.Location = new System.Drawing.Point(72, 36);
			this.cbBaudRate.Name = "cbBaudRate";
			this.cbBaudRate.Size = new System.Drawing.Size(81, 20);
			this.cbBaudRate.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "波特率：";
			// 
			// cbDataBits
			// 
			this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataBits.FormattingEnabled = true;
			this.cbDataBits.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
			this.cbDataBits.Location = new System.Drawing.Point(236, 36);
			this.cbDataBits.Name = "cbDataBits";
			this.cbDataBits.Size = new System.Drawing.Size(81, 20);
			this.cbDataBits.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(177, 39);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 12);
			this.label3.TabIndex = 6;
			this.label3.Text = "数据位：";
			// 
			// cbStopBits
			// 
			this.cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStopBits.FormattingEnabled = true;
			this.cbStopBits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
			this.cbStopBits.Location = new System.Drawing.Point(72, 62);
			this.cbStopBits.Name = "cbStopBits";
			this.cbStopBits.Size = new System.Drawing.Size(81, 20);
			this.cbStopBits.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 65);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 12);
			this.label4.TabIndex = 8;
			this.label4.Text = "停止位：";
			// 
			// cbParity
			// 
			this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbParity.FormattingEnabled = true;
			this.cbParity.Items.AddRange(new object[] {
            "无",
            "奇校验",
            "偶校验"});
			this.cbParity.Location = new System.Drawing.Point(236, 62);
			this.cbParity.Name = "cbParity";
			this.cbParity.Size = new System.Drawing.Size(81, 20);
			this.cbParity.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(177, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 12);
			this.label5.TabIndex = 10;
			this.label5.Text = "校验位：";
			// 
			// btnFind
			// 
			this.btnFind.Location = new System.Drawing.Point(159, 8);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(75, 23);
			this.btnFind.TabIndex = 13;
			this.btnFind.Text = "查找串口";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// SerialSet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(403, 135);
			this.ControlBox = false;
			this.Controls.Add(this.btnFind);
			this.Controls.Add(this.cbParity);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cbStopBits);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbDataBits);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbBaudRate);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbSerial);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "SerialSet";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SerialSet";
			this.Load += new System.EventHandler(this.SerialSet_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbSerial;
		private System.Windows.Forms.ComboBox cbBaudRate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbDataBits;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbStopBits;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbParity;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnFind;
	}
}