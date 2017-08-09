using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Magic.Data
{
	public class MatchRepository : IMatchRepository
	{
		private readonly IDataContextWrapper _context;
		private readonly IGameLog _gameLog;

		public MatchRepository(IDataContextWrapper context, IGameLog gameLog)
		{
			_context = context;
			_gameLog = gameLog;
		}

		public List<Match> LoadDBMatches(string mtgEvent)
		{
			var matchesTable = _context.GetTable<dbMatch>().ToList();
			var matchesTable2 = matchesTable.Where(m => m.Event == mtgEvent).ToList();
			var results = matchesTable.Select(dbm => new Match(dbm));
			return results.ToList();
		}

		public void Update(Match m)
		{ 
			_gameLog.Add($"Match updated to : {m.Player1.Name}({m.Player1.ID}) v {m.Player2.Name}({m.Player2.ID}) : {m.Player1Wins}-{m.Player2Wins}-{m.Draws}", $"{m.Event}", m);

      _context.ExecuteQuery<dbMatch>($"UPDATE [Matches] SET [Player1Wins]={m.Player1Wins}, [Player2Wins]={m.Player2Wins}, [Draws]={m.Draws} WHERE [Player1ID]='{m.Player1.ID}' AND [Player2ID]='{m.Player2.ID}' AND [Event]='{m.Event}' AND [Round]={m.Round}");
			_context.ExecuteQuery<dbMatch>($"UPDATE [Matches] SET [Player2Wins]={m.Player1Wins}, [Player1Wins]={m.Player2Wins}, [Draws]={m.Draws} WHERE [Player2ID]='{m.Player1.ID}' AND [Player1ID]='{m.Player2.ID}' AND [Event]='{m.Event}' AND [Round]={m.Round}");
		}

		public void Insert(Match m)
		{
			var sql = $"INSERT INTO [Matches](Event,Round,Player1ID, Player2ID, Player1,Player2,Player1Wins,Player2Wins,Draws) VALUES('{m.Event}',{m.Round},{m.Player1ID}, {m.Player2ID}, '{m.Player1.Name}','{m.Player2.Name}',{m.Player1Wins},{m.Player2Wins},{m.Draws})";
			_context.ExecuteQuery<dbMatch>(sql);
		}

		public void Save(Match m)
		{
			if (MatchExists(m.Event, m.Round, m.Player1ID, m.Player2ID))
				Update(m);
			else
				Insert(m);
		}

		public bool IsMatch(Match checkedMatch, int player1ID, int player2ID, string eventname, int round)
		{
			if (eventname == checkedMatch.Event && round == checkedMatch.Round)
			{
				if (player1ID == checkedMatch.Player1.ID && player2ID == checkedMatch.Player2.ID)
					return true;
				else if (player1ID == checkedMatch.Player2.ID && player2ID == checkedMatch.Player1.ID)
					return true;
			}

			return true;
		}

		public bool MatchExists(string eventName, int round, int p1, int p2)
		{
			var allMatches = LoadDBMatches(eventName);
			var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
			var roundMatches = eventMatches.Where(m => m.Round == round).ToList();

			var matchesAsP1 = roundMatches.Where(m => m.Player1ID == p1 && m.Player2ID== p2);
			if (matchesAsP1.Any())
			{
				return true;
			}
			else
			{
				var matchesAsP2 = roundMatches.Where(m => m.Player1ID == p2 && m.Player2ID == p1);
				if (matchesAsP2.Any())
				{
					return true;
				}
			}
			return false;
		}

		public Match Read(string eventName, int round, int p1ID, int p2ID)
		{
			var allMatches = LoadDBMatches(eventName);
			var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
			var roundMatches = eventMatches.Where(m => m.Round == round).ToList();

			var matchesAsP1 = roundMatches.Where(m => m.Player1.ID == p1ID && m.Player2.ID== p2ID);
			if (matchesAsP1.Any())
			{
				var foundMatch = matchesAsP1.First();
				return foundMatch;

			}
			else
			{
				var matchesAsP2 = roundMatches.Where(m => m.Player1.ID == p2ID && m.Player2.ID == p1ID);
				if (matchesAsP2.Any())
				{
					var foundMatch = matchesAsP2.First();
					return foundMatch;
				}
			}

			return null;
		}

		public void UpdateAllMatches(IEnumerable<Match> matches)
		{
			matches.ToList().ForEach(m => Save(m));
		}

		public List<dbMatch> GetAllMatches(int playerID)
		{
			var matchesTable = _context.GetTable<dbMatch>().Where(m => m.Player1ID == playerID || m.Player2ID == playerID).ToList();
			var results = matchesTable.Select(m => m.WithPlayerOneAs(playerID));
			return results.ToList();
		}

		public void PopulateMatch(List<Player> players, Match m)
		{
			m.Player1 = players.FirstOrDefault(p => p.ID == m.Player1ID);
			m.Player2 = players.FirstOrDefault(p => p.ID == m.Player2ID);

			if (m.Player1 == null || m.Player2 == null)
				throw new Exception("Player in match not found");
		}
	}
}
