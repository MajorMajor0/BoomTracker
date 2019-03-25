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

using System.IO;

namespace BoomTracker
{
	public static class FileLocation
	{
		public static string Folder => @"C:\Source\BoomTracker\BoomTracker";

		public static string Screens => $"{Folder}\\Resources\\Screens";

		public static string Scores => $"{Folder}\\Resources\\Scores";

		public static string Lines => $"{Folder}\\Resources\\Lines";

		public static string Levels => $"{Folder}\\Resources\\Levels";

		public static string TesseractData => $"{Folder}\\Tessdata";

		public static string TesseractConfig => $"{TesseractData}\\config.txt";

		public static class Data
		{
			public static string Folder => $"{FileLocation.Folder}\\Data";

			public static string Players => $"{Folder}\\Players.dat";

			public static string Games => $"{Folder}\\Games.dat";
		}


		static FileLocation()
		{
			Directory.CreateDirectory(Screens);
			Directory.CreateDirectory(Scores);
			Directory.CreateDirectory(Lines);
			Directory.CreateDirectory(Levels);
			Directory.CreateDirectory(TesseractData);
			Directory.CreateDirectory(Data.Folder);
		}
	}
}
