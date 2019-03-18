using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using AForge.Video;
using AForge.Video.DirectShow;


namespace BoomTracker
{
	public partial class MainWindowViewModel : INotifyPropertyChanged
	{
		public Game Game { get; set; }

		private bool takeScreen;

		private bool gameOn;
		public bool GameOn
		{
			get => gameOn;
			private set
			{
				gameOn = value;
				OnPropertyChanged(nameof(GameOn));
				OnPropertyChanged(nameof(GameOff));
			}
		}

		public bool GameOff => !GameOn;

		private double[] timeAverager = new double[60];
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
			InitializeCommands();
			Game = new Game();
			Game.PropertyChanged += GamePropertyChanged;
			GetVideoDevices();
		}

		private void GamePropertyChanged(object sender, PropertyChangedEventArgs a)
		{
			if (a.PropertyName == nameof(Game.CurrentScore))
			{
				//takeScreen = true;
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

		private static bool block;
		private static bool evenFrame;
		private async void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			evenFrame = true;

			if (!block && evenFrame)
			{
				watch.Restart();
				//try
				//{
				//BitmapImage bi;

				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					block = true;

					if (Game.CheckIfGameIsOn(bitmap))
					{
						GameOn = true;
						await Game.StoreState(bitmap);
						
					}

					else
					{
						GameOn = false;
					}

					if (takeScreen)
					{
						//using (Bitmap scoreBitmap = bitmap.Clone(Tetris.ScoreField.ScoreRectangle, Tetris.PixelFormat))
						//{
						//	scoreBitmap.Save($"C:\\Source\\BoomTracker\\BoomTracker\\Resources\\Scores1\\{Game.CurrentScore}({Game.CurrentScoreConfidence}).bmp", ImageFormat.Bmp);
						//}

						takeScreen = false;
					}

					//DrawFields(bitmap);
					//bi = bitmap.ToBitmapImage();
					block = false;
				}

				// Avoid cross thread operations and prevent leaks
				//bi.Freeze();
				//MainImage = bi;
				//}

				//catch (Exception ex)
				//{
				//	MessageBox.Show($"Error on _videoSource_NewFrame:\n{ex.Message}",
				//	"Error",
				//	MessageBoxButton.OK,
				//	MessageBoxImage.Error);
				//	Stop();
				//}

				if (iav < timeAverager.Length)
				{
					timeAverager[iav++] = watch.ElapsedMilliseconds;
				}
				else
				{
					iav = 0;
					Timer = Math.Round(timeAverager.Average(), 1).ToString();
				}
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
