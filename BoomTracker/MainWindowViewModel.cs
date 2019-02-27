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
using Tesseract;


namespace BoomTracker
{
	public partial class MainWindowViewModel : INotifyPropertyChanged
	{
		private TesseractEngine ocr = new TesseractEngine(FileLocation.TesseractData, "eng");

		private string ones;
		public string Ones
		{
			get => ones;
			set
			{
				if (value != ones)
				{
					ones = value;
					OnPropertyChanged(nameof(Ones));
				}
			}
		}

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

		private BitmapImage sliceImage;

		public BitmapImage SliceImage
		{
			get => mainImage;
			set
			{
				mainImage = value;
				OnPropertyChanged(nameof(SliceImage));
			}
		}

		public int Threshold
		{
			get => TetrisWindow.BlockThreshold;
			set => TetrisWindow.BlockThreshold = value;
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

		public MainWindowViewModel()
		{
			GetVideoDevices();
			ocr.SetVariable("tessedit_char_whitelist", "0123456789");
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

		private void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			try
			{
				BitmapImage bi;
				using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
				{
					Paint(bitmap);
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

		private void Paint(Bitmap bitmap)
		{
			BitmapImage bi;


			using (Graphics g = Graphics.FromImage(bitmap))
			using (Bitmap bm0 = bitmap.Clone(TetrisWindow.ScoreField.ScoreRectangles[0], System.Drawing.Imaging.PixelFormat.DontCare))
			using (var pix = PixConverter.ToPix(bm0))
			using (var page = ocr.Process(bm0, PageSegMode.SingleBlock))
			{

				var o = page.GetText();
				Ones = o;

				bi = bm0.ToBitmapImage();
				bi.Freeze();
				SliceImage = bi;

				g.DrawRectangle(orangePen, TetrisWindow.PlayingField.Rectangle);
				g.DrawRectangle(orangePen, TetrisWindow.NextField.Rectangle);
				g.DrawRectangle(orangePen, TetrisWindow.ScoreField.Rectangle);
				g.DrawRectangle(orangePen, TetrisWindow.LineField.Rectangle);
				g.DrawRectangle(orangePen, TetrisWindow.LevelField.Rectangle);
				g.DrawRectangle(bluePen, TetrisWindow.ScoreField.ScoreRectangles[0]);

				g.DrawRectangle(bluePen, RectX, RectY, RectWidth, RectHeight);




				for (int i = 0; i < TetrisWindow.PlayingField.HorizontalSquares; i++)
				{
					for (int j = 0; j < TetrisWindow.PlayingField.VerticalSquares; j++)
					{
						int x = TetrisWindow.PlayingField.GridPoints[i, j, 0];
						int y = TetrisWindow.PlayingField.GridPoints[i, j, 1];

						Color color = bitmap.GetPixel(x, y);

						if (TetrisWindow.HasBlock(color))
						{
							g.DrawEllipse(greenPen, x, y, 1, 1);
						}

						//Debug.WriteLine($"({x}, {y})");
					}
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
