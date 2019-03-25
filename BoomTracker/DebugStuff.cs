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

#if DEBUG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BoomTracker
{
	internal static class DebugStuff
	{
		public static void MainWindowBonusAsync()
		{
			CalibrateOcr();
		}

		/// <summary>Get the bits that are always lit for a given number in any place in teh score window across known good score snapshots</summary>
		/// <returns></returns>
		public static List<BitmapImage> CalibrateOcr()
		{
			int threshold = 150;

			// Folder containing only known good number snapshots
			string[] scoreFiles = Directory.GetFiles($"{FileLocation.Scores}\\Good");
			string[] lineFiles = Directory.GetFiles(FileLocation.Lines);
			string[] levelFiles = Directory.GetFiles(FileLocation.Levels);

			// Get scores in 6 digit format
			List<string> scoreStrings =
				scoreFiles
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.Select(x => int.Parse(x))
				.Select(x => x.ToString("D6"))
				.Select(x => string.Join("", x.Reverse()))
				.ToList();

			// Get lines in 3 digit format
			List<string> lineStrings =
				lineFiles
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.Select(x => int.Parse(x))
				.Select(x => x.ToString("D3"))
				.Select(x => string.Join("", x.Reverse()))
				.ToList();

			// Get levels in 2 digit format
			List<string> levelStrings =
				levelFiles
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.Select(x => int.Parse(x))
				.Select(x => x.ToString("D2"))
				.Select(x => string.Join("", x.Reverse()))
				.ToList();

			// Create a grid and image for every digit 0 - 9
			List<Bitmap> digitBitmaps = new List<Bitmap>();
			List<bool[,]> digitGrids = new List<bool[,]>();
			for (int i = 0; i < 10; i++)
			{
				digitBitmaps.Add(new Bitmap(Tetris.DigitWidth, Tetris.DigitHeight, Tetris.PixelFormat));
				digitGrids.Add(new bool[Tetris.DigitWidth, Tetris.DigitHeight]);

				for (int x = 0; x < Tetris.DigitWidth; x++)
				{
					for (int y = 0; y < Tetris.DigitHeight; y++)
					{
						digitBitmaps[i].SetPixel(x, y, Color.Black);
						digitGrids[i][x, y] = true;
					}
				}
			}

			for (int fileNumber = 0; fileNumber < scoreFiles.Length; fileNumber++)
			{
				using (Bitmap bitmap = (Bitmap)Image.FromFile(scoreFiles[fileNumber]))
				{
					string scoreString = scoreStrings[fileNumber];

					// i = digit in score, e.g., ones, tens, etc...10^j
					for (int i = 0; i < Tetris.Score.DigitOffsets.Length; i++)
					{
						int number = (int)char.GetNumericValue(scoreString[i]);
						int x0 = Tetris.Score.DigitOffsets[i];

						for (int x = 0; x < Tetris.DigitWidth; x++)
						{
							for (int y = 0; y < Tetris.DigitHeight; y++)
							{
								Color color = bitmap.GetPixel(x + x0, y);
								bool isLit = color.B > threshold;// || color.R > threshold || color.G > threshold;

								if (isLit && digitGrids[number][x, y])
								{
									digitGrids[number][x, y] = true;
									digitBitmaps[number].SetPixel(x, y, Color.White);
								}

								else
								{
									digitGrids[number][x, y] = false;
									digitBitmaps[number].SetPixel(x, y, Color.Black);
								}
							}
						}
					}
				}
			}

			for (int fileNumber = 0; fileNumber < lineFiles.Length; fileNumber++)
			{
				using (Bitmap bitmap = (Bitmap)Image.FromFile(lineFiles[fileNumber]))
				{
					string lineString = lineStrings[fileNumber];

					// i = digit in score, e.g., ones, tens, etc...10^j
					for (int i = 0; i < Tetris.Lines.DigitOffsets.Length; i++)
					{
						int number = (int)char.GetNumericValue(lineString[i]);
						int x0 = Tetris.Lines.DigitOffsets[i];

						for (int x = 0; x < Tetris.DigitWidth; x++)
						{
							for (int y = 0; y < Tetris.DigitHeight; y++)
							{
								Color color = bitmap.GetPixel(x + x0, y);
								bool isLit = color.B > threshold;// || color.R > threshold || color.G > threshold;

								if (isLit && digitGrids[number][x, y])
								{
									digitGrids[number][x, y] = true;
									digitBitmaps[number].SetPixel(x, y, Color.White);
								}

								else
								{
									digitGrids[number][x, y] = false;
									digitBitmaps[number].SetPixel(x, y, Color.Black);
								}
							}
						}
					}
				}
			}

			for (int fileNumber = 0; fileNumber < levelFiles.Length; fileNumber++)
			{
				using (Bitmap bitmap = (Bitmap)Image.FromFile(levelFiles[fileNumber]))
				{
					string levelString = levelStrings[fileNumber];

					// i = digit in score, e.g., ones, tens, etc...10^j
					for (int i = 0; i < Tetris.Level.DigitOffsets.Length; i++)
					{
						int number = (int)char.GetNumericValue(levelString[i]);
						int x0 = Tetris.Level.DigitOffsets[i];

						for (int x = 0; x < Tetris.DigitWidth; x++)
						{
							for (int y = 0; y < Tetris.DigitHeight; y++)
							{
								Color color = bitmap.GetPixel(x + x0, y);
								bool isLit = color.B > threshold;// || color.R > threshold || color.G > threshold;

								if (isLit && digitGrids[number][x, y])
								{
									digitGrids[number][x, y] = true;
									digitBitmaps[number].SetPixel(x, y, Color.White);
								}

								else
								{
									digitGrids[number][x, y] = false;
									digitBitmaps[number].SetPixel(x, y, Color.Black);
								}
							}
						}
					}
				}
			}

			List<BitmapImage> digitImages = digitBitmaps.Select(x => x.ToBitmapImage()).ToList();

			StringBuilder sbx = new StringBuilder();
			StringBuilder sby = new StringBuilder();

			for (int digit = 0; digit < 10; digit++)
			{
				sbx.Append("new int[] {");
				sby.Append("new int[] {");
				for (int x = 0; x < Tetris.DigitWidth; x++)
				{
					for (int y = 0; y < Tetris.DigitHeight; y++)
					{
						if (digitGrids[digit][x, y])
						{
							sbx.Append($"{x},");
							sby.Append($"{y},");
						}
					}
				}

				// Strip end comma
				sbx.Remove(sbx.Length - 1, 1);
				sby.Remove(sby.Length - 1, 1);

				sbx.Append("},\n");
				sby.Append("},\n");
			}

			// Strip end comma
			sbx.Remove(sbx.Length - 2, 1);
			sby.Remove(sby.Length - 2, 1);

			Debug.WriteLine(sbx.ToString());
			Debug.WriteLine("\n");
			Debug.WriteLine(sby.ToString());

			return digitImages;
		}

		public static void StashScoreBox(Bitmap bitmap, int score)
		{
			Bitmap box = bitmap.Clone(Tetris.Score.Rectangle, Tetris.PixelFormat);
			box.Save($"{FileLocation.Scores}\\{score}.bmp");
		}

		public static void StashLinesBox(Bitmap bitmap, int lines)
		{
			Bitmap box = bitmap.Clone(Tetris.Lines.Rectangle, Tetris.PixelFormat);
			box.Save($"{FileLocation.Lines}\\{lines}.bmp");
		}

		public static void StashLevelBox(Bitmap bitmap, int level)
		{
			Bitmap box = bitmap.Clone(Tetris.Level.Rectangle, Tetris.PixelFormat);
			box.Save($"{FileLocation.Levels}\\{level}.bmp");
		}
	}
}
#endif