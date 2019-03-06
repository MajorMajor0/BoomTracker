using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomTracker
{
	public static class TetrisWindow
	{
		public static int BlockThreshold { get; set; } = 10;

		public static class PlayingField
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 280,
				Y = 82,
				Width = 215,
				Height = 340
			};

			public static int VerticalSquares => 20;
			public static int HorizontalSquares => 10;

			public static int SquareHeight => 17;
			public static int SquareWidth => 22;

			public static int[,,] GridPoints { get; set; }
			public static Rectangle[,] GridRectangles { get; set; }

			public static int[,,][] GridPixels { get; set; }

			static PlayingField()
			{
				GridPoints = new int[HorizontalSquares, VerticalSquares, 2];
				GridRectangles = new Rectangle[HorizontalSquares, VerticalSquares];
				GridPixels = new int[HorizontalSquares, VerticalSquares, 35][];

				int yOffset = 7;
				int xOffset = 11;

				int yRectOffset = 6;
				int xRectOffset = 7;

				int RectHeight = 5;
				int RectWidth = 7;

				for (int i = 0; i < HorizontalSquares; i++)
				{
					for (int j = 0; j < VerticalSquares; j++)
					{
						int x0 = Rectangle.Left + i * SquareWidth - i * 3 / 4;
						int y0 = Rectangle.Top + j * SquareHeight;

						int x = x0 + xOffset;
						int y = y0 + yOffset;

						GridPoints[i, j, 0] = x;
						GridPoints[i, j, 1] = y;

						GridRectangles[i, j] = new Rectangle(x0 + xRectOffset, y0 + yRectOffset, xRectOffset, yRectOffset);
						Debug.WriteLine($"{GridRectangles[i, j].X}, {GridRectangles[i, j].Y}");

						// Numbers to be added to rectangle top left corner to get x/y position of pixel k
						int xsub = 0;
						int ysub;

						for (int k = 0; k < 35; k++)
						{

							ysub = k / (RectWidth);
							GridPixels[i, j, k] = new int[2];
							GridPixels[i, j, k][0] = GridRectangles[i, j].X + xsub;
							GridPixels[i, j, k][1] = GridRectangles[i, j].Y + ysub;

							if (xsub++ == RectWidth - 1)
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

			//public static Rectangle[] ScoreRectangles { get; set; }

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

		public static List<bool[,]> History;

		public static bool HasBlock(Color color)
		{
			return color.R + color.G + color.B > BlockThreshold;
		}
	}
}
