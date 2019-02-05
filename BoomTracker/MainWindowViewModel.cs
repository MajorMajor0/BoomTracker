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
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;

namespace BoomTracker
{
	public partial class MainWindowViewModel : INotifyPropertyChanged
	{
		private bool grabNextScreen;

		private BitmapImage mainImage;

		public BitmapImage MainImage
		{
			get => mainImage;
			set
			{
				mainImage = value;
				OnPropertyChanged(nameof(MainImage));
			}
		}

		public int Threshold
		{
			get => TetrisWindow.BlockThreshold;
			set => TetrisWindow.BlockThreshold = value;
		}

		public int RectX { get; set; } = TetrisWindow.PlayingField.LeftPixel;
		public int RectY { get; set; } = TetrisWindow.PlayingField.TopPixel;
		public int RectWidth { get; set; } = TetrisWindow.PlayingField.Width;
		public int RectHeight { get; set; } = TetrisWindow.PlayingField.Height;

		public int Rect2X { get; set; }
		public int Rect2Y { get; set; }
		public int Rect2Width { get; set; }
		public int Rect2Height { get; set; }

		public ObservableCollection<FilterInfo> VideoDevices { get; set; }

		public FilterInfo CurrentDevice
		{
			get => currentDevice;
			set
			{
				currentDevice = value;
				OnPropertyChanged(nameof(CurrentDevice));
			}
		}

		private FilterInfo currentDevice;

		private IVideoSource videoSource;

		public MainWindowViewModel()
		{
			GetVideoDevices();
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

		private void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			try
			{
				BitmapImage bi;
				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					Paint(bitmap);
					bi = bitmap.ToBitmapImage();
				}

				// Avoid cross thread operations and prevent leaks
				bi.Freeze();
				MainImage = bi;
			}

			catch (Exception ex)
			{
				MessageBox.Show($"Error on _videoSource_NewFrame:\n{ex.Message}",
				"Error",
				MessageBoxButton.OK,
				MessageBoxImage.Error);
				Stop();
			}
		}

		private Bitmap GetImage()
		{
			Bitmap returner = null;
			string fileName = @"C:\Source\BoomTracker\BoomTracker\Resources\Level2.bmp";
			try
			{
				Bitmap bm = (Bitmap)Image.FromFile(fileName);
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

			using (Pen bluePen = new Pen(Color.Orange, 1))
			using (Pen redPen = new Pen(Color.Red, 2))
			using (Pen greenPen = new Pen(Color.Green, 2))
			{
				g.DrawRectangle(bluePen, RectX, RectY, RectWidth, RectHeight);
				g.DrawRectangle(redPen, Rect2X, Rect2Y, Rect2Width, Rect2Height);

				for (int i = 0; i < TetrisWindow.PlayingField.HorizontalSquares; i++)
				{
					for (int j = 0; j < TetrisWindow.PlayingField.VerticalSquares; j++)
					{

						int x = TetrisWindow.PlayingField.GridPoints[i, j, 0];
						int y = TetrisWindow.PlayingField.GridPoints[i, j, 1];

						Color color = bitmap.GetPixel(x, y);

						if (TetrisWindow.HasBlock(color))
						{
							g.DrawEllipse(greenPen, x, y, 1, 1);
						}

						//Debug.WriteLine($"({x}, {y})");
					}
				}
			}
		}




		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
