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
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace BoomTracker
{
	[Serializable]
	public static class Data
	{
		public static ObservableCollection<Player> Players { get; set; }

		static Data()
		{
			Load();

			if (Players is null)
			{
				Players = new ObservableCollection<Player>();
			}
		}

		public static void Save()
		{
			var formatter = new BinaryFormatter();
			string backupFile = FileLocation.Data.PlayersBackup();

			try
			{
				if (File.Exists(FileLocation.Data.Players))
				{
					File.Copy(FileLocation.Data.Players, backupFile);
				}

				using (FileStream dataStream = new FileStream(FileLocation.Data.Players, FileMode.OpenOrCreate, FileAccess.Write))
				{
					formatter.Serialize(dataStream, Players);
					dataStream.Close();
				}
			}

			catch (Exception ex)
			{
				//if (File.Exists(backupFile))
				//{
				//	File.Copy(backupFile, FileLocation.Data.Players);
				//}

				MessageBox.Show($"Unable to save to file\n\n{ex.Message}");
			}
		}

		//public static void Save()
		//{

		//	XmlSerializer writer = new XmlSerializer(Players.GetType());

		//	var path = FileLocation.Data.Players;
		//	FileStream file = File.Create(path);

		//	writer.Serialize(file, Players);
		//	file.Close();

		//}

		public static void Load()
		{
			var formatter = new BinaryFormatter();

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
