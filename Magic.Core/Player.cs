using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public class Player
	{
		public string name;
		public List<Player> round1Players;
		public List<Player> round2Players;
		public List<Player> round3Players;
		public List<Player> CurrentPlayers;
		public List<Match> matches;
		public int Score;
		public int? droppedInRound;

        /*public List<Player> roundPlayers(int round)
        {
            var roundMatches = matches.Where(m => m.Round == round);
            return roundMatches.Select(m => m.WithPlayerOneAs(name).Player2).ToList();
        }*/
		public Player(string newName, int newScore)
		{
			name = newName;
			Score = newScore;
			round1Players = new List<Player>();
			round2Players = new List<Player>();
			round3Players = new List<Player>();
			CurrentPlayers = new List<Player>();
            matches = new List<Match>();
		}

        public Player(dbPlayer p)
        :this(p.Name, 0)
        {
        }
	}
}
