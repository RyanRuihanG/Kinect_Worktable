using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPublicPlugInInterface;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace LoadPointListsFiles
{
    public class ClassLoadPointListsFiles : IPlugInGetSetData
    {
        private DataTransfer _myDT = new DataTransfer();
        public DataTransfer DataTransfer { get => _myDT; }

        private List<List<Point3D>> _pointLists = new List<List<Point3D>>();
        public List<List<Point3D>> PointLists { get => _pointLists; }

        public event ChangeDataHandler ChangeData;

        /// <summary>
        /// 插件程序入口点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Action(object sender, EventArgs e)
        {
            //标记是否读取完成
            bool isReadSuccessfully = true;
            //存储读取到的原始点列表 
            List<List<Point>> tempList = new List<List<Point>>();

            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = Application.StartupPath;
            open.Filter = "All files (*.*)|*.*|point files (*.3ctpl)|*.3ctpl";
            open.FilterIndex = 2;
            open.RestoreDirectory = true;

            if (open.ShowDialog() == DialogResult.OK)//打开读取对话框
            {
                int dataHeight = 500;
                string _tpath = open.FileName.ToString();
                FileStream fileStream = new FileStream(_tpath, FileMode.Open, FileAccess.Read, FileShare.Read);
                PointsListsPack tempDataPack;
                BinaryFormatter b = new BinaryFormatter();
                try
                {
                    tempDataPack = (PointsListsPack)b.Deserialize(fileStream);//反序列化文件
                    tempList = tempDataPack.Data_PointLists;//获取原始点列表
                    dataHeight = tempDataPack.Data_Height;//获取原始点列表所依附的图框高度
                }
                catch
                {
                    MessageBox.Show("文件读入失败！","错误");
                    isReadSuccessfully = false;
                }
                fileStream.Close();

                if (isReadSuccessfully == true)
                {
                    NumberInputForm<double> nif = new NumberInputForm<double>(false, true);
                    MessageBox.Show("请输入点坐标放大倍数","提示");
                    if (nif.ShowDialog() == DialogResult.OK)//打开数字输入对话框
                    {
                        double mult = nif.GetNumber;

                        List<List<Point>> tempPtList = new List<List<Point>>();//定义临时列表用于存储变换之后的点列
                        foreach (List<Point> ptl in tempList)
                        {
                            tempPtList.Add(new List<Point>());

                            //反转Y坐标
                            foreach (Point pt in ptl)
                            {
                                Point tpt = new Point();
                                tpt.Y = dataHeight - pt.Y - 37;
                                tpt.X = pt.X;
                                tempPtList[tempPtList.Count - 1].Add(tpt);
                            }
                        }

                        _pointLists.Clear();//清空3D点列结果表

                        foreach (List<Point> pl in tempPtList)
                        {
                            _pointLists.Add(new List<Point3D>());

                            //屏幕坐标变换到实际坐标
                            foreach (Point p in pl)
                            {
                                _pointLists[_pointLists.Count - 1].Add(new Point3D());
                                _pointLists[_pointLists.Count - 1][_pointLists[_pointLists.Count - 1].Count - 1] = pointToPoint3D(p, mult);
                            }
                        }

                        ChangeData?.Invoke(_pointLists);//执行委托实例
                    }
                    else
                    {
                        MessageBox.Show("要求输入倍数过程中被取消，文件未读入。","警告");
                    }
                }
            }
        }

        private Point3D pointToPoint3D(Point inputPt, double icoorMul)
        {
            Point3D temp3dPt = new Point3D();
            temp3dPt.Z = 0;
            temp3dPt.X = inputPt.X * icoorMul;
            temp3dPt.Y = inputPt.Y * icoorMul;

            return temp3dPt;
        }

        public void SetDataTransfer(DataTransfer inputDT)
        {
            _myDT = inputDT;
        }

        public PlugInInfo WhoAmI()
        {
            PlugInInfo tempInfo = new PlugInInfo();
            tempInfo.Name = "打开点列文件";
            return tempInfo;
        }
    }
}
