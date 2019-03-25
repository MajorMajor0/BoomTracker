using AForge.Video;
using AForge.Video.DirectShow;

namespace BoomTracker
{
	public partial class MainWindowViewModel
	{
		private void InitializeCommands()
		{
			StartCommand = new Command(Start, StartCanExecute, "Start", "Start the selected camera.");
			StopCommand = new Command(Stop, StopCanExecute, "Stop", "Stop the selected camera.");
			ScreenshotCommand = new Command(Screenshot, ScreenShotCanExecute, "Screen Shot", "Save screen shot to file.");
			CalibrationCommand = new Command(Calibration, CalibrationCanExecute, "Calibration Window", "Open the calibration window.");
			HelpCommand = new Command(Help, HelpCanExecute, "Help Window", "Open the help window.");
			AboutCommand = new Command(About, AboutCanExecute, "About Window", "Open the about window.");
			NewPlayerCommand = new Command(NewPlayer, NewPlayerCanExecute, "Add Player", "Add a new player");
		}

		public Command StartCommand { get; private set; }

		private bool StartCanExecute()
		{
			return true;
		}

		private void Start()
		{
			if (CurrentDevice != null)
			{
				videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
				videoSource.NewFrame += OnNewFrame;
				videoSource.Start();
			}
		}


		public Command StopCommand { get; private set; }

		private bool StopCanExecute()
		{
			return true;
		}

		public void Stop()
		{
			if (videoSource != null && videoSource.IsRunning)
			{
				videoSource.SignalToStop();
				videoSource.NewFrame -= new NewFrameEventHandler(OnNewFrame);
			}
		}


		public Command ScreenshotCommand { get; private set; }

		private bool ScreenShotCanExecute()
		{
			return GameOn && !takeScreen;
		}

		private void Screenshot()
		{
			takeScreen = true;
		}


		public Command CalibrationCommand { get; private set; }

		private bool CalibrationCanExecute()
		{
			return true;
		}

		private void Calibration()
		{
			CalibrationWindow cw = new CalibrationWindow();
			cw.Show();
			cw.Activate();
		}


		public Command HelpCommand { get; private set; }

		private bool HelpCanExecute()
		{
			return true;
		}

		private void Help()
		{
		}

		public Command AboutCommand { get; private set; }

		private bool AboutCanExecute()
		{
			return true;
		}

		private void About()
		{
		}

		public Command NewPlayerCommand { get; private set; }

		private bool NewPlayerCanExecute()
		{
			return true;
		}

		private void NewPlayer()
		{
			NewPlayerDialog npd = new NewPlayerDialog();

			if (npd.ShowDialog() ?? false)
			{
				Data.Players.Add(new Player { Name = npd.PlayerName });
				OnPropertyChanged(nameof(Players));
			}
		}
	}
}
