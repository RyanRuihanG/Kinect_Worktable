using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeCoordinateMainWindow
{
	/// <summary>
	/// 程序工作状态
	/// </summary>
	enum AppWorkState
	{
		Stop,
		ReadGCode,
		RunningGCode,
		PlugInControl,
		Pause
	}
}
