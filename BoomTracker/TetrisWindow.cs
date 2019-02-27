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

			public static int[,,] GridPoints { get; set; }

			static PlayingField()
			{
				GridPoints = new int[HorizontalSquares, VerticalSquares, 2];

				int vertOffset = SquareHeight / 2;
				int horizontalOffset = SquareWidth / 2;

				for (int i = 0; i < HorizontalSquares; i++)
				{
					for (int j = 0; j < VerticalSquares; j++)
					{
						int x = Rectangle.Left + i * SquareWidth + horizontalOffset - i * 3 / 4;
						int y = Rectangle.Top + j * SquareHeight + vertOffset;
						GridPoints[i, j, 0] = x;
						GridPoints[i, j, 1] = y;
						//Debug.WriteLine($"[{x}, {y}]");
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

			public static Rectangle[] ScoreRectangles { get; set; }

			static ScoreField()
			{
				int h = 18;
				int y = 115;
				int w = 22;

				ScoreRectangles = new Rectangle[]
				{
					new Rectangle { X = 642,  Y = y, Height = h, Width = w },
					new Rectangle { X = 621,  Y = y, Height = h, Width = w },
					new Rectangle { X = 599,  Y = y, Height = h, Width = w },
					new Rectangle { X = 578,  Y = y, Height = h, Width = w },
					new Rectangle { X = 557,  Y = y, Height = h, Width = w },
					new Rectangle { X = 535,  Y = y, Height = h, Width = w }

				};

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
		}


		public static List<bool[,]> History;

		public static int SquareHeight { get; }
		public static int SquareWidth { get; }

		static TetrisWindow()
		{
			SquareHeight = (int)Math.Round(((double)PlayingField.Rectangle.Height / (double)PlayingField.VerticalSquares), MidpointRounding.AwayFromZero);

			SquareWidth = (int)Math.Round(((double)PlayingField.Rectangle.Width / (double)PlayingField.HorizontalSquares), MidpointRounding.AwayFromZero);
		}

		public static bool HasBlock(Color color)
		{
			return color.R + color.G + color.B > BlockThreshold;
		}

	}
}
