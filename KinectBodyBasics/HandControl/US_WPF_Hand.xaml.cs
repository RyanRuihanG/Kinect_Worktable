using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HandControl
{
	/// <summary>
	/// US_WPF_Hand.xaml 的交互逻辑
	/// </summary>
	public partial class US_WPF_Hand : UserControl
	{
		private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
		private int _controlInterval =  3000;
        private double _fillOpacity = 0.5;

        bool isTicked = false;

        private Storyboard SB_Motion = new Storyboard();
        private Storyboard SB_Stop = new Storyboard();

        /// <summary>
        /// 产生事件的毫秒数
        /// </summary>
        public int ControlInterval { get { return _controlInterval; } set { _controlInterval = value; } }
        /// <summary>
        /// 填充进度条的不透明度，大于0小于1
        /// </summary>
        public double FillOpacity
        { get { return _fillOpacity; }
          set
            {
                if (value > 1)
                {
                    throw new Exception("不透明度应该大于0小于1，当前不透明度为" + value.ToString());
                }
                else if (value < 0)
                {
                    throw new Exception("不透明度应该大于0小于1，当前不透明度为" + value.ToString());
                }
                else
                {
                    _fillOpacity = value;
                }
            }
        }

        //定义委托
        public delegate void HandTickHandler(object sender, EventArgs e);
        //定义事件
        public event HandTickHandler HandTick;

		public US_WPF_Hand()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
            HandPic.Source = Imaging.CreateBitmapSourceFromHBitmap(HandRes.Hand.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			_dispatcherTimer.Interval = TimeSpan.FromMilliseconds(_controlInterval);
			_dispatcherTimer.Tick += Timer_Tick;

            //定义Stoyboard
            DoubleAnimationUsingKeyFrames tempDoubleKeyFrames = new DoubleAnimationUsingKeyFrames();
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(4, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(57, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_controlInterval))));
            Storyboard.SetTargetName(tempDoubleKeyFrames,rectangle.Name);
            Storyboard.SetTargetProperty(tempDoubleKeyFrames, new PropertyPath("(FrameworkElement.Width)"));
            SB_Motion.Children.Add(tempDoubleKeyFrames);

            ThicknessAnimationUsingKeyFrames tempThicknessKeyFrames = new ThicknessAnimationUsingKeyFrames();
            tempThicknessKeyFrames.KeyFrames.Add(new EasingThicknessKeyFrame(new Thickness(28, 28, 0, -28), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            tempThicknessKeyFrames.KeyFrames.Add(new EasingThicknessKeyFrame(new Thickness(1, 1, -59, -38), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_controlInterval))));
            Storyboard.SetTargetName(tempThicknessKeyFrames, rectangle.Name);
            Storyboard.SetTargetProperty(tempThicknessKeyFrames, new PropertyPath("(FrameworkElement.Margin)"));
            SB_Motion.Children.Add(tempThicknessKeyFrames);

            //SB_Motion.Children.Add(((Storyboard)FindResource("Motion_Scale")).Children[0]);
            //SB_Motion.Children.Add(((Storyboard)FindResource("Motion_Scale")).Children[1]);

            tempDoubleKeyFrames = new DoubleAnimationUsingKeyFrames();
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_controlInterval))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_controlInterval + 500))));
            Storyboard.SetTargetName(tempDoubleKeyFrames, thCircleLine_Copy.Name);
            Storyboard.SetTargetProperty(tempDoubleKeyFrames, new PropertyPath("(UIElement.Opacity)"));
            SB_Motion.Children.Add(tempDoubleKeyFrames);

            tempDoubleKeyFrames = new DoubleAnimationUsingKeyFrames();
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(_fillOpacity, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(_fillOpacity, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_controlInterval))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_controlInterval + 500))));
            Storyboard.SetTargetName(tempDoubleKeyFrames, rectangle.Name);
            Storyboard.SetTargetProperty(tempDoubleKeyFrames, new PropertyPath("(UIElement.Opacity)"));
            SB_Motion.Children.Add(tempDoubleKeyFrames);

            tempDoubleKeyFrames = new DoubleAnimationUsingKeyFrames();
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(_fillOpacity, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500))));
            Storyboard.SetTargetName(tempDoubleKeyFrames, rectangle.Name);
            Storyboard.SetTargetProperty(tempDoubleKeyFrames, new PropertyPath("(UIElement.Opacity)"));
            SB_Stop.Children.Add(tempDoubleKeyFrames);

            tempDoubleKeyFrames = new DoubleAnimationUsingKeyFrames();
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            tempDoubleKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500))));
            Storyboard.SetTargetName(tempDoubleKeyFrames, thCircleLine_Copy.Name);
            Storyboard.SetTargetProperty(tempDoubleKeyFrames, new PropertyPath("(UIElement.Opacity)"));
            SB_Stop.Children.Add(tempDoubleKeyFrames);





            //            <Storyboard x:Key="Stop">
            //    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Opacity)" Storyboard.TargetName="rectangle">
            //        <EasingDoubleKeyFrame KeyTime="0" Value="0.5"/>
            //        <EasingDoubleKeyFrame KeyTime="0:0:0.5" Value="0"/>
            //    </DoubleAnimationUsingKeyFrames>
            //    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Opacity)" Storyboard.TargetName="thCircleLine_Copy">
            //        <EasingDoubleKeyFrame KeyTime="0" Value="1"/>
            //        <EasingDoubleKeyFrame KeyTime="0:0:0.5" Value="0"/>
            //    </DoubleAnimationUsingKeyFrames>
            //</Storyboard>
        }

		private void Timer_Tick(object sender, EventArgs e)
		{
            isTicked = true;
            _dispatcherTimer.Stop();
            HandTick?.Invoke(sender, e);//触发事件
        }

        public void StartTimer()
        {
            isTicked = false;
            _dispatcherTimer.Start();
            BeginStoryboard(SB_Motion);
            BeginStoryboard((Storyboard)FindResource("Motion_Scale"));
        }

        public void EndTimer()
        {
            _dispatcherTimer.Stop();
            //BeginStoryboard((Storyboard)FindResource("Stop"));
            if (isTicked == false)
            {
                BeginStoryboard(SB_Stop);
            }

            isTicked = false;
        }
	}
}
