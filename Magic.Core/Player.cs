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

        public float MWP(int round=0)
        {
            List<Match> relevantMatches = null;
            if(round==0)
            {
                relevantMatches = matches;
            }
            else
            {
                relevantMatches = matches.Where(m => m.Round == round).ToList();
            }
            
            float matchWins = relevantMatches.Count(m => m.DidPlayerWin(name));
            return (matchWins / (float)relevantMatches.Count())*100;

        }

        public List<Player> Opponents(int round=0)
        {
            List<Match> relevantMatches = matches;
            if(round>0)
            {
                relevantMatches = relevantMatches.Where(m => m.Round == round).ToList();   
            }

            return relevantMatches.Select<Match, Player>(m => (m.Player1.name==name)?m.Player2:m.Player1).ToList();
        }

        public float OMWP(int round=0)
        {
            var opponents = Opponents(round);
            float omwp = (float)opponents.Average(o => o.MWP(round)>33.33f?o.MWP(round):33.33f);
            return omwp;
        }

        public float GWP(int round=0)
        {
            int gameWins = 0;
            int gameLosses = 0;
            matches.ForEach(m => {
                var normalisedMatch = m.WithPlayerOneAs(name);
                gameWins += normalisedMatch.Player1Wins;
                gameLosses += normalisedMatch.Player2Wins;
            });

            float gwp = 100.0f* ((float)(gameWins) / (float)(gameWins+gameLosses));
            return gwp;
        }

        public float OGWP(int round=0)
        {
            var opponents = Opponents(round);
            float ogwp = (float)opponents.Average(o => o.GWP(round) > 33.33f ? o.GWP(round) : 33.33f);
            return ogwp;
        }
	}
}
