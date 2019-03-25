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
