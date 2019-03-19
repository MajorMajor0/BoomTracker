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
			}
		}

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
		private async void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			if (!block)
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

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
