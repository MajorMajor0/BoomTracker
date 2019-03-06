using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace BoomTracker
{
	public class Game
	{
		private static byte[] greenValuesB = new byte[]
		{
			167,
			214,
			107,
			231,
			222,
			148,
			80,
			0,
			49,
			156,
		};

		private static byte[] greenValuesC = new byte[]
		{
			10 ,
			148,
			24,
			66,
			33,
			22,
			35,
			41,
			66,
			49,
		};

		private OCR ocr = new OCR();

		public byte StartLevel { get; set; }

		public byte EndLevel { get; set; }

		public int Score { get; set; }

		public List<State> States { get; set; } = new List<State>();

		public class State
		{
			//public Color[][] Grid { get; set; } = new Color[10][];

			public char?[][] Grid { get; set; } = new char?[10][];

			public int Level { get; set; }

			public int Lines { get; set; }

			public int Score { get; set; }

			private static List<Dictionary<byte, char?>> dictionaries = new List<Dictionary<byte, char?>>();

			public State()
			{
				for (int i = 0; i < 10; i++)
				{
					Grid[i] = new char?[20];
				}
			}

			static State()
			{
				int width = 8;

				for (int j = 0; j < 9; j++)
				{
					var dict = new Dictionary<byte, char?>();

					for (int i = greenValuesB[j] - width; i < greenValuesB[j] + width; i++)
					{
						if (i >= 0 && i <= 255)
						{
							dict.Add((byte)i, 'B');
						}
					}

					for (int i = greenValuesC[j] - width; i < greenValuesC[j] + width; i++)
					{
						if (i >= 0 && i <= 255)
						{
							dict.Add((byte)i, 'C');
						}
					}

					for (int i = 255 - width; i <= 255; i++)
					{
						dict.Add((byte)i, 'A');
					}

					dictionaries.Add(dict);
				}
			}

			public void GetGrid(Bitmap bitmap)
			{
				//BitmapData bmpData = bitmap.LockBits(
				//	TetrisWindow.PlayingField.Rectangle,
				//	ImageLockMode.ReadWrite,
				//	bitmap.PixelFormat);

				//// Get the address of the first line.
				//IntPtr pointer = bmpData.Scan0;

				//// Declare an array to hold the bytes of the bitmap.
				//int nBytes = Math.Abs(bmpData.Stride) * bitmap.Height;
				//byte[] rgbValues = new byte[nBytes];

				//// Copy the RGB values into the array.
				//System.Runtime.InteropServices.Marshal.Copy(pointer, rgbValues, 0, nBytes);

				//for (int i = 0; i <= 10; i++)
				//{
				//	for (int j = 0; j < 20; j++)
				//	{

				//	}

				//}


				//for (int i = 0; i < TetrisWindow.PlayingField.HorizontalSquares; i++)
				//{
				//	for (int j = 0; j < TetrisWindow.PlayingField.VerticalSquares; j++)
				//	{
				//		int x = TetrisWindow.PlayingField.GridPoints[i, j, 0];
				//		int y = TetrisWindow.PlayingField.GridPoints[i, j, 1];

				//		//Color color = bitmap.GetPixel(x, y);

				//		Color color = 

				//		if (color.R != 0 || color.G != 0 || color.B != 0)
				//		{
				//			Debug.WriteLine($"{color.A}\t{color.R}\t{color.G}\t{color.B}\t{color.GetBrightness()}\t{color.GetHue()}");
				//		}

				//		if (!dictionaries[Level].TryGetValue(color.G, out Grid[i][j]))
				//		{
				//			Grid[i][j] = null;
				//		}
				//	}
				//}
			}
		}

		public async Task<State> StoreState(Bitmap bitmap)
		{
			State state = new State();

			using (Bitmap scoreBitmap = bitmap.Clone(TetrisWindow.ScoreField.ScoreRectangle, PixelFormat.DontCare))
			using (Bitmap linesBitmap = bitmap.Clone(TetrisWindow.LineField.LinesRectangle, PixelFormat.DontCare))
			using (Bitmap levelBitmap = bitmap.Clone(TetrisWindow.LevelField.LevelRectangle, PixelFormat.DontCare))
			{
				Task<int>[] tasks = new Task<int>[3];

				tasks[0] = Task.Run(() => ocr.ReadNumber(scoreBitmap, 0));
				tasks[1] = Task.Run(() => ocr.ReadNumber(linesBitmap, 1));
				tasks[2] = Task.Run(() => ocr.ReadNumber(levelBitmap, 2));

				Task getGrid = Task.Run(() => { state.GetGrid(bitmap); });

				state.Score = await tasks[0];
				state.Lines = await tasks[1];
				state.Level = await tasks[2];
				await getGrid;
			}

			States.Add(state);
			return state;
		}
	}
}
