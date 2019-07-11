using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreeCoordinateMainWindow
{
	public partial class Buy : Form
	{
		public Buy()
		{
			InitializeComponent();
			picAliPay.Image = AliPay.alipay;
		}

		private void Buy_Load(object sender, EventArgs e)
		{
			rbAliPay.Checked = true;
		}

		private void rbWeChat_CheckedChanged(object sender, EventArgs e)
		{
			if (rbAliPay.Checked == true)
			{
				picAliPay.Image = AliPay.alipay;
			}
			else
			{
				picAliPay.Image = AliPay.wechat;
			}
		}
	}
}
