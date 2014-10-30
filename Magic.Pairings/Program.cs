using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Magic.Core; 

namespace Magic.Pairings
{
	class Program
	{
		public static List<Player> LoadFile(string fileName)
		{
			var list = new List<Player>();

			using (var file = File.Open(fileName, FileMode.Open))
			{
				byte[] buffer = new byte[1024*1024];
				var bytesRead = file.Read(buffer, 0, 1024*1024);
				var readData = System.Text.Encoding.UTF8.GetString(buffer,0,bytesRead);
				var readDataLines = readData.Split(new string[]{Environment.NewLine},1024, StringSplitOptions.RemoveEmptyEntries);
				
				var matches = new List<Magic.Core.Match>();
				foreach (string dataLine in readDataLines)
				{
					var foundMatch = Magic.Core.Match.ReadFromSQLInsertString(dataLine);
					if(foundMatch.Player1.Length>=1)
						matches.Add(foundMatch);
				}

				foreach(Magic.Core.Match m in matches)
				{
					var foundPlayer = new Player(m.Player1, 0);
					var foundPlayer2 = new Player(m.Player2, 0);
					if(list.Count(p => p.name==foundPlayer.name)<=0)
						list.Add(foundPlayer);

					if (list.Count(p => p.name == foundPlayer2.name) <= 0)
						list.Add(foundPlayer2);
				}

				foreach (string dataLine in readDataLines)
				{
					AddMatch(list, dataLine);
				}
				
			}
			return list;
		}

		public static List<Player> LoadDatabase()
		{
			var outputPlayers = new List<Player>();
			
			var db = new DataContext("Magic");
			var playersTable = db.GetTable<dbPlayer>();

			var matches = new List<Magic.Core.Match>();
			var players = new List<Magic.Core.Player>();

			foreach (Magic.Core.Match m in matches)
			{
				var foundPlayer = new Player(m.Player1, 0);
				var foundPlayer2 = new Player(m.Player2, 0);
				if (players.Count(p => p.name == foundPlayer.name) <= 0)
					players.Add(foundPlayer);

				if (players.Count(p => p.name == foundPlayer2.name) <= 0)
					players.Add(foundPlayer2);
			}

			foreach (string dataLine in readDataLines)
			{
				AddMatch(list, dataLine);
			}

						

			var ignore = System.Console.ReadKey();

			return null;
		}

		static void AddMatch(List<Player> playerList, string matchInput)
		{
			var inputList = matchInput.Split(new char[] { ',' }, StringSplitOptions.None);
			var p1Name = inputList[0].Trim(new char[] { '\'', '(', ')', ' ' });
			var p2Name = inputList[1].Trim(new char[] { '\'', '(', ')', ' ' });
			var round = Convert.ToInt32(inputList[2]);
			var eventName = inputList[3].Trim(new char[] { '\'', '(', ')', ' ' });
			var p1score = Convert.ToInt32(inputList[4]);
			var p2score = Convert.ToInt32(inputList[5]);
			var draws = inputList[6];

			var player1 = playerList.First(player => player.name == p1Name);
			var player2 = playerList.First(player => player.name == p2Name);

			player1.round1Players.Add(player2);
			player2.round1Players.Add(player1);
			if (p1score > p2score)
				player1.Score++;
			else if (p2score > p1score)
				player2.Score++;
		}

		static void Main(string[] args)
		{
			var playerList = LoadFile("playerInputFile.txt");
			//var playerList = GetPlayerList();

			var playerListString = "";
			foreach (Player p in playerList)
			{
				playerListString += PlayerListInfoToString(p);
				playerListString += Environment.NewLine;
			}

			using (var file = File.Open("magicPlayerList.txt", FileMode.Create))
			{
				var outputByte = Encoding.Default.GetBytes(playerListString.ToCharArray());
				file.Write(outputByte, 0, outputByte.Length);
			}
			
			
			playerList = GeneratePairings(playerList, 4);

			var outputString = "";
			foreach (Player p in playerList)
			{
				outputString += PlayerPairingInfoToString(p);
				outputString += Environment.NewLine;
			}

			using (var file = File.Open("magicPairings.txt", FileMode.Create))
			{
				var outputByte = Encoding.Default.GetBytes(outputString.ToCharArray());
				file.Write(outputByte, 0, outputByte.Length);
			}



		}

		static private string PlayerPairingInfoToString(Player p)
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

		static private string PlayerListInfoToString(Player p)
		{
			String output = "";

			output += p.name;
			output += ": ";
			for (int i = 0; i < p.round1Players.Count; i++)
			{
				output += p.round1Players[i].name;
				if (i + 1 < p.round1Players.Count)
					output += ", ";
			}

			output += String.Format(" ({0})", p.Score);


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
