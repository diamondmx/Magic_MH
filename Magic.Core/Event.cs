using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	using NUnit.Framework;
	using System.Diagnostics;

	[DebuggerDisplay("{name} r:{CurrentRound} p:{Players.Count} m:{Matches.Count}")]
	public class Event
	{
		public List<Core.Player> Players;
		public List<Core.Match> Matches;
		public string name;
		public int rounds;
		public int CurrentRound;
		public int RoundMatches;
		public dbEvent myDbEvent;
		public DateTime RoundEndDate;
		public DateTime EventStartDate;


		public void LoadEvent(string eventName)
		{
			name = eventName;

			var loadedEvent = dbEvent.LoadDBEvent(eventName);
			myDbEvent = loadedEvent;
			rounds = loadedEvent.rounds;
			CurrentRound = loadedEvent.currentRound;
			RoundMatches = loadedEvent.roundMatches;
			RoundEndDate = loadedEvent.RoundEndDate;

			var eventPlayers = dbEventPlayers.LoadDBEventPlayers(eventName);

			Players = new List<Player>();
			dbPlayer.LoadDBPlayers().Where(p => eventPlayers.Any(ep => ep.Player == p.Name)).ToList().ForEach(p => Players.Add(new Player(p)));
			eventPlayers.Where(ep => ep.Dropped > 0).ToList().ForEach(ep =>
			{
				foreach (var p in Players)
				{
					if (p.name == ep.Player)
						p.droppedInRound = ep.Dropped;
				}
			});

			Matches = new List<Match>();
			dbMatch.LoadDBMatches(name).Where(m => m.Event == eventName).ToList().ForEach(m => Matches.Add(new Match(m)));

			foreach (var match in Matches)
			{
				if (String.IsNullOrEmpty((match.Player1Name)))
					throw new Exception();

				if (String.IsNullOrEmpty((match.Player2Name)))
					throw new Exception();
			}

			foreach (var p in Players)
			{
				foreach (var m in Matches)
				{
					if (m.Player1Name == p.name)
					{
						m.Player1 = p;
						p.matches.Add(m);
					}

					else if (m.Player2Name == p.name)
					{
						m.Player2 = p;
						p.matches.Add(m);
					}
				}
			}
		}

		public void SaveEvent()
		{
			myDbEvent.Name = name;
			myDbEvent.roundMatches = RoundMatches;
			myDbEvent.rounds = rounds;
			myDbEvent.currentRound = CurrentRound;

			Matches.ForEach(m => m.Save());

			myDbEvent.Save();
		}
	}
}