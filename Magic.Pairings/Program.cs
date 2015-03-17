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
		public static Event LoadDatabase(string eventName)
		{
            var newEvent = new Event();
            newEvent.LoadEvent(eventName);
			return newEvent;
		}
		
		static void Main(string[] args)
		{
			var mainEvent = LoadDatabase("FRF");

			var playerListString = "";
			foreach (Player p in mainEvent.Players)
			{
				playerListString += PlayerListInfoToString(p);
				playerListString += Environment.NewLine;
			}

			using (var file = File.Open("magicPlayerList.txt", FileMode.Create))
			{
				var outputByte = Encoding.Default.GetBytes(playerListString.ToCharArray());
				file.Write(outputByte, 0, outputByte.Length);
			}
			
			//GeneratePairings(mainEvent);

			var outputString = "";
			foreach (Player p in mainEvent.Players)
			{
				outputString += PlayerPairingInfoToString(p, mainEvent.CurrentRound);
				outputString += Environment.NewLine;
			}

			using (var file = File.Open("magicPairings.txt", FileMode.Create))
			{
				var outputByte = Encoding.Default.GetBytes(outputString.ToCharArray());
				file.Write(outputByte, 0, outputByte.Length);
			}

            mainEvent.SaveEvent();
		}

		static private string PlayerPairingInfoToString(Player p, int currentRound)
		{
			String output = "";

			output += p.name;
			output += ": ";
            var listOfOpponents = p.Opponents(currentRound);
			for(int i =0;i<listOfOpponents.Count;i++)
			{
                output += listOfOpponents[i].name;
                if (i + 1 < listOfOpponents.Count)
					output += ", ";
			}

			return output;
		}

		static private string PlayerListInfoToString(Player p)
        {
            string output = "";

            output += p.name + ": ";

            foreach(Magic.Core.Match m in p.matches.OrderBy(m=>m.Round).ThenBy(m=>m.WithPlayerOneAs(p.name).Player2Name))
            {
                var normalised = m.WithPlayerOneAs(p.name);
                output += normalised.Player2Name;
                output += ", ";
            }

            output += p.Score(0);
            if (p.droppedInRound > 0)
                output += "! Dropped in round " + p.droppedInRound;

            return output;
        }

		static private void GeneratePairings(Event mainEvent)
		{
            var inputPlayers = mainEvent.Players;
            var currentRound = mainEvent.CurrentRound;
            var rounds = mainEvent.rounds;
            var eventName = mainEvent.name;
            int activePlayers = inputPlayers.Count(p=>!p.HasDropped(currentRound));


			bool finished = false;
			
			while (!finished)
			{
				var matchesAtStart = inputPlayers.Sum(p => p.Opponents(currentRound).Count());
				if (matchesAtStart == activePlayers * mainEvent.RoundMatches)
                    return;

				// Modify player pairings

				var player = FindRandomPlayerWithFewestOpponents(inputPlayers, mainEvent.RoundMatches, currentRound);

				if (player == null)
					return;

				var matchedPlayer = FindBestMatch(player, inputPlayers, mainEvent.RoundMatches, currentRound);
				if (matchedPlayer == null)
					return;

                var newMatch = new Core.Match(player, player.name, matchedPlayer, matchedPlayer.name, eventName, currentRound, 0,0,0,false);
				player.matches.Add(newMatch);
				matchedPlayer.matches.Add(newMatch);
                mainEvent.Matches.Add(newMatch);

				// End modify player pairings

				var matchesAtEnd = inputPlayers.Sum(p => p.Opponents(round:currentRound).Count);

				if (matchesAtStart == matchesAtEnd)
				{
					finished = true;
				}

				if (matchesAtEnd == inputPlayers.Count*mainEvent.RoundMatches)
				{
					finished = true;
					return;
				}
			}
			return;
		}

		private static Player FindBestMatch(Player player, List<Player> inputPlayers, int matches, int currentRound)
		{
			Player selectedMatch = null;

            selectedMatch = FindMatchWithGivenScore(player, inputPlayers, 0, false, false, matches, currentRound);
			if (selectedMatch != null)
				return selectedMatch;
			
			
			for(var scoreRange=1;scoreRange<99;scoreRange++)
			{
                selectedMatch = FindMatchWithGivenScore(player, inputPlayers, scoreRange, false, false, matches, currentRound);
				if (selectedMatch != null)
					return selectedMatch;
			}

			for (var scoreRange = 1; scoreRange < 99; scoreRange++)
			{
                selectedMatch = FindMatchWithGivenScore(player, inputPlayers, scoreRange, true, false, matches, currentRound);
				if (selectedMatch != null)
					return selectedMatch;
			}

			for (var scoreRange = 1; scoreRange < 99; scoreRange++)
			{
                selectedMatch = FindMatchWithGivenScore(player, inputPlayers, scoreRange, true, true, matches, currentRound);
				if (selectedMatch != null)
					return selectedMatch;
			}

			return null;
		}

		private static Player FindMatchWithGivenScore(Player player, IEnumerable<Player> inputPlayers, int scoreRangeRelaxation, bool relaxRound1MatchRestriction, bool relaxRound2MatchRestriction, int matches, int currentRound)
		{
			var possiblePlayers = new List<Player>();
			foreach (var p in inputPlayers)
			{
				if (p == player)
					continue;

                if (p.HasDropped(currentRound))
                    continue;

				if (Math.Abs(p.Score(0) - player.Score(0)) > scoreRangeRelaxation)
					continue;

				if (p.Opponents(round:1).Contains(player) && (!relaxRound1MatchRestriction))
					continue;

                if (p.Opponents(round: 2).Contains(player) && (!relaxRound2MatchRestriction))
					continue;

				if (p.Opponents(round: currentRound).Contains(player))
					continue;

				if (p.Opponents(round: currentRound).Count >= matches)
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

        private static Player FindPlayerWithHighestScoreFewestOpponents(List<Player> players, int rounds, int currentRound)
		{
			var selectedPlayer = players[0];
			
			foreach (var player in players)
			{
				if (selectedPlayer == player)
					continue;

                if (selectedPlayer.HasDropped(currentRound))
                    continue;

				if (selectedPlayer.Opponents(round:currentRound).Count >= rounds)
					selectedPlayer = player;

				if (player.Opponents(round:currentRound).Count < selectedPlayer.Opponents(round:currentRound).Count)
				{
					selectedPlayer = player;
				}
				else if (player.Opponents(round:currentRound).Count == selectedPlayer.Opponents(round:currentRound).Count)
				{
					if (player.Score() < selectedPlayer.Score())
						selectedPlayer = player;
				}
			}

			return selectedPlayer.Opponents(round:currentRound).Count >= rounds ? null : selectedPlayer;
		}

		private static Player FindRandomPlayerWithFewestOpponents(List<Player> players, int matches, int currentRound)
		{
			var fewestOpponents = 99;
			var selectedPlayers = new List<Player> {};


			foreach (var player in players)
			{
				if (selectedPlayers.Contains(player))
					continue;

                if (player.HasDropped(currentRound))
                    continue;

                if (player.Opponents(currentRound).Count >= matches)
					continue;

				if (player.Opponents(currentRound).Count == fewestOpponents)
				{
					selectedPlayers.Add(player);
				}
                else if (player.Opponents(currentRound).Count < fewestOpponents)
				{
                    fewestOpponents = player.Opponents(currentRound).Count;
					selectedPlayers.Clear();
					selectedPlayers.Add(player);
				}
			}

            if (selectedPlayers.Count > 0 && fewestOpponents < matches)
				return selectedPlayers[new Random().Next()%selectedPlayers.Count()];
			else
				return null;
		}
	}
}
