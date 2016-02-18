using Magic.Data.LocalSetup;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Magic.Data
{
	public class MatchRepository : IMatchRepository
	{
		public MatchRepository()
		{

		}

		public List<Match> LoadDBMatches(string mtgEvent)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var matchesTable = db.GetTable<dbMatch>().Where(m => m.Event == mtgEvent).ToList();
			var results = matchesTable.Select(dbm => new Match(dbm));
			return results.ToList();
		}

		public void Update(Match m)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			db.ExecuteQuery<dbMatch>($"UPDATE [Matches] SET [Player1Wins]={m.Player1Wins}, [Player2Wins]={m.Player2Wins}, [Draws]={m.Draws} WHERE [Player1]='{m.Player1Name}' AND [Player2]='{m.Player2Name}' AND [Event]='{m.Event}' AND [Round]={m.Round}");
			db.ExecuteQuery<dbMatch>($"UPDATE [Matches] SET [Player2Wins]={m.Player1Wins}, [Player1Wins]={m.Player2Wins}, [Draws]={m.Draws} WHERE [Player2]='{m.Player1Name}' AND [Player1]='{m.Player2Name}' AND [Event]='{m.Event}' AND [Round]={m.Round}");
		}

		public void Insert(Match m)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var sql = $"INSERT INTO [Matches](Event,Round,Player1,Player2,Player1Wins,Player2Wins,Draws) VALUES('{m.Event}',{m.Round},'{m.Player1Name}','{m.Player2Name}',{m.Player1Wins},{m.Player2Wins},{m.Draws})";
			db.ExecuteQuery<dbMatch>(sql);
		}

		public void Save(Match m)
		{
			if (MatchExists(m.Event, m.Round, m.Player1Name, m.Player2Name))
				Update(m);
			else
				Insert(m);
		}

		public bool IsMatch(Match checkedMatch, string player1name, string player2name, string eventname, int round)
		{
			if (eventname == checkedMatch.Event && round == checkedMatch.Round)
			{
				if (player1name == checkedMatch.Player1Name && player2name == checkedMatch.Player2Name)
					return true;
				else if (player1name == checkedMatch.Player2Name && player2name == checkedMatch.Player1Name)
					return true;
			}

			return true;
		}

		public bool MatchExists(string eventName, int round, string p1, string p2)
		{
			var allMatches = LoadDBMatches(eventName);
			var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
			var roundMatches = eventMatches.Where(m => m.Round == round).ToList();

			var matchesAsP1 = roundMatches.Where(m => m.Player1Name == p1 && m.Player2Name == p2);
			if (matchesAsP1.Any())
			{
				return true;
			}
			else
			{
				var matchesAsP2 = roundMatches.Where(m => m.Player1Name == p2 && m.Player2Name == p1);
				if (matchesAsP2.Any())
				{
					return true;
				}
			}
			return false;
		}

		public Match Read(string eventName, int round, string p1, string p2)
		{
			var allMatches = LoadDBMatches(eventName);
			var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
			var roundMatches = eventMatches.Where(m => m.Round == round).ToList();

			var matchesAsP1 = roundMatches.Where(m => m.Player1Name == p1 && m.Player2Name == p2);
			if (matchesAsP1.Any())
			{
				var foundMatch = matchesAsP1.First();
				return foundMatch;

			}
			else
			{
				var matchesAsP2 = roundMatches.Where(m => m.Player1Name == p2 && m.Player2Name == p1);
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
	}
}
