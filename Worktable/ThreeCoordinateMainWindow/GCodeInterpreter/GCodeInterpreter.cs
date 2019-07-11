using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPublicPlugInInterface;

namespace GCodeInterpreter
{
	/// <summary>
	/// 用于表示M代码解释结果的枚举
	/// </summary>
	public enum MWorkState
	{
		NULL,
		M1,
		M30,
		Others
	}

	/// <summary>
	/// 用于表示坐标的枚举
	/// </summary>
	public enum CoordinateState
	{
		X,
		Y,
		Z,
		F,
		I,
		J,
		K
	}

	/// <summary>
	/// 用来存储一行G代码的解释结果，可以变成表结构
	/// </summary>
	public class GCodeClass
	{
		private GWorkState _gWorkState;
		private MWorkState _mWorkState;

		private List<CoordinateState> _coordinate;
		private List<double> _value;

		private bool _isError;

		public GCodeClass()
		{
			_coordinate = new List<CoordinateState>();
			_value = new List<double>();

			_gWorkState = GWorkState.NULL;
			_mWorkState = MWorkState.NULL;
			_isError = false;
		}

		public void SetCoordinate(CoordinateState inputCoordinate, double inputValue)
		{
			_coordinate.Add(inputCoordinate);
			_value.Add(inputValue);
		}

		public bool IsError { get => _isError; set => _isError = value; }
		public GWorkState GWorkState { get => _gWorkState; set => _gWorkState = value; }
		public MWorkState MWorkState { get => _mWorkState; set => _mWorkState = value; }

		public List<CoordinateState> Coordinate { get => _coordinate; }
		public List<double> Value { get => _value;  }
	}

	/// <summary>
	/// G代码解释器本体
	/// </summary>
	public class GCodeInterpreterClass
	{
		char[] _rightCode = { 'G', 'N', 'O', '(', 'T', 'M', 'X', 'Y', 'Z', '%', 'S' ,' ','\n','\r'};
		//GWorkState _saveState = GWorkState.NULL;
		//double _savedX = 0;
		//double _savedY = 0;
		//double _savedZ = 0;
		//double _savedF = 0;

		public GCodeClass Interpreter(string OneLine)
		{
			bool GetG = false;
			bool GetX = false;
			bool GetY = false;
			bool GetZ = false;
			bool GetM = false;
			bool GetF = false;
			bool GetI = false;
			bool GetJ = false;
			bool GetK = false;
			bool GetN = false;

			GCodeClass tempResult = new GCodeClass();
			//有穷状态机
			for (int i = 0; i < OneLine.Length; i++)
			{
				if (OneLine[i] == ';' || OneLine[i] == '；')//这后面是注释不管他
				{
					return tempResult;
				}

				if (i == 0)//第一个字符
				{
					if (CheckRightCode(OneLine[i], _rightCode) == false)//第一个字符不等于任何一个合法字符
					{
						tempResult.IsError = true;//标志错误
						break;
					}
				}

				if (OneLine[i] == ',')
				{
					tempResult.IsError = true;//错误
					break;
				}

				//根据G代码的字母来区分下面的数字是什么意思
				if (OneLine[i] == '(' || OneLine[i] == '%' || OneLine[i] == 'T' || 
					OneLine[i] == 'S' || OneLine[i] == 'L' || OneLine[i] == 'H' || OneLine[i] == 'O')//不关心的G代码
				{
					tempResult.GWorkState = GWorkState.Others;
					return tempResult;
				}
				if (OneLine[i] == 'G' || OneLine[i] == 'g')//Get G
				{
					GetG = true;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = false;
					
					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'X' || OneLine[i] == 'x')//Get X
				{
					GetG = false;
					GetM = false;

					GetX = true;
					GetY = false;
					GetZ = false;
					
					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'Y' || OneLine[i] == 'y')//Get Y
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = true;
					GetZ = false;
					
					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'Z' || OneLine[i] == 'z')//Get Z
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = true;
					
					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'M' || OneLine[i] == 'm')//Get M
				{
					GetG = false;
					GetM = true;
					
					GetX = false;
					GetY = false;
					GetZ = false;

					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'F' || OneLine[i] == 'f')//GetF
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = false;
					
					GetF = true;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'I' || OneLine[i] == 'i')
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = false;
					
					GetF = false;

					GetI = true;
					GetJ = false;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'J' || OneLine[i] == 'j')
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = false;
					
					GetF = false;

					GetI = false;
					GetJ = true;
					GetK = false;

					GetN = false;
				}
				else if (OneLine[i] == 'K' || OneLine[i] == 'k')
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = false;
					
					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = true;

					GetN = false;
				}
				if (OneLine[i] == 'N' || OneLine[i] == 'n')//Get G
				{
					GetG = false;
					GetM = false;

					GetX = false;
					GetY = false;
					GetZ = false;

					GetF = false;

					GetI = false;
					GetJ = false;
					GetK = false;

					GetN = true;
				}
				else
				{
					if ((OneLine[i] >= '0' && OneLine[i] <= '9') || (OneLine[i] == '-'))//表示扫描到数字
					{
						List<char> tempListChar = new List<char>();
						int mj = 0;
						for (mj = i; mj < OneLine.Length; mj++)
						{
							if ((OneLine[mj] >= '0' && OneLine[mj] <= '9') || (OneLine[mj] == '.') || (OneLine[mj] == '-'))//如果为数字或小数点
							{
								if (mj != i && OneLine[mj] == '-')
								{
									tempResult.IsError = true;
									return tempResult;
								}
								tempListChar.Add(OneLine[mj]);
							}
							else
							{
								break;
							}
						}
						i = mj - 1;

						char[] tempArray = tempListChar.ToArray();
						double tempNumber = Convert.ToDouble(new string(tempArray));//tempNumber为扫描到的数字
						if (GetM == true)//数字为指令
						{
							switch (tempNumber)
							{
								case 1: tempResult.MWorkState = MWorkState.M1; /*_saveState = GWorkState.M1;*/ break;
								case 30: tempResult.MWorkState = MWorkState.M30; /*_saveState = GWorkState.M30;*/ break;
								default: tempResult.MWorkState = MWorkState.Others; break;
							}
						}
						else if (GetG == true)//数字为指令
						{
							switch (tempNumber)
							{
								case 0: tempResult.GWorkState = GWorkState.G00; /*_saveState = GWorkState.G00;*/ break;
								case 1: tempResult.GWorkState = GWorkState.G01; /*_saveState = GWorkState.G01;*/ break;
								case 2: tempResult.GWorkState = GWorkState.G02; /*_saveState = GWorkState.G02;*/ break;
								case 3: tempResult.GWorkState = GWorkState.G03; /*_saveState = GWorkState.G03;*/ break;
								default: tempResult.GWorkState = GWorkState.Others; break;
							}
						}
						else if (GetF == true)//数字为速度
						{
							tempResult.SetCoordinate(CoordinateState.F, tempNumber);
						}
						else if (GetX == true)//数字为数值
						{
							//_savedX = tempNumber;
							tempResult.SetCoordinate(CoordinateState.X, tempNumber);
						}
						else if (GetY == true)
						{
							//_savedY = tempNumber;
							tempResult.SetCoordinate(CoordinateState.Y, tempNumber);
						}
						else if (GetZ == true)
						{
							//_savedZ = tempNumber;
							tempResult.SetCoordinate(CoordinateState.Z, tempNumber);
						}
						else if (GetI == true)
						{
							tempResult.SetCoordinate(CoordinateState.I, tempNumber);
						}
						else if (GetJ == true)
						{
							tempResult.SetCoordinate(CoordinateState.J, tempNumber);
						}
						else if (GetK == true)
						{
							tempResult.SetCoordinate(CoordinateState.K, tempNumber);
						}
						else if (GetN == true)//忽略
						{

						}
						else
						{
							tempResult.IsError = true;
						}
					}
				}
			}
			return tempResult;
		}

		private bool CheckRightCode(char v, char[] rightCode)
		{
			bool result = false;
			foreach (char c in rightCode)
			{
				if (c == v)
				{
					result = true;//表示找到了合法字符
					break;
				}
			}
			return result;
		}
	}
}
