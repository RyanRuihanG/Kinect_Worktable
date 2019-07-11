using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iinterpolation;
using IPublicPlugInInterface;

namespace LineInter
{
	/// <summary>
	/// 封装插补计算
	/// </summary>
	public class OperatorInter : IInterpolation
	{
		private double _step;
		/// <summary>
		/// 插补步长
		/// </summary>
		public double Step { get { return _step; } }

		public OperatorInter(double inputstep)
		{
			_step = inputstep;
		}

		/// <summary>
		/// 设定插补步长
		/// </summary>
		/// <param name="inputStep">输入步长</param>
		public void SetStep(double inputStep)
		{
			_step = inputStep;
		}

		/// <summary>
		/// 圆弧插补
		/// </summary>
		/// <param name="center">圆心坐标</param>
		/// <param name="start">起点坐标</param>
		/// <param name="end">终点坐标</param>
		/// <param name="direction">方向，1-顺时针 0-逆时针</param>
		/// <returns>StepMark列表</returns>
		public List<StepMark> CircleInterpolation(Point3D center, Point3D start, Point3D end, int direction)
		{
			//填入圆弧插补代码
			List<StepMark> tempStepMark = new List<StepMark>();
			// Quadrant quadrant = Quadrant.First;

			double deviationF = 0;
			double R_1 = Math.Sqrt((start.X - center.X) * (start.X - center.X) + (start.Y - center.Y) * (start.Y - center.Y));
			double R_2 = Math.Sqrt((end.X - center.X) * (end.X - center.X) + (end.Y - center.Y) * (end.Y - center.Y));
			double R = (R_1 + R_2) / 2;
			double m, n;

			double x_3 = (start.X + end.X) / 2;
			double y_3 = (start.Y + end.Y) / 2;
			double k = (end.Y - start.Y) / (end.X - start.X);
			if (R_1 != R_2)
			{
				if (start.Y != end.Y)
				{
					m = (y_3 - center.Y + (1 / k) * x_3 + k * center.X) / (k + 1 / k);
					n = k * m + center.Y - k * center.X;
				}
				else
				{
					m = x_3;
					n = center.Y;
				}
			}
			else
			{
				m = center.X;
				n = center.Y;
			}

			Point3D position = new Point3D(start.X - m, start.Y - n, start.Z - center.Z);//记录当前位置
			Point3D tempEnd = new Point3D(end.X - m, end.Y - n, end.Z - center.Z);

			//判断顺逆， 1->顺   0->逆 

			if (direction == 1)     //顺圆弧画圆
			{
				int flag = 1;       //设置标志位，循环画圆
				while (flag == 1)
				{
					while (position.X >= 0 && position.Y > 0 && flag == 1)//第一象限  
					{
						deviationF = position.X * position.X + position.Y * position.Y - R * R;
						if (deviationF >= 0)   //点在圆外
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYNegative();//向Y负向步进
							tempStepMark.Add(tempMark);

							// deviationF = deviationF - 2 * position.Y + _step;
							position.Y -= _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXPositive();//向X正向步进
							tempStepMark.Add(tempMark);
							// deviationF = deviationF + 2 * position.X + _step;
							position.X += _step;
						}

						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(
							position.Y - tempEnd.Y, 2)) < (_step * _step))               //判断是否接近终点
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag = 2;
							break;
						}
					}

					while (position.X < 0 && position.Y >= 0 && flag == 1)  //第二象限
					{
						deviationF = position.X * position.X + position.Y * position.Y - R * R;
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXPositive();//向X正向步进
							tempStepMark.Add(tempMark);
							// deviationF = deviationF + 2 * position.X + _step;
							position.X += _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYPositive();//向Y正向步进
							tempStepMark.Add(tempMark);
							// deviationF = deviationF + 2 * position.Y + _step;
							position.Y += _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag = 2;
							break;
						}

					}
					while (position.X <= 0 && position.Y < 0 && flag == 1)  //第三象限
					{
						deviationF = position.X * position.X + position.Y * position.Y - R * R;
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYPositive();//向Y正向步进
							tempStepMark.Add(tempMark);
							//  deviationF = deviationF + 2 * position.Y + _step;
							position.Y += _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXNegative();//向X负向步进
							tempStepMark.Add(tempMark);
							// deviationF = deviationF - 2 * position.X + _step;
							position.X -= _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag = 2;
							break;
						}

					}
					while (position.X > 0 && position.Y <= 0 && flag == 1)  //第四象限
					{
						deviationF = position.X * position.X + position.Y * position.Y - R * R;
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXNegative();//向X负向步进
							tempStepMark.Add(tempMark);
							// deviationF = deviationF - 2 * position.X + _step;
							position.X -= _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYNegative();//向Y负向步进
							tempStepMark.Add(tempMark);
							//  deviationF = deviationF - 2 * position.Y + _step;
							position.Y -= _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag = 2;
							break;
						}
					}
				}
			}

			if (direction == 0)     //逆圆弧画圆
			{
				int flag_2 = 1;
				while (flag_2 == 1)
				{
					while (position.X >= 0 && position.Y > 0 && flag_2 == 1)//第一象限  
					{
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXNegative();//向X负向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF - 2 * position.X + _step;
							position.X -= _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYPositive();//向Y正向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF + 2 * position.Y + _step;
							position.Y += _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag_2 = 2;
							break;
						}
					}
					while (position.X < 0 && position.Y >= 0 && flag_2 == 1)  //第二象限
					{
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYNegative();//向Y负向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF - 2 * position.Y + _step;
							position.Y -= _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXNegative();//向X负向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF - 2 * position.X + _step;
							position.X -= _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag_2 = 2;
							break;
						}

					}
					while (position.X <= 0 && position.Y < 0 && flag_2 == 1)  //第三象限
					{
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXPositive();//向X正向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF + 2 * position.X + _step;
							position.X += _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYNegative();//向Y负向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF - 2 * position.Y + _step;
							position.Y -= _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag_2 = 2;
							break;
						}

					}
					while (position.X > 0 && position.Y <= 0 && flag_2 == 1)  //第四象限
					{
						if (deviationF >= 0)
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYPositive();//向Y正向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF + 2 * position.Y + _step;
							position.Y += _step;
						}
						else
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXPositive();//向X正向步进
							tempStepMark.Add(tempMark);
							deviationF = deviationF + 2 * position.X + _step;
							position.X += _step;
						}
						if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
						{
							StepMark finalTempMark = new StepMark();
							finalTempMark.ToEndX();//X至终点
							tempStepMark.Add(finalTempMark);
							position.X = tempEnd.X;

							StepMark tempMark = new StepMark(); //y插补至终点
							tempMark.ToEndY();
							tempStepMark.Add(tempMark);
							position.Y = tempEnd.Y;
							flag_2 = 2;
							break;
						}

					}
				}
			}

			return tempStepMark;

		}
		//              if (direction == 0)     //逆圆弧画圆
		//              {
		//              while ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) > (_step * _step))
		//              {
		//                  while (position.X > 0 && position.Y >= 0)//第一象限  
		//                  {
		//                      if (deviationF >= 0)
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnXNegative();//向X负向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF - 2 * position.X + _step;
		//                          position.X -= _step;
		//                      }
		//                      else
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnYPositive();//向Y正向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF + 2 * position.Y + _step;
		//                          position.Y += _step;
		//                      }
		//                      if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
		//                      {
		//                          StepMark finalTempMark = new StepMark();
		//                          finalTempMark.ToEndX();//X至终点
		//                          tempStepMark.Add(finalTempMark);
		//                          position.X = tempEnd.X;

		//                          StepMark tempMark = new StepMark(); //y插补至终点
		//                          tempMark.ToEndY();
		//                          tempStepMark.Add(tempMark);
		//                          position.Y = tempEnd.Y;

		//                          break;
		//                      }
		//                  }
		//                  while (position.X <= 0 && position.Y > 0)  //第二象限
		//                  {
		//                      if (deviationF >= 0)
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnYNegative();//向Y负向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF - 2 * position.Y + _step;
		//                          position.Y -= _step;
		//                      }
		//                      else
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnXNegative();//向X负向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF - 2 * position.X + _step;
		//                          position.X -= _step;
		//                      }
		//                      if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
		//                      {
		//                          StepMark finalTempMark = new StepMark();
		//                          finalTempMark.ToEndX();//X至终点
		//                          tempStepMark.Add(finalTempMark);
		//                          position.X = tempEnd.X;

		//                          StepMark tempMark = new StepMark(); //y插补至终点
		//                          tempMark.ToEndY();
		//                          tempStepMark.Add(tempMark);
		//                          position.Y = tempEnd.Y;

		//                          break;
		//                      }

		//                  }
		//                  while (position.X < 0 && position.Y <= 0)  //第三象限
		//                  {
		//                      if (deviationF >= 0)
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnXPositive();//向X正向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF + 2 * position.X + _step;
		//                          position.X += _step;
		//                      }
		//                      else
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnYNegative();//向Y负向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF - 2 * position.Y + _step;
		//                          position.Y -= _step;
		//                      }
		//                      if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
		//                      {
		//                          StepMark finalTempMark = new StepMark();
		//                          finalTempMark.ToEndX();//X至终点
		//                          tempStepMark.Add(finalTempMark);
		//                          position.X = tempEnd.X;

		//                          StepMark tempMark = new StepMark(); //y插补至终点
		//                          tempMark.ToEndY();
		//                          tempStepMark.Add(tempMark);
		//                          position.Y = tempEnd.Y;

		//                          break;
		//                      }

		//                  }
		//                  while (position.X >= 0 && position.Y < 0)  //第四象限
		//                  {
		//                      if (deviationF >= 0)
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnYPositive();//向Y正向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF + 2 * position.Y + _step;
		//                          position.Y += _step;
		//                      }
		//                      else
		//                      {
		//                          StepMark tempMark = new StepMark();
		//                          tempMark.StepOnXPositive();//向X正向步进
		//                          tempStepMark.Add(tempMark);
		//                          deviationF = deviationF + 2 * position.X + _step;
		//                          position.X += _step;
		//                      }
		//                      if ((Math.Pow(position.X - tempEnd.X, 2) + Math.Pow(position.Y - tempEnd.Y, 2)) < (_step * _step))
		//                      {
		//                          StepMark finalTempMark = new StepMark();
		//                          finalTempMark.ToEndX();//X至终点
		//                          tempStepMark.Add(finalTempMark);
		//                          position.X = tempEnd.X;

		//                          StepMark tempMark = new StepMark(); //y插补至终点
		//                          tempMark.ToEndY();
		//                          tempStepMark.Add(tempMark);
		//                          position.Y = tempEnd.Y;

		//                          break;
		//                      }

		//                  }
		//              }
		//              }

		//              return tempStepMark;

		//}

		/// <summary>
		/// 直线插补
		/// </summary>
		/// <param name="start">起始点坐标</param>
		/// <param name="end">终点坐标</param>
		/// <returns>StepMark列表</returns>
		public List<StepMark> LineInterpolation(Point3D start, Point3D end)
		{
			List<StepMark> tempStepMark = new List<StepMark>();
			Quadrant quadrant = Quadrant.First;
			Point3D tempEnd = new Point3D(end.X - start.X, end.Y - start.Y, end.Z - start.Z);
			//判断象限
			if (tempEnd.X > 0 && tempEnd.Y >= 0)
			{
				quadrant = Quadrant.First;
			}
			else if (tempEnd.X <= 0 && tempEnd.Y >= 0)
			{
				quadrant = Quadrant.Second;
			}
			else if (tempEnd.X <= 0 && tempEnd.Y < 0)
			{
				quadrant = Quadrant.Third;
			}
			else
			{
				quadrant = Quadrant.Forth;
			}

			if ((start - end).Z != 0)//说明是Z向运动
			{
				StepMark tSM = new StepMark();
				tSM.ToEndZ();
				tempStepMark.Add(tSM);
				return tempStepMark;
			}

			//分象限进行插补计算
			Point3D position = new Point3D(0, 0, 0);//记录当前位置
	        if (tempEnd.X == 0)//竖直线
		    {
				StepMark tempMark = new StepMark();
				tempMark.ToEndY();//Y直接到底
				tempStepMark.Add(tempMark);
				position.Y = tempEnd.Y;
			}
			else
			{
			    if (quadrant == Quadrant.First)//第一象限
				{
					double yShouldBe = 0;
					double K = tempEnd.Y / tempEnd.X;

					while (position.X + _step < tempEnd.X)
					{
						if (position.Y < yShouldBe)//当前位置在下方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYPositive();//向Y正向步进
							tempStepMark.Add(tempMark);
							position.Y += _step;
						}
						else//当前位置在上方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXPositive();//向X正向步进
							tempStepMark.Add(tempMark);
							position.X += _step;
							yShouldBe += _step * K;
						}
					}

					while (position.Y < yShouldBe)//直到出现在了上方
					{
						StepMark tempMark = new StepMark();
						tempMark.StepOnYPositive();//向Y正向步进
						tempStepMark.Add(tempMark);
						position.Y += _step;
					}
					//X插补至终点
					StepMark finalTempMark = new StepMark();
					finalTempMark.ToEndX();//X至终点
					tempStepMark.Add(finalTempMark);
					yShouldBe += Math.Abs(K * (tempEnd.X - position.X));
					position.X = end.X;

					if (position.Y < yShouldBe)//如果还在下方
					{
						StepMark tempMark = new StepMark();
						//y插补至终点
						tempMark.ToEndY();
						tempStepMark.Add(tempMark);
						position.Y = end.Y;
					}
				}
				else if (quadrant == Quadrant.Second)//第二象限
				{
					double yShouldBe = 0;
					double K = Math.Abs(tempEnd.Y / tempEnd.X);

					while (position.X - _step > tempEnd.X)
					{
						if (position.Y < yShouldBe)//当前位置在下方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYPositive();//向Y正向步进
							tempStepMark.Add(tempMark);
							position.Y += _step;
						}
						else//当前位置在上方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXNegative();//向X负向步进
							tempStepMark.Add(tempMark);
							position.X -= _step;
							yShouldBe += _step * K;
						}
					}

					while (position.Y < yShouldBe)//直到出现在了上方
					{
						StepMark tempMark = new StepMark();
						tempMark.StepOnYPositive();//向Y正向步进
						tempStepMark.Add(tempMark);
						position.Y += _step;
					}
					//X插补至终点
					StepMark finalTempMark = new StepMark();
					finalTempMark.ToEndX();//X至终点
					tempStepMark.Add(finalTempMark);
					yShouldBe += Math.Abs(K * (tempEnd.X - position.X));
					position.X = end.X;

					if (position.Y < yShouldBe)//如果还在下方
					{
						StepMark tempMark = new StepMark();
						//y插补至终点
						tempMark.ToEndY();
						tempStepMark.Add(tempMark);
						position.Y = end.Y;
					}
				}
				else if (quadrant == Quadrant.Third)//第三象限
				{
					double yShouldBe = 0;
					double K = Math.Abs(tempEnd.Y / tempEnd.X);

					while (position.X - _step > tempEnd.X)
					{
						if (position.Y > yShouldBe)//当前位置在上方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYNegative();//向Y负向步进
							tempStepMark.Add(tempMark);
							position.Y -= _step;
						}
						else//当前位置在下方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXNegative();//向X负向步进
							tempStepMark.Add(tempMark);
							position.X -= _step;
							yShouldBe -= _step * K;
						}
					}

					while (position.Y > yShouldBe)//直到出现在了下方
					{
						StepMark tempMark = new StepMark();
						tempMark.StepOnYNegative();//向Y负向步进
						tempStepMark.Add(tempMark);
						position.Y -= _step;
					}
					//X插补至终点
					StepMark finalTempMark = new StepMark();
					finalTempMark.ToEndX();//X至终点
					tempStepMark.Add(finalTempMark);
					yShouldBe -= Math.Abs(K * (tempEnd.X - position.X));
					position.X = end.X;

					if (position.Y > yShouldBe)//如果还在上方
					{
						StepMark tempMark = new StepMark();
						//y插补至终点
						tempMark.ToEndY();
						tempStepMark.Add(tempMark);
						position.Y = end.Y;
					}
				}
				else//第四象限
				{
					double yShouldBe = 0;
					double K = Math.Abs(tempEnd.Y / tempEnd.X);

					while (position.X + _step < tempEnd.X)
					{
						if (position.Y > yShouldBe)//当前位置在上方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnYNegative();//向Y负向步进
							tempStepMark.Add(tempMark);
							position.Y -= _step;
						}
						else//当前位置在下方
						{
							StepMark tempMark = new StepMark();
							tempMark.StepOnXPositive();//向X正向步进
							tempStepMark.Add(tempMark);
							position.X += _step;
							yShouldBe -= _step * K;
						}
					}

					while (position.Y > yShouldBe)//直到出现在了下方
					{
						StepMark tempMark = new StepMark();
						tempMark.StepOnYNegative();//向Y负向步进
						tempStepMark.Add(tempMark);
						position.Y -= _step;
					}
					//X插补至终点
					StepMark finalTempMark = new StepMark();
					finalTempMark.ToEndX();//X至终点
					tempStepMark.Add(finalTempMark);
					yShouldBe -= Math.Abs(K * (tempEnd.X - position.X));
					position.X = end.X;

					if (position.Y > yShouldBe)//如果还在上方
					{
						StepMark tempMark = new StepMark();
						//y插补至终点
						tempMark.ToEndY();
						tempStepMark.Add(tempMark);
						position.Y = end.Y;
					}
				}
			}
			return tempStepMark;
		}
	}
}
