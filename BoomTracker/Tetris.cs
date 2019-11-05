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
using System.Drawing;

using System.Drawing.Imaging;

namespace BoomTracker
{
	public static class Tetris
	{
		private static class DigitPixels
		{
			public static List<int[]> X = new List<int[]>
			{
				new int[] {2,2,3,3,3,3,4,4,4,4,4,4,6,8,13,14,15,15,15,16,16,16,16,16,16,17,17,17,17,18,18},
				new int[] {9,9,9,10,10,10,10,10,10,10,10,10,10,11,11,11,11,11,11,11,11,11,11,11,11,11,12,12,12,12,12,12,12,12,12,12,12,12,12,13,13,14},
				new int[] {3,3,3,4,4,4,4,5,5,5,5,6,6,6,6,7,7,7,7,7,7,8,8,8,8,8,9,9,9,9,10,10,10,10,11,11,11,11,11,12,12,12,12,12,13,13,13,13,13,14,14,14,14,14,15,15,15,16,16,16,16,17,17,17},
				new int[] {5,8,9,9,10,10,11,11,11,11,11,11,12,12,12,12,12,13,13,13,13,13,14,14,14,14,14,14,15,15,15,15,15,15,15,16,16,16,16,16,17,17},
				new int[] {3,3,4,4,4,5,5,10,11,11,11,12,12,12,12,12,13,13,13,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,15,15,15,15,15,15,15,15,15,15,15,16,16},
				new int[] {3,4,4,4,5,5,6,9,10,11,12,13,13,14,15,15,15,15,15,15,15,16,16,16,16,16,16,17,17,17,17,18},
				new int[] {2,2,3,3,3,3,3,3,4,4,4,4,4,4,4,4,5,5,5,5,5,6,7,11,13,14,14,15,15,15,15,15,16,16,16,16,17,17},
				new int[] {3,4,4,8,8,8,8,8,9,9,9,9,9,9,10,10,10,10,10,10,10,11,11,12,13,13,14,15,16,16,17},
				new int[] {2,3,3,3,3,3,3,4,4,4,4,4,4,4,5,5,5,5,5,5,5,6,6,6,7,7,8,8,9,9,9,10,10,10,11,11,11,12,12,12,12,13,13,13,13,13,13,14,14,14,14,14,15,15,15,15,15,15,15,15,15,15,15,15,16,16,16,16,16,16,16,17,17,17,17,17,17},
				new int[] {3,4,4,5,5,5,6,7,8,9,9,10,10,11,11,11,12,12,12,12,13,13,13,13,13,14,14,14,14,15,15,15,15,15,15,16,16,16,16,16,16,16,16,16,17,17,17,17,17,17,17,18,18}
			};

			public static List<int[]> Y = new List<int[]>
			{
				new int[] {7,8,7,8,9,10,6,7,8,9,10,11,13,14,3,4,5,6,7,6,7,8,9,10,11,7,8,9,10,8,10},
				new int[] {4,14,15,4,5,6,7,8,9,10,11,14,15,3,4,5,6,7,8,9,10,11,12,13,14,15,3,4,5,6,7,8,9,10,11,12,13,14,15,14,15,14},
				new int[] {4,13,14,3,4,13,14,3,12,13,14,11,12,13,14,10,11,12,13,14,15,9,10,11,14,15,9,10,11,15,9,10,11,15,2,8,9,10,15,2,8,9,10,15,2,8,9,10,15,6,7,8,9,15,3,6,7,4,5,6,7,4,5,6},
				new int[] {14,2,2,8,2,8,2,3,6,7,8,15,2,3,6,7,8,2,3,4,8,9,2,3,4,8,9,14,2,3,9,10,11,12,13,2,10,11,12,13,11,12},
				new int[] {9,10,8,9,10,7,10,11,3,4,11,3,4,5,10,11,3,4,5,6,7,8,9,10,11,12,13,14,4,5,6,7,8,9,10,11,12,13,14,4,5,6,7,8,9,10,11,12,13,14,10,11},
				new int[] {5,3,5,6,6,14,6,2,2,2,2,2,7,7,7,8,9,10,11,12,13,8,9,10,11,12,13,9,10,11,12,11},
				new int[] {8,10,7,8,9,10,11,12,6,7,8,9,10,11,12,13,5,7,8,9,14,8,3,15,2,9,14,9,10,11,12,13,10,11,12,13,11,12},
				new int[] {4,3,4,2,11,12,13,14,2,10,11,12,13,14,2,9,10,11,12,13,14,2,9,2,2,7,2,3,3,4,4},
				new int[] {11,5,6,7,10,11,13,5,6,7,10,11,12,13,3,6,8,9,12,14,15,8,9,15,8,15,9,15,2,9,15,2,9,15,2,9,15,2,8,9,15,2,3,8,9,14,15,2,3,8,9,15,2,3,5,7,8,9,10,11,12,13,14,15,5,7,9,10,11,12,13,5,6,10,11,12,13},
				new int[] {5,5,6,3,5,8,8,8,2,2,15,2,15,2,9,14,2,8,9,14,2,8,9,13,14,2,3,8,9,3,4,5,8,9,11,3,4,5,6,7,8,9,10,11,5,6,7,8,9,10,11,8,10}
			};

			public static int ReadThreshold => 75;

		}

		/// <summary>OCR will guess numbers in this order to avoid accidentally masking a number with fewer pixels </summary>
		private static readonly int[] guessOrder = new int[] { 8, 0, 9, 6, 5, 3, 2, 4, 7, 1, 10 };

		public static PixelFormat PixelFormat => PixelFormat.Format24bppRgb;

		public static byte BytesPerPixel;

		public static Rectangle Image = new Rectangle(0, 0, (int)imageWidth, (int)imageHeight);

		/// <summary>Output NES resolution (generated resolution = 256 x 240)</summary>
		private static double NESHeight => 224.0;
		private static double NESWidth => 256.0;

		private static double imageWidth => 720.0;
		private static double imageHeight => 480.0;

		/// <summary>Height of a digit in the score field, lines field and level field</summary>
		public static int DigitWidth = 21;

		/// <summary>Width of a digit in the score field, lines field and level field</summary>
		public static int DigitHeight = 17;

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
			//private static readonly double BlockWidthNes = 7.0;

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
				BlockHeight = BlockHeightNes * Image.Height / NESHeight;
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
						double x0 = Rectangle.Left + BlockMargin + i * (BlockWidth + BlockMargin);
						double y0 = Rectangle.Top + BlockMargin + j * (BlockHeight + BlockMargin);

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
							int pixel = BytesPerPixel * (x + Image.Width * y);

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

		public static Dictionary<char, Tetromino> Tetrominos = new Dictionary<char, Tetromino>
			{
				{ 'T', new Tetromino { Piece = 'T', Display="AAA\n A"} },
				{ 'J', new Tetromino { Piece = 'J', Display="CCC\n  C"} },
				{ 'Z', new Tetromino { Piece = 'Z', Display="CC\n CC" } },
				{ 'O', new Tetromino { Piece = 'O', Display="AA\nAA"} },
				{ 'S', new Tetromino { Piece = 'S', Display=" BB\nBB" } },
				{ 'L', new Tetromino { Piece = 'L', Display="BBB\nB"} },
				{ 'I', new Tetromino { Piece = 'I', Display="AAAA" } }
			};

		public static class Next
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 535,
				Y = 200,
				Width = 87,
				Height = 88
			};

			static Next()
			{
				SetPixels();
				SetRectangles();

				SetAddresses(Tetrominos['T']);
				SetAddresses(Tetrominos['J']);
				SetAddresses(Tetrominos['Z']);
				SetAddresses(Tetrominos['O']);
				SetAddresses(Tetrominos['S']);
				SetAddresses(Tetrominos['L']);
				SetAddresses(Tetrominos['I']);
			}

			private static void SetPixels()
			{
				Tetrominos['T'].Pixels = new[] { new[] { 23, 43 }, new[] { 44, 43 }, new[] { 65, 43 }, new[] { 44, 60 } };
				Tetrominos['J'].Pixels = new[] { new[] { 23, 43 }, new[] { 44, 43 }, new[] { 65, 43 }, new[] { 65, 60 } };
				Tetrominos['Z'].Pixels = new[] { new[] { 23, 43 }, new[] { 44, 43 }, new[] { 44, 60 }, new[] { 65, 60 } };
				Tetrominos['O'].Pixels = new[] { new[] { 34, 43 }, new[] { 54, 43 }, new[] { 34, 60 }, new[] { 55, 60 } };
				Tetrominos['S'].Pixels = new[] { new[] { 44, 43 }, new[] { 65, 43 }, new[] { 23, 60 }, new[] { 44, 60 } };
				Tetrominos['L'].Pixels = new[] { new[] { 23, 43 }, new[] { 44, 43 }, new[] { 65, 43 }, new[] { 23, 60 } };
				Tetrominos['I'].Pixels = new[] { new[] { 12, 51 }, new[] { 34, 51 }, new[] { 55, 51 }, new[] { 75, 51 } };
			}

			private static void SetRectangles()
			{
				int w = 4;
				int h = 4;

				int dw = w / 2;
				int dh = h / 2;

				Tetrominos['T'].Rectangles = new Rectangle[]
				{
					new Rectangle( 18 - dw, 40 - dh, w, h ), // 23, 43
					new Rectangle( 44 - dw, 40 - dh, w, h ), // 44, 43
					new Rectangle( 69 - dw, 40 - dh, w, h ), // 65, 43
					new Rectangle( 44 - dw, 63 - dh, w, h )  // 44, 60
				};

				Tetrominos['J'].Rectangles = new Rectangle[]
				{
					new Rectangle( 18 - dw, 40 - dh, w, h ), //  3, 43
					new Rectangle( 44 - dw, 40 - dh, w, h ), // 44, 43
					new Rectangle( 69 - dw, 40 - dh, w, h ), // 65, 43
					new Rectangle( 69 - dw, 63 - dh, w, h )  // 65, 60
				};

				Tetrominos['Z'].Rectangles = new Rectangle[]
				{
					new Rectangle( 23 - dw, 43 - dh, w, h ),
					new Rectangle( 44 - dw, 43 - dh, w, h ),
					new Rectangle( 44 - dw, 60 - dh, w, h ),
					new Rectangle( 65 - dw, 60 - dh, w, h )
				};

				Tetrominos['O'].Rectangles = new Rectangle[]
				{
					new Rectangle( 29 - dw, 40 - dh, w, h ), // 34, 43
					new Rectangle( 59 - dw, 40 - dh, w, h ), // 54, 43
					new Rectangle( 29 - dw, 63 - dh, w, h ), // 34, 60
					new Rectangle( 59 - dw, 63 - dh, w, h )  // 55, 60
				};

				Tetrominos['S'].Rectangles = new Rectangle[]
				{
					new Rectangle( 44 - dw, 43 - dh, w, h ),
					new Rectangle( 65 - dw, 43 - dh, w, h ),
					new Rectangle( 23 - dw, 60 - dh, w, h ),
					new Rectangle( 44 - dw, 60 - dh, w, h )
				};

				Tetrominos['L'].Rectangles = new Rectangle[]
				{
					new Rectangle( 18 - dw, 40 - dh, w, h ), // 23, 43
					new Rectangle( 44 - dw, 40 - dh, w, h ), // 44, 43
					new Rectangle( 69 - dw, 40 - dh, w, h ), // 65, 43
					new Rectangle( 18 - dw, 63 - dh, w, h )  // 23, 60
				};

				Tetrominos['I'].Rectangles = new Rectangle[]
				{
					new Rectangle( 8 - dw, 51 - dh, w, h ),//12, 51
					new Rectangle( 34 - dw, 51 - dh, w, h ),
					new Rectangle( 55 - dw, 51 - dh, w, h ),
					new Rectangle( 79 - dw, 51 - dh, w, h )//75, 51
				};
			}

			private static void SetAddresses(Tetromino tetromino)
			{
				int imageWidth = Image.Width;
				List<int> addresses = new List<int>();

				foreach (Rectangle rect in tetromino.Rectangles)
				{
					int x0 = rect.X;
					int y0 = rect.Y;
					for (int x1 = 0; x1 < rect.Width; x1++)
					{
						for (int y1 = 0; y1 < rect.Width; y1++)
						{
							int x = x0 + x1;
							int y = y0 + y1;

							int address = BytesPerPixel * (x + imageWidth * y);
							addresses.Add(address);
						}
					}
				}

				tetromino.Addresses = addresses.ToArray();
			}

			public static unsafe bool Read(Bitmap bitmap, out Tetromino nextPiece)
			{
				bool returner = false;
				BitmapData bmData = bitmap.LockBits(Rectangle, ImageLockMode.ReadOnly, PixelFormat);
				byte* scan0 = (byte*)bmData.Scan0.ToPointer();
				nextPiece = null;

				foreach (var tet in Tetrominos.Values)
				{
					bool goodGuess = true;
					int i = 0;

					foreach (int address in tet.Addresses)
					{
						i++;
						if (scan0[address] < DigitPixels.ReadThreshold &&
							scan0[address + 1] < DigitPixels.ReadThreshold &&
							scan0[address + 2] < DigitPixels.ReadThreshold)
						{
							goodGuess = false;
							break;
						}
					}

					if (goodGuess)
					{
						returner = true;
						nextPiece = tet;
						break;
					}
				}

				bitmap.UnlockBits(bmData);
				return returner;
			}
		}

		public static class Score
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 536,
				Y = 115,
				Width = 128,
				Height = 17
			};

			/// <summary>X offset from Scorefield.Rectangle of digit</summary>
			public static int[] DigitOffsets = new int[] { 107, 85, 64, 43, 22, 0 };

			/// <summary>Given a digit 0-9 and a place 0-5, these are the addresses that should be lit if the place is showing that address when reading the score rectangle</summary>
			public static int[,][] Addresses = new int[10, 6][];

			static Score()
			{
				SetAddresses();
			}

			private static void SetAddresses()
			{
				int imageWidth = Image.Width;

				for (int digit = 0; digit < 10; digit++)
				{
					for (int place = 0; place < 6; place++)
					{
						List<int> addresses = new List<int>();

						int x0 = DigitOffsets[place];

						for (int i = 0; i < DigitPixels.X[digit].Length; i++)
						{
							int x = DigitPixels.X[digit][i] + x0;
							int y = DigitPixels.Y[digit][i];

							int address = BytesPerPixel * (x + imageWidth * y);

							addresses.Add(address);
						}
						Addresses[digit, place] = addresses.ToArray();
					}
				}
			}

			public static unsafe bool Read(Bitmap bitmap, out int score)
			{
				BitmapData bmData = bitmap.LockBits(Rectangle, ImageLockMode.ReadOnly, PixelFormat);
				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				score = 0;

				for (int place = 0; place < 6; place++)
				{
					for (int i = 0; i < 11; i++)
					{
						int numberGuess = guessOrder[i];
						if (numberGuess == 10)
						{
							bitmap.UnlockBits(bmData);
							return false;
						}

						bool goodGuess = true;

						foreach (var address in Addresses[numberGuess, place])
						{
							if (scan0[address] < DigitPixels.ReadThreshold)
							{
								goodGuess = false;
								break;
							}
						}

						if (goodGuess)
						{
							score += numberGuess * (int)Math.Pow(10, place);
							break;
						}
					}
				}

				bitmap.UnlockBits(bmData);
				return true;
			}
		}

		public static class Lines
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 430,
				Y = 30,
				Width = 64,
				Height = 17
			};

			/// <summary>X offset from Lines.Rectangle of digit</summary>
			public static int[] DigitOffsets = new int[] { 43, 22, 0 };

			/// <summary>Given a digit 0-9 and a place 0-2, these are the addresses that should be lit if the place is showing that address when reading the score rectangle</summary>
			public static int[,][] Addresses = new int[10, 3][];

			static Lines()
			{
				SetAddresses();
			}

			private static void SetAddresses()
			{
				int imageWidth = Image.Width;

				for (int digit = 0; digit < 10; digit++)
				{
					for (int place = 0; place < 3; place++)
					{
						List<int> addresses = new List<int>();

						int x0 = DigitOffsets[place];

						for (int i = 0; i < DigitPixels.X[digit].Length; i++)
						{
							int x = DigitPixels.X[digit][i] + x0;
							int y = DigitPixels.Y[digit][i];

							int address = BytesPerPixel * (x + imageWidth * y);

							addresses.Add(address);
						}
						Addresses[digit, place] = addresses.ToArray();
					}
				}
			}

			public static unsafe bool Read(Bitmap bitmap, out int score)
			{
				BitmapData bmData = bitmap.LockBits(Rectangle, ImageLockMode.ReadOnly, PixelFormat);
				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				score = 0;

				for (int place = 0; place < 3; place++)
				{
					for (int i = 0; i < 11; i++)
					{
						int numberGuess = guessOrder[i];

						if (numberGuess == 10)
						{
							bitmap.UnlockBits(bmData);
							return false;
						}

						bool goodGuess = true;

						foreach (var address in Addresses[numberGuess, place])
						{
							if (scan0[address] < DigitPixels.ReadThreshold)
							{
								goodGuess = false;
								break;
							}
						}

						if (goodGuess)
						{
							score += numberGuess * (int)Math.Pow(10, place);
							break;
						}
					}
				}
				bitmap.UnlockBits(bmData);
				return true;
			}
		}

		public static class Level
		{
			public static Rectangle Rectangle { get; } = new Rectangle
			{
				X = 579,
				Y = 336,
				Width = 43,
				Height = 17
			};

			/// <summary>X offset from Level.Rectangle of digit</summary>
			public static int[] DigitOffsets = new int[] { 21, 0 };

			/// <summary>Given a digit 0-9 and a place 0-1, these are the addresses that should be lit if the place is showing that address when reading the score rectangle</summary>
			public static int[,][] Addresses = new int[10, 2][];

			static Level()
			{
				SetAddresses();
			}

			private static void SetAddresses()
			{
				int imageWidth = Image.Width;

				for (int digit = 0; digit < 10; digit++)
				{
					for (int place = 0; place < 2; place++)
					{
						List<int> addresses = new List<int>();

						int x0 = DigitOffsets[place];

						for (int i = 0; i < DigitPixels.X[digit].Length; i++)
						{
							int x = DigitPixels.X[digit][i] + x0;
							int y = DigitPixels.Y[digit][i];

							int address = BytesPerPixel * (x + imageWidth * y);

							addresses.Add(address);
						}
						Addresses[digit, place] = addresses.ToArray();
					}
				}
			}

			public static unsafe bool Read(Bitmap bitmap, out int score)
			{
				BitmapData bmData = bitmap.LockBits(Rectangle, ImageLockMode.ReadOnly, PixelFormat);
				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				score = 0;

				for (int place = 0; place < 2; place++)
				{
					for (int i = 0; i < 11; i++)
					{
						int numberGuess = guessOrder[i];
						if (numberGuess == 10)
						{
							bitmap.UnlockBits(bmData);
							return false;
						}

						bool goodGuess = true;

						foreach (var address in Addresses[numberGuess, place])
						{
							if (scan0[address] < DigitPixels.ReadThreshold)
							{
								goodGuess = false;
								break;
							}
						}

						if (goodGuess)
						{
							score += numberGuess * (int)Math.Pow(10, place);
							break;
						}
					}
				}
				bitmap.UnlockBits(bmData);
				return true;
			}
		}

		public static class GameInProgress
		{
			/// <summary>This entire rectangle should be black during a game (or paused) and non-black when not during a game</summary>
			public static Rectangle Rectangle { get; } = new Rectangle(545, 36, 5, 3);

			public static int[] Addresses { get; private set; }

			static GameInProgress()
			{
				SetAddresses();
			}

			private static void SetAddresses()
			{
				List<int> addresses = new List<int>();

				int imageWidth = Image.Width;

				for (int x = 0; x < Rectangle.Width; x++)
				{
					for (int y = 0; y < Rectangle.Height; y++)
					{
						int address = BytesPerPixel * (x + imageWidth * y);
						addresses.Add(address);
					}
				}
				Addresses = addresses.ToArray();
			}

			/// <summary>Check whether the designated rectangle contains any non-black pixels. The rectangle is selected to be all black during a game and all non-black during menu screens
			/// </summary>
			/// <param name="bitmap"></param>
			/// <returns></returns>
			public static unsafe bool Check(Bitmap bitmap)
			{
				bool returner = true;
				BitmapData bmData = bitmap.LockBits(
				Rectangle,
				ImageLockMode.ReadOnly,
				PixelFormat);

				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				int Nk = bmData.Stride * bmData.Height;

				// Starting with 2 for red (0 = blue, 1 = green), advance by the number of bytes per pixel to check red pixel
				foreach (var address in Addresses)
				{
					if (scan0[address + 2] > Palette.BlackThreshold)
					{
						returner = false;
					}
				}

				bitmap.UnlockBits(bmData);
				return returner;
			}
		}

		public static class NintendoOn
		{
			/// <summary> This rectangle is always lit when Tetris is turned on except when paused</summary>
			public static Rectangle Rectangle { get; } = new Rectangle(35, 92, 3, 3);

			public static int[] Addresses { get; private set; }

			static NintendoOn()
			{
				SetAddresses();
			}

			private static void SetAddresses()
			{
				List<int> addresses = new List<int>();

				int imageWidth = Image.Width;

				for (int x = 0; x < Rectangle.Width; x++)
				{
					for (int y = 0; y < Rectangle.Height; y++)
					{
						int address = BytesPerPixel * (x + imageWidth * y);
						addresses.Add(address);
					}
				}
				Addresses = addresses.ToArray();
			}

			/// <summary>Check whether the designated rectangle contains any black pixels. The rectangle is selected to be always lit while Tetris is running, regardless of what screen is shown.
			/// </summary>
			/// <param name="bitmap"></param>
			/// <returns></returns>
			public static unsafe bool Check(Bitmap bitmap)
			{
				bool returner = true;
				BitmapData bmData = bitmap.LockBits(
				Rectangle,
				ImageLockMode.ReadOnly,
				PixelFormat);

				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				int Nk = bmData.Stride * bmData.Height;

				// Starting with 2 for red (0 = blue, 1 = green), advance by the number of bytes per pixel to check red pixel
				foreach (var address in Addresses)
				{
					if (scan0[address + 2] < Palette.BlackThreshold)
					{
						returner = false;
					}
				}

				bitmap.UnlockBits(bmData);
				return returner;
			}

		}

		public static class Pause
		{
			/// <summary> This rectangle is always lit when Tetris is paused</summary>
			public static Rectangle Rectangle { get; } = new Rectangle(347, 245, 3, 3);

			public static int[] Addresses { get; private set; }

			static Pause()
			{
				SetAddresses();
			}

			private static void SetAddresses()
			{
				List<int> addresses = new List<int>();

				int imageWidth = Image.Width;

				for (int x = 0; x < Rectangle.Width; x++)
				{
					for (int y = 0; y < Rectangle.Height; y++)
					{
						int address = BytesPerPixel * (x + imageWidth * y);
						addresses.Add(address);
					}
				}
				Addresses = addresses.ToArray();
			}

			/// <summary>Check whether the designated rectangle contains any black pixels. The rectangle is selected to be always lit while Tetris is running, regardless of what screen is shown.
			/// </summary>
			/// <param name="bitmap"></param>
			/// <returns></returns>
			public static unsafe bool Check(Bitmap bitmap)
			{
				bool returner = true;
				BitmapData bmData = bitmap.LockBits(
				Rectangle,
				ImageLockMode.ReadOnly,
				PixelFormat);

				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				int Nk = bmData.Stride * bmData.Height;

				// Starting with 2 for red (0 = blue, 1 = green), advance by the number of bytes per pixel to check red pixel
				foreach (var address in Addresses)
				{
					if (scan0[address] < Palette.BlackThreshold)
					{
						returner = false;
					}
				}

				bitmap.UnlockBits(bmData);
				return returner;
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
