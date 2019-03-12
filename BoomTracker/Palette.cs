using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace BoomTracker
{
	public static class Palette
	{
		/// <summary>How far to look above and below teh color value and still get a hit</summary>
		public static int ScanWidth = 20;

		public static int BlackThreshold = 3;

		public static class ScanTarget
		{
			// Based on midpoints, red + green
			public static int[] B { get; } = new int[]
			{
				200,
				305,
				309,
				269,
				232
			};

			// Based on midpoints, red + green
			public static int[] C { get; } = new int[]
			{
				 56,
				146,
				162,
				 51,
				189
			};
		}

		public static class MidPoint
		{
			public static byte[,] B { get; } = new byte[,]
			{
				{  35, 165, 255 }, // Level  0
				{  98, 208,   0 }, // Level  1
				{ 246,  63, 255 }, // Level  2
				{  30, 239,   0 }, // Level  3
				{ 1,   231,  81 }, // Level  4
				{ 100, 100, 100 }, // Level  5
				{ 100, 100, 100 }, // Level  6
				{ 100, 100, 100 }, // Level  7
				{ 100, 100, 100 }, // Level  8
				{ 100, 100, 100 }, // Level  9
				{  35, 165, 255 }, // Level 10
				{   0, 146,   0 }, // Level 11
			};

			public static byte[,] C { get; } = new byte[,]
			{
				{  46,  10, 255 }, // Level  0
				{   0, 146,   0 }, // Level  1
				{ 162,   0, 219 }, // Level  2
				{  41,  10, 238 }, // Level  3
				{ 189,   0,  69 }, // Level  4
				{ 100, 100, 100 }, // Level  5
				{ 100, 100, 100 }, // Level  6
				{ 100, 100, 100 }, // Level  7
				{ 100, 100, 100 }, // Level  8
				{ 100, 100, 100 }, // Level  9
				{  46,  10, 255 }, // Level 10
				{  98, 208,   0 }, // Level 11
			};
		}

		public static class Average
		{
			public static byte[,] B { get; } = new byte[,]
			{
				{  34, 167, 255 }, // Level  0
				{  95, 213,   0 }, // Level  1
				{ 246,  63, 255 }, // Level  2
				{ 29,  239,   0 }, // Level  3
				{ 0,   231,  80 }, // Level  4
				{ 100, 100, 100 }, // Level  5
				{ 100, 100, 100 }, // Level  6
				{ 100, 100, 100 }, // Level  7
				{ 100, 100, 100 }, // Level  8
				{ 100, 100, 100 }, // Level  9
				{  34, 167, 255 }, // Level 10
				{   0, 145,   0 }, // Level 11
			};

			public static byte[,] C { get; } = new byte[,]
			{
				{  46,  10, 255 }, // Level  0
				{   0, 145,   0 }, // Level  1
				{ 173,   0, 235 }, // Level  2
				{ 45,   10, 255 }, // Level  3
				{ 190,   0,  68 }, // Level  4
				{ 100, 100, 100 }, // Level  5
				{ 100, 100, 100 }, // Level  6
				{ 100, 100, 100 }, // Level  7
				{ 100, 100, 100 }, // Level  8
				{ 100, 100, 100 }, // Level  9
				{  46,  10, 255 }, // Level 10
				{  95, 213,   0 }, // Level 11
			};
		}

		public static class Mode
		{
			public static byte[,] B { get; } = new byte[,]
			{
				{  34, 167, 255 }, // Level  0
				{  96, 213,   0 }, // Level  1
				{ 247,  63, 255 }, // Level  2
				{  29, 239,   0 }, // Level  3
				{   0, 231,  80 }, // Level  4
				{ 100, 100, 100 }, // Level  5
				{ 100, 100, 100 }, // Level  6
				{ 100, 100, 100 }, // Level  7
				{ 100, 100, 100 }, // Level  8
				{ 100, 100, 100 }, // Level  9
				{  34, 167, 255 }, // Level 10
				{   0, 145,   0 }, // Level 11
			};

			public static byte[,] C { get; } = new byte[,]
			{
				{  46,  10, 255 }, // Level  0
				{   0, 145,   0 }, // Level  1
				{ 173,   0, 235 }, // Level  2
				{  45,  10, 255 }, // Level  3
				{ 190,   0,  68 }, // Level  4
				{ 100, 100, 100 }, // Level  5
				{ 100, 100, 100 }, // Level  6
				{ 100, 100, 100 }, // Level  7
				{ 100, 100, 100 }, // Level  8
				{ 100, 100, 100 }, // Level  9
				{  46,  10, 255 }, // Level 10
				{  96, 213,   0 }, // Level 11
			};
		}

		/// <summary>
		/// Dictionary to scan through to match a bit value to a char for display in the tetris playing field
		/// </summary>
		public static List<Dictionary<int, char?>> ScanDictionary { get; private set; }

		/// <summary>
		/// Dictionary to match a char value to a brush for display in the playing field
		/// </summary>
		public static List<Dictionary<char, SolidColorBrush>> BrushDictionary { get; private set; }

		static Palette()
		{
			CreateScanDictionaries();
			CreateBrushDictionaries();
		}

		private static void CreateScanDictionaries()
		{
			ScanDictionary = new List<Dictionary<int, char?>>();
			// Create a dictionary for each level 0 - 9
			for (int level = 0; level < ScanTarget.B.Length; level++)
			{
				var dict = new Dictionary<int, char?>();

		
				// Add dictionary entries corresponding to 'B'
				//for (int i = bColors[level, colorChoice] - colorScanWidth; i < bColors[level, colorChoice] + colorScanWidth; i++)
				for (int i = ScanTarget.B[level] - ScanWidth; i < ScanTarget.B[level] + ScanWidth; i++)
				{
					// Cut off to allow for black background
					if (i >= BlackThreshold)
					{
						// If i is already in the , it needs to have the value B. Else the width is too wide
						if (dict.TryGetValue(i, out char? checkChar))
						{
							if (checkChar != 'B')
							{
								throw new Exception("Collision adding values to dictionary. The colorScanWidth is too wide or teh color choice is bad");
							}
						}

						else
						{
							dict.Add(i, 'B');
						}
					}
				}

				// Add dictionary entries corresponding to 'C'
				//for (int i = cColors[level, colorChoice] - colorScanWidth; i < cColors[level, colorChoice] + colorScanWidth; i++)
				for (int i = ScanTarget.C[level] - ScanWidth; i < ScanTarget.C[level] + ScanWidth; i++)
				{
					if (level == 4)
					{

					}
					// Toss overflow and underflow. Cut off at 2 to allow for black background.
					if (i >= BlackThreshold)
					{
						if (dict.TryGetValue(i, out char? checkChar))
						{
							if (checkChar != 'C')
							{
								throw new Exception("Collision adding values to dictionary. The colorScanWidth is too wide or teh color choice is bad");
							}
						}

						else
						{
							dict.Add(i, 'C');
						}
					}
				}

				for (int i = 255 * 2 - ScanWidth; i <= 255 * 2; i++)
				{
					dict.Add(i, 'A');
				}

				ScanDictionary.Add(dict);
			}
		}

		private static void CreateBrushDictionaries()
		{
			BrushDictionary = new List<Dictionary<char, SolidColorBrush>>();

			// Create a dictionary for each level 0 - 9
			for (int level = 0; level < ScanTarget.B.Length; level++)
			{
				var dict = new Dictionary<char, SolidColorBrush>
				{
					{ 'A', new SolidColorBrush(Color.FromArgb(255, Average.C[level, 0], Average.C[level, 1], Average.C[level, 2])) },
					{ 'B', new SolidColorBrush(Color.FromArgb(255, Average.B[level, 0], Average.B[level, 1], Average.B[level, 2])) },
					{ 'C', new SolidColorBrush(Color.FromArgb(255, Average.C[level, 0], Average.C[level, 1], Average.C[level, 2])) }
				};
				BrushDictionary.Add(dict);
			}
		}
	}
}
