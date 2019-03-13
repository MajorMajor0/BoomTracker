using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace BoomTracker
{
	public static class Tetris
	{

	/// <summary>This entire rectangle should be black during a game and non-black when not during a game</summary>
		public static Rectangle IsGameRectangle { get; } = new Rectangle(545, 36, 5, 3);

		public static PixelFormat PixelFormat => PixelFormat.Format24bppRgb;

		private static byte BytesPerPixel;

		// Bitmap size produced by USB converter
		public static double ImageWidth = 720.0;
		public static double ImageHeight = 480.0;

		// Output NES resolution (generated resolution = 256 x 240)
		public static double NESHeight = 224.0;
		public static double NESWidth = 256.0;

		public static class PlayingField
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				// Playing field rectangle measured directly in bitmap
				X = 280,
				Y = 80,
				Width = 215,
				Height = 340
			};

			public static int Nrows => 20;
			public static int NColumns => 10;

			/// <summary>Displayed height of one Tetris block</summary>
			public static double BlockHeight { get; set; }
			/// <summary>Displayed width of one Tetris block</summary>
			public static double BlockWidth { get; set; }
			/// <summary>Displayed width of margin around Tetris block</summary>
			public static double BlockMargin => 2;

			/// <summary>Height of one Tetris block in NES resolution</summary>
			private static readonly double BlockHeightNes = 7.0;
			/// <summary>Width of one Tetris block in NES resolution</summary>
			private static readonly double BlockWidthNes = 7.0;

			/// <summary>X Y Point at center of block[i,j]</summary>
			public static int[,,] BlockCenters { get; set; }

			/// <summary>Rectangle field at center of block to read average color</summary>
			public static Rectangle[,] BlockFields { get; set; }
			public static int FieldHeight => 5;
			public static int FieldWidth => 6;

			/// <summary>X Y position of pixels in block[i,j]</summary>
			public static int[,,][] BlockPixels { get; set; }

			/// <summary>Array at ij contains the addresses of pixels in block[i,j] int[3] = [blue][green][red]</summary>
			public static int[,][] BlockAddresses { get; set; }

			static PlayingField()
			{
				BlockHeight = BlockHeightNes * ImageHeight / NESHeight;
				//BlockWidth = BlockWidthNes * ImageWidth / NESWidth;

				// Measured manually from bitmap
				BlockWidth = 19.4;

				BlockCenters = new int[NColumns, Nrows, 2];
				BlockFields = new Rectangle[NColumns, Nrows];

				// Empirically measured from bitmap as the best spot to put the measureing field
				int yRectOffset = 7;
				int xRectOffset = 8;

				BlockPixels = new int[NColumns, Nrows, FieldHeight * FieldWidth][];
				BlockAddresses = new int[NColumns, Nrows][];
				for (int i = 0; i < NColumns; i++)
				{
					for (int j = 0; j < Nrows; j++)
					{
						// Top left corner of current block
						double x0 = (double)Rectangle.Left + BlockMargin + (double)i * (BlockWidth + BlockMargin);
						double y0 = (double)Rectangle.Top + BlockMargin + (double)j * (BlockHeight + BlockMargin);

						BlockCenters[i, j, 0] = (int)(x0 + BlockWidth / 2);
						BlockCenters[i, j, 1] = (int)(y0 + BlockHeight / 2);

						BlockFields[i, j] = new Rectangle((int)x0 + xRectOffset, (int)y0 + yRectOffset, FieldWidth, FieldHeight);

						// Numbers to be added to rectangle top left corner to get x/y position of pixel k
						int xsub = 0;
						int ysub;

						// Set pixels and addresses
						BlockAddresses[i, j] = new int[FieldHeight * FieldWidth];
						for (int k = 0; k < FieldWidth * FieldHeight; k++)
						{
							BlockPixels[i, j, k] = new int[2];

							// Set the xy pixel[k] of rectangle[i,j]
							// Y offset is the row that the pixel is in
							ysub = k / (FieldWidth);

							int x = BlockFields[i, j].X + xsub;
							int y = BlockFields[i, j].Y + ysub;

							BlockPixels[i, j, k][0] = x;
							BlockPixels[i, j, k][1] = y;

							// Set pixel address
							int pixel = BytesPerPixel * (x + (int)ImageWidth * y);

							BlockAddresses[i, j][k] = pixel;

							if (xsub++ == FieldWidth - 1)
							{
								xsub = 0;
							}
						}
					}
				}
			}
		}

		public static class NextField
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 535,
				Y = 200,
				Width = 87,
				Height = 88
			};
		}

		public static class ScoreField
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 529,
				Y = 25,
				Width = 144,
				Height = 131
			};

			public static Tesseract.Rect ScoreRect { get; }

			public static Rectangle ScoreRectangle { get; } = new Rectangle
			{
				X = 535,
				Y = 115,
				Height = 18,
				Width = 664 - 535
			};

			static ScoreField()
			{
				ScoreRect = new Tesseract.Rect(ScoreRectangle.X, ScoreRectangle.Y, ScoreRectangle.Width, ScoreRectangle.Height);


				//ScoreRectangles = new Rectangle[]
				//{
				//	new Rectangle { X = 642,  Y = y, Height = h, Width = w },
				//	new Rectangle { X = 621,  Y = y, Height = h, Width = w },
				//	new Rectangle { X = 599,  Y = y, Height = h, Width = w },
				//	new Rectangle { X = 578,  Y = y, Height = h, Width = w },
				//	new Rectangle { X = 557,  Y = y, Height = h, Width = w },
				//	new Rectangle { X = 535,  Y = y, Height = h, Width = 664 - 535 }
				//};
			}
		}

		public static class LineField
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 274,
				Y = 25,
				Width = 229,
				Height = 29
			};

			public static Rectangle LinesRectangle { get; } = new Rectangle
			{
				X = 428,
				Y = 28,
				Height = 21,
				Width = 68
			};

			public static Tesseract.Rect LinesRect { get; }

			static LineField()
			{
				LinesRect = new Tesseract.Rect(LinesRectangle.X, LinesRectangle.Y, LinesRectangle.Width, LinesRectangle.Height);
			}
		}

		public static class LevelField
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 529,
				Y = 314,
				Width = 122,
				Height = 46
			};

			public static Rectangle LevelRectangle { get; } = new Rectangle
			{
				X = 575,
				Y = 335,
				Width = 48,
				Height = 20
			};

			public static Tesseract.Rect LevelRect { get; }

			static LevelField()
			{
				LevelRect = new Tesseract.Rect(LevelRectangle.X, LevelRectangle.Y, LevelRectangle.Width, LevelRectangle.Height);
			}
		}

		static Tetris()
		{
			BytesPerPixel = BppDictionary[PixelFormat];
		}

		public static Dictionary<PixelFormat, byte> BppDictionary = new Dictionary<PixelFormat, byte>
		{
			{ PixelFormat.Format16bppArgb1555, 2 },
			{ PixelFormat.Format16bppGrayScale, 2 },
			{ PixelFormat.Format16bppRgb555, 2 },
			{ PixelFormat.Format16bppRgb565, 2 },
			{ PixelFormat.Format1bppIndexed, 1 },
			{ PixelFormat.Format24bppRgb, 3 },
			{ PixelFormat.Format32bppArgb, 4 },
			{ PixelFormat.Format32bppRgb, 4 },
			{ PixelFormat.Format48bppRgb, 6 },
			{ PixelFormat.Format64bppArgb, 8 },
			{ PixelFormat.Format64bppPArgb, 8 },
			{ PixelFormat.Format8bppIndexed, 1 }
		};
	}
}
