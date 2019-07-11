using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreeCoordinateMainWindow
{
	public partial class AboutBox : Form
	{
		public AboutBox()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnBuy_Click(object sender, EventArgs e)
		{
			Buy formB = new Buy();
			formB.Show();
		}

		private void AboutBox_Load(object sender, EventArgs e)
		{
			string _tpath = "LicDoc.mlic";

			try
			{
				StreamReader file = new StreamReader(_tpath, System.Text.Encoding.Default);
				string line;

				line = file.ReadLine();
				if (line == "13071137")
				{
					btnBuy.Enabled = false;
					btnBuy.Visible = false;
				}
			}
			catch
			{

			}
		}
	}
}
