using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResLib
{
    public static class ResLibClass
    {
		/// <summary>
		/// 获得按钮图片
		/// </summary>
		/// <param name="inputCommand">glow down normal</param>
		/// <returns></returns>
		public static Image GetButtonImageEStop(string inputCommand)
		{
			Image tempImg;
			if (inputCommand == "glow")
			{
				tempImg = MainFormRes.btnEstop_Glow;
			}
			else if (inputCommand == "down")
			{
				tempImg = MainFormRes.btnEstop_Down;
			}
			else
			{
				tempImg = MainFormRes.btnEstop_Normal;
			}
			return tempImg;
		}

		/// <summary>
		/// 获得按钮图片
		/// </summary>
		/// <param name="inputCommand">glow down normal</param>
		/// <returns></returns>
		public static Image GetButtonImageStart(string inputCommand)
		{
			Image tempImg;
			if (inputCommand == "glow")
			{
				tempImg = MainFormRes.btnStart_Glow;
			}
			else if (inputCommand == "down")
			{
				tempImg = MainFormRes.btnStart_Down;
			}
			else
			{
				tempImg = MainFormRes.btnStart_Normal;
			}
			return tempImg;
		}

		/// <summary>
		/// 获得按钮图片
		/// </summary>
		/// <param name="inputCommand">glow down normal</param>
		/// <returns></returns>
		public static Image GetButtonImagePause(string inputCommand)
		{
			Image tempImg;
			if (inputCommand == "glow")
			{
				tempImg = MainFormRes.btnPause_Glow;
			}
			else if (inputCommand == "down")
			{
				tempImg = MainFormRes.btnPause_Down;
			}
			else
			{
				tempImg = MainFormRes.btnPause_Normal;
			}
			return tempImg;
		}

		/// <summary>
		/// 获得按钮图片
		/// </summary>
		/// <param name="inputCommand">glow down normal</param>
		/// <returns></returns>
		public static Image GetButtonImageEnd(string inputCommand)
		{
			Image tempImg;
			if (inputCommand == "glow")
			{
				tempImg = MainFormRes.btnEnd_Glow;
			}
			else if (inputCommand == "down")
			{
				tempImg = MainFormRes.btnEnd_Down;
			}
			else
			{
				tempImg = MainFormRes.btnEnd_Normal;
			}
			return tempImg;
		}

		/// <summary>
		/// 开关
		/// </summary>
		/// <param name="isOn">On of Off</param>
		/// <returns></returns>
		public static Image GetSwitchImage(bool isOn)
		{
			Image tempImg;
			if (isOn)
			{
				tempImg = MainFormRes.switch_on;
			}
			else
			{
				tempImg = MainFormRes.switch_off;
			}
			return tempImg;
		}
    }
}
