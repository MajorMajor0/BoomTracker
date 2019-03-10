using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;


namespace BoomTracker
{
	public partial class MainWindowViewModel : INotifyPropertyChanged
	{
		public Game Game { get; } = new Game();

		private Game.State state = new Game.State();
		public Game.State State
		{
			get => state;
			set
			{
				state = value;
				OnPropertyChanged(nameof(State));
				OnPropertyChanged(nameof(Game));
			}
		}

		private double[] averager = new double[60];
		int iav = 0;

		private string timer;
		public string Timer
		{
			get => timer;
			set
			{
				if (value != timer)
				{
					timer = value;
					OnPropertyChanged(nameof(Timer));
				}
			}
		}

		private Stopwatch watch = Stopwatch.StartNew();
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

		private BitmapImage levelImage;
		public BitmapImage LevelImage
		{
			get => levelImage;
			set
			{
				levelImage = value;
				OnPropertyChanged(nameof(LevelImage));
			}
		}

		private BitmapImage linesImage;
		public BitmapImage LinesImage
		{
			get => linesImage;
			set
			{
				linesImage = value;
				OnPropertyChanged(nameof(LinesImage));
			}
		}

		private BitmapImage scoreImage;
		public BitmapImage ScoreImage
		{
			get => scoreImage;
			set
			{
				scoreImage = value;
				OnPropertyChanged(nameof(ScoreImage));
			}
		}

		public int RectX { get; set; } = Tetris.PlayingField.Rectangle.Left;
		public int RectY { get; set; } = Tetris.PlayingField.Rectangle.Top;
		public int RectWidth { get; set; } = Tetris.PlayingField.Rectangle.Width;
		public int RectHeight { get; set; } = Tetris.PlayingField.Rectangle.Height;

		public int Rect2X { get; set; } = Tetris.NextField.Rectangle.Left;
		public int Rect2Y { get; set; } = Tetris.NextField.Rectangle.Top;
		public int Rect2Width { get; set; } = Tetris.NextField.Rectangle.Width;
		public int Rect2Height { get; set; } = Tetris.NextField.Rectangle.Height;

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

		private static bool block;
		private static bool evenFrame;
		private async void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			evenFrame = !evenFrame;

			if (!block && evenFrame)
			{
				watch.Restart();
				try
				{
					BitmapImage bi;

				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					block = true;
					State = await Game.StoreState(bitmap);
					block = false;

					//DrawFields(bitmap);

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

			if (iav < averager.Length)
				{
					averager[iav++] = watch.ElapsedMilliseconds;
				}
				else
				{
					iav = 0;
					Timer = Math.Round(averager.Average(), 1).ToString();
				}
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

		Pen orangePen = new Pen(Color.Orange, 1);
		Pen bluePen = new Pen(Color.Blue, 1);
		Pen greenPen = new Pen(Color.Green, 2);
		private void DrawFields(Bitmap bitmap)
		{
		//using (Graphics g = Graphics.FromImage(bitmap))
			//{
			//	foreach (var xy in Tetris.PlayingField.BlockPixels)
			//	{
			//		bitmap.SetPixel(xy[0], xy[1], Color.Red);
			//	}

			//	//foreach (var rect in Tetris.PlayingField.BlockFields)
			//	//{
			//	//	g.DrawRectangle(orangePen, rect);
			//	//}
			//	//g.DrawRectangle(bluePen, RectX, RectY, RectWidth, RectHeight);
			//}


			//g.DrawRectangle(orangePen, TetrisWindow.LineField.LinesRectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.PlayingField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.NextField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.ScoreField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.LineField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.LevelField.Rectangle);
			//g.DrawRectangle(bluePen, TetrisWindow.ScoreField.ScoreRectangle);
			//Debug.WriteLine(watch.ElapsedMilliseconds);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
