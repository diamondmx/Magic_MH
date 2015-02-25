using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public class Utilities
	{
		public static List<Magic.Core.Player> GetAllPlayersFromMatches(string mtgEvent)
		{
			var matches = new List<Match>();
			dbMatch.LoadDBMatches(mtgEvent).ForEach(m=>matches.Add(new Match(m)));

			var players = Magic.Core.Player.FromMatchList(matches, mtgEvent);
			return players;
		}
	}
}
