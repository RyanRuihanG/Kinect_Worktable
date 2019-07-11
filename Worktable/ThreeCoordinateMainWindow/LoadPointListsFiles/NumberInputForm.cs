using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadPointListsFiles
{
	public partial class NumberInputForm<T> : Form
	{
		private T _getNumber;//向外传递用户输入数字
		private bool _canBeNegative = true;//是否允许负数的标志
		private bool _canBeFraction = true;//是否允许小数
		public NumberInputForm(bool canBeNegative,bool canBeFraction)
		{
			_canBeNegative = canBeNegative;
			_canBeFraction = canBeFraction;
			InitializeComponent();
		}
		public NumberInputForm(bool canBeNegative, bool canBeFraction, T defaultNumber)
		{
			_canBeNegative = canBeNegative;
			_canBeFraction = canBeFraction;
			InitializeComponent();
			tbInput.Text = defaultNumber.ToString();
		}

		public T GetNumber { get => _getNumber; }

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (tbInput.Text != "")
			{
				DialogResult = DialogResult.OK;
				if (_getNumber is int)
				{
					_getNumber = (T)(Object)Convert.ToInt32(tbInput.Text);
				}
				else if (_getNumber is double)
				{
					_getNumber = (T)(Object)Convert.ToDouble(tbInput.Text);
				}
				Close();
			}
		}

		private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
		{
			bool dotHaveBeenInput = false;
			if (_canBeNegative == true)//允许负数
			{
				if ((tbInput.Text == "") && e.KeyChar == (char)45)//如果第一个输入的是负号
				{
					return;
				}
			}
			if (e.KeyChar == '.')
			{
				if (!(_canBeFraction == true && dotHaveBeenInput == false && tbInput.Text != ""))//不是第一个，允许小数，之前没收入过点
				{
					e.Handled = true;
					if (_canBeFraction == true)
					{
						MessageBox.Show("小数点不合法");
					}
					else
					{
						MessageBox.Show("不允许输入小数");
					}
				}
			}
			//如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
			else if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
			{
				e.Handled = true;
				MessageBox.Show("请输入数字");
			}
		}
	}
}
