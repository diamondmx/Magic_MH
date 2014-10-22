using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Pairings
{
	class Player
	{
		public string name;
		public List<Player> round1Players;
		public List<Player> round2Players; 
		public List<Player> CurrentPlayers;
		public int Score;

		public Player(string newName, int newScore)
		{
			name = newName;
			Score = newScore;
			round1Players = new List<Player>();
			round2Players = new List<Player>();
			CurrentPlayers = new List<Player>();
		}

	}

	class Program
	{
		static void Main(string[] args)
		{
			var playerList = GetPlayerList();
			playerList = GeneratePairings(playerList, 4);

			var outputString = "";
			foreach (Player p in playerList)
			{
				outputString += PlayerInfoToString(p);
				outputString += Environment.NewLine;
			}

			using (var file = File.Open("magicPairings.txt", FileMode.Create))
			{
				var outputByte = Encoding.Default.GetBytes(outputString.ToCharArray());
				file.Write(outputByte, 0, outputByte.Length);
			}



		}

		static private string PlayerInfoToString(Player p)
		{
			String output = "";

			output += p.name;
			output += ": ";
			for(int i =0;i<p.CurrentPlayers.Count;i++)
			{
				output += p.CurrentPlayers[i].name;
				if(i+1<p.CurrentPlayers.Count)
					output += ", ";
			}

			return output;
		}

		static private List<Player> GetPlayerList()
		{
			var aaron = new Player("Aaron", 0);
			var adam = new Player("Adam", 0);
			var alex = new Player("Alex", 0);
			var artem = new Player("Artem", 0);
			var cbLadd = new Player("CB Ladd", 0);
			var charlie = new Player("Charlie", 0);
			var david = new Player("David", 0);
			var elise = new Player("Elise", 0);
			var jeffrey = new Player("Jeffrey", 0);
			var john = new Player("John", 0);
			var markH = new Player("Mark H", 0);
			var markR = new Player("Mark R", 0);
			var michael = new Player("Michael", 0);
			var nicholas = new Player("Nicholas", 0);
			var nikita = new Player("Nikita", 0);
			var pete = new Player("Pete", 0);
			var sean = new Player("Sean", 0);
			var erik = new Player("Erik", 0);
			
			return new List<Player>{aaron, adam, alex, artem, cbLadd, charlie,david,elise,jeffrey,john,markH,markR,michael,nicholas,nikita,pete,sean,erik};
		}

		static private List<Player> GeneratePairings(List<Player> inputPlayers, int rounds)
		{
			var outputPlayers = inputPlayers;

			bool finished = false;
			
			while (!finished)
			{
				var matchesAtStart = inputPlayers.Sum(p => p.CurrentPlayers.Count);
				if (matchesAtStart == inputPlayers.Count * rounds)
					return inputPlayers;

				// Modify player pairings

				//var player = FindPlayerWithHighestScoreFewestOpponents(inputPlayers, rounds);
				var player = FindRandomPlayerWithFewestOpponents(inputPlayers, rounds);

				if (player == null)
					return inputPlayers;

				var matchedPlayer = FindBestMatch(player, inputPlayers, rounds);
				if (matchedPlayer == null)
					return inputPlayers;

				player.CurrentPlayers.Add(matchedPlayer);
				matchedPlayer.CurrentPlayers.Add(player);

				// End modify player pairings

				var matchesAtEnd = inputPlayers.Sum(p => p.CurrentPlayers.Count);

				if (matchesAtStart == matchesAtEnd)
				{
					finished = true;					
				}

				if (matchesAtEnd == inputPlayers.Count*rounds)
				{
					finished = true;
					return inputPlayers;
				}
			}
			return inputPlayers;
		}

		private static Player FindBestMatch(Player player, List<Player> inputPlayers, int rounds)
		{
			Player selectedMatch = null;

			selectedMatch = FindMatchWithGivenScore(player, inputPlayers, 0, false, false, rounds);
			if (selectedMatch != null)
				return selectedMatch;
			
			
			for(var scoreRange=1;scoreRange<99;scoreRange++)
			{
				selectedMatch = FindMatchWithGivenScore(player, inputPlayers, scoreRange, false, false, rounds);
				if (selectedMatch != null)
					return selectedMatch;
			}

			for (var scoreRange = 1; scoreRange < 99; scoreRange++)
			{
				selectedMatch = FindMatchWithGivenScore(player, inputPlayers, scoreRange, true, false, rounds);
				if (selectedMatch != null)
					return selectedMatch;
			}

			for (var scoreRange = 1; scoreRange < 99; scoreRange++)
			{
				selectedMatch = FindMatchWithGivenScore(player, inputPlayers, scoreRange, true, true, rounds);
				if (selectedMatch != null)
					return selectedMatch;
			}

			return null;
		}

		private static Player FindMatchWithGivenScore(Player player, IEnumerable<Player> inputPlayers, int scoreRangeRelaxation, bool relaxRound1MatchRestriction, bool relaxRound2MatchRestriction, int rounds)
		{
			var possiblePlayers = new List<Player>();
			foreach (var p in inputPlayers)
			{
				if (p == player)
					continue;

				if (Math.Abs(p.Score - player.Score) > scoreRangeRelaxation)
					continue;

				if (p.round1Players.Contains(player) && (!relaxRound1MatchRestriction))
					continue;

				if (p.round2Players.Contains(player) && (!relaxRound2MatchRestriction))
					continue;

				if (p.CurrentPlayers.Contains(player))
					continue;

				if (p.CurrentPlayers.Count >= rounds)
					continue;

				possiblePlayers.Add(p);
			}

			if (possiblePlayers.Count == 1)
				return possiblePlayers[0];
			else if (possiblePlayers.Count > 1)
			{
				var rand = new Random();
				var selected = rand.Next() % (possiblePlayers.Count);
				return possiblePlayers[selected];
			}
			else
			{
				return null;
			}

		}

		private static Player FindPlayerWithHighestScoreFewestOpponents(List<Player> players, int rounds)
		{
			var selectedPlayer = players[0];
			
			foreach (var player in players)
			{
				if (selectedPlayer == player)
					continue;

				if (selectedPlayer.CurrentPlayers.Count >= rounds)
					selectedPlayer = player;

				if (player.CurrentPlayers.Count < selectedPlayer.CurrentPlayers.Count)
				{
					selectedPlayer = player;
				}
				else if (player.CurrentPlayers.Count == selectedPlayer.CurrentPlayers.Count)
				{
					if (player.Score < selectedPlayer.Score)
						selectedPlayer = player;
				}
			}

			return selectedPlayer.CurrentPlayers.Count >= rounds ? null : selectedPlayer;
		}

		private static Player FindRandomPlayerWithFewestOpponents(List<Player> players, int rounds)
		{
			var fewestOpponents = 99;
			var selectedPlayers = new List<Player> {};


			foreach (var player in players)
			{
				if (selectedPlayers.Contains(player))
					continue;

				if (player.CurrentPlayers.Count >= rounds)
					continue;

				if (player.CurrentPlayers.Count == fewestOpponents)
				{
					selectedPlayers.Add(player);
				}
				else if (player.CurrentPlayers.Count < fewestOpponents)
				{
					fewestOpponents = player.CurrentPlayers.Count;
					selectedPlayers.Clear();
					selectedPlayers.Add(player);
				}
			}

			if (selectedPlayers.Count > 0 && fewestOpponents <rounds)
				return selectedPlayers[new Random().Next()%selectedPlayers.Count()];
			else
				return null;
		}
	}
}
