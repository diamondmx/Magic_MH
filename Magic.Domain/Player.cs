using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[DebuggerDisplay("{Name} m:{Matches.Count} s:{Score(0)}")]
	public class Player
	{
		public int ID;
		public string Name;
		public List<Match> Matches;
		public int? DroppedInRound;
		public string Email;

		public Player(dbPlayer p)
		{
			ID = p.ID;
			Name = p.Name;
			Matches = new List<Match>();
			DroppedInRound = 0;
			Email = p.Email;
		}

		public Player(string newName, string email, int id)
		{
			ID = id;
			Name = newName;
			Email = email;
			Matches = new List<Match>();
		}

		public int matchesCompleted(int round)
		{
			return GetRelevantMatches(round).Count(m => m.Player1Wins >= 2 || m.Player2Wins >= 2 || m.Draws >= 3 ||((m.Player1Wins >= 1 || m.Player2Wins >=1) && m.Draws >=1));
		}

		public int Score(int round = 0)
		{
			List<Match> relevantMatches = null;

			if(round<=0)
			{
				relevantMatches = Matches;
			}
			else
			{
				relevantMatches = Matches.Where(m => m.Round == round).ToList();
			}

            return relevantMatches.Sum(m =>
            {
                var nm = m.WithPlayerOneAs(ID);
                return MatchScore(nm.Player1Wins, nm.Player2Wins, nm.Draws);
            });
        }

    public int MatchScore(int player1Wins, int player2Wins, int draws)
    {
        if ((player1Wins + draws >=2) || (player2Wins + draws>=2)) // Match Complete
        {
            if (player1Wins > player2Wins)
                return 3;
            else if (player2Wins > player1Wins)
                return 0;
            else
                return 1;
        }
        else
        {
            return 0; // Match incomplete
        }

    }

		public float MWP(int round = 0)
		{
			var relevantMatches = GetRelevantMatches(round);


			var roundScore = Score(round);
			var maxScore = (relevantMatches.Count())*3;
			return ((float)roundScore / (float)maxScore)*100.0f;

		}

		public List<Match> GetRelevantMatches(int round)
		{
			List<Match> relevantMatches = Matches;
			if (round <= 0)
			{
				relevantMatches = Matches;
			}
			else
			{
				relevantMatches = relevantMatches.Where(m => m.Round == round).ToList();
			}

			return relevantMatches;
		}

		public List<Player> Opponents(int round = 0)
		{
			var relevantMatches = GetRelevantMatches(round);

			return relevantMatches.Select<Match, Player>(m => (m.Player1.ID == ID) ? m.Player2 : m.Player1).ToList();
		}

		public float OMWP(int round = 0)
		{
			var opponents = Opponents(round);
			if (opponents.Count <= 0)
				return 33.33f;

			float omwp = (float)opponents.Average(o => o.MWP(round) > 33.33f ? o.MWP(round) : 33.33f);
			return omwp;
		}

		public float GWP(int round = 0)
		{
			int gameWins = 0;
			int gameLosses = 0;

			var relevantMatches = GetRelevantMatches(round);

			relevantMatches.ForEach(m =>
			{
				var normalisedMatch = m.WithPlayerOneAs(ID);
				gameWins += normalisedMatch.Player1Wins;
				gameLosses += normalisedMatch.Player2Wins;
			});
			if (gameLosses == 0 && gameWins == 0)
			{
				return 0.0f;
			}
			else
			{
				float gwp = 100.0f * ((float)(gameWins) / (float)(gameWins + gameLosses));
				return gwp;
			}
		}

		public float OGWP(int round = 0)
		{
			var opponents = Opponents(round);
			if (opponents.Count() <= 0) return 33.33f;
			float ogwp = (float)opponents.Average(o => o.GWP(round) > 33.33f ? o.GWP(round) : 33.33f);
			return ogwp;
		}

		public bool HasDropped(int currentRound)
		{
			if (DroppedInRound > 0 && currentRound > DroppedInRound)
				return true;

			return false;
		}
	}
}
