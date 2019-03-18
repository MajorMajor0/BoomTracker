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
	public class OCR
	{
		private static TesseractEngine[] engines;

		static OCR()
		{
			engines = new TesseractEngine[3];

			for (int i = 0; i < engines.Length; i++)
			{
				engines[i] = new TesseractEngine(FileLocation.TesseractData, "eng");
				engines[i].SetVariable("tessedit_char_whitelist", "0123456789");
			}

			SetScoreAddresses();
		}

		public int[] ReadNumber(Bitmap bitmap, int engineNumber)
		{
			int[] returner = new int[2];

			using (var page = engines[engineNumber].Process(bitmap, PageSegMode.SingleWord))
			{
				returner[1] = (int)(page.GetMeanConfidence() * 1000);

				if (!int.TryParse(page.GetText(), out returner[0]))
				{
					returner[0] = 0;
				}
			}

			return returner;
		}

		private static List<int[]> scorePixelsX = new List<int[]>
		{
			new int[] {2,2,3,3,3,3,4,4,4,4,4,4,5,5,6,7,8,9,11,12,13,14,15,15,15,15,16,16,16,16,16,16,17,17,17,17,17,17,18,18,18},
			new int[] {8,9,9,9,10,10,10,10,10,10,10,10,10,10,10,10,11,11,11,11,11,11,11,11,11,11,11,11,11,12,12,12,12,12,12,12,12,12,12,12,12,12,13,13,14,14,15,15},
			new int[] {2,3,3,3,4,4,4,4,5,5,5,5,5,5,6,6,6,6,6,6,7,7,7,7,7,7,8,8,8,8,8,9,9,9,9,9,9,10,10,10,10,10,11,11,11,11,11,12,12,12,12,12,13,13,13,13,13,14,14,14,14,14,14,15,15,15,15,15,15,16,16,16,16,17,17,17,18},
			new int[] {4,5,5,6,6,7,8,9,9,9,10,10,10,11,11,11,11,11,11,12,12,12,12,12,12,12,12,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,15,15,15,15,15,15,15,15,15,15,16,16,16,16,16,16,17,17,17,17,17,18,18},
			new int[] {3,3,4,4,4,5,5,5,6,7,9,10,11,11,11,12,12,12,12,12,13,13,13,13,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,14,14,15,15,15,15,15,15,15,15,15,15,16,16,16,16,16},
			new int[] {3,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,6,6,6,7,8,9,9,10,10,11,11,12,12,12,13,13,13,14,14,14,15,15,15,15,16,16,16,16,16,16,17,17,17,17,17,17,18,18,18},
			new int[] {2,3,3,3,3,3,3,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,6,6,6,7,7,8,9,9,10,10,11,11,12,12,13,13,13,14,14,14,15,15,15,15,15,15,16,16,16,16,16,17,17,17,17,18,18},
			new int[] {3,3,4,4,5,5,6,7,8,8,8,8,8,9,9,9,9,9,9,10,10,10,10,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,14,15,15,15,15,16,16,16,16,17,17,18},
			new int[] {2,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,7,9,9,10,10,11,11,11,12,12,12,12,13,13,13,13,13,13,14,14,14,14,14,15,15,15,15,15,15,15,15,15,15,15,16,16,16,16,16,16,16,16,16,16,17,17,17,17,17,17,17,18,18,18,18,18},
			new int[] {2,3,3,4,4,4,5,5,5,5,5,5,6,7,8,8,9,9,10,10,11,11,11,11,12,12,12,13,13,13,13,13,14,14,14,14,15,15,15,15,15,15,15,16,16,16,16,16,16,16,16,16,17,17,17,17,17,17,17,17,18,18,18,18,18,18}

		};

		private static List<int[]> scorePixelsY = new List<int[]>
		{
			new int[] {7,8,7,8,9,10,6,7,8,9,10,11,7,11,12,13,14,14,2,3,3,4,5,9,10,11,6,7,8,9,10,11,6,7,8,9,10,11,8,9,10},
			new int[] {15,4,14,15,3,4,5,6,7,8,9,10,11,12,14,15,3,4,5,6,7,8,9,10,11,12,13,14,15,3,4,5,6,7,8,9,10,11,12,13,14,15,14,15,14,15,14,15},
			new int[] {14,4,13,14,4,12,13,14,3,11,12,13,14,15,10,11,12,13,14,15,10,11,12,13,14,15,9,10,11,14,15,2,9,10,11,14,15,2,8,9,10,15,2,8,9,10,15,2,8,9,10,15,2,7,8,9,15,2,6,7,8,9,15,2,3,6,7,8,15,4,5,6,7,4,5,6,6},
			new int[] {13,13,14,2,14,2,2,2,8,15,2,8,15,2,3,6,7,8,15,2,3,4,6,7,8,9,15,2,3,4,5,6,7,8,9,14,2,3,4,5,8,9,14,2,3,4,8,9,10,11,12,13,14,2,3,10,11,12,13,2,10,11,12,13,11,12},
			new int[] {8,10,8,9,10,8,9,10,10,6,11,11,3,4,11,3,4,5,10,11,2,3,4,5,6,7,8,9,10,11,12,13,14,2,3,4,5,6,7,8,9,10,11,12,13,14,4,6,7,8,9,10,11,12,13,14,8,9,10,11,14},
			new int[] {3,5,6,2,3,4,5,6,13,2,3,4,5,6,13,14,2,6,14,2,2,2,15,2,15,2,15,2,7,15,2,7,14,2,7,14,7,8,13,14,8,9,10,11,12,13,8,9,10,11,12,13,9,10,11},
			new int[] {10,6,8,9,10,11,12,6,7,8,9,10,11,12,13,4,6,7,8,9,10,12,13,14,8,9,14,8,14,8,2,15,2,15,2,15,2,15,2,14,15,2,9,14,9,10,11,12,13,14,9,10,11,12,13,10,11,12,13,11,12},
			new int[] {2,4,2,4,2,3,2,2,2,11,12,13,14,2,10,11,12,13,14,2,9,10,11,12,13,14,2,8,9,2,8,9,2,7,8,2,3,6,7,2,3,4,6,2,3,4,5,3,4,4},
			new int[] {11,5,11,5,6,7,10,11,13,3,5,6,7,8,9,10,11,12,13,14,3,8,9,14,9,2,15,2,15,2,8,15,2,8,9,15,2,3,8,9,14,15,2,3,8,9,14,2,3,6,7,8,9,10,11,12,13,14,3,4,5,6,7,9,10,11,12,13,4,5,6,10,11,12,13,5,6,10,11,12},
			new int[] {5,5,6,5,6,7,3,4,5,6,7,8,8,2,2,15,2,15,2,15,2,9,14,15,2,9,14,2,3,9,13,14,2,3,9,13,2,3,4,8,9,10,12,3,4,5,6,7,8,9,10,11,4,5,6,7,8,9,10,11,5,6,7,8,9,10}
		};

		public static int[,][] ScoreAddresses = new int[10, 6][];

		public unsafe static bool GetScore(BitmapData bmData, out int score)
		{
			Stopwatch watch = Stopwatch.StartNew();

			byte* scan0 = (byte*)bmData.Scan0.ToPointer();

			//int place = 0;
			score = 0;

			for (int place = 0; place < 6; place++)
			{
				for (int numberGuess = 0; numberGuess < 11; numberGuess++)
				{
					if (numberGuess == 10)
					{
						return false;
					}

					bool goodGuess = true;

					foreach (var address in ScoreAddresses[numberGuess, place])
					{
						//scan0[address] = 0;
						//scan0[address + 1] = 255;
						//scan0[address + 2] = 0;

						if (scan0[address] < 80) // Blue
						{
							//colors[1] += scan0[address + 1]; // Green
							//colors[2] += scan0[address + 2]; // Red
							goodGuess = false;
							break;
						}
					}

					if (goodGuess)
					{
						score += numberGuess * (int)Math.Pow(10, place);
						break;
					}
				}
			}

			Debug.WriteLine(watch.ElapsedTicks);

			return true;

		}

		private static void SetScoreAddresses()
		{
			int imageWidth = (int)Tetris.ImageWidth;

			for (int digit = 0; digit < 10; digit++)
			{
				for (int place = 0; place < 6; place++)
				{
					List<int> addresses = new List<int>();

					int x0 = Tetris.ScoreField.DigitRectangles[place].X;

					for (int i = 0; i < scorePixelsX[digit].Length; i++)
					{
						int x = scorePixelsX[digit][i] + x0;
						int y = scorePixelsY[digit][i];

						int address = Tetris.BytesPerPixel * (x + imageWidth * y);

						addresses.Add(address);
					}
					ScoreAddresses[digit, place] = addresses.ToArray();
				}
			}
		}
	}
}
