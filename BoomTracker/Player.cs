using System;
using System.Collections.Generic;
using System.Linq;

namespace BoomTracker
{
	[Serializable]
	public class Player
	{
		public string Name { get; set; }

		public IEnumerable<Game> Games => Data.Games.Where(x => x.Player == Name);

		public int PersonalBestScore => Games.Max(x => x.FinalScore);

		public int PersonalBestLines => Games.Max(x => x.FinalLines);

		public int PersonalBestLevel => Games.Max(x => x.FinalLevel);
	}
}
