using LiveGraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestInterface.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestLiveGraph : Page
    {
        private const int N = 256;
        private GraphCanvas graph;
        private WriteableBitmap wb;
        private DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100.0) };
        private Random rnd = new Random();
        private uint[] points = new uint[N];
        private int pointsCount;
        private uint[] pointsToDraw = new uint[256];
        private int ticks;
        private float left;
        private double? lastRenderTime;

        public TestLiveGraph()
        {
            this.InitializeComponent();

            CompositionTarget.Rendering += CompositionTarget_Rendering;
            Loaded += MainPage_Loaded;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            timer.Tick += Timer_Tick;

            wb = new WriteableBitmap((int)image.Width, (int)image.Height);
            graph = new GraphCanvas(wb);
            image.Source = wb;

            left = wb.PixelWidth;

            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (pointsCount < points.Length)
            {
                var r = (double)wb.PixelHeight * 0.5 + ((rnd.NextDouble() * 30.0) - 15.0);
                int x = ticks;
                int y = (int)r;
                uint p = (uint)(x | (y << 16));
                points[pointsCount] = p;
                pointsCount = (pointsCount + 1) % points.Length;
            }

            ++ticks;

            //if (!graph.AddDataPoint((int)r))
            //{
            //    Debug.WriteLine("Pipe full!");
            //}
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            textBlock.Text = graph.Foo();
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Escape)
                Application.Current.Exit();
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            var re = e as RenderingEventArgs;
            var curRenderTime = re.RenderingTime.TotalSeconds;
            lastRenderTime = lastRenderTime ?? curRenderTime;

            float dt = (float)(curRenderTime - lastRenderTime);
            lastRenderTime = curRenderTime;

            //const int dx = 5;
            int drawCount = 0;
            for (int i = 0; i < pointsCount; ++i)
            {
                uint p = points[i];
                int x = (short)(p & 0xFFFF);
                int y = (short)(p >> 16);

                //x = x+dx*i;
                x = x * 5;

                if (((int)left + x) >= 0)
                {
                    pointsToDraw[drawCount++] = (uint)(x | (y << 16));

                    //points[i] = (uint)(x | (y << 16));
                }
                else
                {
                    //points[i] = points[pointsCount - 1];
                    //points[pointsCount - 1] = p;
                    //--pointsCount;
                }
            }
            graph.Draw((int)left, pointsToDraw, drawCount);
            wb.Invalidate();

            left -= dt * 30.0f;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

