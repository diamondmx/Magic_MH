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
		public List<Match> matches;
		public int? droppedInRound;

        /*public List<Player> roundPlayers(int round)
        {
            var roundMatches = matches.Where(m => m.Round == round);
            return roundMatches.Select(m => m.WithPlayerOneAs(name).Player2).ToList();
        }*/
		public Player(string newName)
		{
			name = newName;
            matches = new List<Match>();
		}

        public Player(dbPlayer p)
        :this(p.Name)
        {
        }

        public int Score(int round=0)
        {
            if(round<=0)
            {
                return matches.Where(m => m.Player1Wins > m.Player2Wins).Count();
            }
            else
            {
                var roundMatches = matches.Where(m => m.Round == round).ToList();
                roundMatches.ForEach(m => m.SetPlayerOneTo(name));
                var wonMatches = roundMatches.Where(m => m.Player1Wins > m.Player2Wins);
                var score = wonMatches.Count();
                return score;
            }



        }
	}
}
