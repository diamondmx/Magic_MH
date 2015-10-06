using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	[DebuggerDisplay("{name} m:{matches.Count} s:{Score(0)}")]
	public class Player
	{
		public string name;
		public List<Match> matches;
		public int? droppedInRound;
		public dbPlayer myDbPlayer = null;

		public Player(string newName)
		{
			name = newName;
			matches = new List<Match>();
		}

		public Player(dbPlayer p)
		: this(p.Name)
		{
			myDbPlayer = p;
		}

		public void SavePlayer()
		{
			if (myDbPlayer != null)
				myDbPlayer.Save();
		}

		public int matchesCompleted(int round)
		{
			return GetRelevantMatches(round).Count(m => m.Player1Wins >= 2 || m.Player2Wins >= 2);
		}

		public int Score(int round = 0)
		{
			List<Match> relevantMatches = null;

			if(round<=0)
			{
				relevantMatches = matches;
			}
			else
			{
				relevantMatches = matches.Where(m => m.Round == round).ToList();
			}

			return relevantMatches.Sum(m =>
			{
				var nm = m.WithPlayerOneAs(name);
				if (nm.Player1Wins > nm.Player2Wins)
					return 3;
				else if (nm.Player1Wins == nm.Player2Wins)
					return 1;
				else
					return 0;
			});
		}

		public float MWP(int round = 0)
		{
			//List<Match> relevantMatches = null;
			//if (round == 0)
			//{
			//	relevantMatches = matches;
			//}
			//else
			//{
			//	relevantMatches = matches.Where(m => m.Round == round).ToList();
			//}

			//float matchWins = relevantMatches.Count(m => m.DidPlayerWin(name));
			//return (matchWins / (float)relevantMatches.Count()) * 100;
			var relevantMatches = GetRelevantMatches(round);


			var roundScore = Score(round);
			var maxScore = (relevantMatches.Count())*3;
			return ((float)roundScore / (float)maxScore)*100.0f;

		}

		public List<Match> GetRelevantMatches(int round)
		{
			List<Match> relevantMatches = matches;
			if (round <= 0)
			{
				relevantMatches = matches;
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

			return relevantMatches.Select<Match, Player>(m => (m.Player1.name == name) ? m.Player2 : m.Player1).ToList();
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
				var normalisedMatch = m.WithPlayerOneAs(name);
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
			if (droppedInRound > 0 && currentRound > droppedInRound)
				return true;

			return false;
		}
	}
}
