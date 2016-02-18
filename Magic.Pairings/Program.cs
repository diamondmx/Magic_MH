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
using Magic.Domain;

namespace Magic.Pairings
{
	class Program
	{
		public Program()
		{
		}

		static void Main(string[] args)
		{
			var dataContext = new Magic.Data.DataContextWrapper(Magic.Data.LocalSetup.Constants.currentConnectionString);
			var eventPlayerRepo = new Magic.Data.EventPlayerRepository(dataContext);
			var playerRepo = new Magic.Data.PlayerRepository(dataContext);
			var matchRepo = new Magic.Data.MatchRepository();
			var eventRepo = new Magic.Data.EventRepository(dataContext, eventPlayerRepo, matchRepo, playerRepo);
			var eventManager = new EventManager(eventRepo);
			var matchManager = new MatchManager(matchRepo);
			var pairingsManager = new PairingsManager(eventManager);

			var mainEvent = pairingsManager.LoadDatabase("OGW");

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

			pairingsManager.GeneratePairings(mainEvent);

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

			matchManager.UpdateAllMatches(mainEvent.Matches, mainEvent.CurrentRound);
		}

		static private string PlayerPairingInfoToString(Player p, int currentRound)
		{
			String output = "";

			output += p.name;
			output += ": ";
			var listOfOpponents = p.Opponents(currentRound);
			for (int i = 0; i < listOfOpponents.Count; i++)
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

			foreach (Magic.Domain.Match m in p.matches.OrderBy(m => m.Round).ThenBy(m => m.WithPlayerOneAs(p.name).Player2Name))
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

		
	}
}
