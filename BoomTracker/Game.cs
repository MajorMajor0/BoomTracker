using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace BoomTracker
{
	public class Game
	{
		private OCR ocr = new OCR();

		public byte StartLevel { get; set; }

		public byte EndLevel { get; set; }

		private int currentLevel;

		public int Score { get; set; }

		public List<State> States { get; set; } = new List<State>();

		public class State
		{
			public char?[][] Grid { get; set; } = new char?[10][];

			private int level;
			public int Level
			{
				get => level;
				set
				{
					if (value == level + 1)
					{
						level = value;
					}
				}
			}

			public int Lines { get; set; }

			public int Score { get; set; }

			public State()
			{
				for (int i = 0; i < 10; i++)
				{
					Grid[i] = new char?[20];
				}
			}

			public unsafe void GetGrid(Bitmap bitmap, int level)
			{
				Stopwatch watch = Stopwatch.StartNew();
				BitmapData bmData = bitmap.LockBits(
				new Rectangle(0, 0, (int)Tetris.ImageWidth, (int)Tetris.ImageHeight),
				ImageLockMode.ReadWrite,
				Tetris.PixelFormat);

				byte* scan0 = (byte*)bmData.Scan0.ToPointer();
				int nLevels = Palette.ScanTarget.B.GetLength(0);

				int Nk = Tetris.PlayingField.FieldWidth * Tetris.PlayingField.FieldHeight;
				for (int i = 0; i < Tetris.PlayingField.NColumns; i++)
				{
					for (int j = 0; j < Tetris.PlayingField.Nrows; j++)
					{
						int[] colors = { 0, 0, 0 };
						for (int k = 0; k < Nk; k++)
						{
							int address = Tetris.PlayingField.BlockAddresses[i, j][k];
							colors[0] += scan0[address]; // Blue
							colors[1] += scan0[address + 1]; // Green
							colors[2] += scan0[address + 2]; // Red

							//scan0[address] = 255; // Blue
							//scan0[address + 1]=0; // Green
							//scan0[address + 2]=0; // Red
						}

						int colorCheck = (colors[1] + colors[2]) / Nk;

						if (level >= nLevels)
						{
							Grid[i][j] = null;
						}

						else if (!Palette.ScanDictionary[level].TryGetValue(colorCheck, out Grid[i][j]))
						{
							Grid[i][j] = null;
						}

						if (colorCheck > 10 && Grid[i][j] == null)
						{
						}
					}
				}

				bitmap.UnlockBits(bmData);
			}
		}

		public async Task<State> StoreState(Bitmap bitmap)
		{
			State state = new State();

			using (Bitmap scoreBitmap = bitmap.Clone(Tetris.ScoreField.ScoreRectangle, PixelFormat.DontCare))
			using (Bitmap linesBitmap = bitmap.Clone(Tetris.LineField.LinesRectangle, PixelFormat.DontCare))
			using (Bitmap levelBitmap = bitmap.Clone(Tetris.LevelField.LevelRectangle, PixelFormat.DontCare))
			{
				Task<int>[] tasks = new Task<int>[3];

				tasks[0] = Task.Run(() => ocr.ReadNumber(scoreBitmap, 0));
				tasks[1] = Task.Run(() => ocr.ReadNumber(linesBitmap, 1));
				tasks[2] = Task.Run(() => ocr.ReadNumber(levelBitmap, 2));

				Task getGrid = Task.Run(() => { state.GetGrid(bitmap, currentLevel); });

				state.Score = await tasks[0];
				state.Lines = await tasks[1];
				state.Level = await tasks[2];
				currentLevel = state.Level;
				await getGrid;
			}

			States.Add(state);
			return state;
		}
	}
}
