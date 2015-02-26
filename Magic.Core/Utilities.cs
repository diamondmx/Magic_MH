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

        public static void LoadEvent(List<Core.Player> plist, List<Core.Match> mlist, string mtgEvent)
        {
            var players = new List<Player>;
            dbPlayer.LoadDBPlayers().Where(p => p.Active).ToList().ForEach(p => players.Add(new Player(p)));
            
            var matches = new List<Match>();
			dbMatch.LoadDBMatches(mtgEvent).ForEach(m=>matches.Add(new Match(m)));


        }
	}
}
