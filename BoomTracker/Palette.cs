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

using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace BoomTracker
{
	public static class Palette
	{
		public static int NLevels { get; set; }

		/// <summary>How far to look above and below teh color value and still get a hit</summary>
		public static int ScanWidth = 20;

		public static int BlackThreshold = 3;

		//public static class ScanTarget
		//{
		//	// Based on midpoints, red + green
		//	public static int[] B { get; set; }

		//	// Based on midpoints, red + green
		//	public static int[] C { get; set; }
		//}

		public static class MidPoint
		{
			public static int[][] B { get; } = new int[][]
			{
				new []{  35, 165, 255 }, // Level  0
				new []{  98, 208,   0 }, // Level  1
				new []{ 246,  63, 255 }, // Level  2
				new []{  30, 239,   0 }, // Level  3
				new []{   1, 231,  81 }, // Level  4
				new []{ 113, 117, 255 }, // Level  5
				new []{  79,  81,  78 }, // Level  6
				new []{  94,   0,   1 }, // Level  7
				new []{ 183,  15,   0 }, // Level  8
				new []{ 236, 131,   0 }, // Level  9
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },

			};

			public static int[][] C { get; } = new int[][]
			{
				new []{  46,  10, 255 }, // Level  0
				new []{   0, 146,   0 }, // Level  1
				new []{ 162,   0, 219 }, // Level  2
				new []{  41,  10, 238 }, // Level  3
				new []{ 189,   0,  69 }, // Level  4
				new []{   2, 230,  84 }, // Level  5
				new []{ 179,  11,   0 }, // Level  6
				new []{ 100,   0, 237 }, // Level  7
				new []{  47,  12, 241 }, // Level  8
				new []{ 163,  13,   0 }, // Level  9
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
				new []{ 200, 200, 200 },
			};
		}

		public static class Average
		{
			public static int[][] B { get; } = new int[][]
			{
				new []{  34, 167, 255 }, // Level  0
				new []{  95, 213,   0 }, // Level  1
				new []{ 246,  63, 255 }, // Level  2
				new []{  29, 239,   0 }, // Level  3
				new []{   0, 231,  80 }, // Level  4
				new []{ 113, 117, 255 }, // Level  5
				new []{  78,  81,  78 }, // Level  6
				new []{ 102,   0,   0 }, // Level  7
				new []{ 179,  12,   0 }, // Level  8
				new []{ 244, 130,   0 }, // Level  9
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },
				new []{ 100, 100, 100 },

			};

			public static int[][] C { get; } = new int[][]
			{
				new[]{  46,  10, 255 }, // Level  0
				new[]{   0, 145,   0 }, // Level  1
				new[]{ 173,   0, 235 }, // Level  2
				new[]{ 45,   10, 255 }, // Level  3
				new[]{ 190,   0,  68 }, // Level  4
				new[]{   0, 230,  81 }, // Level  5
				new[]{ 179,  11,   0 }, // Level  6
				new[]{ 110,   0, 255 }, // Level  7
				new[]{  45, 110, 255 }, // Level  8
				new[]{ 179,  11,   0 }, // Level  9
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
			};
		}

		public static class Mode
		{
			public static int[][] B { get; } = new int[][]
			{
				new[]{  34, 167, 255 }, // Level  0
				new[]{  96, 213,   0 }, // Level  1
				new[]{ 247,  63, 255 }, // Level  2
				new[]{  29, 239,   0 }, // Level  3
				new[]{   0, 231,  80 }, // Level  4
				new[]{ 113, 117, 255 }, // Level  5
				new[]{  79,  81,  78 }, // Level  6
				new[]{ 102,   0,   0 }, // Level  7
				new[]{ 180,  12,   0 }, // Level  8
				new[]{ 245, 130,   0 }, // Level  9
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
			};

			public static int[][] C { get; } = new int[][]
			{
				new[]{  46,  10, 255 }, // Level  0
				new[]{   0, 145,   0 }, // Level  1
				new[]{ 173,   0, 235 }, // Level  2
				new[]{  45,  10, 255 }, // Level  3
				new[]{ 190,   0,  68 }, // Level  4
				new[]{   0, 230,  81 }, // Level  5
				new[]{ 179,  11,   0 }, // Level  6
				new[]{ 111,   0, 255 }, // Level  7
				new[]{  45,  10, 255 }, // Level  8
				new[]{ 179,  12,   0 }, // Level  9
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
				new[]{ 100, 100, 100 },
			};
		}

		/// <summary>
		/// Dictionary to scan through to match a bit value to a char for display in the tetris playing field
		/// </summary>
		public static Dictionary<ValueTuple<int, int, int>, char?>[] ScanDictionary { get; set; }

		/// <summary>
		/// Dictionary to match a char value to a brush for display in the playing field
		/// </summary>
		public static List<Dictionary<char, SolidColorBrush>> BrushDictionary { get; private set; }

		static Palette()
		{
			NLevels = MidPoint.B.GetLength(0);

			for (int i = 0; i < 10; i++)
			{
				for (int k = 1; k < 3; k++)
				{
					MidPoint.B[i + k * 10] = MidPoint.B[i];
					MidPoint.C[i + k * 10] = MidPoint.C[i];

					Average.B[i + k * 10] = Average.B[i];
					Average.C[i + k * 10] = Average.C[i];

					Mode.B[i + k * 10] = Mode.B[i];
					Mode.C[i + k * 10] = Mode.C[i];
				}
			}
			CreateScanDictionaries();
			//CreateBrushDictionaries();
		}

		private static void CreateScanDictionaries()
		{
			ScanDictionary = new Dictionary<ValueTuple<int, int, int>, char?>[NLevels];

			// Create a dictionary for each level
			for (int level = 0; level < NLevels; level++)
			{
				ScanDictionary[level] = new Dictionary<ValueTuple<int, int, int>, char?>();

				// Add dictionary entries corresponding to 'B'
				// Loop over +/-width for r, stored as 0 place in MidPoint.C
				var bPoints = MidPoint.B[level];

				for (int r = bPoints[0] - ScanWidth; r < bPoints[0] + ScanWidth; r++)
				{
					// Loop over +/-width for g, stored as 1 place in MidPoint.B
					for (int g = bPoints[1] - ScanWidth; g < bPoints[1] + ScanWidth; g++)
					{
						// Loop over +/-width for b, stored as 2 place in MidPoint.B
						for (int b = bPoints[2] - ScanWidth; b < bPoints[2] + ScanWidth; b++)
						{
							//Toss negative values
							int R = Math.Max(r, 0);
							int G = Math.Max(g, 0);
							int B = Math.Max(b, 0);

							// If (R, G, B) is already in the dictionary, it needs to have the value B. Else the width is too wide
							if (ScanDictionary[level].TryGetValue((R, G, B), out char? checkChar))
							{
								if (checkChar != 'B')
								{
									throw new Exception("Collision adding values to dictionary. The colorScanWidth is too wide.");
								}
							}

							else
							{
								ScanDictionary[level].Add((R, G, B), 'B');
							}
						}
					}
				}

				// Add dictionary entries corresponding to 'C'
				// Loop over +/-width for r, stored as 0 place in MidPoint.C
				var cPoints = MidPoint.C[level];

				for (int r = cPoints[0] - ScanWidth; r < cPoints[0] + ScanWidth; r++)
				{
					// Loop over +/-width for g, stored as 1 place in MidPoint.C
					for (int g = cPoints[1] - ScanWidth; g < cPoints[1] + ScanWidth; g++)
					{
						// Loop over +/-width for b, stored as 2 place in MidPoint.C
						for (int b = cPoints[2] - ScanWidth; b < cPoints[2] + ScanWidth; b++)
						{
							//Toss negative values
							int R = Math.Max(r, 0);
							int G = Math.Max(g, 0);
							int B = Math.Max(b, 0);

							// If i is already in the , it needs to have the value C. Else the width is too wide
							if (ScanDictionary[level].TryGetValue((R, G, B), out char? checkChar))
							{
								if (checkChar != 'C')
								{
									throw new Exception("Collision adding values to dictionary. The colorScanWidth is too wide.");
								}
							}

							else
							{
								ScanDictionary[level].Add((R, G, B), 'C');
							}
						}
					}
				}

				for (int r = 255 - ScanWidth; r <= 255; r++)
				{
					for (int g = 255 - ScanWidth; g <= 255; g++)
					{
						for (int b = 255 - ScanWidth; b <= 255; b++)
						{
							ScanDictionary[level].Add((r, g, b), 'A');
						}
					}
				}
			}
		}

		//private static void CreateBrushDictionaries()
		//{
		//	BrushDictionary = new List<Dictionary<char, SolidColorBrush>>();



		//	// Create a dictionary for each level 0 - 9
		//	for (int level = 0; level < ScanTarget.B.Length; level++)
		//	{
		//		var dict = new Dictionary<char, SolidColorBrush>
		//		{
		//			{ 'A', new SolidColorBrush(Color.FromArgb(255, Average.C[level, 0], Average.C[level, 1], Average.C[level, 2])) },
		//			{ 'B', new SolidColorBrush(Color.FromArgb(255, Average.B[level, 0], Average.B[level, 1], Average.B[level, 2])) },
		//			{ 'C', new SolidColorBrush(Color.FromArgb(255, Average.C[level, 0], Average.C[level, 1], Average.C[level, 2])) }
		//		};
		//		BrushDictionary.Add(dict);
		//	}
		//}
	}
}
