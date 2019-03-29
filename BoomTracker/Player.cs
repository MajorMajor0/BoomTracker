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
using System.Linq;

namespace BoomTracker
{
	[Serializable]
	public class Player
	{
		public string Name { get; set; }

		//public IEnumerable<Game> Games => Data.Games.Where(x => x.Player == Name);

		public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<Game>();

		public int PersonalBestScore => Games.Any() ? Games.Max(x => x.FinalScore) : 0;

		public int PersonalBestLines => Games.Any() ? Games.Max(x => x.FinalLines) : 0;

		public int PersonalBestLevel => Games.Any() ? Games.Max(x => x.FinalLevel) : 0;
	}
}
