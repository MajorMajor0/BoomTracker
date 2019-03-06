using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using Tesseract;


namespace BoomTracker
{
	public partial class MainWindowViewModel : INotifyPropertyChanged
	{


		public Game Game { get; } = new Game();

		private Game.State state;
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

		public int RectX { get; set; } = TetrisWindow.PlayingField.Rectangle.Left;
		public int RectY { get; set; } = TetrisWindow.PlayingField.Rectangle.Top;
		public int RectWidth { get; set; } = TetrisWindow.PlayingField.Rectangle.Width;
		public int RectHeight { get; set; } = TetrisWindow.PlayingField.Rectangle.Height;

		public int Rect2X { get; set; } = TetrisWindow.NextField.Rectangle.Left;
		public int Rect2Y { get; set; } = TetrisWindow.NextField.Rectangle.Top;
		public int Rect2Width { get; set; } = TetrisWindow.NextField.Rectangle.Width;
		public int Rect2Height { get; set; } = TetrisWindow.NextField.Rectangle.Height;

		public ObservableCollection<FilterInfo> VideoDevices { get; set; }

		public FilterInfo CurrentDevice
		{
			get => currentDevice;
			set
			{
				currentDevice = value;
				OnPropertyChanged(nameof(CurrentDevice));
			}
		}

		private FilterInfo currentDevice;

		private IVideoSource videoSource;



		public Threshold ThresholdFilter { get; set; } = new Threshold(15);
		public Grayscale GsFilter { get; set; } = new Grayscale(0, .1, 0);

		private static int scale => 4;
		private ResizeBicubic resizeBicubic =
		new ResizeBicubic(
		TetrisWindow.ScoreField.ScoreRectangle.Width * scale,
		TetrisWindow.ScoreField.ScoreRectangle.Height * scale);


		private ResizeBilinear resizeBilinear =
		new ResizeBilinear(
		TetrisWindow.ScoreField.ScoreRectangle.Width * scale,
		TetrisWindow.ScoreField.ScoreRectangle.Height * scale);


		public MainWindowViewModel()
		{
			GetVideoDevices();
			ThresholdFilter = new Threshold(15);
			State = new Game.State();
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
		private static bool taskKiller = false;
		private async void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			evenFrame = !evenFrame;

			if (!block && evenFrame)
			{
				watch.Restart();
				//try
				//{
					BitmapImage bi;

					using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
					{
						block = true;
						State = await Game.StoreState(bitmap);
						block = false;

						DrawFields(bitmap);

						if (Game.States.Count == 40)
						{
							bitmap.Save(@"C:\Source\BoomTracker\BoomTracker\Resources\Level0_2.bmp");
						}

						bi = bitmap.ToBitmapImage();
					}

					// Avoid cross thread operations and prevent leaks
					bi.Freeze();
					MainImage = bi;
				//}

				//catch (Exception ex)
				//{
				//	MessageBox.Show($"Error on _videoSource_NewFrame:\n{ex.Message}",
				//	"Error",
				//	MessageBoxButton.OK,
				//	MessageBoxImage.Error);
				//	Stop();
				//}

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

		//private async Task Paint(Bitmap bitmap)
		//{
		//	using (Graphics g = Graphics.FromImage(bitmap))
		//	{
		//		DrawFields(g);
		//	}

		//	using (Bitmap scoreBitmap = bitmap.Clone(TetrisWindow.ScoreField.ScoreRectangle, System.Drawing.Imaging.PixelFormat.DontCare))
		//	using (Bitmap linesBitmap = bitmap.Clone(TetrisWindow.LineField.LinesRectangle, System.Drawing.Imaging.PixelFormat.DontCare))
		//	using (Bitmap levelBitmap = bitmap.Clone(TetrisWindow.LevelField.LevelRectangle, System.Drawing.Imaging.PixelFormat.DontCare))
		//	{

		//		List<Task> tasks = new List<Task>();

		//		tasks.Add(Task.Run(() =>
		//		{
		//			//var scoreBitmapGs = GsFilter.Apply(scoreBitmap);
		//			//scoreBitmapGs = ThresholdFilter.Apply(scoreBitmapGs);
		//			var scoreBitmap2 = resizeBilinear.Apply(scoreBitmap);
		//			using (var scorePage = scoreOCR.Process(scoreBitmap2, PageSegMode.SingleWord))
		//			{
		//				Score = scorePage.GetText();
		//				scoreImage = scoreBitmap2.ToBitmapImage();
		//				scoreImage.Freeze();
		//				ScoreImage = scoreImage;
		//			}
		//		}
		//		));

		//		tasks.Add(Task.Run(() =>
		//		{
		//			using (var linesPage = lineOCR.Process(linesBitmap, PageSegMode.SingleWord))
		//			{
		//				Lines = linesPage.GetText();
		//				linesImage = linesBitmap.ToBitmapImage();
		//				linesImage.Freeze();
		//				LinesImage = linesImage;
		//			}
		//		}
		//		));

		//		tasks.Add(Task.Run(() =>
		//		{
		//			using (var levelPage = levelOCR.Process(levelBitmap, PageSegMode.SingleWord))
		//			{
		//				Level = levelPage.GetText();
		//				levelImage = levelBitmap.ToBitmapImage();
		//				levelImage.Freeze();
		//				LevelImage = levelImage;
		//			}
		//		}
		//		));

		//		await Task.WhenAll(tasks);

		//		//for (int i = 0; i < TetrisWindow.PlayingField.HorizontalSquares; i++)
		//		//{
		//		//	for (int j = 0; j < TetrisWindow.PlayingField.VerticalSquares; j++)
		//		//	{
		//		//		int x = TetrisWindow.PlayingField.GridPoints[i, j, 0];
		//		//		int y = TetrisWindow.PlayingField.GridPoints[i, j, 1];

		//		//		Color color = bitmap.GetPixel(x, y);

		//		//		if (TetrisWindow.HasBlock(color))
		//		//		{
		//		//			g.DrawEllipse(greenPen, x, y, 1, 1);
		//		//		}
		//		//	}
		//		//}
		//	}
		//}


		Pen orangePen = new Pen(Color.Orange, 1);
		Pen bluePen = new Pen(Color.Blue, 1);
		Pen greenPen = new Pen(Color.Green, 2);
		private void DrawFields(Bitmap bitmap)
		{
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				foreach (var xy in TetrisWindow.PlayingField.BlockPixels)
				{
					bitmap.SetPixel(xy[0], xy[1], Color.Red);
				}

				//foreach (var rect in TetrisWindow.PlayingField.GridRectangles)
				//{
				//	g.DrawRectangle(orangePen, rect);
				//}
				//g.DrawRectangle(bluePen, RectX, RectY, RectWidth, RectHeight);
			}


			//g.DrawRectangle(orangePen, TetrisWindow.LineField.LinesRectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.PlayingField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.NextField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.ScoreField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.LineField.Rectangle);
			//g.DrawRectangle(orangePen, TetrisWindow.LevelField.Rectangle);
			//g.DrawRectangle(bluePen, TetrisWindow.ScoreField.ScoreRectangle);

		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
