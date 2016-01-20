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
		private bool _locked;

		public List<Event> LoadAllEvents()
		{
			List<Event> results = new List<Event>();

			var events = dbEvent.LoadAllDBEvents();
			foreach(var item in events)
			{
				var loadedEvent = new Event();
				loadedEvent.PopulateEvent(item);
				results.Add(loadedEvent);
			}

			return results;
		}

		public void PopulateEvent(dbEvent loadedEvent)
		{
			name = loadedEvent.Name;
			myDbEvent = loadedEvent;
			rounds = loadedEvent.Rounds;
			CurrentRound = loadedEvent.CurrentRound;
			RoundMatches = loadedEvent.RoundMatches;
			EventStartDate = loadedEvent.StartDate;
			RoundEndDate = loadedEvent.RoundEndDate;

			var eventPlayers = dbEventPlayers.LoadDBEventPlayers(loadedEvent.Name);

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
			dbMatch.LoadDBMatches(name).Where(m => m.Event == loadedEvent.Name).ToList().ForEach(m => Matches.Add(new Match(m)));

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

		public void LoadEvent(string eventName)
		{
			var loadedEvent = dbEvent.LoadDBEvent(eventName);
			PopulateEvent(loadedEvent);
		}

		public void SaveEvent(bool saveMatches=true)
		{
			if (myDbEvent.Name != name)
			{
				myDbEvent.Name = name;
				ApplyNameChange();
				saveMatches = true;
			}

			myDbEvent.RoundMatches = RoundMatches;
			myDbEvent.Rounds = rounds;
			myDbEvent.CurrentRound = CurrentRound;
			myDbEvent.RoundEndDate = RoundEndDate;
			myDbEvent.StartDate = EventStartDate;

			if (saveMatches)
			{
				UpdateAllMatches();
			}

			myDbEvent.Save();
		}

		private async Task UpdateAllMatches()
		{
			Matches.ForEach(m => m.Save());
		}

		private void ApplyNameChange()
		{
			Matches.ForEach(m => m.Event = name);
		}

		public bool Locked(int round)
		{
			if (_locked)
				return true;
			if (round < CurrentRound)
				return true;

			return false;
		}

        public int RoundMatchCount(int round)
        {
            if (round > 0)
            {
                return Players.Max(p => p.matches.Count(m => m.Round == round));
            }
            else
            {
                return Players.Max(p => p.matches.Count());
            }
        }

        public int TotalMatches(int round)
        {
            if(round>0)
            {
                return Matches.Count(m => m.Round == round);
            }
            else
            {
                return Matches.Count();
            }
                
                    
        }

        public int TotalMatchesPlayed(int round)
        {
            return Players.Sum(p => p.matchesCompleted(round));
        }
	}
}