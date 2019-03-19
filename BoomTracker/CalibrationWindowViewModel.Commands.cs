using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using AForge.Video;


namespace BoomTracker
{
    public partial class CalibrationWindowViewModel
	{
		private void InitializeCommands()
		{
			StartCommand = new Command(Start, StartCanExecute, "Start", "Start the selected camera.");
			StopCommand = new Command(Stop, StopCanExecute, "Stop", "Stop the selected camera.");
			ScreenshotCommand = new Command(Screenshot, ScreenShotCanExecute, "Screen Shot", "Save screen shot to file.");
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
			return true;
		}

		private void Screenshot()
		{
			takeScreen = true;
		}
	}
}
