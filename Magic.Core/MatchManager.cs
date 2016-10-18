using Magic.Data;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public interface IMatchManager
	{
		void Update(Match m);
		void UpdateAllMatches(List<Match> matches, int round);
		List<PlayerScoreItem> GetPlayerStatistics(string playerName);
	}

	public class MatchManager : IMatchManager
	{
		private IMatchRepository _matchRepository;

		public MatchManager(IMatchRepository matchRepo)
		{
			_matchRepository = matchRepo;
		}
		
		public void Update(Match m)
		{
			_matchRepository.Update(m);
		}

		public void Save(Match m)
		{
			_matchRepository.Save(m);
		}

		public void UpdateAllMatches(List<Match> matches, int round)
		{
			IEnumerable<Match> relevantMatches;
      if (round==0)
			{
				relevantMatches = matches;
			}
			else
			{
				relevantMatches = matches.Where(m => m.Round == round);
			}

			_matchRepository.UpdateAllMatches(relevantMatches);
		}

		public List<PlayerScoreItem> GetPlayerStatistics(string playerName)
		{
			var matches = _matchRepository.GetAllMatches(playerName);

			List<PlayerScoreItem> playerScores = matches.GroupBy(m=>m.Player2).Select(lm=> 
				new PlayerScoreItem{
					Name = lm.Key,
					GameWins = lm.Sum(m=>m.Player1Wins),
					GameDraws = lm.Sum(m=>m.Draws),
					GameLosses = lm.Sum(m=>m.Player2Wins),
					MatchWins = lm.Count(m=>ParseMatchResult(m)==MatchResult.Win),
					MatchLosses = lm.Count(m => ParseMatchResult(m) == MatchResult.Loss),
					MatchDraws = lm.Count(m => ParseMatchResult(m) == MatchResult.Draw)
				}
			).ToList();

			var totalScores = new PlayerScoreItem
			{
				Name = "Total",
				GameWins = matches.Sum(m => m.Player1Wins),
				GameDraws = matches.Sum(m => m.Draws),
				GameLosses = matches.Sum(m => m.Player2Wins),
				MatchWins = matches.Count(m => ParseMatchResult(m) == MatchResult.Win),
				MatchLosses = matches.Count(m => ParseMatchResult(m) == MatchResult.Loss),
				MatchDraws = matches.Count(m => ParseMatchResult(m) == MatchResult.Draw)
			};

			playerScores.Add(totalScores);

			return playerScores;
		}

		private enum MatchResult
		{
			Win,
			Loss,
			Draw,
			Incomplete
		}

		private MatchResult ParseMatchResult(dbMatch m)
		{
			return ParseMatchResult(m.Player1Wins, m.Player2Wins, m.Draws);
		}

		private MatchResult ParseMatchResult(int p1wins, int p2wins, int draws)
		{
			if ((p1wins + draws >= 2) || (p2wins + draws >= 2)) // Match Complete
			{
				if (p1wins > p2wins)
					return MatchResult.Win;
				else if (p2wins > p1wins)
					return MatchResult.Loss;
				else
					return MatchResult.Draw;
			}
			else
			{
				return MatchResult.Incomplete; // Match incomplete
			}
		}
	}
}
