/*This file is part of BoomTracker.
 * 
 * Robin is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General internal License as published 
 * version 3 of the License, or (at your option) any later version.
 * 
 * Robin is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU 
 * General internal License for more details. 
 * 
 * You should have received a copy of the GNU General internal License
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
	internal class DebugStuff
	{
		public static async Task MainWindowBonusAsync()
		{
		}

		/// <summary>Get the bits that are always lit for a given number in any place in teh score window across known good score snapshots</summary>
		/// <returns></returns>
		public static List<BitmapImage> CalibrateOcr()
		{
			// Folder containing only known good score snapshots
			string folder = @"C:\Source\BoomTracker\BoomTracker\Resources\Scores\Good";
			string[] files = Directory.GetFiles(folder);

			// Get scores in 6 digit format
			List<string> scoreStrings =
				files
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.Select(x => int.Parse(x))
				.Select(x => x.ToString("D6"))
				.Select(x => string.Join("", x.Reverse()))
				.ToList();

			Rectangle[] placeRectangles = Tetris.ScoreField.DigitRectangles;
			int rectWidth = placeRectangles[0].Width;
			int rectHeight = placeRectangles[0].Height;

			// Create a grid and image for every digit 0 - 9
			List<Bitmap> digitBitmaps = new List<Bitmap>();
			List<bool[,]> digitGrids = new List<bool[,]>();
			for (int i = 0; i < 10; i++)
			{
				digitBitmaps.Add(new Bitmap(rectWidth, rectHeight, Tetris.PixelFormat));
				digitGrids.Add(new bool[rectWidth, rectHeight]);

				for (int x = 0; x < rectWidth; x++)
				{
					for (int y = 0; y < rectHeight; y++)
					{
						digitBitmaps[i].SetPixel(x, y, Color.Black);
						digitGrids[i][x, y] = true;
					}
				}
			}

			for (int fileNumber = 0; fileNumber < files.Length; fileNumber++)
			{
				using (Bitmap bitmap = (Bitmap)Image.FromFile(files[fileNumber]))
				{
					string scoreString = scoreStrings[fileNumber];

					// i = digit in score, e.g., ones, tens, etc...10^j
					for (int i = 0; i < placeRectangles.Length; i++)
					{
						int number = (int)char.GetNumericValue(scoreString[i]);
						Rectangle rect = placeRectangles[i];

						for (int x = 0; x < rectWidth; x++)
						{
							for (int y = 0; y < rectHeight; y++)
							{
								Color color = bitmap.GetPixel(x + rect.X, y);
								bool isLit = color.B > 128;

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
				for (int x = 0; x < rectWidth; x++)
				{
					for (int y = 0; y < rectHeight; y++)
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
	}
}
#endif