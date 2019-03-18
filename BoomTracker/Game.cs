using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace BoomTracker
{
	public class Game : INotifyPropertyChanged
	{
		private OCR ocr = new OCR();

		private char?[][] currentGrid = new char?[10][];
		public char?[][] CurrentGrid
		{
			get => currentGrid;
			set
			{
				currentGrid = value;
				OnPropertyChanged(nameof(CurrentGrid));
			}
		}

		public int StartLevel { get; set; }

		public int FinalLevel { get; set; }

		public int FinalLines { get; set; }

		public int FinalScore { get; set; }


		private int currentLevel;
		public int CurrentLevel
		{
			get => currentLevel;
			set
			{
				if (value == currentLevel + 1 || currentScore == 0)
				{
					currentLevel = value;
					OnPropertyChanged(nameof(CurrentLevel));
				}
			}
		}

		private int currentLines;
		public int CurrentLines
		{
			get => currentLines;
			set
			{
				if (value == currentLines + 1 ||
					value == currentLines + 2 ||
					value == currentLines + 3 ||
					value == currentLines + 4 ||
					currentScore == 0)
				{
					currentLines = value;
					OnPropertyChanged(nameof(CurrentLines));
				}
			}
		}


		//private int currentScoreBuffer;
		public int CurrentScoreConfidence;
		private int currentScore;
		public int CurrentScore
		{
			get => currentScore;
			set
			{
				if (value != currentScore)
				{
					//if (value == currentScoreBuffer)
					//{
					currentScore = value;
					OnPropertyChanged(nameof(CurrentScore));
					//}
					//else
					//{
					//	currentScoreBuffer = value;
					//}

				}
			}
		}

		public Stack<State> States { get; set; } = new Stack<State>();

		public class State
		{
			public char?[][] Grid { get; set; } = new char?[10][];

			public int Level { get; set; }

			public int Lines { get; set; }

			public int Score { get; set; }

			public State()
			{
				for (int i = 0; i < 10; i++)
				{
					Grid[i] = new char?[20];
				}
			}

			public unsafe char?[][] GetGrid(Bitmap bitmap, int level)
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

						//Debug.WriteLine($"{colors[2] / Nk}\t{colors[1] / Nk}\t{colors[0] / Nk}");
					}
				}

				bitmap.UnlockBits(bmData);
				return Grid;
			}

		}

		public Game()
		{
			for (int i = 0; i < 10; i++)
			{
				CurrentGrid[i] = new char?[20];
			}
		}

		public async Task<State> StoreState(Bitmap bitmap)
		{
			State state = new State();

			//using (Bitmap scoreBitmap = bitmap.Clone(Tetris.ScoreField.ScoreRectangle, Tetris.PixelFormat))
			using (Bitmap linesBitmap = bitmap.Clone(Tetris.LineField.LinesRectangle, Tetris.PixelFormat))
			using (Bitmap levelBitmap = bitmap.Clone(Tetris.LevelField.LevelRectangle, Tetris.PixelFormat))
			{
				Task<int[]>[] tasks = new Task<int[]>[3];

				//tasks[0] = Task.Run(() => ocr.ReadNumber(scoreBitmap, 0));
				tasks[1] = Task.Run(() => ocr.ReadNumber(linesBitmap, 1));
				tasks[2] = Task.Run(() => ocr.ReadNumber(levelBitmap, 2));

				var cl = await tasks[2];
				CurrentLevel = cl[0];

				CurrentGrid = state.GetGrid(bitmap, CurrentLevel);

				//int[] cs = await tasks[0];
				//CurrentScore = cs[0];
				//CurrentScoreConfidence = cs[1];


				BitmapData bmData = bitmap.LockBits(Tetris.ScoreField.ScoreRectangle, ImageLockMode.ReadOnly, Tetris.PixelFormat);
				if( OCR.GetScore(bmData, out int score))
				{
					CurrentScore = score;
				}
				bitmap.UnlockBits(bmData);

				int[] cl1 = await tasks[1];
				CurrentLines = cl1[0];

				state.Score = currentScore;
				state.Lines = currentLines;
				state.Level = currentLevel;
			}

			States.Push(state);
			OnPropertyChanged(nameof(States));
			return state;
		}

		/// <summary>Check whether the designated rectangle contains any non-black pixels. The rectangle is selected to be all black during a game and all non-black during menu screens
		/// </summary>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		public unsafe bool CheckIfGameIsOn(Bitmap bitmap)
		{
			Stopwatch watch = Stopwatch.StartNew();
			BitmapData bmData = bitmap.LockBits(
			Tetris.IsGameRectangle,
			ImageLockMode.ReadOnly,
			Tetris.PixelFormat);

			byte* scan0 = (byte*)bmData.Scan0.ToPointer();

			int Nk = bmData.Stride * bmData.Height;

			// Starting with 2 for red (0 = blue, 1 = green), advance by teh number of bytes per pixel to check red pixel
			for (int i = 2; i < Tetris.PlayingField.NColumns; i += Tetris.BppDictionary[Tetris.PixelFormat])
			{
				// Red
				if (scan0[i] > Palette.BlackThreshold)
				{
					return false;
				}
			}

			bitmap.UnlockBits(bmData);
			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
