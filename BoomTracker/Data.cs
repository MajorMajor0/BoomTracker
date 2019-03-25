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
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace BoomTracker
{
	[Serializable]
	public static class Data
	{
		public static List<Game> Games { get; set; }

		public static ObservableCollection<Player> Players { get; set; }

		static Data()
		{
			Load();

			if (Games is null)
			{
				//Games = new Game[0].ToLookup(x => x.Player, x => x) as Lookup<string, Game>;
				Games = new List<Game>();
			}

			if (Players is null)
			{
				Players = new ObservableCollection<Player>();
			}
		}

		public static void Save()
		{
			var formatter = new BinaryFormatter();
			try
			{
				using (FileStream dataStream =
					new FileStream(FileLocation.Data.Games, FileMode.OpenOrCreate, FileAccess.Write))
				{
					formatter.Serialize(dataStream, Games);
					dataStream.Close();
				}

				using (FileStream dataStream = new FileStream(FileLocation.Data.Players, FileMode.OpenOrCreate, FileAccess.Write))
				{
					formatter.Serialize(dataStream, Players);
					dataStream.Close();
				}
			}

			catch (Exception ex)
			{
				MessageBox.Show($"Unable to save to file\n\n{ex.Message}");
			}
		}

		public static void Load()
		{
			var formatter = new BinaryFormatter();
			if (File.Exists(FileLocation.Data.Games))
			{
				try
				{
					using (FileStream dataStream = new FileStream(FileLocation.Data.Games, FileMode.Open, FileAccess.Read))
					{
						//Games = (Lookup<string, Game>)formatter.Deserialize(dataStream);
						Games = (List<Game>)formatter.Deserialize(dataStream);
						dataStream.Close();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Problem loading {FileLocation.Data.Games}\n\n{ex.Message}.");
				}
			}

			if (File.Exists(FileLocation.Data.Players))
			{
				try
				{
					using (FileStream dataStream = new FileStream(FileLocation.Data.Players, FileMode.Open, FileAccess.Read))
					{
						Players = (ObservableCollection<Player>)formatter.Deserialize(dataStream);

						dataStream.Close();
					}
				}

				catch (Exception)
				{
					Console.WriteLine($"Problem loading {FileLocation.Data.Players}.");
				}
			}
		}
	}
}
