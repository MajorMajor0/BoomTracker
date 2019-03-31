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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace BoomTracker
{
	[Serializable]
	public class Game : INotifyPropertyChanged
	{
		[NonSerialized]
		private bool isToppedOut;
		public bool IsToppedOut
		{
			get => isToppedOut;
			set
			{
				if (value != isToppedOut)
				{
					isToppedOut = value;
					OnPropertyChanged(nameof(IsToppedOut));
					OnTopOut(new EventArgs());
				}
			}
		}

		[NonSerialized]
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

		public int StartLevel => States.Min(x => x.Level);

		public int FinalLevel => States.Max(x => x.Level);

		public int FinalLines => States.Max(x => x.Lines);

		public int FinalScore => States.Max(x => x.Score);

		[NonSerialized]
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

		[NonSerialized]
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

		[NonSerialized]
		private int currentScore;
		public int CurrentScore
		{
			get => currentScore;
			set
			{
				if (value > currentScore)
				{
					currentScore = value;
					OnPropertyChanged(nameof(CurrentScore));
				}
			}
		}

		[NonSerialized]
		private Tetromino nextPiece;
		public Tetromino NextPiece
		{
			get => nextPiece;
			set
			{
				if (value != nextPiece)
				{
					nextPiece = value;
					OnPropertyChanged(nameof(NextPiece));
				}
			}
		}

		public List<State> States { get; set; } = new List<State>();

		[Serializable]
		public class State
		{
			public DateTime TimeStamp { get; set; } = DateTime.Now;

			public char?[][] Grid { get; set; } = new char?[10][];

			public int Level { get; set; }

			public int Lines { get; set; }

			public int Score { get; set; }

			public char? Next{ get; set; }

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
				new Rectangle(0, 0, Tetris.Image.Width, Tetris.Image.Height),
				ImageLockMode.ReadWrite,
				Tetris.PixelFormat);

				byte* scan0 = (byte*)bmData.Scan0.ToPointer();

				int Nk = Tetris.PlayingField.FieldWidth * Tetris.PlayingField.FieldHeight;
				for (int i = 0; i < Tetris.PlayingField.NColumns; i++)
				{
					for (int j = 0; j < Tetris.PlayingField.Nrows; j++)
					{
						var color = (0, 0, 0);
						for (int k = 0; k < Nk; k++)
						{
							int address = Tetris.PlayingField.BlockAddresses[i, j][k];
							color.Item3 += scan0[address]; // Blue
							color.Item2 += scan0[address + 1]; // Green
							color.Item1 += scan0[address + 2]; // Red
						}

						color.Item1 /= Nk;
						color.Item2 /= Nk;
						color.Item3 /= Nk;

						if (!Palette.ScanDictionary[level].TryGetValue(color, out Grid[i][j]))
						{
							Grid[i][j] = null;
						}

						// Debug.WriteLine($"{color.Item1}\t{color.Item2}\t{color.Item3}");
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

		public State StoreState(Bitmap bitmap)
		{
			State state = new State();

			//Task<int[]>[] tasks = new Task<int[]>[3];

			if (Tetris.Score.Read(bitmap, out int score))
			{
				CurrentScore = score;
			}

			if (Tetris.Lines.Read(bitmap, out int lines))
			{
				CurrentLines = lines;
			}

			if (Tetris.Level.Read(bitmap, out int level))
			{
				CurrentLevel = level;
			}

			if (Tetris.Next.Read(bitmap, out Tetromino nextPiece))
			{
				NextPiece = nextPiece;
			}

			CurrentGrid = state.GetGrid(bitmap, CurrentLevel);
			if (currentGrid[0][0] == 'D')
			{
				IsToppedOut = true;
			}

			else
			{
				state.Score = currentScore;
				state.Lines = currentLines;
				state.Level = currentLevel;
				state.Next = nextPiece?.Piece;
				States.Add(state);
				OnPropertyChanged(nameof(States));
			}

			return state;
		}

		protected virtual void OnTopOut(EventArgs e)
		{
			TopOut?.Invoke(this, e);
		}

		public event EventHandler TopOut;


		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
