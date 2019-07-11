//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.BodyBasics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Windows.Threading;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using EncoderDecoder;
    using TDCommunication;
    using System.Threading;
    using System.Windows.Forms;
    using System.Runtime.Serialization.Formatters.Binary;
    using IPublicPlugInInterface;
    //using System.Drawing;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //DispatcherTimer MainTimer = new DispatcherTimer();

        List<List<System.Drawing.Point>> drawPoints = new List<List<System.Drawing.Point>>();

        private bool StartControl = false;

        private bool LeftSet = false;
        private bool RightSet = false;
        private bool ForwardSet = false;
        private bool BackSet = false;
        private bool UpSet = false;
        private bool DownSet = false;

        private bool StartLeft = false;
        private bool StartRight = false;
        private bool StartForward = false;
        private bool StartBack = false;
        private bool StartUp = false;
        private bool StartDown = false;

        private bool TimeLeft = false;
        private bool TimeRight = false;
        private bool TimeForward = false;
        private bool TimeBack = false;
        private bool TimeUp = false;
        private bool TimeDown = false;

        private bool AppStateIsDrawing = false;
        private bool AppStateIsButton = true;
        private bool isReleased = true;

        //串口通信相关
        /// <summary>
        /// 串口数据队列
        /// </summary>
        Queue<List<byte>> serialDataQueue = new Queue<List<byte>>();
        Decoder globalDecoder = new Decoder();
        Encoder globalEncoder = new Encoder();

        //线程
        Thread _threadSerialDataProcessing;
        Thread _threadOperating;

        private enum PositonType
        {
            None = 0,
            Left = 1,
            Right = 2,
            Forward = 3,
            Back = 4,
            Up = 5,
            Down =6

        }
        private PositonType pt = PositonType.None;

        //控制模式：true-按键，false-画笔
        private bool ControlMode = true;

        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HandSize = 30;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred(不稳定状态)
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;

        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> bones;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        private int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Pen> bodyColors;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            // one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();

            // get the coordinate mapper
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            // get the depth (display) extents
            FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            // get size of joint space
            this.displayWidth = frameDescription.Width;//512
            this.displayHeight = frameDescription.Height;//424


            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // 用线段表示骨架
            this.bones = new List<Tuple<JointType, JointType>>();

            // 躯干
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // 右臂
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // 左臂
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // 右腿
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // 左腿
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));

            // populate body colors, one for each BodyIndex
            this.bodyColors = new List<Pen>();

            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));

            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // use the window object as the view model in this simple example
            this.DataContext = this;

            // initialize the components (controls) of the window
            this.InitializeComponent();
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;

                _threadOperating = new Thread(new ThreadStart(OperationWorking));
                _threadSerialDataProcessing = new Thread(new ThreadStart(SerialDataProcessing));

                _threadSerialDataProcessing.Name = "串口数据处理";
                _threadOperating.Name = "串口操作";

                _threadOperating.Start();
                _threadSerialDataProcessing.Start();
            }
        }


        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

            _threadOperating.Abort();
            _threadSerialDataProcessing.Abort();
        }

        /// <summary>
        /// 相当于主循环
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    // 画一个透明背景设置渲染的大小（绘制矩形，黑色背景）
                    dc.DrawRectangle(Brushes.DarkGray, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
                    //虚拟按钮绘制
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 4), new Rect(50, 120, 50, 100));//左
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 4), new Rect(240, 120, 50, 100));//右
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 4), new Rect(120, 50, 100, 50));//前
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 4), new Rect(120, 240, 100, 50));//后
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 4), new Rect(120, 120, 100, 40));//上
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 4), new Rect(120, 180, 100, 40));//下

                    int penIndex = 0;
                    foreach (Body body in this.bodies)
                    {
                        Pen drawPen = this.bodyColors[penIndex++];

                        if (body.IsTracked)
                        {
                            //this.DrawClippedEdges(body, dc);

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            // 关节点转换为深度(显示)空间：X Y坐标
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            foreach (JointType jointType in joints.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = InferredZPositionClamp;
                                }

                                DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                                jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                            }

                            //this.DrawBody(joints, jointPoints, dc, drawPen);

                            //this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            //this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);

                            //////////user//////////////
                            //显示坐标
                            Lhand_PostionX.Content = (jointPoints[JointType.HandLeft].X).ToString("#0.00");
                            Lhand_PostionY.Content = (jointPoints[JointType.HandLeft].Y).ToString("#0.00");
                            Rhand_PostionX.Content = (jointPoints[JointType.HandRight].X).ToString("#0.00");
                            Rhand_PostionY.Content = (jointPoints[JointType.HandRight].Y).ToString("#0.00");

                            if (StartControl && joints[JointType.HandLeft].TrackingState == TrackingState.Tracked)
                            {

                                Canvas.SetTop(_controlHand, jointPoints[JointType.HandLeft].Y - 30);
                                Canvas.SetLeft(_controlHand, jointPoints[JointType.HandLeft].X - 30);

                                double deltaY = 25;

                                if (body.HandLeftState == HandState.Lasso && body.HandRightState == HandState.Lasso)
                                    ControlMode = false;
                                if (body.HandLeftState == HandState.Closed && body.HandRightState == HandState.Closed && Math.Abs(jointPoints[JointType.HandLeft].Y - jointPoints[JointType.HandRight].Y) < deltaY)
                                    ControlMode = true;

                                if (ControlMode)
                                {
                                    if (AppStateIsDrawing == true)
                                    {
                                        AppStateIsDrawing = false;
                                        AppStateIsButton = true;
                                        BeginStoryboard((Storyboard)FindResource("Area_Show"));
                                    }
                                    this.PositonControl(joints, jointPoints);//进入按键控制
                                }
                                else
                                {
                                    if (AppStateIsButton == true)
                                    {
                                        AppStateIsButton = false;
                                        AppStateIsDrawing = true;
                                        BeginStoryboard((Storyboard)FindResource("Area_Dispear"));//按钮消失
                                        drawPoints.Clear();
                                    }
                                    this.DrawControl(body, joints, jointPoints, dc);//进入画笔控制
                                }
                            }
                            else if (joints[JointType.HandLeft].TrackingState != TrackingState.Tracked)  // 
                            {
                                _controlHand.Opacity = 0;
                                //mainTimer.Stop();
                            }

                            //huatu
                            if (!ControlMode)
                            {
                                Pen tempPen = new Pen(Brushes.Red, 2);
                                foreach (List<System.Drawing.Point> lp in drawPoints)
                                {
                                    for (int i = 0; i < lp.Count - 1; i++)
                                    {
                                        Point tpt = new Point(lp[i].X, lp[i].Y);
                                        Point tpt2 = new Point(lp[i + 1].X, lp[i + 1].Y);
                                        dc.DrawLine(tempPen, tpt, tpt2);
                                    }
                                }
                            }
                        }
                    }

                    // 防止图像出界
                    this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                }
            }
        }

        /// <summary>
        /// 按键控制
        /// </summary>
        private void PositonControl(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints)
        {
            ModeLable.Content = "按键控制";
            double ax = jointPoints[JointType.HandLeft].X;
            double ay = jointPoints[JointType.HandLeft].Y;

            if (ay > 424 || ay < 0 || ax < 0 || ax > 512)
                _controlHand.Opacity = 0;
            else
                _controlHand.Opacity = 100;

            if (pt == PositonType.None)
            {
                if (ax > 50 && ax < 100 && ay > 120 && ay < 200)//左
                    pt = PositonType.Left;
                else if (ax > 240 && ax < 290 && ay > 120 && ay < 200)//右
                    pt = PositonType.Right;
                else if (ax > 120 && ax < 220 && ay > 50 && ay < 100)//前
                    pt = PositonType.Forward;
                else if (ax > 120 && ax < 220 && ay > 240 && ay < 290)//后
                    pt = PositonType.Back;
                else if (ax > 120 && ax < 220 && ay > 120 && ay < 160)//上
                    pt = PositonType.Up;
                else if (ax > 120 && ax < 220 && ay > 180 && ay < 220)//下
                    pt = PositonType.Down;
            }

            if (pt == PositonType.Left)//左
            {
                if (ax > 50 && ax < 100 && ay > 120 && ay < 200)//左
                {
                    if (TimeLeft == false && StartLeft == false)
                    {
                        TimeLeft = true;
                        _controlHand.StartTimer();

                    }
                    else if (TimeLeft == true && StartLeft == true)
                    {
                        test.Content = "ok!!";
                        if (LeftSet == false)
                        {
                            LeftSet = true;
                            //操作串口
                            GetFinishMsgEvent.Set();
                        }
                    }
                }
                else
                {
                    pt = PositonType.None;
                    TimeLeft = false;
                    StartLeft = false;
                    LeftSet = false;
                    _controlHand.EndTimer();
                    test.Content = "no";
                }
            }
            else if (pt == PositonType.Right)//右
            {
                if (ax > 240 && ax < 290 && ay > 120 && ay < 200)
                {
                    if (TimeRight == false && StartRight == false)
                    {
                        TimeRight = true;
                        _controlHand.StartTimer();
                    }
                    else if (TimeRight == true && StartRight == true)
                    {
                        test.Content = "ok!!";
                        if (RightSet == false)
                        {
                            RightSet = true;
                            //操作串口
                            GetFinishMsgEvent.Set();
                        }
                    }
                }
                else
                {
                    pt = PositonType.None;
                    TimeRight = false;
                    StartRight = false;
                    RightSet = false;
                    _controlHand.EndTimer();
                    test.Content = "no";
                }
            }
            else if (pt == PositonType.Forward)//前
            {
                if (ax > 120 && ax < 220 && ay > 50 && ay < 100)
                {
                    if (TimeForward == false && StartForward == false)
                    {
                        TimeForward = true;
                        _controlHand.StartTimer();
                    }
                    else if (TimeForward == true && StartForward == true)
                    {
                        test.Content = "ok!!";
                        if (ForwardSet == false)
                        {
                            ForwardSet = true;
                            //操作串口
                            GetFinishMsgEvent.Set();
                        }
                    }
                }
                else
                {
                    pt = PositonType.None;
                    TimeForward = false;
                    StartForward = false;
                    ForwardSet = false;
                    _controlHand.EndTimer();
                    test.Content = "no";
                }
            }
            else if (pt == PositonType.Back)//后
            {
                if (ax > 120 && ax < 220 && ay > 240 && ay < 290)
                {
                    if (TimeBack == false && StartBack == false)
                    {
                        TimeBack = true;
                        _controlHand.StartTimer();
                    }
                    else if (TimeBack == true && StartBack == true)
                    {
                        test.Content = "ok!!";
                        if (BackSet == false)
                        {
                            BackSet = true;
                            //操作串口
                            GetFinishMsgEvent.Set();
                        }
                    }
                }
                else
                {
                    pt = PositonType.None;
                    TimeBack = false;
                    StartBack = false;
                    BackSet = false;
                    _controlHand.EndTimer();
                    test.Content = "no";
                }
            }
            else if (pt == PositonType.Up)//上
            {
                if (ax > 120 && ax < 220 && ay > 120 && ay < 160)
                {
                    if (TimeUp == false && StartUp == false)
                    {
                        TimeUp = true;
                        _controlHand.StartTimer();
                    }
                    else if (TimeUp == true && StartUp == true)
                    {
                        test.Content = "ok!!";
                        if (UpSet == false)
                        {
                            UpSet = true;
                            //操作串口
                            GetFinishMsgEvent.Set();
                        }
                    }
                }
                else
                {
                    pt = PositonType.None;
                    TimeUp = false;
                    StartUp = false;
                    UpSet = false;
                    _controlHand.EndTimer();
                    test.Content = "no";
                }
            }
            else if (pt == PositonType.Down)//下
            {
                if (ax > 120 && ax < 220 && ay > 180 && ay < 220)
                {
                    if (TimeDown == false && StartDown == false)
                    {
                        TimeDown = true;
                        _controlHand.StartTimer();
                    }
                    else if (TimeDown == true && StartDown == true)
                    {
                        test.Content = "ok!!";
                        if (DownSet == false)
                        {
                            DownSet = true;
                            //操作串口
                            GetFinishMsgEvent.Set();
                        }
                    }
                }
                else
                {
                    pt = PositonType.None;
                    TimeDown = false;
                    StartDown = false;
                    DownSet = false;
                    _controlHand.EndTimer();
                    test.Content = "no";
                }
            }
        }

        /// <summary>
        /// 画笔控制
        /// </summary>
        private void DrawControl(Body body, IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext dc)
        {
            double a = jointPoints[JointType.HandLeft].X;
            _controlHand.Opacity = 0;//画笔模式不显示手
            ModeLable.Content = "画笔控制";
            Pen bluePen = new Pen(Brushes.Azure, 8);
            Pen redPen = new Pen(Brushes.Red, 8);
            if (body.HandRightState == HandState.Closed)//手闭合画红圆
            {
                if (isReleased == true)//之前放开，现在闭合，说明第一次进入手闭合
                {
                    drawPoints.Add(new List<System.Drawing.Point>());//添加新点列
                }
                isReleased = false;
                dc.DrawEllipse(Brushes.Red, redPen, new Point(jointPoints[JointType.HandRight].X, jointPoints[JointType.HandRight].Y), 3, 3);
            }
            else//其他情况画蓝圆
            {
                isReleased = true;
                dc.DrawEllipse(Brushes.Azure, bluePen, new Point(jointPoints[JointType.HandRight].X, jointPoints[JointType.HandRight].Y), 3, 3);
            }

            if (body.HandRightState == HandState.Closed)
            {
                //向点列最后一列添加点
                drawPoints[drawPoints.Count - 1].Add(new System.Drawing.Point((int)jointPoints[JointType.HandRight].X, (int)jointPoints[JointType.HandRight].Y));
            }
            
        }

        /// <summary>
        /// 画点
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            // Draw the bones
            foreach (var bone in this.bones)
            {
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            }

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                Brush drawBrush = null;

                //根据追踪状态情况选择画笔
                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }
                //画点
                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// 画骨架。。也就是画线
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// /// <param name="drawingPen">specifies color to draw a specific bone</param>
        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // 除非关节已跟踪，我们假定所有骨头都推断，画笔为推断默认色
            Pen drawPen = this.inferredBonePen;
            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                drawPen = drawingPen;
            }

            drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        /// <summary>
        /// 画手。。其实就是画圆
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition, DrawingContext drawingContext)
        {
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        /// <summary>
        /// 对人体不在或不全在Kinect摄像头范围内的处理
        /// Draws indicators to show which edges are clipping body data
        /// </summary>
        /// <param name="body">body to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawClippedEdges(Body body, DrawingContext drawingContext)
        {
            FrameEdges clippedEdges = body.ClippedEdges;

            if (clippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, this.displayHeight - ClipBoundsThickness, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, this.displayHeight));
            }

            if (clippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(this.displayWidth - ClipBoundsThickness, 0, ClipBoundsThickness, this.displayHeight));
            }
        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }

        //开始手势控制按钮
        private void button_Click(object sender, RoutedEventArgs e)
        {
            StartControl = true;
        }

        //结束手势控制按钮
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            StartControl = false;
            _controlHand.Opacity = 0;
        }

        private void _controlHand_HandTick(object sender, EventArgs e)
        {
            if (pt == PositonType.Left)
                StartLeft = true;
            else if (pt == PositonType.Right)
                StartRight = true;
            else if (pt == PositonType.Forward)
                StartForward = true;
            else if (pt == PositonType.Back)
                StartBack = true;
            else if (pt == PositonType.Up)
                StartUp = true;
            else if (pt == PositonType.Down)
                StartDown = true;

            //_controlHand.EndTimer();
        }

        #region 串口通信
        Action<System.Windows.Controls.TextBox, string> textBoxAddText = (x, y) => { x.Text += y; };
        
        private void SerialControl_QueueAdded(object sender, EventArgs e)
        {
            if (_serialControl.DataQueue.Count > 0)
            {
                byte[] tempArray;
                tempArray = _serialControl.DataQueue.Dequeue().ToArray();
                serialDataQueue.Enqueue(new List<byte>(tempArray));
                //tbShowSerial.Clear();
                tbShowSerial.Dispatcher.Invoke(new MethodInvoker(delegate ()
                {
                    tbShowSerial.Clear();
                }));

                foreach (byte tB in tempArray)
                {
                    //tbShowSerial.Text += (tB.ToString("X2") + "  ");'
                    tbShowSerial.Dispatcher.Invoke(textBoxAddText, tbShowSerial, tB.ToString("X2") + " ");
                }
            }
        }

        /// <summary>
        /// 串口消息数据处理
        /// </summary>
        private void SerialDataProcessing()
        {
            while (true)
            {
                if (serialDataQueue.Count > 0)
                {
                    DataPackage tempDP = globalDecoder.Decode(serialDataQueue.Dequeue().ToArray());
                    if (tempDP.Command == CommandClass._FINISHED_)
                    {
                        //继续
                        GetFinishMsgEvent.Set();
                    }
                }
            }
        }


        #endregion

        #region 操作线程

        static AutoResetEvent GetFinishMsgEvent = new AutoResetEvent(false);

        void OperationWorking()
        {
            double deltaX = 1;
            double deltaY = 1;
            double deltaZ = 1;

            while (true)
            {
                DataPackage tempDP = new DataPackage();
                tempDP.DataF = 500;
                tempDP.Command = CommandClass._SET_POSITION_;

                if (StartLeft == true && TimeLeft == true)
                {
                    tempDP.DataX = -1 * deltaX;
                }
                else if (StartRight == true && TimeRight == true)
                {
                    tempDP.DataX = deltaX;
                }
                else if (StartForward == true && TimeForward == true)
                {
                    tempDP.DataY = deltaY;
                }
                else if (StartBack == true && TimeBack == true)
                {
                    tempDP.DataY = -1 * deltaY;
                }
                else if (StartUp == true && TimeUp == true)
                {
                    tempDP.DataZ = deltaZ;
                }
                else if (StartDown == true && TimeDown == true)
                {
                    tempDP.DataZ = -1 * deltaZ;
                }

                //如果有数据且串口是开的，那么编码并发送数据
                if ((tempDP.DataX != 0 || tempDP.DataY != 0 || tempDP.DataZ != 0) && _serialControl.MyPort.IsOpen == true)
                {
                    byte[] tempBytes = globalEncoder.Encode(tempDP);
                    _serialControl.WriteBytes(tempBytes, 0, tempBytes.Length);//写串口
                }

                GetFinishMsgEvent.WaitOne();//等待完成
            }
        }
        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.Opacity == 1)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                //saveDialog.InitialDirectory = System.Windows.Application.StartupPath;
                saveDialog.Filter = "point files(*.3ctpl)|*.3ctpl";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;
                saveDialog.FileName = "untitled.3ctpl";

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileStream fileStream = new FileStream(saveDialog.FileName, FileMode.Create);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    //数据打包
                    //List<List<Point>> tempPointLists = new List<List<Point>>();
                    //tempPointLists.Add(drawPoints);

                    PointsListsPack tempPLP = new PointsListsPack();
                    tempPLP.Data_Height = displayHeight;
                    tempPLP.Data_Width = displayWidth;
                    tempPLP.Data_PointLists = drawPoints;

                    //序列化文件
                    binaryFormatter.Serialize(fileStream, tempPLP);
                    fileStream.Close();
                }
            }
        }
    }
}
