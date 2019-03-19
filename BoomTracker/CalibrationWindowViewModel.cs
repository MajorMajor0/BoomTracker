using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;

namespace BoomTracker
{
	public partial class CalibrationWindowViewModel : INotifyPropertyChanged
	{
		private bool takeScreen;

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

		public ObservableCollection<FilterInfo> VideoDevices { get; set; }

		private FilterInfo currentDevice;
		public FilterInfo CurrentDevice
		{
			get => currentDevice;
			set
			{
				currentDevice = value;
				OnPropertyChanged(nameof(CurrentDevice));
			}
		}

		private IVideoSource videoSource;

		public int RectX { get; set; } = Tetris.PlayingField.Rectangle.Left;
		public int RectY { get; set; } = Tetris.PlayingField.Rectangle.Top;
		public int RectWidth { get; set; } = Tetris.PlayingField.Rectangle.Width;
		public int RectHeight { get; set; } = Tetris.PlayingField.Rectangle.Height;

		public int Rect2X { get; set; } = Tetris.NextField.Rectangle.Left;
		public int Rect2Y { get; set; } = Tetris.NextField.Rectangle.Top;
		public int Rect2Width { get; set; } = Tetris.NextField.Rectangle.Width;
		public int Rect2Height { get; set; } = Tetris.NextField.Rectangle.Height;

		public CalibrationWindowViewModel()
		{
			InitializeCommands();
			GetVideoDevices();
		}

		private static bool block;
		private void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			if (!block)
			{
				try
				{
					BitmapImage bi;

					using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
					{
						block = true;

						if (takeScreen)
						{
							using (Bitmap scoreBitmap = bitmap.Clone(new Rectangle(0, 0, Tetris.ImageWidth, Tetris.ImageWidth), Tetris.PixelFormat))
							{
								scoreBitmap.Save($"{FileLocation.Screens}{DateTime.Now.ToString()}.bmp", ImageFormat.Bmp);
							}

							takeScreen = false;
						}

						DrawFields(bitmap);
						bi = bitmap.ToBitmapImage();
						block = false;
					}

					//Avoid cross thread operations and prevent leaks
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

		private Pen orangePen = new Pen(Color.Orange, 1);
		private Pen bluePen = new Pen(Color.Blue, 1);
		private void DrawFields(Bitmap bitmap)
		{
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				//foreach (var xy in Tetris.PlayingField.BlockPixels)
				//{
				//	bitmap.SetPixel(xy[0], xy[1], Color.Red);
				//}

				//foreach (var rect in Tetris.PlayingField.BlockFields)
				//{
				//	g.DrawRectangle(orangePen, rect);
				//}
				g.DrawRectangle(bluePen, RectX, RectY, RectWidth, RectHeight);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
