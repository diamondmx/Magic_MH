using System.Collections.Generic;
using Magic.Domain;
using Magic.Data;

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
	}

	public class EventManager : IEventManager
	{
		private readonly IEventRepository _eventRepository;

		public EventManager(IEventRepository eventRepo)
		{
			_eventRepository = eventRepo;
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
  }
}
