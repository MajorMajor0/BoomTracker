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
		private int calibrationNumber;
		public int CalibrationNumber
		{
			get => calibrationNumber;
			set
			{
				calibrationNumber = value;
				OnPropertyChanged(nameof(CalibrationNumber));
			}
		}

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

		public int Rect2X { get; set; } = Tetris.Next.Rectangle.Left;
		public int Rect2Y { get; set; } = Tetris.Next.Rectangle.Top;
		public int Rect2Width { get; set; } = Tetris.Next.Rectangle.Width;
		public int Rect2Height { get; set; } = Tetris.Next.Rectangle.Height;

		public CalibrationWindowViewModel()
		{
			InitializeCommands();
			GetVideoDevices();
		}

		private void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			try
			{
				BitmapImage bi;

				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					DrawFields(bitmap);

					WriteScoreCalibration(bitmap);
					WriteLevelCalibration(bitmap);
					WriteLineCalibration(bitmap);

					if (takeScreen)
					{
						using (Bitmap scoreBitmap = bitmap.Clone(new Rectangle(0, 0, Tetris.Image.Width, Tetris.Image.Height), Tetris.PixelFormat))
						{
							string fileName = $"{FileLocation.Screens}{DateTime.Now.ToString()}.bmp";
							scoreBitmap.Save($"{FileLocation.Screens}\\{DateTime.Now.ToString("yyyy-MM-dd HHmmss")}.bmp", ImageFormat.Bmp);
						}

						takeScreen = false;
					}

					bi = bitmap.ToBitmapImage();
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

		private unsafe void WriteScoreCalibration(Bitmap bitmap)
		{
			BitmapData bmData = bitmap.LockBits(Tetris.Score.Rectangle, ImageLockMode.ReadOnly, Tetris.PixelFormat);
			byte* scan0 = (byte*)bmData.Scan0.ToPointer();

			for (int place = 0; place < 6; place++)
			{
				foreach (var address in Tetris.Score.Addresses[CalibrationNumber, place])
				{
					scan0[address] = 255;// Blue
					scan0[address + 1] = 0; // Green
					scan0[address + 2] = 0; // Red
				}
			}
			bitmap.UnlockBits(bmData);
		}

		private unsafe void WriteLevelCalibration(Bitmap bitmap)
		{
			BitmapData bmData = bitmap.LockBits(Tetris.Level.Rectangle, ImageLockMode.ReadOnly, Tetris.PixelFormat);
			byte* scan0 = (byte*)bmData.Scan0.ToPointer();

			for (int place = 0; place < 2; place++)
			{
				foreach (var address in Tetris.Level.Addresses[CalibrationNumber, place])
				{
					scan0[address] = 255;// Blue
					scan0[address + 1] = 0; // Green
					scan0[address + 2] = 0; // Red
				}
			}
			bitmap.UnlockBits(bmData);
		}

		private unsafe void WriteLineCalibration(Bitmap bitmap)
		{
			BitmapData bmData = bitmap.LockBits(Tetris.Lines.Rectangle, ImageLockMode.ReadOnly, Tetris.PixelFormat);
			byte* scan0 = (byte*)bmData.Scan0.ToPointer();

			for (int place = 0; place < 3; place++)
			{
				foreach (var address in Tetris.Lines.Addresses[CalibrationNumber, place])
				{
					scan0[address] = 255;// Blue
					scan0[address + 1] = 0; // Green
					scan0[address + 2] = 0; // Red
				}
			}
			bitmap.UnlockBits(bmData);
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
