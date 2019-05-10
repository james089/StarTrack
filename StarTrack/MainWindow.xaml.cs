using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static StarTrack.MathFunc;

namespace StarTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static BackgroundWorker runRoutine = new BackgroundWorker();

        private const int UPDATE_INTERVAL = 50; //ms 
        private const int DOT_SIZE = 5; //ms 
        public MainWindow()
        {
            InitializeComponent();
            runRoutine.DoWork += new DoWorkEventHandler(runRoutine_DoWork);
            runRoutine.ProgressChanged += new ProgressChangedEventHandler(runRoutine_ProgressChanged);
            runRoutine.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runRoutine_Completed);
            runRoutine.WorkerReportsProgress = true;
            runRoutine.WorkerSupportsCancellation = true;

            InitAll();
            CB_track1.ItemsSource = List_radius;
            CB_track2.ItemsSource = List_radius;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Coordinate.Visibility = (bool)Chk_showGrid.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            lbl_version.Content = "ver." + Assembly.GetExecutingAssembly().GetName().Version;
        }

        static DateTime startTime;
        private void runRoutine_DoWork(object sender, DoWorkEventArgs e)
        {
            startTime = DateTime.Now;
            while(!runRoutine.CancellationPending)
            {
                runRoutine.ReportProgress((int)e.Argument);
                Thread.Sleep(UPDATE_INTERVAL);
            }
        }

        private void runRoutine_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                var p1 = TCor(GetStarLocation(Convert.ToInt32(TB_vel1.Text), (DateTime.Now - startTime).TotalSeconds, Convert.ToInt32(CB_track1.Text)));
                var p2 = TCor(GetStarLocation(Convert.ToInt32(TB_vel2.Text), (DateTime.Now - startTime).TotalSeconds, Convert.ToInt32(CB_track2.Text)));

                drawStar(p1, p2);
            }
            else if (e.ProgressPercentage == 2)
            {
                var p1 = TCor(GetStarLocation(Convert.ToInt32(TB_vel1.Text), (DateTime.Now - startTime).TotalSeconds, Convert.ToInt32(CB_track1.Text)));
                var p2 = TCor(GetStarLocation(Convert.ToInt32(TB_vel2.Text), (DateTime.Now - startTime).TotalSeconds, Convert.ToInt32(CB_track2.Text)));

                drawLine(p1, p2);
            }
        }

        private void runRoutine_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void CB_track1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            if (CB_track1.SelectedIndex == -1 || CB_track2.SelectedIndex == -1) return;
            mCanvas.Children.Clear();

            var p1 = TCor(new Point(Convert.ToDouble(CB_track1.SelectedValue), 0));
            var p2 = TCor(new Point(Convert.ToDouble(CB_track2.SelectedValue), 0));

            drawStar(p1, p2);
        }

        private void CB_track2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            CB_track1_SelectionChanged(null, null);
        }

        private void drawStar(Point p1, Point p2)
        {
            mCanvas.Children.Clear();
            Ellipse star1 = CreateEllipse(DOT_SIZE, DOT_SIZE, p1.X, p1.Y);
            Ellipse star2 = CreateEllipse(DOT_SIZE, DOT_SIZE, p2.X, p2.Y);
            star1.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            star2.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            mCanvas.Children.Add(star1);
            mCanvas.Children.Add(star2);
        }

        private void drawLine(Point p1, Point p2)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Color.FromRgb(0, 191, 255));
            line.StrokeThickness = 1; 

            line.X1 = p1.X; line.X2 = p2.X;
            line.Y1 = p1.Y; line.Y2 = p2.Y;
            mCanvas.Children.Add(line);
        }

        private void Btn_runStar_Click(object sender, RoutedEventArgs e)
        {
            mCanvas.Children.Clear();
            if (!runRoutine.IsBusy)
                runRoutine.RunWorkerAsync(1);
        }

        private void Btn_drawLines_Click(object sender, RoutedEventArgs e)
        {
            mCanvas.Children.Clear();
            if (!runRoutine.IsBusy)
                runRoutine.RunWorkerAsync(2);
        }

        private void Btn_stop_Click(object sender, RoutedEventArgs e)
        {
            runRoutine.CancelAsync();
        }

        private void Chk_showGrid_Checked(object sender, RoutedEventArgs e)
        {
            Coordinate.Visibility = (bool)Chk_showGrid.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
