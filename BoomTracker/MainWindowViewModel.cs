using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AForge.Video;
using AForge.Video.DirectShow;


namespace BoomTracker
{
	public partial class MainWindowViewModel : INotifyPropertyChanged
	{
		public Player CurrentPlayer { get; set; }

		public ObservableCollection<Player> Players => Data.Players;

		public Game Game { get; set; }

		private bool takeScreen;

		//private bool takeScore;

		//private bool takeLines;

		//private bool takeLevel;

		private bool gameOn;
		public bool GameOn
		{
			get => gameOn;
			private set
			{
				gameOn = value;
				OnPropertyChanged(nameof(GameOn));
				OnPropertyChanged(nameof(GameOnBrush));
				OnPropertyChanged(nameof(GameOnString));
			}
		}

		public string GameOnString => gameOn ? "Game On" : "Game Off";

		private static SolidColorBrush onBrush = new SolidColorBrush(Colors.Green);
		private static SolidColorBrush offBrush = new SolidColorBrush(Colors.Red);
		public System.Windows.Media.Brush GameOnBrush => gameOn ? onBrush : offBrush;

		private double[] timeAverager = new double[60];
		private int iav = 0;

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
			//Game.PropertyChanged += GamePropertyChanged;
			GetVideoDevices();
		}

		//private void GamePropertyChanged(object sender, PropertyChangedEventArgs a)
		//{
		//	if (a.PropertyName == nameof(Game.CurrentScore))
		//	{
		//		takeScore = true;
		//	}

		//	if (a.PropertyName == nameof(Game.CurrentLevel))
		//	{
		//		takeLevel = true;
		//	}

		//	if (a.PropertyName == nameof(Game.CurrentLines))
		//	{
		//		takeLines = true;
		//	}
		//}

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
		private void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			if (!block)
			{
				watch.Restart();

				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					block = true;

					if (Game.CheckIfGameIsOn(bitmap))
					{
						GameOn = true;
						Game.StoreState(bitmap);
						//#if DEBUG
						//						if (takeScore)
						//						{
						//							DebugStuff.StashScoreBox(bitmap, Game.CurrentScore);
						//							takeScore = false;
						//						}

						//						if (takeLines)
						//						{
						//							DebugStuff.StashLinesBox(bitmap, Game.CurrentLines);
						//							takeLines = false;
						//						}

						//						if (takeLevel)
						//						{
						//							DebugStuff.StashLevelBox(bitmap, Game.CurrentLevel);
						//							takeLevel = false;
						//						}
						//#endif
					}

					else
					{
						GameOn = false;
					}

					if (takeScreen)
					{
						//StoreScreen(bitmap);
						takeScreen = false;
					}

					block = false;
				}

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

		private void StoreScreen(Bitmap bitmap)
		{
			Task.Run(() =>
			{
				using (Bitmap scoreBitmap = bitmap.Clone(Tetris.Image, Tetris.PixelFormat))
				{
					scoreBitmap.Save($"{FileLocation.Screens}\\{DateTime.Now.ToShortDateString()}({DateTime.Now.ToShortTimeString()}).bmp", ImageFormat.Bmp);
				}
			});
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
