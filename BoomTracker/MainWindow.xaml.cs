using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;

namespace BoomTracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

		public int RectX { get; set; }
		public int RectY { get; set; }
		public int RectWidth { get; set; }
		public int RectHeight { get; set; }

		public int Rect2X { get; set; }
		public int Rect2Y { get; set; }
		public int Rect2Width { get; set; }
		public int Rect2Height { get; set; }

		public ObservableCollection<FilterInfo> VideoDevices { get; set; }

		public FilterInfo CurrentDevice
		{
			get { return currentDevice; }
			set { currentDevice = value; this.OnPropertyChanged("CurrentDevice"); }
		}

		private FilterInfo currentDevice;

		private IVideoSource videoSource;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			GetVideoDevices();
			Closing += MainWindow_Closing;
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			StopCamera();
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			StartCamera();
		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			StopCamera();
		}

		private void ImageButton_Click(object sender, RoutedEventArgs e)
		{


			StopCamera();

			Bitmap bitmap = GetImage();

			Paint(bitmap);
			MainImage.Source = bitmap.ToBitmapImage();

			BlobCounter bc = new BlobCounter();
			bc.MaxHeight = 30;
			bc.MaxWidth = 30;
			bc.MinHeight = 17;
			bc.MinWidth = 17;
			bc.BackgroundThreshold = System.Drawing.Color.FromArgb(20, 20, 20);

			bc.ProcessImage(bitmap);

			var blobs = bc.GetObjectsInformation();
			var rects = bc.GetObjectsRectangles();

			var fieldBlobs = blobs.Where(x =>
				x.CenterOfGravity.X > TetrisWindow.PlayingField.LeftPixel &&
				x.CenterOfGravity.X < TetrisWindow.PlayingField.RightPixel &&
				x.CenterOfGravity.Y > TetrisWindow.PlayingField.BottomPixel &&
				x.CenterOfGravity.Y < TetrisWindow.PlayingField.TopPixel);

			var fieldrects = rects.Where(x =>
				x.Location.X > TetrisWindow.PlayingField.LeftPixel &&
				x.Location.X < TetrisWindow.PlayingField.RightPixel &&
				x.Location.Y > TetrisWindow.PlayingField.BottomPixel &&
				x.Location.Y < TetrisWindow.PlayingField.TopPixel);
			// Expected blobs = 84

			var blobSizes = blobs.Select(x => $"({x.Rectangle.Height} x {x.Rectangle.Width})");
		}

		private void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			try
			{
				BitmapImage bi;
				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					bi = bitmap.ToBitmapImage();
				}

				// Avoid cross thread operations and prevent leaks
				bi.Freeze();

				Dispatcher.BeginInvoke(new ThreadStart(delegate { MainImage.Source = bi; }));
			}

			catch (Exception ex)
			{
				MessageBox.Show($"Error on _videoSource_NewFrame:\n{ex.Message}",
				"Error",
				MessageBoxButton.OK,
				MessageBoxImage.Error);
				StopCamera();
			}
		}

		private void GetVideoDevices()
		{
			VideoDevices = new ObservableCollection<FilterInfo>();
			foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
			{
				VideoDevices.Add(filterInfo);
			}

			if (VideoDevices.Any())
			{
				CurrentDevice = VideoDevices[0];
			}

			else
			{
				MessageBox.Show("No video sources found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void StartCamera()
		{
			if (CurrentDevice != null)
			{
				videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
				videoSource.NewFrame += OnNewFrame;
				videoSource.Start();
			}
		}

		private void StopCamera()
		{
			if (videoSource != null && videoSource.IsRunning)
			{
				videoSource.SignalToStop();
				videoSource.NewFrame -= new NewFrameEventHandler(OnNewFrame);
			}
		}

		private Bitmap GetImage()
		{
			Bitmap returner = null;
			string fileName = @"C:\Source\BoomTracker\BoomTracker\Resources\Level2.bmp";
			try
			{
				Bitmap bm = (Bitmap)System.Drawing.Image.FromFile(fileName);
				returner = bm;
			}

			catch
			{
				MessageBox.Show($"{fileName} bonked.");
			}

			return returner;
			//var x = Grayscale.CommonAlgorithms.BT709.Apply(bm);
		}


		private void Paint(Bitmap bitmap)
		{
			using (Graphics g = Graphics.FromImage(bitmap))

			using (System.Drawing.Pen bluePen = new System.Drawing.Pen(System.Drawing.Color.Blue, 2))
			using (System.Drawing.Pen redPen = new System.Drawing.Pen(System.Drawing.Color.Red, 2))
			{
				//g.DrawEllipse(bluePen, 271,78, 10, 10);


				g.DrawRectangle(bluePen, RectX, RectY, RectWidth, RectHeight);
				g.DrawRectangle(redPen, Rect2X, Rect2Y, Rect2Width, Rect2Height);

			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}
	}
}
