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

		private int currentScore;
		public int CurrentScore
		{
			get => currentScore;
			set
			{
				if (currentScore != value)
				{
					currentScore = value;
					OnPropertyChanged(nameof(CurrentScore));
				}
			}
		}

		public List<State> States { get; set; } = new List<State>();

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

			using (Bitmap scoreBitmap = bitmap.Clone(Tetris.ScoreField.ScoreRectangle, Tetris.PixelFormat))
			using (Bitmap linesBitmap = bitmap.Clone(Tetris.LineField.LinesRectangle, Tetris.PixelFormat))
			using (Bitmap levelBitmap = bitmap.Clone(Tetris.LevelField.LevelRectangle, Tetris.PixelFormat))
			{
				Task<int>[] tasks = new Task<int>[3];

				tasks[0] = Task.Run(() => ocr.ReadNumber(scoreBitmap, 0));
				tasks[1] = Task.Run(() => ocr.ReadNumber(linesBitmap, 1));
				tasks[2] = Task.Run(() => ocr.ReadNumber(levelBitmap, 2));

				CurrentLevel = await tasks[2];
				CurrentGrid = state.GetGrid(bitmap, CurrentLevel);
				CurrentScore = await tasks[0];
				CurrentLines = await tasks[1];

				state.Score = currentScore;
				state.Lines = currentLines;
				state.Level = CurrentLevel;
			}

			States.Add(state);
			OnPropertyChanged(nameof(States));
			return state;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
