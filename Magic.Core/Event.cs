using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    using NUnit.Framework;

    public class Event
    {
        public List<Core.Player> Players;
        public List<Core.Match> Matches;
        public string name;
        public int rounds;
        public int currentRound;

        public void LoadEvent(string eventName)
        {
            name = eventName;

            var loadedEvent = dbEvent.LoadDBEvent(eventName);
            rounds = loadedEvent.rounds;

            var eventPlayers = dbEventPlayers.LoadDBEventPlayers(eventName);

            Players = new List<Player>();
            dbPlayer.LoadDBPlayers().Where(p => eventPlayers.Any(ep => ep.Player == p.Name)).ToList().ForEach(p => Players.Add(new Player(p)));

            Matches = new List<Match>();
            dbMatch.LoadDBMatches(name).Where(m => m.Event == eventName).ToList().ForEach(m => Matches.Add(new Match(m)));

	        foreach (var match in Matches)
	        {
		        if(String.IsNullOrEmpty((match.Player1Name)))
							throw new Exception();

						if(String.IsNullOrEmpty((match.Player2Name)))
							throw new Exception();
	        }

            foreach (var p in Players)
            {
                foreach (var m in Matches)
                {   
                    if (m.Player1Name == p.name)
                    {
                        m.Player1 = p;
                        p.matches.Add(m);
                    }

                    else if (m.Player2Name == p.name)
                    {
                        m.Player2 = p;
                        p.matches.Add(m);
                    }
                }
            }
        }
    }
}