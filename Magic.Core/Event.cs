using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	using LocalSetup;
	using NUnit.Framework;
	using System.Diagnostics;

	[DebuggerDisplay("{name} r:{CurrentRound} p:{Players.Count} m:{Matches.Count}")]
	public class Event
	{
		public List<Player> Players = new List<Player>();
		public List<Match> Matches = new List<Match>();
		public string name = "";
		public int rounds = 1;
		public int CurrentRound = 1;
		public int RoundMatches = 4;
		public dbEvent myDbEvent = null;
		public DateTime RoundEndDate = DateTime.Today;
		public DateTime EventStartDate = DateTime.Today;
		private bool _locked = false;

		private readonly IDataContextWrapper _dataContext;

		public Event()
		{
			_dataContext = new DataContextWrapper(Constants.currentConnectionString);
    }

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

			//LoadPlayers
			var PlayerRepository = new PlayerRepository(_dataContext);
			Players = new List<Player>();
			PlayerRepository.LoadDBPlayers().Where(p => eventPlayers.Any(ep => ep.Player == p.Name)).ToList().ForEach(p => Players.Add(new Player(p)));

			//LoadEventPlayers
			eventPlayers.Where(ep => ep.Dropped > 0).ToList().ForEach(ep =>
			{
				foreach (var p in Players)
				{
					if (p.name == ep.Player)
						p.droppedInRound = ep.Dropped;
				}
			});

			//LoadMatches
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
			if(myDbEvent==null)
			{
				CreateEvent(saveMatches);
			}
			else
			{
				UpdateEvent(saveMatches);
			}
		}

		public void CreateEvent(bool saveMatches)
		{
			myDbEvent = new dbEvent()
			{
				Name = name,
				RoundMatches = RoundMatches,
				Rounds = rounds,
				CurrentRound = CurrentRound,
				RoundEndDate = RoundEndDate,
				StartDate = EventStartDate
			};

			myDbEvent.Create();
		}

		public void UpdateEvent(bool saveMatches)
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

			myDbEvent.Update();
		}


		public async Task UpdateAllMatches()
		{
			await Task.Run(()=> Matches.ForEach(m => m.Save()));
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

		public void AddPlayer(Player newPlayer)
		{
			var dbPlayer = new dbPlayer() { Name = newPlayer.name };
			myDbEvent.AddPlayer(dbPlayer);
		}
			
	}
}