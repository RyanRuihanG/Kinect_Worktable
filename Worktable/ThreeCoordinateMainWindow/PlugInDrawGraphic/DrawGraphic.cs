using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPublicPlugInInterface;

namespace PlugInDrawGraphic
{
	public class DrawGraphic : IPlugInGetData
	{
		private DataTransfer _myDT;
		public DataTransfer DataTransfer { get => _myDT; }

		private List<List<Point3D>> _3dPointLists;
		public List<List<Point3D>> PointLists { get => _3dPointLists; }

		/// <summary>
		/// 插件程序入口点
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Action(object sender, EventArgs e)
		{
			_3dPointLists = new List<List<Point3D>>();
			PreviewWindow form1 = new PreviewWindow(_myDT);
			form1.Show();
		}

		public void SetDataTransfer(DataTransfer inputDT)
		{
			_myDT = inputDT;
		}

		public PlugInInfo WhoAmI()
		{
			PlugInInfo tempPII = new PlugInInfo();
			tempPII.Name = "绘制预览";
			return tempPII;
		}
	}
}
