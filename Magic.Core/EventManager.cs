using System.Collections.Generic;
using Magic.Domain;
using Magic.Data;
using System.Linq;

namespace Magic.Core
{
	public interface IEventManager
	{
		Event LoadEvent(string eventName);
		List<Event> LoadAllEvents();
		void SaveEvent(Event thisEvent);
		void CreateEvent(Event thisEvent);
		void AddPlayer(Event thisEvent, Player newPlayer);
		dbEvent GetCurrentEvent();
		List<dbPlayerPrize> GetPrizeAssignments(Event thisEvent, int round);
  }

	public class EventManager : IEventManager
	{
		private readonly IEventRepository _eventRepository;
		private readonly IRoundPrizeRepository _roundPrizeRepository;

		public EventManager(IEventRepository eventRepo, IRoundPrizeRepository roundPrizeRepo)
		{
			_eventRepository = eventRepo;
			_roundPrizeRepository = roundPrizeRepo;
		}

		public Event LoadEvent(string eventName)
		{
			return _eventRepository.LoadEvent(eventName);
		}

		public List<Event> LoadAllEvents()
		{
			return _eventRepository.LoadAllEvents();
		}

		public void SaveEvent(Event thisEvent)
		{
			_eventRepository.SaveEvent(thisEvent);
		}

		public void CreateEvent(Event thisEvent)
		{
			_eventRepository.CreateEvent(thisEvent);
		}

		public void AddPlayer(Event thisEvent, Player newPlayer)
		{
			_eventRepository.AddPlayer(thisEvent, newPlayer);
		}

		public dbEvent GetCurrentEvent()
		{
			return _eventRepository.GetCurrentEvent();
		}

		public List<dbPlayerPrize> GetPrizeAssignments(Event thisEvent, int round)
		{
			var prizeAssignments = new List<dbPlayerPrize>();
			var sortedPlayers = thisEvent.Players.OrderByDescending(p => p.Score(round)).ThenByDescending(p => p.OMWP(round)).ThenByDescending(p => p.GWP(round)).ThenByDescending(p => p.OGWP(round));

			var prizes = _roundPrizeRepository.LoadDBRoundPrizes(thisEvent.name);

			prizeAssignments = sortedPlayers.Select((p, index) =>
			{
				var thisPrize = prizes.FirstOrDefault(prize => prize.EventName == thisEvent.name && prize.Round == round && prize.Position == index+1);
				if(thisPrize!=null)
				{
					return new dbPlayerPrize
					{
						EventName = thisEvent.name,
						Notes = thisPrize.Other,
						Position = index+1,
						Packs = thisPrize.Packs,
						Round = round,
						Player = p.Name,
						PlayerID = p.ID,
						Recieved = 0
					};
				}
				else
				{
					return null;
				}
      }).OfType<dbPlayerPrize>().ToList();
			
			return prizeAssignments;
		}
  }
}
