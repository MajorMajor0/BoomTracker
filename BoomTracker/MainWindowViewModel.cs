/*This file is part of BoomTracker.
 * 
 * BoomTracker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published 
 * version 3 of the License, or (at your option) any later version.
 * 
 * BoomTracker is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU 
 * General Public License for more details. 
 * 
 * You should have received a copy of the GNU General Public License
 *  along with BoomTracker.  If not, see<http://www.gnu.org/licenses/>.*/

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
		public string CurrentName => CurrentPlayer?.Name?.ToUpper();

		public int PersonalBest => CalculatePB();


		private Player currentPlayer;
		public Player CurrentPlayer
		{
			get => currentPlayer;
			set
			{
				if (currentPlayer != value)
				{
					currentPlayer = value;
					OnPropertyChanged(nameof(CurrentPlayer));
					OnPropertyChanged(nameof(CurrentName));
					OnPropertyChanged(nameof(PersonalBest));
				}
			}
		}

		public ObservableCollection<Player> Players => Data.Players;

		private Game game;
		public Game Game
		{
			get => game;
			set
			{
				if (value != game)
				{
					game = value;
					OnPropertyChanged(nameof(Game));
				}
			}
		}

		private bool takeScreen;
#if DEBUG
		private bool takeScore;

		private bool takeLines;

		private bool takeLevel;
#endif
		private bool topOutHold;

		private bool gameOn;
		public bool GameOn
		{
			get => gameOn;
			private set
			{
				if (value != gameOn)
				{
					gameOn = value;

					if (gameOn)
					{
						Game = new Game();
						Game.TopOut += ToppedOut;
						Game.PropertyChanged += ScoreChanged;
					}

					else
					{
						if (game.States.Count > 900)
						{
							Game.TopOut -= ToppedOut;
							Game.PropertyChanged -= ScoreChanged;
							CurrentPlayer.Games.Add(Game);
						}

						Game = null;
					}

					OnPropertyChanged(nameof(GameOn));
					OnPropertyChanged(nameof(GameOnBrush));
					OnPropertyChanged(nameof(GameOnString));
				}
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

		private void ToppedOut(object sender, EventArgs a)
		{
			topOutHold = true;
			GameOn = false;
		}

		private void ScoreChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentScore")
			{
				OnPropertyChanged(nameof(PersonalBest));
			}
		}

		private int CalculatePB()
		{
			if (CurrentPlayer is null)
			{
				return 0;
			}

			return Math.Max(CurrentPlayer.PersonalBestScore, Game.CurrentScore);
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
					if (topOutHold)
					{
						if (Tetris.GameInProgress.Check(bitmap) && Tetris.NintendoOn.Check(bitmap))
						{
							block = false;
							return;
						}

						else
						{
							topOutHold = false;
						}
					}


					if (Tetris.GameInProgress.Check(bitmap) && Tetris.NintendoOn.Check(bitmap))
					{
						GameOn = true;
						Game.StoreState(bitmap);
					}

					else
					{
						GameOn = false;
					}

					if (takeScreen)
					{
						StoreScreen(bitmap);
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
				try
				{
					using (Bitmap scoreBitmap = bitmap.Clone(Tetris.Image, Tetris.PixelFormat))
					{
						scoreBitmap.Save($"{FileLocation.Screens}\\{DateTime.Now.ToString("yyyy-MM-dd HHmmss")}.bmp", ImageFormat.Bmp);
					}
				}
				catch { }

			});
		}
#if DEBUG
		private void StashBoxes(Bitmap bitmap)
		{
			if (takeScore)
			{
				DebugStuff.StashScoreBox(bitmap, Game.CurrentScore);
				takeScore = false;
			}

			if (takeLines)
			{
				DebugStuff.StashLinesBox(bitmap, Game.CurrentLines);
				takeLines = false;
			}

			if (takeLevel)
			{
				DebugStuff.StashLevelBox(bitmap, Game.CurrentLevel);
				takeLevel = false;
			}
		}
#endif
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			Debug.WriteLine(prop);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
