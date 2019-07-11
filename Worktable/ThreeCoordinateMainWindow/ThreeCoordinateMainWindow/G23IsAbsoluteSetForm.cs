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
	public partial class G23IsAbsoluteSetForm : Form
	{
		private bool _isG23Absolute = false;
		public G23IsAbsoluteSetForm(bool isG23Absolute)
		{
			_isG23Absolute = isG23Absolute;
			InitializeComponent();
		}

		public bool IsG23Absolute { get => _isG23Absolute; set => _isG23Absolute = value; }

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			if (rbAbsolute.Checked == true)//绝对圆心
			{
				_isG23Absolute = true;
			}
			else
			{
				_isG23Absolute = false;
			}
			Close();
		}

		private void G23IsAbsoluteSetForm_Load(object sender, EventArgs e)
		{
			if (_isG23Absolute == true)
			{
				rbAbsolute.Checked = true;
				rbRelative.Checked = false;
			}
			else
			{
				rbRelative.Checked = true;
				rbAbsolute.Checked = false;
			}
		}
	}
}
