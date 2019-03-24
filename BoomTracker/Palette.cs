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
			public static int[,] B { get; } = new int[,]
			{
				{  35, 165, 255 }, // Level  0
				{  98, 208,   0 }, // Level  1
				{ 246,  63, 255 }, // Level  2
				{  30, 239,   0 }, // Level  3
				{   1, 231,  81 }, // Level  4
				{ 113, 117, 255 }, // Level  5
				{  79,  81,  78 }, // Level  6
				{  94,   0,   1 }, // Level  7
				{ 183,  15,   0 }, // Level  8
				{ 236, 131,   0 }, // Level  9
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },

			};

			public static int[,] C { get; } = new int[,]
			{
				{  46,  10, 255 }, // Level  0
				{   0, 146,   0 }, // Level  1
				{ 162,   0, 219 }, // Level  2
				{  41,  10, 238 }, // Level  3
				{ 189,   0,  69 }, // Level  4
				{   2, 230,  84 }, // Level  5
				{ 179,  11,   0 }, // Level  6
				{ 100,   0, 237 }, // Level  7
				{  47,  12, 241 }, // Level  8
				{ 163,  13,   0 }, // Level  9
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
				{ 200, 200, 200 },
			};
		}

		public static class Average
		{
			public static int[,] B { get; } = new int[,]
			{
				{  34, 167, 255 }, // Level  0
				{  95, 213,   0 }, // Level  1
				{ 246,  63, 255 }, // Level  2
				{  29, 239,   0 }, // Level  3
				{   0, 231,  80 }, // Level  4
				{ 113, 117, 255 }, // Level  5
				{  78,  81,  78 }, // Level  6
				{ 102,   0,   0 }, // Level  7
				{ 179,  12,   0 }, // Level  8
				{ 244, 130,   0 }, // Level  9
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },

			};

			public static int[,] C { get; } = new int[,]
			{
				{  46,  10, 255 }, // Level  0
				{   0, 145,   0 }, // Level  1
				{ 173,   0, 235 }, // Level  2
				{ 45,   10, 255 }, // Level  3
				{ 190,   0,  68 }, // Level  4
				{   0, 230,  81 }, // Level  5
				{ 179,  11,   0 }, // Level  6
				{ 110,   0, 255 }, // Level  7
				{  45, 110, 255 }, // Level  8
				{ 179,  11,   0 }, // Level  9
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
			};
		}

		public static class Mode
		{
			public static int[,] B { get; } = new int[,]
			{
				{  34, 167, 255 }, // Level  0
				{  96, 213,   0 }, // Level  1
				{ 247,  63, 255 }, // Level  2
				{  29, 239,   0 }, // Level  3
				{   0, 231,  80 }, // Level  4
				{ 113, 117, 255 }, // Level  5
				{  79,  81,  78 }, // Level  6
				{ 102,   0,   0 }, // Level  7
				{ 180,  12,   0 }, // Level  8
				{ 245, 130,   0 }, // Level  9
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
			};

			public static int[,] C { get; } = new int[,]
			{
				{  46,  10, 255 }, // Level  0
				{   0, 145,   0 }, // Level  1
				{ 173,   0, 235 }, // Level  2
				{  45,  10, 255 }, // Level  3
				{ 190,   0,  68 }, // Level  4
				{   0, 230,  81 }, // Level  5
				{ 179,  11,   0 }, // Level  6
				{ 111,   0, 255 }, // Level  7
				{  45,  10, 255 }, // Level  8
				{ 179,  12,   0 }, // Level  9
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
				{ 100, 100, 100 },
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
				for (int j = 0; j < 3; j++)
				{
					for (int k = 1; k < 3; k++)
					{
						MidPoint.B[i + k * 10, j] = MidPoint.B[i, j];
						MidPoint.C[i + k * 10, j] = MidPoint.C[i, j];

						Average.B[i + k * 10, j] = Average.B[i, j];
						Average.C[i + k * 10, j] = Average.C[i, j];

						Mode.B[i + k * 10, j] = Mode.B[i, j];
						Mode.C[i + k * 10, j] = Mode.C[i, j];
					}
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
				var bPoints = MidPoint.B;

				for (int r = bPoints[level, 0] - ScanWidth; r < bPoints[level, 0] + ScanWidth; r++)
				{
					// Loop over +/-width for g, stored as 1 place in MidPoint.B
					for (int g = bPoints[level, 1] - ScanWidth; g < bPoints[level, 1] + ScanWidth; g++)
					{
						// Loop over +/-width for b, stored as 2 place in MidPoint.B
						for (int b = bPoints[level, 2] - ScanWidth; b < bPoints[level, 2] + ScanWidth; b++)
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
				var cPoints = MidPoint.C;

				for (int r = cPoints[level, 0] - ScanWidth; r < cPoints[level, 0] + ScanWidth; r++)
				{
					// Loop over +/-width for g, stored as 1 place in MidPoint.C
					for (int g = cPoints[level, 1] - ScanWidth; g < cPoints[level, 1] + ScanWidth; g++)
					{
						// Loop over +/-width for b, stored as 2 place in MidPoint.C
						for (int b = cPoints[level, 2] - ScanWidth; b < cPoints[level, 2] + ScanWidth; b++)
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
