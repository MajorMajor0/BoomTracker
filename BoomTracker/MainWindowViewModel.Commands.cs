using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;

namespace BoomTracker
{
	public partial class MainWindowViewModel
	{
		public Command StartCommand => new Command(Start, StartCanExecute, "Start", "Start the selected camera.");

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


		public Command StopCommand => new Command(Stop, StopCanExecute, "Stop", "Stop the selected camera.");

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


		public Command ImageCommand => new Command(SetImage, ImageCanExecute, "Load Static", "Get static image from file.");

		private bool ImageCanExecute()
		{
			return true;
		}

		private void SetImage()
		{
			Stop();

			Bitmap bitmap = GetImage();

			//Paint(bitmap);
			MainImage = bitmap.ToBitmapImage();
		}

		public Command ScreenshotCommand => new Command(Screenshot, ScreenShotCanExecute, "Screen Shot", "Save screen shot to file.");
		private bool ScreenShotCanExecute()
		{
			return true;
		}

		private void Screenshot()
		{
			string fileName = $"Screen {DateTime.Now.ToString()}";
			
		}
	}
}
