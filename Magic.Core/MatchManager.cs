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
		void UpdatePairing(Match m);
		void UpdateAllMatches(List<Match> matches, int round);
		PlayerScoreSummary GetPlayerStatistics(int playerID);
		void Save(Match m);
		void Delete(Match m);
		int GetMatchCountInRound(string eventName, int round);
		void DeleteAllInRound(string eventName, int round);
	}

	public class MatchManager : IMatchManager
	{
		private IMatchRepository _matchRepository;
		private IEventRepository _eventRepository;
		private IPlayerRepository _playerRepository;

		public MatchManager(IMatchRepository matchRepo, IEventRepository eventRepo, IPlayerRepository playerRepo)
		{
			_matchRepository = matchRepo;
			_eventRepository = eventRepo;
			_playerRepository = playerRepo;
		}
		
		public void Update(Match m)
		{
			_matchRepository.Update(m);
		}

		public void UpdatePairing(Match m)
		{
			_matchRepository.UpdatePairing(m);
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

		public PlayerScoreSummary GetPlayerStatistics(int playerID)
		{
			var matches = _matchRepository.GetAllMatches(playerID);

			List<PlayerScoreItem> playerScores = matches.GroupBy(m=>m.Player2ID).Select(lm=> 
				new PlayerScoreItem{
					PlayerID = lm.Key,
					Name = _playerRepository.GetPlayerName(lm.Key),
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
				PlayerID = 0,
				Name = "Total",
				GameWins = matches.Sum(m => m.Player1Wins),
				GameDraws = matches.Sum(m => m.Draws),
				GameLosses = matches.Sum(m => m.Player2Wins),
				MatchWins = matches.Count(m => ParseMatchResult(m) == MatchResult.Win),
				MatchLosses = matches.Count(m => ParseMatchResult(m) == MatchResult.Loss),
				MatchDraws = matches.Count(m => ParseMatchResult(m) == MatchResult.Draw)
			};

			var topEightGroups = matches.Where(m => m.Round == 4).GroupBy(m => m.Event);
			var wins = 0;
			foreach (var group in topEightGroups)
			{
				if (group.All(m => m.HasWon(playerID)))
					wins++;
			}

			var leaguesUndefeatedSwissMatches = 0;
			var events = _eventRepository.LoadAllDBEvents();
			var leagueGroups = matches.Where(m => ((m.Round==1||m.Round==2|m.Round==3) && (m.Player1ID == playerID || m.Player2ID == playerID))).GroupBy(m => m.Event);
			foreach(var group in leagueGroups)
			{
				if(!group.All(m=>m.Event == group.First().Event))
				{
					throw new Exception("Group matches don't all meet expectations");
				}

				var thisEvent = events.Single(e => e.Name == group.First().Event);
				var swissRounds = thisEvent.RoundMatches * thisEvent.Rounds;

				if(group.All(m=>m.HasWon(playerID)))
				{
					leaguesUndefeatedSwissMatches += 1;
				}

			}

			var summary = new PlayerScoreSummary
			{
				OpponentScoreItems = playerScores,
				Totals = totalScores,
				LeagueTopEights = topEightGroups.Count(),
				LeagueWins = wins,
				LeaguesUndefeatedInMatches = leaguesUndefeatedSwissMatches
			};

			return summary;
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

		public void Delete(Match m)
		{
			_matchRepository.Delete(m);
		}

		public int GetMatchCountInRound(string eventName, int round)
		{
			return _matchRepository.GetMatchCountInRound(eventName, round);
		}

		public void DeleteAllInRound(string eventName, int round)
		{
			_matchRepository.DeleteAllInRound(eventName, round);
		}
	}
}
