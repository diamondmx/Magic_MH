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

		public static List<Player> FromMatchList(List<Match> matches, string eventName)
		{
			var players = new List<Player>();

			foreach (Magic.Core.Match m in matches)
			{
				var foundPlayer = new Player(m.Player1.name, 0);
				var foundPlayer2 = new Player(m.Player2.name, 0);
				if (players.Count(p => p.name == foundPlayer.name) <= 0)
					players.Add(foundPlayer);

				if (players.Count(p => p.name == foundPlayer2.name) <= 0)
					players.Add(foundPlayer2);
			}

			foreach (Magic.Core.Match m in matches)
			{
				AddMatch(players, m);
			}

			return players;
		}

		private	static void AddMatch(List<Player> playerList, Magic.Core.Match m)
		{
			var player1 = playerList.First(player => player.name == m.Player1.name);
			var player2 = playerList.First(player => player.name == m.Player2.name);
            player1.matches.Add(m);
            player2.matches.Add(m);

			switch (m.Round)
			{
				case 1:
					player1.round1Players.Add(player2);
					player2.round1Players.Add(player1);
					break;
				case 2:
					player1.round2Players.Add(player2);
					player2.round2Players.Add(player1);
					break;
				case 3:
					player1.round3Players.Add(player2);
					player2.round3Players.Add(player1);
					break;
			}

			if (m.Player1Wins > m.Player2Wins)
				player1.Score++;
			else if (m.Player2Wins > m.Player1Wins)
				player2.Score++;
		}



	}
}
