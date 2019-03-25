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
