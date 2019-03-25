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

using AForge.Video;
using AForge.Video.DirectShow;


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
