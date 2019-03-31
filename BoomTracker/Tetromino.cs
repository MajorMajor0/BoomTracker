using System.Drawing;

namespace BoomTracker
{
	public class Tetromino
	{
		public char Piece { get; set; }
		public Rectangle[] Rectangles = new Rectangle[4];
		public int[][] Pixels { get; set; } = new int[4][];
		public int[] Addresses { get; set; }
		public string Display { get; set; }
	}
}
