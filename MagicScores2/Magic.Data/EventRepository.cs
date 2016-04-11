using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public class EventRepository : IEventRepository
	{
		private readonly IDataContextWrapper _dataContext;
		private readonly IEventPlayerRepository _eventPlayerRepository;
		private readonly IMatchRepository _matchRepository;
		private readonly IPlayerRepository _playerRepository;

		public EventRepository(IDataContextWrapper dataContext, IEventPlayerRepository eventPlayerRepo, IMatchRepository matchRepo, IPlayerRepository playerRepo)
		{
			_dataContext = dataContext;
			_eventPlayerRepository = eventPlayerRepo;
			_matchRepository = matchRepo;
			_playerRepository = playerRepo;
		}

		public List<Event> LoadAllEvents()
		{
			List<Event> results = new List<Event>();

			var events = LoadAllDBEvents();
			foreach (var item in events)
			{
				var loadedEvent = new Event();
				PopulateEvent(loadedEvent, item);
				results.Add(loadedEvent);
			}

			return results;
		}

		public void PopulateEvent(Event pop, dbEvent loadedEvent)
		{
			pop.name = loadedEvent.Name;
			pop.myDbEvent = loadedEvent;
			pop.rounds = loadedEvent.Rounds;
			pop.CurrentRound = loadedEvent.CurrentRound;
			pop.RoundMatches = loadedEvent.RoundMatches;
			pop.EventStartDate = loadedEvent.StartDate;
			pop.RoundEndDate = loadedEvent.RoundEndDate;

			var eventPlayers = _eventPlayerRepository.LoadDBEventPlayers(loadedEvent.Name);

			//LoadPlayers
			pop.Players = new List<Player>();
			_playerRepository.LoadDBPlayers().Where(p => eventPlayers.Any(ep => ep.Player == p.Name)).ToList().ForEach(p => pop.Players.Add(new Player(p.Name)));

			//LoadEventPlayers
			eventPlayers.Where(ep => ep.Dropped > 0).ToList().ForEach(ep =>
			{
				foreach (var p in pop.Players)
				{
					if (p.name == ep.Player)
						p.droppedInRound = ep.Dropped;
				}
			});

			//LoadMatches
			pop.Matches = new List<Match>();
			_matchRepository.LoadDBMatches(pop.name).Where(m => m.Event == loadedEvent.Name).ToList().ForEach(m => pop.Matches.Add(m));

			foreach (var match in pop.Matches)
			{
				if (String.IsNullOrEmpty((match.Player1Name)))
					throw new Exception();

				if (String.IsNullOrEmpty((match.Player2Name)))
					throw new Exception();
			}

			foreach (var p in pop.Players)
			{
				foreach (var m in pop.Matches)
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

		public Event LoadEvent(string eventName)
		{
			var newEvent = new Event();
			var loadedEvent = LoadDBEvent(eventName);
			PopulateEvent(newEvent, loadedEvent);
			return newEvent;
		}

		public void SaveEvent(Event saveEvent)
		{
			if (saveEvent.myDbEvent == null)
			{
				CreateEvent(saveEvent);
			}
			else
			{
				UpdateEvent(saveEvent);
			}
		}



		public void CreateEvent(Event createEvent)
		{
			createEvent.myDbEvent = new dbEvent()
			{
				Name = createEvent.name,
				RoundMatches = createEvent.RoundMatches,
				Rounds = createEvent.rounds,
				CurrentRound = createEvent.CurrentRound,
				RoundEndDate = createEvent.RoundEndDate,
				StartDate = createEvent.EventStartDate
			};

			Create(createEvent.myDbEvent);
		}

		public void UpdateEvent(Event updateEvent)
		{
			updateEvent.myDbEvent.Name = updateEvent.name;
			updateEvent.myDbEvent.RoundMatches = updateEvent.RoundMatches;
			updateEvent.myDbEvent.Rounds = updateEvent.rounds;
			updateEvent.myDbEvent.CurrentRound = updateEvent.CurrentRound;
			updateEvent.myDbEvent.RoundEndDate = updateEvent.RoundEndDate;
			updateEvent.myDbEvent.StartDate = updateEvent.EventStartDate;


			Update(updateEvent.myDbEvent);
		}


		public dbEvent LoadDBEvent(string eventName)
		{
			var result = _dataContext.GetTable<dbEvent>().First(e => e.Name == eventName);
			result.dbName = result.Name;
			return result;
		}

		public List<dbEvent> LoadAllDBEvents()
		{
			var results = _dataContext.GetTable<dbEvent>().ToList();
			results.ForEach(r => r.dbName = r.Name);
			return results;
		}

		public void Create(dbEvent e)
		{
			var sqlCreate = "INSERT INTO Events (Name, CurrentRound, Rounds, RoundMatches, Locked, StartDate, RoundEndDate)";
			var sqlValues = $"VALUES ('{e.Name}', {e.CurrentRound}, {e.Rounds}, {e.RoundMatches}, {Convert.ToInt32(e.Locked)}, '{e.StartDate}', '{e.RoundEndDate}')";
			var fullSql = sqlCreate + sqlValues;

			try
			{
				_dataContext.ExecuteCommand(fullSql);
				e.dbName = e.Name;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void Update(dbEvent e)
		{
			var sqlUpdate = $"UPDATE Events SET Name='{e.Name}', CurrentRound={e.CurrentRound}, Rounds={e.Rounds}, RoundMatches={e.RoundMatches}, Locked={Convert.ToInt32(e.Locked)}, StartDate='{e.StartDate}', RoundEndDate='{e.RoundEndDate}' WHERE Name='{e.dbName}'";

			try
			{
				_dataContext.ExecuteCommand(sqlUpdate);
				e.dbName = e.Name;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void AddPlayer(Event e, Player player)
		{
			var eventName = e.myDbEvent.Name;

			var sqlAddPlayerToPlayers = $"IF NOT EXISTS(SELECT * FROM Players WHERE Players.Name = '{player.name}') INSERT INTO Players(Name) VALUES('{player.name}')";
			var sqlAddPlayerToEvent = $"IF NOT EXISTS(SELECT * FROM EventPlayers WHERE Player='{player.name}' AND EventName='{eventName}') INSERT INTO EventPlayers(Player, EventName, Dropped) VALUES('{player.name}', '{eventName}', 0)";

			try
			{
				_dataContext.ExecuteCommand(sqlAddPlayerToPlayers);
				_dataContext.ExecuteCommand(sqlAddPlayerToEvent);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
