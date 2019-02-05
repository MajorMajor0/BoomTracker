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
			public static int LeftPixel => 280;
			public static int TopPixel => 82;
			public static int RightPixel => 495;
			public static int BottomPixel => 422;

			public static int VerticalSquares => 20;
			public static int HorizontalSquares => 10;

			public static int[,,] GridPoints { get; set; }

			public static int Height { get; set; }
			public static int Width { get; set; }

			static PlayingField()
			{
				GridPoints = new int[HorizontalSquares, VerticalSquares, 2];

				int vertOffset = SquareHeight / 2;
				int horizontalOffset = SquareWidth / 2;

				for (int i = 0; i < PlayingField.HorizontalSquares; i++)
				{
					for (int j = 0; j < PlayingField.VerticalSquares; j++)
					{
						int x = PlayingField.LeftPixel + i * SquareWidth + horizontalOffset - i*3/4;
						int y = PlayingField.TopPixel + j * SquareHeight + vertOffset;
						PlayingField.GridPoints[i, j, 0] = x;
						PlayingField.GridPoints[i, j, 1] = y;
						//Debug.WriteLine($"[{x}, {y}]");
					}
				}
			}
		}

		public static class NextField
		{
			public static int LeftPixel => 522;
			public static int TopPixel => 225;
			public static int RightPixel => 612;
			public static int BottomPixel => 315;

			public static int Height { get; set; }
			public static int Width { get; set; }
		}

		public static List<bool[,]> History;

		public static int SquareHeight { get; }
		public static int SquareWidth { get; }

		static TetrisWindow()
		{
			PlayingField.Height = PlayingField.BottomPixel - PlayingField.TopPixel;
			PlayingField.Width = PlayingField.RightPixel - PlayingField.LeftPixel;

			NextField.Height = NextField.BottomPixel - NextField.TopPixel;
			NextField.Width = NextField.RightPixel - NextField.LeftPixel;

			SquareHeight = (int)Math.Round(((double)PlayingField.Height / (double)PlayingField.VerticalSquares), MidpointRounding.AwayFromZero);

			SquareWidth = (int)Math.Round(((double)PlayingField.Width / (double)PlayingField.HorizontalSquares), MidpointRounding.AwayFromZero);
		}

		public static bool HasBlock(Color color)
		{
			return color.R + color.G + color.B > BlockThreshold;
		}
	}
}
